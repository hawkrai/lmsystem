var bugManagement = {
    init: function () {
        var that = this;
        that._initializeTooltips();
        that.initButtonAction();
        that._setColumnsSize();

        $('.deleteButton').on('click', 'span', $.proxy(this._onDeleteClicked, this));
    },

    _actionsColumnWidth: 60,
    _numberingColumnWidth: 20,

    _setColumnsSize: function () {
        // Set "№" column size
        $('.odd')
            .children(":first")
            .width(this._numberingColumnWidth);

        //set "Действия" column width
        $('[name="bugGridActionsCol"]')
            .parent()
            .width(this._actionsColumnWidth);
    },

    _initializeTooltips: function () {
        $(".deleteButton").tooltip({ title: "Удалить ошибку", placement: 'top' });
        $(".editBug").tooltip({ title: "Редактировать ошибку", placement: 'top' });
    },

    _onDeleteClicked: function (eventArgs) {

        var itemId = eventArgs.target.dataset.modelId;
        var context = {
            itemId: eventArgs.target.dataset.modelId
        };

        bootbox.confirm({
            title: 'Удаление ошибки',
            message: 'Вы дествительно хотите удалить ошибку?',
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
            _bugDetails._deleteBug(this.itemId);
        }
    },

    initButtonAction: function () {
        $('.addBugButton').handle("click", function () {
            $.savingDialog("Документирование ошибки", "/BTS/AddBug", null, "primary", function (data) {
                datatable.fnDraw();
                alertify.success("Добавлена новая ошибка");
            });
            return false;
        });
    },

    bugEditItemActionHandler: function () {
        bugManagement.init();
        $('.editBugButton').handle("click", function () {
            var that = this;
            $.savingDialog("Редактирование ошибки", $(that).attr('href'), null, "primary", function (data) {
                datatable.fnDraw();
                alertify.success("Ошибка успешно изменена");
            });
            return false;
        });

        $(".deleteBugButton").handle("click", function () {
            var that = this;
            bootbox.confirm("Вы действительно хотите удалить ошибку?", function (isConfirmed) {
                if (isConfirmed) {
                    dataTables.deleteRow("BugList", $(that).attr("href"));
                    datatable.fnDraw();
                    alertify.success("Ошибка удалена");
                }
            });
            return false;
        });
    }
};

var _bugDetails = {

    _webServiceUrl: '/BTS/',
    _deleteMethodName: 'DeleteBug',

    _onBugDeleted: function (result) {
        datatable.fnDraw();
        alertify.success("Ошибка удалена");
    },

    _deleteBug: function (id) {
        $.ajax({
            url: this._webServiceUrl + this._deleteMethodName,
            type: "DELETE",
            data: { id: id },
            dataType: "json",
            success: $.proxy(this._onBugDeleted, this)
        });
    },
}

$(document).ready(function () {
    bugManagement.init();
});