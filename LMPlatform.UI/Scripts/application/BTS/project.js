var projectManagement = {
    init: function () {
        var that = this;
        $(".addProjectButton").tooltip({ title: "Добавить проект", placement: 'right' });
        that.initButtonAction();

        $("#chat-btn").on('click', function () {
            //var form = $(event.target).parents('form');
            var commentText = $('#CommentText').val();
            $.post('/BTS/ProjectManagement', { comment: commentText });
            $("#CommentText").val('');
        });
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
        $(".projectDetailsButton").tooltip({ title: "Информация о проекте", placement: 'left' });
        $(".editProject").tooltip({ title: "Редактировать проект", placement: 'top' });
        $(".deleteProject").tooltip({ title: "Удалить проект", placement: 'right' });
    }
};

$(document).ready(function () {
    projectManagement.init();
});