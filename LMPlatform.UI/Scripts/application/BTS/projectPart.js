var projectUserManagement = {
    init: function () {
        var that = this;
        $(".projectUserButton").tooltip({ title: "Добавить участника", placement: 'right' });
        that.initButtonAction();

        $("#groups").change(function () {
            $("#students").empty();
            var groupId = $("#groups").val();
            $.post("/BTS/GetStudents", { groupId: groupId }, function (data) {
                $.each(data, function (i, data) {
                    $("#students").append('<option value="' + data.Value + '">' +
                                        data.Text + '</option>');
                });
            });
        });
    },

    initButtonAction: function () {
        $('.projectUserButton').handle("click", function () {
            $.savingDialog("Добавление участника к проекту", "/BTS/AssignUserOnProject", null, "primary", function (data) {
                datatable.fnDraw();
                alertify.success("К проекту добавлен новый участник");
            });
            return false;
        });
    },

    projectUserEditItemActionHandler: function () {
        $('.editProjectUserButton').handle("click", function () {
            var that = this;
            $.savingDialog("Редактирование участника проекта", $(that).attr('href'), null, "primary", function (data) {
                datatable.fnDraw();
                alertify.success("Роль участника успешно изменена");
            });
            return false;
        });

        $(".deleteProjectUserButton").handle("click", function () {
            var that = this;
            bootbox.confirm("Вы действительно хотите удалить участника?", function (isConfirmed) {
                if (isConfirmed) {
                    dataTables.deleteRow("ProjectUserList", $(that).attr("href"));
                    datatable.fnDraw();
                    alertify.success("Участник проекта удален");
                }
            });
            return false;
        });
        $(".editProjectUser").tooltip({ title: "Редактировать участника проекта", placement: 'top' });
        $(".deleteProjectUser").tooltip({ title: "Удалить участника проекта", placement: 'right' });
    }
};

$(document).ready(function () {
    projectUserManagement.init();
    $("#groups").change();
});