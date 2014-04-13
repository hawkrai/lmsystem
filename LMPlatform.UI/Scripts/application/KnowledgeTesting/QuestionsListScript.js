var questionsList = {
    initList: function () {
        $('.questionTypeButton').on('click', $.proxy(this._onTypeButtonClicked, this));
        $('.editButton').on('click', 'span', $.proxy(this._onEditClicked, this));
        $('.deleteButton').on('click', 'span', $.proxy(this._onDeleteClicked, this));
        
        this._initializeTooltips();
        this._setColumnsSize();
    },
    
    _actionsColumnWidth: 50,
    _numberingColumnWidth: 10,
    
    _setColumnsSize: function () {
        // Set "№" column size
        $('.odd')
            .children(":first")
            .width(this._numberingColumnWidth);

        //set "Действия" column width
        $('[name="questionGridActionsCol"]')
            .parent()
            .width(this._actionsColumnWidth);
    },
    
    init: function() {
        $('#insertQuestionsFromAnotherTestButton').off().on('click', $.proxy(this._insertFromAnotherTestClicked, this));
        $('#testNamesDropdown').off().on('change', $.proxy(this._submitSelectorForm, this));
        $('#selectorSearchString').off().on('keypress', $.proxy(this._submitSelectorForm, this));
        $('#addNewQuestionButton').off().on('click', $.proxy(this._addNewQuestionButtonClicked, this));
        $('#selectQuestionButton').off().on('click', $.proxy(this._selectQuestionButtonClicked, this));
    },
    
    _initializeTooltips: function () {
        $(".editButton").tooltip({ title: "Редактировать вопрос", placement: 'left' });
        $(".deleteButton").tooltip({ title: "Удалить вопрос", placement: 'left' });
    },
    
    _onTypeButtonClicked: function(eventArgs) {
        $('#quetionTypes').modal('hide');
        questionDetails.draw(eventArgs.delegateTarget.id);
    },
    
    _submitSelectorForm: function () {
        $('#selectorFilterForm').submit();
    },

    _insertFromAnotherTestClicked: function () {
        $('#testToCopyId').val(getUrlValue('testId'));
        var questionsSelectorForm = $('#questionsSelectorForm');
        questionsSelectorForm.submit();
    },
   
    _addNewQuestionButtonClicked: function() {
        this._chooseQuestionType();
    },
    
    _selectQuestionButtonClicked: function() {
        $('#quetionsSelector').modal();
    },
    
    _chooseQuestionType: function () {
        $('#quetionTypes').modal();
    },
    
    _onEditClicked: function (eventArgs) {
        var itemId = eventArgs.target.dataset.modelId;
        questionDetails.showDialog(itemId);
    },
    
    _onDeleteClicked: function (eventArgs) {
        var context = {
            itemId: eventArgs.target.dataset.modelId
        };

        bootbox.confirm({
            title: 'Удаление вопроса из теста',
            message: 'Вы дествительно хотите удалить вопрос?',
            buttons: {
                'cancel': {
                    label: 'Отмена',
                    className: 'btn btn-primary btn-sm'
                },
                'confirm': {
                    label: 'Удалить',
                    className: 'btn btn-primary btn-sm',
                }
            },
            callback: $.proxy(this._onDeleteConfirmed, context)
        });
    },
    
    _onDeleteConfirmed: function (result) {
        if (result) {
            questionDetails.deleteQuestion(this.itemId);
        }
    }
};

function initQuestionsList() {
    questionsList.initList();
};

function questionsAddedFromAnothertest() {    
    datatable.fnDraw();
    $('#quetionsSelector').modal('hide');
}

$(document).ready(function () {
    questionsList.init();
});