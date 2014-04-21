$(function () {
    $(".projectButton").on('click', function () {
        getAddProjectForm($(this).data('url'));
    });

    $("#chat-btn").on('click', function () {
        var form = $(event.target).parents('form');
        var commentText = $('#CommentText').val();
        $.post('/BTS/ProjectManagement', { comment: commentText });
        form.submit();
    });

    //$('#projectNameList').on('change', function () {
    //    var form = $(event.target).parents('form');
    //    var projectId = $('#projectNameList').val();

    //    $.post('/BTS/ProjectManagement', { projectId: projectId });

    //    form.submit();
    //});
});

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
                className: "btn-primary btn-submit",
                callback: function () {
                }
            }
        }
    });

    var form = $('#addOrEditProjectForm').find('form');
    var sendBtn = $('#addOrEditProjectForm').parents().find('.modal-dialog').find('.btn-submit');

    sendBtn.click(function () {
            //if ($(form).valid()) {
            //    form.submit();
            //} else {
            //    return false;
            //}
        form.submit();
    });
}