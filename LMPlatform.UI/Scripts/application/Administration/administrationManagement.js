$(document).ready(function () {
});

////functions
function initStudentManagement() {
    initManagement(".editButton", "Редактировать", "Редактирование студента", "#studentList");
    initDeleteDialog(".deleteButton", "Удалить", "Удаление студента", "#studentList");
};

function initLecturerManagement() {
    initManagement(".editButton", "Редактировать", "Редактирование преподавателя", "#professorsList");
    initManagement(".addButton", "Добавить преподавателя", "Добавление преподавателя", "#professorsList");
    initDeleteDialog(".deleteButton", "Удалить", "Удаление преподавателя", "#professorsList");
};

function initGroupManagement() {
    initManagement(".editButton", "Редактировать", "Редактирование группы", "#groupList");
    initManagement(".addButton", "Добавить группу", "Добавление группы", "#groupList");
    initDeleteDialog(".deleteButton", "Удалить", "Удаление группы", "#groupList");
};

function initManagement(btnSelector, btnTooltipTitle, saveDialogTitle, updateTableId) {
    var btn = $(btnSelector);
    btn.tooltip({ title: btnTooltipTitle, placement: 'right' });

    btn.handle("click", function () {
        var actionUrl = $(this).attr('href');
        $.savingDialog(saveDialogTitle, actionUrl, null, "primary", function (data) {
            updateTable(updateTableId);
            return false;
        });
        return false;
    });
};

function initDeleteDialog(btnSelector, btnTooltipTitle, saveDialogTitle, updateTableId) {
    var btn = $(btnSelector);
    btn.tooltip({ title: btnTooltipTitle, placement: 'right' });

    btn.handle("click", function () {
        var actionUrl = $(this).attr('href');
        bootbox.confirm({
            message: "Вы действительно хотите удалить?",

            buttons: {
                'cancel': {
                    label: "Отмена",
                    className: 'btn-default pull-left'
                },
                'confirm': {
                    label: 'Да',
                    className: 'btn-danger pull-right'
                }
            },


            callback: function (result) {
                if (result) {
                    $.post(actionUrl, function () {
                        updateTable(updateTableId);
                        alert("success");
                    }).fail(function () {
                        alert("error");
                    });
                }
            }
        });
    });
};

function updateTable(updateTableId) {
    if (updateTableId) {
        $(updateTableId).dataTable().fnDraw();
    }
};


