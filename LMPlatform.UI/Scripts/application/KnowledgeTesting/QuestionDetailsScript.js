questionDetails = {
    init: function () {
        $('#addNewVariant').bind('click', $.proxy(this._onAddNewVariantbuttonClicked, this));
        $('#saveButton').bind('click', $.proxy(this._onSaveButtonClicked, this));
        this._answersElement = $("#answersContent").get(0);
        this._initEditor();
    },
    
    _webServiceUrl: '/Questions/',
    _saveMethodName: 'SaveQuestion',
    _deleteMethodName: 'DeleteQuestion',
    _getMethodName: 'GetQuestion',

    _initializeTooltips: function() {
        $("[name='checkVariant']").tooltip({ title: "Отметьте, если ответ правильный", placement: 'right' });
    },
    
    _onSaveButtonClicked: function () {
        var modelToSave = koWrapper.getModel();
        modelToSave.TestId = new Number(getUrlValue('testId'));
        modelToSave.Description = CKEDITOR.instances.taskArea.getData();
        modelToSave.QuestionType = questionTypes[modelToSave.templateName];
        
        if (modelToSave.QuestionType == 0) {
            this._populateOneCorrectAnswerVariants(modelToSave);
        }

        if (this._validate()) {
            this._saveQuestion(modelToSave);
        }
    },
    
    //TODO: fix this kostyl' by nativa knockout solution
    _populateOneCorrectAnswerVariants: function (modelToSave) {
        var boolArray = Enumerable.From($('[name="oneCorrectVariant"]'))
               .Select(function (radioButton) {
                   return $(radioButton).is(':checked') ? 1 : 0;
               }).ToArray();

        for (var i = 0; i < boolArray.length; i++) {
            modelToSave.Answers[i].IsCorrect = boolArray[i];
        }
    },
    
    _onAddNewVariantbuttonClicked: function() {
        koWrapper.koViewModel.Answers.push({ Content: 'Новый ответ', IsCorrect: 0 });
        this._initializeTooltips();
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
    
    _getNewQuestion: function () {
        return {
            Id: 0,
            Title: 'Название вопроса',
            ComplexityLevel: 1,
            Answers: [
                { Content: 'Ответ 1', IsCorrect: 0 },
                { Content: 'Ответ 2', IsCorrect: 0 }]
        };
    },
    
    _loadQuestion: function (id) {
        $.ajax({
            url: this._webServiceUrl + this._getMethodName,
            type: "GET",
            data: { id: id },
            dataType: "json",
            success: $.proxy(this._onQuestionLoaded, this)
        });
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
        alertify.success("Предмет успешно изменен");
        $('#quetionDetails').modal('hide');
    },

    draw: function (questionType) {
        var questionModel = this._getNewQuestion();
        questionModel.templateName = questionType + 'Template';
        this._fillQuestion(questionModel);
        $('#quetionDetails').modal();
    },
    
    showDialog: function(id) {
        this._loadQuestion(id);
    },
    
    _onQuestionLoaded: function (questionResult) {
        questionResult.templateName = questionTypesByNumber[questionResult.QuestionType];
        this._fillQuestion(questionResult);
        this._initializeTooltips();
        $('#quetionDetails').modal();
    },
    
    _fillQuestion: function (questionModel) {
        koWrapper.createOrUpdateViewModel(questionModel);
        koWrapper.createOrUpdateViewModel(questionModel);
        CKEDITOR.instances.taskArea.setData(questionModel.Description);
        this._initializeAnswersElementsEvents();
    },
    
    _initEditor: function () {
        CKEDITOR.inline('taskArea');
    },
    
    _validate: function () {
        return true;
    }
};

$(document).ready(function () {
    questionDetails.init();
});

var questionTypes = {    
    "hasOneCorrectVariantTemplate": 0,
    "hasManyCorrectVariantsTemplate": 1,
    "hasTextVariantsTemplate": 2,
    "sequenceVariantsTemplate" : 3
};

var questionTypesByNumber = {
    0: "hasOneCorrectVariantTemplate",
    1: "hasManyCorrectVariantsTemplate",
    2: "hasTextVariantsTemplate",
    3: "sequenceVariantsTemplate"
};

function getChecked() {
    return "on";
}