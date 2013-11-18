var masterPageManagement = {
    init: function () {
        var that = this;
        that.initDateTimePickerFromEvent();
    },
    initDateTimePickerFromEvent: function () {
        $('#dateEvent').datepicker({
            format: "dd.mm.yyyy",
            language: "ru"
        });
        $('#dateEvent').children('div.datepicker').removeClass('datepicker-inline');
    },
};

$(document).ready(function () {
    masterPageManagement.init();
    shared.setFooter();
});
