function CompanyDocumentsModel(jsonModel) {
    var self = this;
    self.model = ko.mapping.fromJS(jsonModel);
    self.save = function (formElement) {
        ko.utils.postModel(formElement, self.model, function(result) {
            alert(result);
        });
    };
}