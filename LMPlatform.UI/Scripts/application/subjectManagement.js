var subjectManagement = {    

    init: function () {
        var that = this;
        $(".addSubject").tooltip({ title: "Добавить предмет", placement: 'right' });
        $(".addSubject").attr('style', "color:#333;font-size: 24px;");
        that.initButtonAction();
    },
    initButtonAction: function () {
        $('.addSubject').handle("click", function () {
            $.savingDialog("Создание предмета", "/subject/create", null, "success", function (data) {
                //$("#tableFeeds").empty();
                //$("#tableFeeds").append(data.partial);
                //newsFeedsManagement.newsFeedActionHandler();
            });
            return false;
        });
    }
};

$(document).ready(function () {
    subjectManagement.init();
});