$(document).ready(function () {
    var values = [10, 9, 8, 7, 6, 6, 5, 4, 2];
    var options = { width: '100%', height: '300px' };
    $('#chart').sparkline(values, options);
});

////functions
function initStudentManagement() {
    $('#studentList').css('border-top', 'none');
    $('#studentList').css('border-right', 'none');
    $('th').attr('style', " border-top: 1px solid #dddddd");
    $('#studentList').find('thead tr th:last-child').css('border', 'none');

    styleEditBtn(".editButton");

    initManagement(".editButton", "Редактировать", "Редактирование студент", "#studentList");
};

function initLecturerManagement() {

    $('#professorsList').css('border-top', 'none');
    $('#professorsList').css('border-right', 'none');
    $('th').attr('style', " border-top: 1px solid #dddddd");
    $('#professorsList').find('thead tr th:last-child').css('border', 'none');

    styleEditBtn(".editButton");


    $('#buttonActionSection').insertBefore('#professorsList');
    styleAddBtn("#buttonActionSection");

    initManagement(".addButton", "Добавить преподавателя", "Добавление преподавателя", "#professorsList");
    initManagement(".editButton", "Редактировать", "Редактирование преподавателя", "#professorsList");
};

function initGroupManagement() {
    $('#buttonActionSection').insertBefore('#groupList');
    styleAddBtn("#buttonActionSection");
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

function styleAddBtn(selector) {
    $(selector).css({
        'display': 'inline-block',
        'float': 'left',
        'margin': '15px 0px 0px 15px',
        'width': '30px',
        'height': '30px',
        'padding': '7px',
        'border-top-left-radius': '5px',
    });
    
    $(selector).addClass('managementBtn');
    $(selector).find('.glyphicon').css({ 'color': '#ffffff' });
};

function styleEditBtn(selector) {
    var cells = $(selector).parent('td');
    cells.css({ 'border': 'none' });
    cells.find('.glyphicon').css({ 'color': '#ffffff' });

    $(cells).addClass('managementBtn');
    
    $(cells).css({
        'width': '35px',
        'border-top-right-radius': '10px',
    });
};


