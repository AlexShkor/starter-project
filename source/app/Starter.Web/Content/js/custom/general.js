function GeneralViewModel(jsonModel) {
    var self = this;
    self.model = ko.mapping.fromJS(jsonModel);
    self.save = function (formElement) {
        ko.utils.postJsonNoQuotes(formElement, ko.mapping.toJS(self.model));
    };
}

function GeneralAjaxViewModel(jsonModel) {
    var self = this;
    self.model = ko.mapping.fromJS(jsonModel);
    self.save = function (formElement) {
        ko.utils.postModel(formElement, self.model);
    };
}

function GeneralFilterListViewModel(jsonFilterModel, baseUrl, excludeKeysList) {
    var self = this;
    self.filter = ko.mapping.fromJS(jsonFilterModel);
    self.excludeKeysList = excludeKeysList;
    ApplyFilterModel(self, baseUrl);
}

function ApplyFilterModel(self, urlPart) {
    self.items = ko.observableArray();
    self.filter.pages = ko.observableArray();

    self.goToPrevPage = function () {
        goToPage(self.filter.CurrentPage() - 1);
    };
    self.goToNextPage = function () {
        goToPage(self.filter.CurrentPage() + 1);
    };

    self.goToPage = function (item) {
        var page = item.Number();
        goToPage(page);
    };

    self.search = function () {
        submitForm();
    };

    var goToPage = function (page) {
        if (page > 0 && self.filter.CurrentPage() != page
            && page <= self.filter.TotalPages()) {
            self.filter.CurrentPage(page);
            submitForm();
        }
    };

    self.updatePagination = function () {
        self.filter.pages.removeAll();
        if (self.filter.pages().length != self.filter.TotalPages()) {
            for (var i = 1; i <= self.filter.TotalPages() ; i++) {
                self.filter.pages.push({
                    Number: ko.observable(i)
                });
            }
        }
    };
    self.updatePagination();

    self.update = function () {
        self.filter.CurrentPage(1);
        submitForm();
        return true;
    };

    self.sort = function (model, event) {
        var key = $(event.target).data("key");
        var desc = self.filter.OrderByKey() == key && !self.filter.Desc();
        self.filter.Desc(desc);
        self.filter.OrderByKey(key);
        self.update();
        return true;
    };

    var submitForm = function () {
        location.hash = location.hash = "search/" + ko.utils.buildQueryUrl(ko.mapping.toJS(self.filter), self.excludeKeysList);
    };

    Sammy(function () {
        var serverCallback = function (resp) {
            ko.mapping.fromJS(
                resp.Filter, {
                    'ignore': self.excludeKeysList,
                },
                self.filter);
            var data = resp.Items;
            self.items.removeAll();
            for (var i = 0; i < data.length; i++) {
                self.items.push(data[i]);
            }
            self.updatePagination();

        };

        this.get('#search/:q', function () {

            $.get(urlPart + "search?" + this.params.q, serverCallback);
        });

        this.get('#all', function () {
            $.get(urlPart + "search", serverCallback);
        });

        if (location.hash == "") {
            $.get(urlPart + "search" + window.location.search, serverCallback);
        }

    }).run();
}

