var questionsList = {
    init: function () {
        $('#questionsList').on('dblclick', "tr", this._onRowDoubleClick);
        $('.questionTypeButton').on('click', $.proxy(this._onTypeButtonClicked, this));
        $('#addNewQuestionButton').on('click', $.proxy(this._addNewQuestionButtonClicked, this));
    },
    
    _onTypeButtonClicked: function(eventArgs) {
        this._questionDetails = this._processQuestionTypeSelection(eventArgs.target.id);
        $('#quetionTypes').modal('hide');
        this._questionDetails.draw();
    },
    
    _processQuestionTypeSelection: function(typeId) {
        switch (typeId) {
            case "oneCorrectAnswer":
                return new HasOneCorrectVariantQuestionType();
            
        default:
            return null;
        }
    },
    
    _addNewQuestionButtonClicked: function() {
        this._chooseQuestionType();
    },
    
    _chooseQuestionType: function () {
        $('#quetionTypes').modal();
    },

    _onRowDoubleClick: function() {
        var aData = datatable.fnGetData(datatable.fnGetPosition(this));
        var itemId = aData[0];
        questionDetails.showDialog(itemId);
    }
};

$(document).ready(function () {
    questionsList.init();
});