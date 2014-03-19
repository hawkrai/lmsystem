$(function () {
    $(".msgButton").on('click', function () {
        getMessageForm($(this).data('url'));
    });
});

function initDisplayMessageDialog() {
    $(".msg-row").unbind();
    $(".msg-row").on('click', function () {
        getMessagePartial($(this).data('url'));

        setReadStyle($(this));
    });
}

function getMessageForm(messageFormUrl) {
    $.get(messageFormUrl,
            {},
          function (data) {
              showDialog(data);
          });
}

function getMessagePartial(partialViewUrl) {
    $.get(partialViewUrl,
            {},
          function (data) {
              bootbox.dialog({
                  message: data,
                  title: "Сообщение"
              });

          });
}

function showDialog(messageForm) {
    bootbox.dialog({
        message: messageForm,
        title: "Написать сообщение",
        buttons: {
            main: {
                label: "Отправить",
                className: "btn-primary btn-submit",
                callback: function () {
                }
            }
        }
    });

    var form = $('#msgForm').find('form');
    var sendBtn = $('#msgForm').parents().find('.modal-dialog').find('.btn-submit');

    sendBtn.click(function () {
        return submitMessageForm(form);
    });
}

function submitMessageForm(form) {
    $(form).append('<input type=\"hidden\" id=\"itemAttachments\" name=\"itemAttachments\" />');
    $.validator.unobtrusive.parse($(form));
    if ($(form).valid()) {
        $("#itemAttachments").val(getCollectionItemAttachments());
        form.find('input[type = submit]').trigger('click');
    } else {
        return false;
    }
}

function getCollectionItemAttachments() {
    var itemAttachmentsTable = $('#fileupload').find('table').find('tbody tr');

    var data = $.map(itemAttachmentsTable, function (e) {
        var newObject = null;
        if (e.className === "template-download fade in") {
            if (e.id === "-1") {
                newObject = { Id: 0, Title: "", Name: $(e).find('td a').text(), AttachmentType: $(e).find('td.type').text(), FileName: $(e).find('td.guid').text() };
            } else {
                newObject = { Id: e.id, Title: "", Name: $(e).find('td a').text(), AttachmentType: $(e).find('td.type').text(), FileName: $(e).find('td.guid').text() };
            }
        }
        return newObject;
    });

    var dataAsString = JSON.stringify(data);
    return dataAsString;
}

function ajaxChosenInit(elem) {
    $(elem).ajaxChosen({
        type: 'GET',
        url: '/Message/GetSelectListOptions',
        dataType: 'json',
        keepTypingMsg: "Продолжайте печатать...",
        lookingForMsg: "Поиск"
    },
       function (data) {
           var terms = {};

           $.each(data, function (i, val) {
               terms[i] = val;
           });

           return terms;
       },
       {
           no_results_text: "Пользователь не найден...",
           width: '858px'
       }
   );
}

function setReadStyle(row) {
    var tabId = $(row).parents(".message-tab").attr('id');
    if (tabId == 'inbox') {
        $(row).removeClass("unread").addClass("is-read");
    }
}