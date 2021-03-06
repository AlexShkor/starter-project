function NotificationsSettingsViewModel(jsonModel) {
    var self = this;

    var mapping = {
        'DocumentsExpiration': {
            create: function (options) {
                return new NotificationModel(options.data);
            }
        }
    };
    self.model = ko.mapping.fromJS(jsonModel, mapping);

    self.addNotificationsSettings = function () {
        self.model.DocumentsExpiration.push(new NotificationModel({
            DaysBeforeEvent: "",
            Receivers: [{ Email: "" }]
        }));
    };
    
    self.remove = function (item) {
        self.model.DocumentsExpiration.remove(item);
    };

    self.save = function (formElement) {
        ko.utils.postModel(formElement, self.model);
    };
}

function NotificationModel(data) {
    var self = this;
    ko.mapping.fromJS(data, {}, this);


    this.addEmail = function (item) {
        if (self.Receivers().length < 3) {
            var index = self.Receivers().indexOf(item);
            self.Receivers.splice(index + 1, 0, { Email: ko.observable("") });
        }
    };

    this.removeEmail = function (item) {
        if (self.Receivers().length > 1) {
            self.Receivers.remove(item);
        }
    };
}