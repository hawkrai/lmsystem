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
            alert('ergegerg');

            return false;
        });
    },

    applyCss: function () {
        $('.addNewsButton.fa').tooltip({ title: 'Добавить новость', placement: 'right' });
        $('.editNewsButton.fa').tooltip({ title: 'Редактировать новость', placement: 'left' });
        $('.deleteNewsButton.fa').tooltip({ title: 'Удалить новость', placement: 'right' });
     },

    updateHandlerActions: function () {
        $('#addNewsButton').handle("click", function () {
            var that = this;
            $.savingDialog("Создание новости", $(that).attr("href"), null, "primary", function (data) {
                $('.conteinerModule').empty();
                $('.conteinerModule').append(data);
                subjectWorking.updateHandlerActions();
            });
            return false;
        });
        $('a.editNewsButton').handle("click", function () {
            var that = this;
            $.savingDialog("Редактирование новости", $(that).attr("href"), null, "primary", function (data) {
                $('.conteinerModule').empty();
                $('.conteinerModule').append(data);
                subjectWorking.updateHandlerActions();
            });
            return false;
        });
        $('a.deleteNewsButton').handle("click", function () {
            var that = this;
            bootbox.confirm("Вы действительно хотите удалить новость?", function (isConfirmed) {
                if (isConfirmed) {
                    $.post($(that).attr("href"), null, function (data) {
                        $('.conteinerModule').empty();
                        $('.conteinerModule').append(data);
                        subjectWorking.updateHandlerActions();
                    });
                }
            });
            return false;
        });
        
        subjectWorking.applyCss();
    }
};

$(document).ready(function () {
    subjectWorking.init();
});