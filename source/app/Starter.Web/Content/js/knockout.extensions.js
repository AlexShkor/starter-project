// http://stackoverflow.com/questions/10728052/form-submitted-to-mvc-action-via-knockout-has-extra-double-quotes
ko.utils.postJsonNoQuotes = function (urlOrForm, data, options) {
    options = options || {};
    var params = options['params'] || {};
    var includeFields = options['includeFields'] || this.fieldsIncludedWithJsonPost;
    var url = urlOrForm;

    // If we were given a form, use its 'action' URL and pick out any requested field values
    if ((typeof urlOrForm == 'object') && (ko.utils.tagNameLower(urlOrForm) === "form")) {
        var originalForm = urlOrForm;
        url = originalForm.action;
        for (var i = includeFields.length - 1; i >= 0; i--) {
            var fields = ko.utils.getFormFields(originalForm, includeFields[i]);
            for (var j = fields.length - 1; j >= 0; j--)
                params[fields[j].name] = fields[j].value;
        }
    }

    data = ko.utils.unwrapObservable(data);
    var form = document.createElement("form");
    form.style.display = "none";
    form.action = url;
    form.method = options["method"] || "post";
    for (var key in data) {
        var input = document.createElement("input");
        input.name = key;
        // I think this is the offending line....
        // input.value = ko.utils.stringifyJson(ko.utils.unwrapObservable(data[key]));
        input.value = ko.utils.unwrapObservable(data[key]);
        form.appendChild(input);
    }
    for (var key in params) {
        var input = document.createElement("input");
        input.name = key;
        input.value = params[key];
        form.appendChild(input);
    }
    document.body.appendChild(form);
    options['submitter'] ? options['submitter'](form) : form.submit();
    setTimeout(function () { form.parentNode.removeChild(form); }, 0);
};

ko.utils.postModel = function (urlOrForm, model, callback) {
    var url = urlOrForm;
    var scrollTop = function () {
        $("html, body").animate({ scrollTop: 0 }, "slow");
    };

    // If we were given a form, use its 'action' URL
    if ((typeof urlOrForm == 'object') && (ko.utils.tagNameLower(urlOrForm) === "form")) {
        url = urlOrForm.action;
    }

    data = ko.mapping.toJS(model);
    jQuery.ajax({
        type: "POST",
        url: url,
        dataType: 'json',
        contentType: 'application/json',
        async: false,
        data: ko.utils.stringifyJson(data),
        success: function (results) {
            if (results) {
                if (results.RedirectUrl > '') {
                    window.location = results.RedirectUrl;
                    return;
                }
                if (results.Errors) {
                    model.Errors(results.Errors);
                    if (results.Errors.length > 0) scrollTop();
                }
                if (results.SuccessMessage !== undefined) {
                    model.SuccessMessage(results.SuccessMessage);
                    if (results.SuccessMessage > '') scrollTop();
                }
            }
            if (callback) {
                callback(results);
            }
        }
    });
};

ko.extenders.numeric = function (target, options) {
    //create a writeable computed observable to intercept writes to our observable
    var result = ko.computed({
        read: target,  //always return the original observables value
        write: function (newValue) {
            var current = target(),
                newValueAsNum = isNaN(newValue) ? 0 : parseFloat(+newValue),
                valueToWrite = round(newValueAsNum, options);

            //only write if it changed
            if (valueToWrite !== current) {
                target(valueToWrite);
            } else {
                //if the rounded value is the same, but a different value was written, force a notification for the current field
                if (newValue !== current) {
                    target.notifySubscribers(valueToWrite);
                }
            }
        }
    });

    //initialize with current value to make sure it is rounded appropriately
    result(target());

    //return the new computed observable
    return result;
};

/*----------------------------------------------------------------------*/
/* Extended KO Bindings                                                 */
/*----------------------------------------------------------------------*/
ko.bindingHandlers.sortable = {
    init: function (element, valueAccessor) {
        // cached vars for sorting events
        var startIndex = -1,
            koArray = valueAccessor();

        var sortableSetup = {
            // cache the item index when the dragging starts
            start: function (event, ui) {
                startIndex = ui.item.index();

                // set the height of the placeholder when sorting
                ui.placeholder.height(ui.item.height());
            },
            // capture the item index at end of the dragging
            // then move the item
            stop: function (event, ui) {

                // get the new location item index
                var newIndex = ui.item.index();

                if (startIndex > -1) {
                    //  get the item to be moved
                    var item = koArray()[startIndex];

                    //  remove the item
                    koArray.remove(item);

                    //  insert the item back in to the list
                    koArray.splice(newIndex, 0, item);

                    //  ko rebinds the array so we need to remove duplicate ui item
                    ui.item.remove();

                    if (koArray.on_reorder) {
                        koArray.on_reorder();
                    }
                }

            },
            placeholder: 'fruitMoving'
        };

        // bind
        $(element).sortable(sortableSetup);
    }
};


ko.utils.submitForm = function (urlOrForm, data, options) {
    options = options || {};
    var params = options['params'] || {};
    var includeFields = options['includeFields'] || this.fieldsIncludedWithJsonPost;
    var url = urlOrForm;

    // If we were given a form, use its 'action' URL and pick out any requested field values
    if ((typeof urlOrForm == 'object') && (ko.utils.tagNameLower(urlOrForm) === "form")) {
        var originalForm = urlOrForm;
        url = originalForm.action;
        for (var i = includeFields.length - 1; i >= 0; i--) {
            var fields = ko.utils.getFormFields(originalForm, includeFields[i]);
            for (var j = fields.length - 1; j >= 0; j--)
                params[fields[j].name] = fields[j].value;
        }
    }

    data = ko.utils.unwrapObservable(data);
    var form = document.createElement("form");
    form.style.display = "none";
    form.action = url;
    form.method = options["method"] || "post";
    mapDataToForm(data, form);
    for (var key in params) {
        var input = document.createElement("input");
        input.name = key;
        input.value = params[key];
        form.appendChild(input);
    }
    document.body.appendChild(form);
    options['submitter'] ? options['submitter'](form) : form.submit();
    setTimeout(function () { form.parentNode.removeChild(form); }, 0);
};

ko.utils.buildQueryUrl = function (data, excludeKeysList, includeDefaultValues) {
    var query = "";
    parseValues(data, function (key, value) {
        if ((value || includeDefaultValues) && isKeyExcluded(key,excludeKeysList)) {
            query += encodeURIComponent(key) + "=" + encodeURIComponent(value) + "&";
        }
    });
    return query;
};

var isKeyExcluded = function (key, keysList) {
    if (!keysList) {
        return false;
    }
    for (var i = 0; i < keysList.length; i++) {
        if (key.indexOf(keysList[i] + ".")) {
            return true;
        }
    }
    return false;
};

var mapDataToForm = function (data, form, prefix) {
    prefix = prefix || "";
    for (var key in data) {
        if (data[key] == null) {
            continue;
        }
        if (typeof data[key] == "object") {
            mapDataToForm(data[key], form, prefix + key + ".");
        } else {
            var input = document.createElement("input");
            input.name = prefix + key;
            // I think this is the offending line....
            // input.value = ko.utils.stringifyJson(ko.utils.unwrapObservable(data[key]));
            input.value = ko.utils.unwrapObservable(data[key]);
            form.appendChild(input);
        }
    }
};

var parseValues = function (data, callback, prefix) {
    prefix = prefix || "";
    for (var key in data) {
        if (data[key] == null) {
            continue;
        }
        if (typeof data[key] == "object") {
            parseValues(data[key], callback, prefix + key + ".");
        } else {
            callback(prefix + key, ko.utils.unwrapObservable(data[key]));
        }
    }
};

ko.bindingHandlers.attrIf = {
    update: function (element, valueAccessor, allBindingsAccessor) {
        var h = ko.utils.unwrapObservable(valueAccessor());
        var show = ko.utils.unwrapObservable(h._if);
        if (show) {
            ko.bindingHandlers.attr.update(element, valueAccessor, allBindingsAccessor);
        } else {
            for (var k in h) {
                if (h.hasOwnProperty(k) && k.indexOf("_") !== 0) {
                    $(element).removeAttr(k);
                }
            }
        }
    }
};

function round(number, options) {
    options = options || {};
    var precision = options.precision || 0;
    var showZeros = options.showZeros || false;

    var roundingMultiplier = Math.pow(10, precision);
    var roundedValue = Math.round(number * roundingMultiplier) / roundingMultiplier;

    var result = roundedValue + '';
    if (precision > 0 && showZeros) {
        var dotIndex = result.indexOf('.');
        var requiredZerosNumber = precision;
        if (dotIndex == -1) {
            result += '.';
        } else {
            requiredZerosNumber -= result.length - dotIndex - 1;
        }
        for (var i = 0; i < requiredZerosNumber; i++) {
            result += '0';
        }
    }
    return result;
}

ko.bindingHandlers.returnKey = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        ko.utils.registerEventHandler(element, 'keydown', function (evt) {
            if (evt.keyCode === 13) {
                evt.preventDefault();
                evt.target.blur();
                valueAccessor().call(viewModel);
            }
        });
    }
};

ko.bindingHandlers.clear = {
    init: function (elem, valueAccessor) {
        $(elem).click(function () {
            var value = valueAccessor();
            if (value.removeAll) {
                value.removeAll();
            } else {
                value(null);
            }
            
        });
    }
};