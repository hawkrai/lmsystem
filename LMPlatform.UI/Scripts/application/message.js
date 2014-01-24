function addRecipient(recipientId, recipientName) {
    if (recipientId != '')
        addRecipientHtml($('#msgRecipients'), recipientId, recipientName);
};

function addRecipientHtml(recipientContainer, recipientId, recipientName) {
    var index = $(recipientContainer).find(".recipient").length;
    var recipientHtml = "<span class='recipient'><input type=hidden name='Recipients[" + index + "]' value='" + recipientId
        + "'/>" + recipientName + "; <sup><a href='#'>[Удалить]</a><sup></span>";

    $(recipientContainer).append(recipientHtml);
};