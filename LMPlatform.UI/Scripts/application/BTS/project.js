var projectManagement = {
    init: function () {
        var that = this;
        that._initializeTooltips();
        that.initButtonAction();
        that._setColumnsSize();

        $('.deleteButton').on('click', 'span', $.proxy(this._onDeleteClicked, this));
    },

    _actionsColumnWidth: 65,
    _numberingColumnWidth: 20,

    _initializeTooltips: function () {
        $(".editProject").tooltip({ title: "Редактировать проект", placement: 'left' });
        //$(".deleteProject").tooltip({ title: "Удалить проект", placement: 'right' });
        $(".deleteButton").tooltip({ title: "Удалить проект", placement: 'top' });
    },

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

    _onDeleteClicked: function (eventArgs) {

        var itemId = eventArgs.target.dataset.modelId;
        var context = {
            itemId: eventArgs.target.dataset.modelId
        };
        
        bootbox.confirm({
            title: 'Удаление проекта',
            message: 'Вы дествительно хотите удалить проект?',
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
            projectDetails._deleteProject(this.itemId);
        }
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

        //$(".deleteProjectButton").handle("click", function () {
        //    var that = this;
        //    bootbox.confirm("Вы действительно хотите удалить проект?", function (isConfirmed) {
        //        if (isConfirmed) {
        //            dataTables.deleteRow("ProjectList", $(that).attr("href"));
        //            datatable.fnDraw();
        //            alertify.success("Проект удален");
        //        }
        //    });
        //    return false;
        //});
    }
};

var projectDetails = {

    _webServiceUrl: '/BTS/',
    _deleteMethodName: 'DeleteProject',

    _onProjectDeleted: function (result) {
        datatable.fnDraw();
        alertify.success("Проект удален");
    },

    _deleteProject: function (id) {
        $.ajax({
            url: this._webServiceUrl + this._deleteMethodName,
            type: "DELETE",
            data: { id: id },
            dataType: "json",
            success: $.proxy(this._onProjectDeleted, this)
        });
    },
}

var projectDetails = {

    _webServiceUrl: '/BTS/',
    _deleteMethodName: 'DeleteProject',

    _onProjectDeleted: function (result) {
        datatable.fnDraw();
        alertify.success("Проект удален");
    },

    _deleteProject: function (id) {
        $.ajax({
            url: this._webServiceUrl + this._deleteMethodName,
            type: "DELETE",
            data: { id: id },
            dataType: "json",
            success: $.proxy(this._onProjectDeleted, this)
        });
    },
}

$(document).ready(function () {
    projectManagement.init();
});