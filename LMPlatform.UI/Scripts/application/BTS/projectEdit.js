$(function () {
    $(".editProjectButton").on('click', function () {
        getEditProjectForm($(this).data('url'));
    });
});

function getEditProjectForm(editProjectFormUrl) {
    $.get(editProjectFormUrl,
            {},
          function (data) {
              showDialog(data);
          });
}

function showDialog(editProjectFormUrl) {
    bootbox.dialog({
        message: editProjectFormUrl,
        title: "Изменить проект",
        buttons: {
            main: {
                label: "Изменить",
                className: "btn-primary btn-submit",
                callback: function () {
                }
            }
        }
    });

    var form = $('#addOrEditProjectForm').find('form');
    var sendBtn = $('#addOrEditProjectForm').parents().find('.modal-dialog').find('.btn-submit');

    sendBtn.click(function () {
        form.submit();
    });
}