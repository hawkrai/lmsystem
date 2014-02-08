questionDetails = {
    init: function () {
        $('#addNewVariant').bind('click', $.proxy(this._onAddNewVariantbuttonClicked, this));
        $('#saveButton').bind('click', $.proxy(this._onSaveButtonClicked, this));
        this._answersElement = $("#answersContent").get(0);
    },
    
    _webServiceUrl: '/Questions/',
    _saveMethodName: 'SaveQuestion',
    _deleteMethodName: 'DeleteQuestion',


    _onSaveButtonClicked: function () {
        var modelToSave = koWrapper.getModel();
        modelToSave.TestId = new Number(getUrlValue('testId'));
        if (this._validate()) {
            this._saveQuestion(modelToSave);
        }
    },
    
    _onAddNewVariantbuttonClicked: function() {

        koWrapper.koViewModel.Answers.push({ Content: 'new', IsCorrect: true });
        this._initializeAnswersElementsEvents();
    },
    
    _initializeAnswersElementsEvents: function () {
        $('.deleteAnswer').off();
        $('.deleteAnswer').on('click', $.proxy(this._onDeleteAnswerClicked, this));
    },
    
    _onDeleteAnswerClicked: function(eventArgs) {
        var itemIndex = $('.deleteAnswer').toArray().indexOf(eventArgs.target);
        koWrapper.koViewModel.Answers.splice(itemIndex, 1);
    },
    
    _loadQuestion: function () {
        return {
            Title : 'title',
            Answers: [
                { Content: 'FirstAnswer', IsCorrect: true },
                { Content: 'SecondAnswer', IsCorrect: true }]
        };
    },
    
    _saveQuestion: function (model) {
        $.ajax({
            url: this._webServiceUrl + this._saveMethodName,
            type: "POST",
            data: JSON.stringify(model),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: $.proxy(this._onQuestionSaved, this)
        });
    },
    
    deleteQuestion: function (id) {
        $.ajax({
            url: this._webServiceUrl + this._deleteMethodName,
            type: "DELETE",
            data: { id: id },
            dataType: "json",
            success: $.proxy(this._onQuestionDeleted, this)
        });
    },
    
    _onQuestionDeleted: function (result) {
        datatable.fnDraw();
    },
    
    _onQuestionSaved: function () {
        datatable.fnDraw();
        $('#quetionDetails').modal('hide');
    },

    draw: function (questionType) {
        var questionModel = this._loadQuestion();
        questionModel.templateName = questionType + 'Template';
        this._fillQuestion(questionModel);
        $('#quetionDetails').modal();
    },
    
    _fillQuestion: function (questionModel) {
        koWrapper.createOrUpdateViewModel(questionModel);
        this._initializeAnswersElementsEvents();
    },
    
    _validate: function () {
        return true;
    }
};

$(document).ready(function () {
    questionDetails.init();
});