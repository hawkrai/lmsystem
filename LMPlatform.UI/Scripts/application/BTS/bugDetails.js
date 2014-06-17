var bugDetails = {
    init: function () {
        var that = this;
        that.initButtonAction();

        $.post("/BTS/IsUserAnAssignedDeveloper", null, function (data) {
            if (data == "True") {
                $("#editBug").removeClass("hidden");
            }
        });

        $('.bugStatusHelper').mouseover(function (e) {
            $.post("/BTS/GetBugStatusHelper", null, function (data) {
                $('.bugStatusHelper').easyTooltip({
                    yOffset: 150,
                    tooltipId: "easyTooltip3",
                    content: data
                });
            });
        });
    },

    initButtonAction: function () {
        $('.editBugButton').handle("click", function () {
            var that = this;
            $.savingDialog("Редактирование ошибки", $(that).attr('href'), null, "primary", function (data) {
                location.reload(true);
                alertify.success("Ошибка успешно изменена");
            });
            return false;
        });

        $("#statuses").change(function () {
            if ($("#statuses").val() == 2) {
                $("#developers").empty();
                $.post("/BTS/GetDeveloperNames", null, function (data) {
                    $.each(data, function (i, data) {
                        $("#developers").append('<option value="' + data.Value + '">' +
                            data.Text + '</option>');
                    });
                });
                $("#addignedDeveloperDropDownList").removeClass("hidden");
            } else {
                $("#addignedDeveloperDropDownList").addClass("hidden");
            }
        });
    }
};

$(document).ready(function () {
    bugDetails.init();
});