var questionsList = {
    init: function () {
        $('.questionTypeButton').on('click', $.proxy(this._onTypeButtonClicked, this));
        $('#addNewQuestionButton').on('click', $.proxy(this._addNewQuestionButtonClicked, this));
        $('#selectQuestionButton').on('click', $.proxy(this._selectQuestionButtonClicked, this));
        $('.editButton').on('click', 'span', $.proxy(this._onEditClicked, this));
        $('.deleteButton').on('click', 'span', $.proxy(this._onDeleteClicked, this));
        $('#insertQuestionsFromAnotherTestButton').on('click', $.proxy(this._insertFromAnotherTestClicked, this));
        $('#testNamesDropdown').on('change', $.proxy(this._submitSelectorForm, this));
        $('#selectorSearchString').on('keypress', $.proxy(this._submitSelectorForm, this));
    },
    
    _onTypeButtonClicked: function(eventArgs) {
        $('#quetionTypes').modal('hide');
        questionDetails.draw(eventArgs.delegateTarget.id);
        this._initEditor();
    },
    
    _submitSelectorForm: function () {
        $('#selectorFilterForm').submit();
    },

    _insertFromAnotherTestClicked: function () {
        $('#testToCopyId').val(getUrlValue('testId'));
        var questionsSelectorForm = $('#questionsSelectorForm');
        questionsSelectorForm.submit();
    },
    
    _initEditor: function() {
        CKEDITOR.inline('taskArea', {
            extraPlugins: 'mathedit',
            disableObjectResizing: true,
            skin: 'moono'
        });
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

        bootbox.confirm('Вы действительно хотите удалить этот вопрос?', $.proxy(this._onDeleteConfirmed, context));
    },
    
    _onDeleteConfirmed: function () {
        questionDetails.deleteQuestion(this.itemId);
    }
};

function initQuestionsList() {
    questionsList.init();
};

function questionsAddedFromAnothertest() {    
    datatable.fnDraw();
    $('#quetionsSelector').modal('hide');
}