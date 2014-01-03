baseQuestionType = {
    constructor: function () {
        $('#saveButton').bind('click', $.proxy(this._onSaveButtonClicked, this));
        $('#addNewVariantButton').bind('click', $.proxy(this._onAddNewVariantbuttonClicked, this));
        this._answersElement = $("#answersContent").get(0);
    },
    
    _onAddNewVariantbuttonClicked: function() {

       koWrapper.koViewModel.answers.push({ Content: 'new', IsCorrect: true });
    },
    
    _loadQuestion: function () {
        return {            
            answers: [
                { Content: 'FirstAnswer', IsCorrect: true },
                { Content: 'SecondAnswer', IsCorrect: true }]
        };
    },

    draw: function () {
        $('#quetionDetails').modal();
        var questionModel = this._loadQuestion();

        this._fillQuestion(questionModel);
    },
    
    _fillQuestion: function (questionModel) {
        questionModel.templateName = this.templateName;
        koWrapper.createOrUpdateViewModel(questionModel);
    },
};