var subjectManagement = {    

    init: function () {
        var that = this;
        $(".addSubject").tooltip({ title: "Добавить предмет", placement: 'right' });
        $(".addSubject").attr('style', "color:#333;font-size: 24px;");
        that.initButtonAction();
    },
    initButtonAction: function () {
        $('.addSubject').handle("click", function () {
            $.savingDialog("Создание предмета", "/subject/create", null, "primary", function (data) {
                //$("#tableFeeds").empty();
                //$("#tableFeeds").append(data.partial);
                //newsFeedsManagement.newsFeedActionHandler();
            });
            return false;
        });
    },
    
    subjectEditItemActionHandler: function() {
        $('.editSubjectAction').handle("click", function () {
            var that = this;
            $.savingDialog("Редактирование предмета", $(that).attr('href'), null, "primary", function (data) {
                //$("#tableFeeds").empty();
                //$("#tableFeeds").append(data.partial);
                //newsFeedsManagement.newsFeedActionHandler();
                
            });
            return false;
        });
        
        $(".deleteSubjectAction").handle("click", function () {
            var that = this;
            bootbox.confirm("Вы действительно хотите удалить предмет?", function (isConfirmed) {
                if (isConfirmed) {
                    dataTables.deleteRow("subjectList", $(that).attr("href"));
                }
            });
            return false;
        });
    }
};

$(document).ready(function () {
    subjectManagement.init();
});