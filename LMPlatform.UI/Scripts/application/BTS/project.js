var projectManagement = {

    init: function () {
        var that = this;
        //$(".addProjectButton").tooltip({ title: "Добавить проект", placement: 'right' });
        that.initButtonAction();
        that._setColumnsSize();
    },

    _actionsColumnWidth: 65,
    _numberingColumnWidth: 20,

    _setColumnsSize: function () {
        // Set "№" column size
        $('.odd')
            .children(":first")
            .width(this._numberingColumnWidth);

        //set "Действия" column width
        $('[name="projectGridActionsCol"]')
            .parent()
            .width(this._actionsColumnWidth);
    },

    initButtonAction: function () {
        $('.addProjectButton').handle("click", function () {
            $.savingDialog("Добавление проекта", "/BTS/AddProject", null, "primary", function (data) {
                datatable.fnDraw();
                alertify.success("Добавлен новый проект");
            });
            return false;
        });
    },

    projectEditItemActionHandler: function () {
        projectManagement.init();
        $('.editProjectButton').handle("click", function () {
            var that = this;
            $.savingDialog("Редактирование проекта", $(that).attr('href'), null, "primary", function (data) {
                datatable.fnDraw();
                alertify.success("Проект успешно изменен");
            });
            return false;
        });

        $(".deleteProjectButton").handle("click", function () {
            var that = this;
            bootbox.confirm("Вы действительно хотите удалить проект?", function (isConfirmed) {
                if (isConfirmed) {
                    dataTables.deleteRow("ProjectList", $(that).attr("href"));
                    datatable.fnDraw();
                    alertify.success("Проект удален");
                }
            });
            return false;
        });
        $(".editProject").tooltip({ title: "Редактировать проект", placement: 'left' });
        $(".deleteProject").tooltip({ title: "Удалить проект", placement: 'right' });
    }
};

$(document).ready(function () {
    projectManagement.init();
});