function NewDriverSelectLocationModel(jsonModel) {
    var self = this;
    self.model = ko.mapping.fromJS(jsonModel);
    self.save = function (formElement) {
        window.location = "/drivers/new/" + self.model.LocationId();
    };
}

function InitDriverDocumentsPage(jsonModel) {
    var model = new DriverDocumentsModel(jsonModel);
    ko.applyBindings(model);
    $(".toggle-files-list").live("click", function () {
        $(this).next().toggle();
    });
        
    $('.fileupload').fileupload({
        dataType: 'json',
        singleFileUploads: false,
        add: function (e, data) {
            data.submit();
        },
        done: function (e, data) {
            for (var i = 0; i < data.result.length; i++) {
                model.addUploadedFile(data.result[i]);
            }
        }
    });
}

function DriverDocumentsModel(jsonModel) {
    var self = this;
    self.model = ko.observableArray();

    self.addUploadedFile = function(fileModel) {
        for (var i = 0; i < self.model().length; i++) {
            if (fileModel.DocumentId == self.model()[i].DocumentId()) {
                self.model()[i].addUploadedFile(fileModel);
            }
        }
    };
    
    for (var i = 0; i < jsonModel.length; i++) {
        var doc = ko.mapping.fromJS(jsonModel[i]);
        DriverDocumentModel(doc);
        self.model.push(doc);
    }
}

function DriverDocumentModel(model) {
    var Loading = "#preloader";

    model.UploadedFiles = ko.observableArray();

    model.save = function (item) {
        ko.utils.postModel(window.location, item);
    };

    model.deleteSavedFile = function(item) {
        $.get("/drivers/" + model.DriverId() + "/docs/" + model.DocumentId() + "/files/" + item.FileId() + "/delete", function() {
            model.SavedFiles.remove(item);
        });
    };
    
    model.DueDate= ko.computed(function() {
        var date = model.DocumentDate();
        var lengthOfValidity = model.LengthOfValidityModel.LengthOfValidity();
        if (date){
            var d = new Date(date);
            d.setMonth(d.getMonth() + lengthOfValidity);
            var str = (d.getMonth() +1) + "/" + d.getDate() + "/" + d.getFullYear();
            return str;
        };
        return "not set";
    }, model);

    model.addUploadedFile = function (jsonModel) {
        var file = ko.mapping.fromJS(jsonModel);
        file.IsNotesVisible = ko.observable(false);
        model.UploadedFiles.push(file);
    };

    model.toggleNotesBox = function(item) {
        item.IsNotesVisible(!item.IsNotesVisible());
    };
    
    model.deleteUploadedFile = function(file) {
        model.UploadedFiles.remove(file);
    };

    model.FileInputId = ko.computed(function() {
        return "fileUpload" + model.DocumentId();
    });
    
    var getPrimaryFile = function (items) {
        for (var i = 0; i < items.length; i++) {
            if (items[i].IsPrimary()) {
                return items[i];
            }
        }
        return null;
    };

    model.setReminder = function(item) {
        ko.utils.postModel('/drivers/set-reminder', item.Reminder);
    };

    model.overrideValidity = function (item) {
        ko.utils.postModel('/drivers/override-validity', item.LengthOfValidityModel);
    };

    model.toggleNotesBox = function(item) {
        item.IsNotesVisible(!item.IsNotesVisible());
    };

    model.PrimaryFileName = ko.computed(function() {
        var file = getPrimaryFile(model.UploadedFiles());
        if (file != null) {
            return file.FileName();
        }
        file = getPrimaryFile(model.SavedFiles());
        if (file != null) {
            return file.FileName();
        }
        return ".. no primary file selected";
    });
    
    model.IsPrimarySelected = ko.computed(function() {
        return getPrimaryFile(model.UploadedFiles()) != null;
    });

    model.saveFiles = function(doc) {
        ko.utils.postModel(doc.SaveFilesUrl(), doc.UploadedFiles, function(result) {
            for (var i = 0; i < result.length; i++) {
                var file = ko.mapping.fromJS(result[i]);
                if (file.IsPrimary()) {
                    var primaryFile = getPrimaryFile(model.SavedFiles());
                    if (primaryFile != null) {
                        primaryFile.IsPrimary(false);
                        primaryFile.PrimaryLabel(null);
                    }
                }
                model.SavedFiles.push(file);
            }
            if (result.length > 0) {
                model.UploadedFiles.removeAll();
            }
        });
    };
}

function MoveDriverModel(jsonModel) {
    var self = this;
    self.model = ko.mapping.fromJS(jsonModel);

    self.selectLocation= function (item) {
        self.model.LocationId(item.Value());
    };
    self.save = function (formElement) {
        ko.utils.postModel(formElement, self.model);
    };
}