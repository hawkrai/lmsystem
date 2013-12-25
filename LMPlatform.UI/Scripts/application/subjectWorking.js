var subjectWorking = {
    init: function() {
        var that = this;
        that.initModuleAction();
    },

    initModuleAction: function () {
        $('.moduleLinks').handle("click", function () {
            var that = this;
            $('.conteinerModule').spin('large');
            
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
    }
};

$(document).ready(function () {
    subjectWorking.init();
});