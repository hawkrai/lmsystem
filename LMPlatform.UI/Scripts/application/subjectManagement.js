var subjectManagement = {    

    init: function () {
        var that = this;
        $(".addSubject").tooltip({ title: "Добавить предмет", placement: 'right' });
        that.initButtonAction();
    },
    initButtonAction: function () {
        $('.addSubject').handle("click", function () {
            $.savingDialog("Создание предмета", "/subject/create", null, "primary", function (data) {
                datatable.fnDraw();
            });
            return false;
        });
    },
    
    subjectEditItemActionHandler: function() {
        $('.editSubjectAction').handle("click", function () {
            var that = this;
            $.savingDialog("Редактирование предмета", $(that).attr('href'), null, "primary", function (data) {
                datatable.fnDraw();
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
        $(".editSubject").tooltip({ title: "Редактировать предмет", placement: 'left' });
        $(".deleteSubject").tooltip({ title: "Удалить предмет", placement: 'right' });

    }
};

$(document).ready(function () {
    subjectManagement.init();
});