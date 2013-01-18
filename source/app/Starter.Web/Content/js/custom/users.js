
function UserViewModel(jsonModel) {
    var self = this;
    self.model = ko.mapping.fromJS(jsonModel);
   
    self.addEmail = function (item) {
        var index = self.model.AdditionalEmails().indexOf(item);
        self.model.AdditionalEmails.splice(index + 1, 0, { Email: ko.observable("") });
    };
    self.removeEmail = function (item) {
        self.model.AdditionalEmails.remove(item);
    };
    self.save = function (formElement) {
        ko.utils.postModel(formElement, self.model);
    };
}
