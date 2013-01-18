function DocumentTypeModel(jsonModel) {
    var self = this;
    self.model = ko.mapping.fromJS(jsonModel);
    self.save = function (formElement) {
        ko.utils.postModel(formElement, self.model);
    };

    self.newTagValue = ko.observable("");
    self.addTag = function () {
        if (self.newTagValue() > "") {
            self.model.Tags.push({
                Text: self.newTagValue()
            });
            self.newTagValue("");
        }
    };

    self.removeTag = function (item) {
        self.model.Tags.remove(item);
    };

    self.addAttribute = function (item) {
        var index = self.model.Attributes().indexOf(item);
        self.model.Attributes.splice(index + 1, 0, new DocumentAttributeModel(self.model.AttributeTypes));
    };
    self.removeAttribute = function (item) {
        self.model.Attributes.remove(item);
    };
}


function DocumentAttributeModel(items) {
    this.Title = ko.observable();
    this.IsRequired = ko.observable();
    this.DocumentDataType = ko.observable();
    this.AvailableDocumentDataTypes = ko.mapping.fromJS(ko.mapping.toJS(items));
}
