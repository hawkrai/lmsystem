$(function () {
    $(".msg-send").on('click', function () {
        getMessageForm($(this).data('url'));
    });
});

function getMessageForm(messageFormUrl) {
    $.get(messageFormUrl,
            {},
          function (data) {
              showDialog(data);
          });
}

function showDialog(messageForm) {
    bootbox.dialog({
        message: messageForm,
        title: "Отправка сообщения",
    });
}

function submitMessageForm() {
    var form = $('#msgForm').find('form');
    if (form) {
        $(form).append('<input type=\"hidden\" id=\"itemAttachments\" name=\"itemAttachments\" />');
        $.validator.unobtrusive.parse($(form));
        if ($(form).valid()) {
            $("#itemAttachments").val(getCollectionItemAttachments());
            $(form).submit();
        }
    }
}

function successAjaxForm(result) {
    var box = $('#msgForm').parents(".modal");
    if (result.resultMessage) {
        if (result.code == 200) {
            $(box).modal('hide');
            alertify.success(result.resultMessage);
        } else {
            alertify.error(result.resultMessage);
        }
    } else {
        $('#msgForm').parent().html(result);
        $('form').removeData('validator');
        $('form').removeData('unobtrusiveValidation');
        $.validator.unobtrusive.parse('form');
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
           width: "100%"
       }
   );
}