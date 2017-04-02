var subjectWorking = {
    init: function () {
        var that = this;
        that.initModuleAction();
    },

    initModuleAction: function () {
        //$('.moduleLinks').handle("click", function () {
        //    var that = this;
        //    $('.conteinerModule').spin('large');
        //    $.post($(that).attr("href"), null, function (data) {
        //        $('.conteinerModule').empty();
        //        $('.conteinerModule').append(data);
        //        subjectWorking.updateHandlerActions();
        //    });
        //});

    	//var links = $('ul.sidebar-menu').find('li');

        //$(links).first().find('a')[0].click();
        //$(links).first().addClass("active");

        $('.navLink').handle("click", function () {
            var that = this;
            var links = $('ul.sidebar-menu').find('li');
            links.each(function () {
                $(this).removeClass("active");
            });
            $(that).addClass("active");
        });

        $('#subGroups').handle("click", function() {
            var that = this;
            $.savingDialog("Управление подгруппами", $(that).attr("href"), null, "primary", function (data) {
                alertify.success("Подгруппы изменены");
                $('#loadGroups').click();

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
            }, null, '690px');
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

    subGroupsActionHandler: function () {

    	$("#GroupId").tooltip({ title: "Выберите группу из списка", placement: 'right' });
    	$(".bright").tooltip({ title: "Добавить выделенных студентов в подгруппу", placement: 'left' });
    	$(".double-right").tooltip({ title: "Добавить всех студентов в подгруппу", placement: 'left' });
    	$(".bleft").tooltip({ title: "Переместить выделенных студентов из подгруппы", placement: 'right' });
    	$(".double-left").tooltip({ title: "Переместить всех студентов из подгруппы", placement: 'right' });

    	$("#subGroup").find("a.bright").handle("click", function () {
    		var selectStudents = $("#StudentFirstList").find("option");
            selectStudents.each(function (index, element) {
                if (element.selected == true) {
                    element.selected = false;
                    $("#StudentSecondList").append(element);
                }
            });
        });
    	$("#subGroup").find("a.bleft").handle("click", function () {
    		var selectStudents = $("#StudentSecondList").find("option");
            selectStudents.each(function (index, element) {
                if (element.selected == true) {
                    element.selected = false;
                    $("#StudentFirstList").append(element);
                }
            });
        });
    	$("#subGroup").find("a.double-right").handle("click", function () {
    		var selectStudents = $("#StudentFirstList").find("option");
            selectStudents.each(function (index, element) {
                    element.selected = false;
                    $("#StudentSecondList").append(element);
            });
        });
    	$("#subGroup").find("a.double-left").handle("click", function () {
    		var selectStudents = $("#StudentSecondList").find("option");
            selectStudents.each(function (index, element) {
                    element.selected = false;
                    $("#StudentFirstList").append(element);
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