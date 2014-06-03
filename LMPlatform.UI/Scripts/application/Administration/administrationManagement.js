$(document).ready(function () {
});

////functions
function initStudentManagement() {
    initManagement(".editButton", "Редактировать", "Редактирование студента", "#studentList");
    initDeleteDialog(".deleteButton", "Удалить", "Удаление студента", "#studentList");
    initStatDialog(".statButton", "Статистика посещаемости");
};

function initLecturerManagement() {
    initManagement(".editButton", "Редактировать", "Редактирование преподавателя", "#professorsList");
    initManagement(".addButton", "Добавить преподавателя", "Добавление преподавателя", "#professorsList");
    initDeleteDialog(".deleteButton", "Удалить", "Удаление преподавателя", "#professorsList");
    initStatDialog(".statButton", "Статистика посещаемости");
};

function initGroupManagement() {
    initManagement(".editButton", "Редактировать", "Редактирование группы", "#groupList");
    initManagement(".addButton", "Добавить группу", "Добавление группы", "#groupList");
    initDeleteDialog(".deleteButton", "Удалить", "Удаление группы", "#groupList");
};

function initManagement(btnSelector, btnTooltipTitle, dialogTitle, updateTableId) {
    var btn = $(btnSelector);
    btn.tooltip({ title: btnTooltipTitle, placement: 'right' });

    btn.handle("click", function () {
        var actionUrl = $(this).attr('href');
        showForm(actionUrl, dialogTitle);
        return false;
    });
};

function successAjaxForm(result) {
    var box = $('#adminModal').parents(".modal");
    if (result.resultMessage) {
        $(box).modal('hide');
        updateTable(".dataTable");
        alertify.success(result.resultMessage);
    } else {
        $('#adminModal').html(result);
        $('form').removeData('validator');
        $('form').removeData('unobtrusiveValidation');
        $.validator.unobtrusive.parse('form');
    }
}

function initStatDialog(btnSelector, btnTooltipTitle) {
    var btn = $(btnSelector);
    btn.tooltip({ title: btnTooltipTitle, placement: 'right' });

    btn.handle("click", function () {
        var actionUrl = $(this).attr('href');
        $.get(actionUrl, {},
          function (data) {
              bootbox.dialog({
                  message: data,
                  title: "Статистика посещаемости",
              });
          });
    });
}


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
                    className: 'btn btn-sm'
                },
                'confirm': {
                    label: 'Да',
                    className: 'btn btn-primary btn-sm'
                }
            },

            callback: function (result) {
                if (result) {
                    $.post(actionUrl, function (data) {
                        updateTable(updateTableId);
                        if (data.resultMessage)
                            alertify.success(data.resultMessage);
                    }).fail(function () {
                        alertify.error("Произошла ошибка");
                    });
                }
            }
        });
    });
};

function showForm(formUrl, formTitle) {
    $.get(formUrl,
            {},
          function (data) {
              bootbox.dialog({
                  message: data,
                  title: formTitle
              });
          });
}

function updateTable(updateTableId) {
    if (updateTableId) {
        $(updateTableId).dataTable().fnDraw();
    }
};


