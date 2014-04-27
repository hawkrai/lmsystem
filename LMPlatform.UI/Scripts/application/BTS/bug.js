$(function () {
    $(".addBugButton").on('click', function () {
        getAddBugForm($(this).data('url'));
    });

    this._initializeTooltips();
});

function _initializeTooltips() {
    $(".editBugButton").tooltip({ title: "Редактировать ошибку", placement: 'left' });
    $(".deleteBugButton").tooltip({ title: "Удалить ошибку", placement: 'left' });
    $(".bugDetailsButton").tooltip({ title: "Информация об ошибке", placement: 'left' });
}

function getAddBugForm(addBugFormUrl) {
    $.get(addBugFormUrl,
            {},
          function (data) {
              showDialog(data);
          });
}

function showDialog(addBugForm) {
    bootbox.dialog({
        message: addBugForm,
        title: "Документировать ошибку",
        buttons: {
            main: {
                label: "Документировать",
                className: "btn-primary btn-submit",
                callback: function () {
                }
            }
        }
    });

    var form = $('#addBugForm').find('form');
    var sendBtn = $('#addBugForm').parents().find('.modal-dialog').find('.btn-submit');

    sendBtn.click(function () {
        form.submit();
    });
}