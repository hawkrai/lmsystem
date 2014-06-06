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
                alertify.success("Создан новый предмет");
            });
            return false;
        });
    },
    
    subjectEditItemActionHandler: function() {
        $('.editSubjectAction').handle("click", function () {
            var that = this;
            $.savingDialog("Редактирование предмета", $(that).attr('href'), null, "primary", function (data) {
                dataTables.fnDraw();
                alertify.success("Предмет успешно изменен");
            });
            return false;
        });
        
        $(".deleteSubjectButton").handle("click", function () {
            var that = this;
            bootbox.confirm("Вы действительно хотите удалить предмет?", function (isConfirmed) {
                if (isConfirmed) {
                    $.ajax({
                        type: 'GET',
                        url: $(that).attr("href"),
                        dataType: "json",
                        contentType: "application/json",

                    }).success(function (data, status) {
                    });
                    dataTables.deleteRow("subjectList", $(that).attr("href"));
                    alertify.success("Предмет удален");
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