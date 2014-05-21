var projectUserManagement = {
    init: function () {
        var that = this;
        //$(".projectUserButton").tooltip({ title: "Добавить участника", placement: 'right' });
        that.initButtonAction();
        that._setColumnsSize();

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

    _actionsColumnWidth: 70,
    _numberingColumnWidth: 20,

    _setColumnsSize: function () {
        // Set "№" column size
        $('.odd')
            .children(":first")
            .width(this._numberingColumnWidth);

        //set "Действия" column width
        $('[name="projectUserGridActionsCol"]')
            .parent()
            .width(this._actionsColumnWidth);

        $("#ProjectUsersList_wrapper").css('margin-bottom', '0px');
    },

    initButtonAction: function () {
        $('.projectUserButton').handle("click", function () {
            $.savingDialog("Добавление участника к проекту", "/BTS/AssignUserOnProject", null, "primary", function (data) {
                datatable.fnDraw();
                alertify.success("Добавлен новый участник");
            });
            return false;
        });
    },

    projectUserEditItemActionHandler: function () {
        projectUserManagement.init();
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

$(document).ready(function() {
    projectUserManagement.init();
    $("#groups").change();

    document.onmouseover = document.onmouseout = handler;

    function handler(e) {
        e = e || event;
        if (e.type == 'mouseover') {
            var toElem = e.srcElement || e.target;
            if (str(toElem) == "TD") {
                //var id = toElem.parentElement.outerHTML.find("span").val();
                var name = toElem.parentNode.childNodes[1];
                var id = toElem.parentNode.childNodes[3].children[0].children[0].children[0].textContent;
                $.post("/BTS/GetUserInformation", { id: id }, function (data) {
                    //$("#userPanel").append(data);
                    name.easyTooltip({
                        tooltipId: "easyTooltip2",
                        content: data
                    });
                });
            }
        }
    }

//else if (e.type == 'mouseout') {
    //    fromElem = e.srcElement || e.target;
    //    toElem = e.toElement || e.relatedTarget;
    //}

    function str(el) {
        return el ? (el.id || el.nodeName) : 'null';
    }
});