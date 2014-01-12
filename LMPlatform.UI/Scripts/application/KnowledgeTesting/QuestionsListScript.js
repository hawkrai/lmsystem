var questionsList = {
    init: function () {
        $('.questionTypeButton').on('click', $.proxy(this._onTypeButtonClicked, this));
        $('#addNewQuestionButton').on('click', $.proxy(this._addNewQuestionButtonClicked, this));
        $('.editButton').on('click', 'span', $.proxy(this._onEditClicked, this));
        $('.deleteButton').on('click', 'span', $.proxy(this._onDeleteClicked, this));
    },
    
    _onTypeButtonClicked: function(eventArgs) {
        $('#quetionTypes').modal('hide');
        questionDetails.draw(eventArgs.target.id);
        this._initEditor();
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