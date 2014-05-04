var testsDetails = {
    init: function () {
        $('#saveButton').bind('click', $.proxy(this._onSaveButtonClicked, this));
    },

    _webServiceUrl: '/Tests/',
    _saveMethodName: 'SaveTest',
    _getMethodName: 'GetTest',
    _deleteMethodName: 'DeleteTest',

    formatTitle: function () {
        var title = koWrapper.getModel().Title;
        return title == null ? "Новый тест" : title;
    },

    getTextForTimeLabel: function () {
        return koWrapper.getModel().SetTimeForAllTest
            ? 'Время на весь тест (мин)'
            : 'Время на 1 вопрос(сек)';
    },

    _onSaveButtonClicked: function () {
        var modelToSave = koWrapper.getModel();
        modelToSave.SubjectId = new Number(getUrlValue('subjectId'));
        if (this._validate()) {
            this._saveTest(modelToSave);
        }
    },

    _saveTest: function (model) {
        $.ajax({
            url: this._webServiceUrl + this._saveMethodName,
            type: "POST",
            data: JSON.stringify(model),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: $.proxy(this._onTestSaved, this)
        });
    },

    _loadTest: function (id) {
        $.ajax({
            url: this._webServiceUrl + this._getMethodName,
            type: "GET",
            data: { id: id },
            dataType: "json",
            success: $.proxy(this._onTestLoaded, this)
        });
    },
    
    deleteTest: function (id) {
        $.ajax({
            url: this._webServiceUrl + this._deleteMethodName,
            type: "DELETE",
            data: { id: id },
            dataType: "json",
            success: $.proxy(this._onTestDeleted, this)
        });
    },

    _onTestLoaded: function (testResult) {
        koWrapper.createOrUpdateViewModel(testResult);
        $('#testDetails').modal();
    },
    
    _onTestSaved: function () {
        datatable.fnDraw();
        $('#testDetails').modal('hide');
        alertify.success("Тест успешно сохранен");
    },
    
    _onTestDeleted: function(result) {
        datatable.fnDraw();
    },

    _validate: function () {
        return true;
    },

    showDialog: function (id) {
        this._loadTest(id);
    }
};

$(document).ready(function () {
    testsDetails.init();
});
