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

	    $("#avatar-load").off("click").on("click", function () {
	    	//$("input[type=file][name=avatar-file-container]").val("");
	    	$("input[type=file][name=avatar-file-container]").click();
	    });

	    $("input[type=file][name=avatar-file-container]").off("change").on("change", function () {

	    	var formData = new FormData();

	    	formData.append("file", $("input[type=file][name=avatar-file-container]")[0].files[0]);

	    	$.ajax({
	    		url: "/Account/GenerateBase64",
	    		type: "POST",
	    		data: formData,
	    		processData: false,
	    		contentType: false,
	    		success: function (data) {
	    			$("#avatarContainer").attr("src", data);
	    			$("#avatar").val($("#avatarContainer").attr("src"));
	    		},
	    	});
	    });

    },
};

$(document).ready(function () {
    accountManagement.init();
});