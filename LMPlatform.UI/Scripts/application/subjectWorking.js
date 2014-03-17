var subjectWorking = {
    init: function () {
        var that = this;
        that.initModuleAction();
    },

    initModuleAction: function () {
        $('.moduleLinks').handle("click", function () {
            var that = this;
            $('.conteinerModule').spin('large');
            $.post($(that).attr("href"), null, function (data) {
                $('.conteinerModule').empty();
                $('.conteinerModule').append(data);
                subjectWorking.updateHandlerActions();
            });
        });

        $('.navLink').handle("click", function () {
            var that = this;
            var links = $('ul.nav.navbar-nav.side-nav').find('li');
            links.each(function () {
                $(this).removeClass("active");
            });
            $(that).addClass("active");
            return false;
        });

        $('#subGroups').handle("click", function() {
            var that = this;
            $.savingDialog("Управление подгруппами", $(that).attr("href"), null, "primary", function (data) {
                alertify.success("Подгруппы изменены");
                
            }, function (forms) {
                var subjectId = $(".conteinerModule").attr('data-subjectId');
                
                $(forms).append('<input type=\"hidden\" id=\"subjectId\" name=\"subjectId\" />');
                $("#subjectId").val(subjectId);

                $(forms).append('<input type=\"hidden\" id=\"groupId\" name=\"groupId\" />');
                $("#groupId").val($('#GroupId').first('option[selected=true]'));

                $(forms).append('<input type=\"hidden\" id=\"subGroupFirstIds\" name=\"subGroupFirstIds\" />');
                $("#subGroupFirstIds").val(subjectWorking.getSubGroupIds("StudentFirstList"));
                
                $(forms).append('<input type=\"hidden\" id=\"subGroupSecondIds\" name=\"subGroupSecondIds\" />');
                $("#subGroupSecondIds").val(subjectWorking.getSubGroupIds("StudentSecondList"));
            });
            return false;
        });
    },
    
    getSubGroupIds: function(listName) {
        var subGroup = $('#' + listName).find('option');
        var studentIds = "";
        $(subGroup).each(function (index, element) {
            studentIds += $(element).attr('value') + ",";
        });
        return studentIds;
    },

    applyCss: function () {
        $('.addNewsButton.fa').tooltip({ title: 'Добавить новость', placement: 'right' });
        $('.editNewsButton.fa').tooltip({ title: 'Редактировать новость', placement: 'left' });
        $('.deleteNewsButton.fa').tooltip({ title: 'Удалить новость', placement: 'right' });
     },

    updateHandlerActions: function() {
        $('#addNewsButton').handle("click", function() {
            var that = this;
            $.savingDialog("Создание новости", $(that).attr("href"), null, "primary", function(data) {
                $('.conteinerModule').empty();
                $('.conteinerModule').append(data);
                subjectWorking.updateHandlerActions();
                alertify.success("Создана новая новость");
            });
            return false;
        });
        $('a.editNewsButton').handle("click", function() {
            var that = this;
            $.savingDialog("Редактирование новости", $(that).attr("href"), null, "primary", function(data) {
                $('.conteinerModule').empty();
                $('.conteinerModule').append(data);
                subjectWorking.updateHandlerActions();
                alertify.success("Новость успешно изменена");
            });
            return false;
        });
        $('a.deleteNewsButton').handle("click", function() {
            var that = this;
            bootbox.confirm("Вы действительно хотите удалить новость?", function(isConfirmed) {
                if (isConfirmed) {
                    $.post($(that).attr("href"), null, function(data) {
                        $('.conteinerModule').empty();
                        $('.conteinerModule').append(data);
                        subjectWorking.updateHandlerActions();
                        alertify.success("Новость успешна удалена");
                    });
                }
            });
            return false;
        });
        $('#addLecturesButton').handle("click", function() {
            var that = this;
            $.savingDialog("Создание лекции", $(that).attr("href"), null, "primary", function(data) {
                    $('.conteinerModule').empty();
                    $('.conteinerModule').append(data);
                    subjectWorking.updateHandlerActions();
                    alertify.success("Создана новая лекция");
                },
                function(forms) {
                    $(forms).append('<input type=\"hidden\" id=\"attachments\" name=\"attachments\" />');
                    $("#attachments").val(subjectWorking.getLecturesFileAttachments());
                });
            subjectWorking.applyCss();

            return false;

        });

        $('a.deleteLecturesButton').handle("click", function () {
            var that = this;
            bootbox.confirm("Вы действительно хотите удалить лекцию?", function (isConfirmed) {
                if (isConfirmed) {
                    $.post($(that).attr("href"), null, function (data) {
                        $('.conteinerModule').empty();
                        $('.conteinerModule').append(data);
                        subjectWorking.updateHandlerActions();
                        alertify.success("Лекция успешна удалена");
                    });
                }
            });
            return false;
        });

        $('a.editLecturesButton').handle("click", function () {
            var that = this;
            $.savingDialog("Редактирование лекции", $(that).attr("href"), null, "primary", function (data) {
                $('.conteinerModule').empty();
                $('.conteinerModule').append(data);
                subjectWorking.updateHandlerActions();
                alertify.success("Лекция успешно изменена");
            },
            function (forms) {
                $(forms).append('<input type=\"hidden\" id=\"attachments\" name=\"attachments\" />');
                $("#attachments").val(subjectWorking.getLecturesFileAttachments());
            });
            return false;
        });
    },

    getLecturesFileAttachments : function() {
        var itemAttachmentsTable = $('#fileupload').find('table').find('tbody tr');
        var data = $.map(itemAttachmentsTable, function (e) {
            var newObject = null;
            if (e.className === "template-download fade in") {
                if (e.id === "-1") {
                    newObject = { Id: 0, Title: "", Name: $(e).find('td a').text(), AttachmentType: $(e).find('td.type').text(), FileName: $(e).find('td.guid').text() };
                } 
                else {
                    newObject = { Id: e.id, Title: "", Name: $(e).find('td a').text(), AttachmentType: $(e).find('td.type').text(), FileName: $(e).find('td.guid').text() };
                }
            }
            return newObject;
        });
        var dataAsString = JSON.stringify(data);
        return dataAsString;
    },

    subGroupsActionHandler: function() {
        $("#subGroupFirst").find("a.bright").handle("click", function () {
            var selectStudents = $("#StudentList").find("option");
            selectStudents.each(function (index, element) {
                if (element.selected == true) {
                    element.selected = false;
                    $("#StudentFirstList").append(element);
                }
            });
        });
        $("#subGroupFirst").find("a.bleft").handle("click", function () {
            var selectStudents = $("#StudentFirstList").find("option");
            selectStudents.each(function (index, element) {
                if (element.selected == true) {
                    element.selected = false;
                    $("#StudentList").append(element);
                }
            });
        });
        $("#subGroupFirst").find("a.double-right").handle("click", function () {
            var selectStudents = $("#StudentList").find("option");
            selectStudents.each(function (index, element) {
                    element.selected = false;
                    $("#StudentFirstList").append(element);
            });
        });
        $("#subGroupFirst").find("a.double-left").handle("click", function () {
            var selectStudents = $("#StudentFirstList").find("option");
            selectStudents.each(function (index, element) {
                    element.selected = false;
                    $("#StudentList").append(element);
            });
        });
        $("#subGroupTwo").find("a.bright").handle("click", function () {
            var selectStudents = $("#StudentList").find("option");
            selectStudents.each(function (index, element) {
                if (element.selected == true) {
                    element.selected = false;
                    $("#StudentSecondList").append(element);
                }
            });
        });
        $("#subGroupTwo").find("a.bleft").handle("click", function () {
            var selectStudents = $("#StudentSecondList").find("option");
            selectStudents.each(function (index, element) {
                if (element.selected == true) {
                    element.selected = false;
                    $("#StudentList").append(element);
                }
            });
        });
        $("#subGroupTwo").find("a.double-right").handle("click", function () {
            var selectStudents = $("#StudentList").find("option");
            selectStudents.each(function (index, element) {
                element.selected = false;
                $("#StudentSecondList").append(element);
            });
        });
        $("#subGroupTwo").find("a.double-left").handle("click", function () {
            var selectStudents = $("#StudentSecondList").find("option");
            selectStudents.each(function (index, element) {
                element.selected = false;
                $("#StudentList").append(element);
            });
        });

        $("#GroupId").change(function() {
            var subjectId = $(".conteinerModule").attr('data-subjectId');
            var element = $(this).find("option:selected");
            $.post("/subject/subgroupschangegroup", { subjectId: subjectId, groupId: $(element).attr('value') }, function (data) {
                $("#containerSubGroupEdit").empty();
                $("#containerSubGroupEdit").append(data);
            });
        });
    }
};

$(document).ready(function () {
    subjectWorking.init();
});