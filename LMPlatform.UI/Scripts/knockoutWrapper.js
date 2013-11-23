var koWrapper = {
    init: function () {
        this.koViewModel = null;
    },

    //Get model as JSON object
    getModel: function () {
        return this.koViewModel ? ko.mapping.toJS(this.koViewModel) : null;
    },

    //Create observable view model
    createViewModel: function (model, settings) {
        settings = settings != null ? settings : {};
        var viewModel = ko.mapping.fromJS(model, settings);
        this.koViewModel = viewModel;
        ko.applyBindings(viewModel);

        return viewModel;
    },

    updateViewModel: function (model, settings) {
        if (model) {
            settings = settings != null ? settings : {};
            this.koViewModel = ko.mapping.fromJS(model, settings, this.koViewModel);
        }
        return this.koViewModel;
    },

    createOrUpdateViewModel: function (model, settings) {
        if (this.koViewModel == null) {
            return this.createViewModel(model, settings);
        } else {
            return this.updateViewModel(model, settings);
        }
    },

    //The method updates field of an observable object with values from plainObject.
    //Field names of observable and plain objects must coinside
    updateObservableObject: function (observableObject, plainObject) {
        for (var field in plainObject) {
            if (typeof (plainObject[field]) !== "object" && plainObject[field] !== null) {
                if ($.isFunction(observableObject[field])) {
                    observableObject[field](plainObject[field]);
                }
            } else {
                this.updateObservableObject(plainObject[field], observableObject[field]);
            }
        }
    }
};

$(document).ready(function () {
    koWrapper.init();
});


