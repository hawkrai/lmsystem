var testsList = {
    init: function() {
        $('.editButton').on('click', 'span', $.proxy(this._onEditClicked, this));
        $('.deleteButton').on('click', 'span', $.proxy(this._onDeleteClicked, this));
        $('.lockButton').on('click', 'span', $.proxy(this._onLockClicked, this));
        //$('.startTestButton').on('click', 'span', $.proxy(this._onStartTestClicked, this));
        $('#addNewTestButton').on('click', $.proxy(this._addNewTestButtonClicked, this));

        this._initializeTooltips();
        this._setColumnsSize();
    },
    
    _actionsColumnWidth: 160,
    _numberingColumnWidth: 10,
    
    _initializeTooltips: function() {
        $(".editButton").tooltip({ title: "Редактировать тест", placement: 'left' });
        $(".deleteButton").tooltip({ title: "Удалить тест", placement: 'left' });
        $(".lockButton").tooltip({ title: "Доступность теста", placement: 'left' });
        $(".questionsButton").tooltip({ title: "Перейти к вопросам", placement: 'left' });
        $('.startTestButton').tooltip({ title: "Пройти тест", placement: 'left' });
    },
    
    _setColumnsSize: function () {
        // Set "№" column size
        $('.odd')
            .children(":first")
            .width(this._numberingColumnWidth);

        //set "Действия" column width
        $('[name="testGridActionsCol"]')
            .parent()
            .width(this._actionsColumnWidth);
    },
    
    _onStartTestClicked: function() {
        alert('пройти тест');
    },

    _addNewTestButtonClicked: function() {
        testsDetails.showDialog(0);
    },

    _onEditClicked: function(eventArgs) {
        var itemId = eventArgs.target.dataset.modelId;
        testsDetails.showDialog(itemId);
    },
    
    _onLockClicked: function (eventArgs) {
        var itemId = eventArgs.target.dataset.modelId;
        testsUnlocks.showDialog(itemId);
    },

    _onDeleteClicked: function (eventArgs) {
        var context = {
            itemId: eventArgs.target.dataset.modelId
        };
        
        bootbox.confirm({
            title: 'Удаление теста',
            message: 'Вы дествительно хотите удалить тест?',
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
            testsDetails.deleteTest(this.itemId);
        }
    }
};

function initTestList() {
    testsList.init();
}