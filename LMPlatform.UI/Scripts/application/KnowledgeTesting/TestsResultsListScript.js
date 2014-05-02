var testsResultsList = {
    init: function () {
        $('.groupButton').on('click', $.proxy(this._onGroupClicked, this));
    },

    _onGroupClicked: function() {
        alert('Результаты для группы!');
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
    }
};

//function initTestResultsList() {
//    testsResultsList.init();
//}

$(document).ready(function () {
    testsResultsList.init();
});
