$(function () {
    $(".projectButton").on('click', function () {
        getAddProjectForm($(this).data('url'));
    });

    _initializeTooltips();

    $("#chat-btn").on('click', function () {
        var form = $(event.target).parents('form');
        var commentText = $('#CommentText').val();
        $.post('/BTS/ProjectManagement', { comment: commentText });
        form.submit();
    });
});

function _initializeTooltips() {
    $(".editProjectButton").tooltip({ title: "Редактировать проект", placement: 'left' });
    $(".deleteProjectButton").tooltip({ title: "Удалить проект", placement: 'left' });
    $(".projectDetailsButton").tooltip({ title: "Информация о проекте", placement: 'left' });
}

function getAddProjectForm(addProjectFormUrl) {
    $.get(addProjectFormUrl,
            {},
          function (data) {
              showDialog(data);
          });
}

function showDialog(addProjectForm) {
    bootbox.dialog({
        message: addProjectForm,
        title: "Добавить проект",
        buttons: {
            main: {
                label: "Добавить",
                //className: "btn-primary btn-submit",
                className: "btn btn-primary btn-submit",
                callback: function () {
                }
            }
        }
    });

    var form = $('#addOrEditProjectForm').find('form');
    var sendBtn = $('#addOrEditProjectForm').parents().find('.modal-dialog').find('.btn-submit');

    sendBtn.click(function () {
        form.submit();
        alertify("Добавлен новый проект");
    });
}

//if ($(form).valid()) {
//    form.submit();
//} else {
//    return false;
//}

//$('#projectNameList').on('change', function () {
//    var form = $(event.target).parents('form');
//    var projectId = $('#projectNameList').val();

//    $.post('/BTS/ProjectManagement', { projectId: projectId });

//    form.submit();
//});