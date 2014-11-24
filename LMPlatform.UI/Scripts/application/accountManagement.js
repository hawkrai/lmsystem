var accountManagement = {
    
    init: function () {
        var that = this;
        that.activeLinkHandle();

        if ($("#avatar").val() != '' && $("#avatar").val() != null) {
        	$("#avatarContainer").attr("src", $("#avatar").val());
        }
    },
    
    activeLinkHandle: function () {
        $("#personalData").handle('click', function () {
            var that = this;
            $(that).addClass("active");
            $.post('/Account/PersonalData', null, function (data) {
                $('#privateData').empty();
                $('#privateData').append(data);
                accountManagement.activeLinkHandle();
            });
            return false;
        });

        $('#updatePersonalData').handle('click', function() {
            var that = this;
            $('#privateData').spin('large');
            $.post('/Account/UpdatePerconalData', $('form[name=personalData]').serialize(), function (data) {
            
                if (data == true) {
                    $('#privateData').find('.spinner').each(function () {
                        $(this).remove();
                    });
                    alertify.success("Персональные данные изменены");
                }
            });
            return false;
        });

	    $(".tempAvatar").on("click", function() {
	    	var that = this;
	    	$("#avatarContainer").attr("src", $(that).attr("src"));
		    $("#avatar").val($("#avatarContainer").attr("src"));
	    });

    },
};

$(document).ready(function () {
    accountManagement.init();
});