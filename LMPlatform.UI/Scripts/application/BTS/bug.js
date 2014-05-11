var bugManagement = {
    init: function () {
        var that = this;
        //$(".addBugButton").tooltip({ title: "Задокументировать ошибку", placement: 'right' });
        that.initButtonAction();
        that._setColumnsSize();
    },

    _actionsColumnWidth: 70,
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
        $(".bugDetailsButton").tooltip({ title: "Информация об ошибке", placement: 'left' });
        $(".editBug").tooltip({ title: "Редактировать ошибку", placement: 'top' });
        $(".deleteBug").tooltip({ title: "Удалить ошибку", placement: 'right' });
    }
};

$(document).ready(function () {
    bugManagement.init();
});