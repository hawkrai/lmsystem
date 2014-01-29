$(function () {
    initDialog(".msgButton", "Написать сообщение", "Отправка сообщения", "");
});

function initDialog(btnSelector, btnTooltipTitle, saveDialogTitle) {
    var btn = $(btnSelector);
    btn.tooltip({ title: btnTooltipTitle, placement: 'right' });

    btn.handle("click", function () {
        var actionUrl = $(this).attr('href');
        $.savingDialog(saveDialogTitle, actionUrl, null, "primary", function (data) {
            
            return false;
        });
        return false;
    });
};
