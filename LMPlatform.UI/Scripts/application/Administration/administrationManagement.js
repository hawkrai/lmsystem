$(document).ready(function () {
    var values = [10, 9, 8, 7, 6, 6, 5, 4, 2];
    var options = { width: '100%', height: '300px' };
    $('#chart').sparkline(values, options);
});

////functions
function initStudentManagement() {
    initManagement(".editButton", "Редактировать", "Редактирование студент", "#studentList");
};

function initLecturerManagement() {
    initManagement(".addButton", "Добавить преподавателя", "Добавление преподавателя", "#professorsList");
    initManagement(".editButton", "Редактировать", "Редактирование преподавателя", "#professorsList");
};

function initGroupManagement() {
    initManagement(".addButton", "Добавить группу", "Добавление группы", "#groupList");
};

function initManagement(btnSelector, btnTooltipTitle, saveDialogTitle, updateTableId) {
    var btn = $(btnSelector);
    btn.tooltip({ title: btnTooltipTitle, placement: 'right' });

    btn.on("click", function () {
        var actionUrl = $(this).attr('href');
        $.savingDialog(saveDialogTitle, actionUrl, null, "primary", function (data) {
            updateTable(updateTableId);
            return false;
        });
        return false;
    });
};

function updateTable(updateTableId) {
    if (updateTableId) {
        $(updateTableId).dataTable().fnDraw();
    }
};


