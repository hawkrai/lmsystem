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
                } else {
	                var form =
		                '<div>' +
	                	'<ul>';
				
				for (var i = 0; i < data.length; i++) {
					form += '<li style="color:red">' + data[i] + '</li>';
				}

				form += '</ul>' +
					'</div>';

                	bootbox.dialog({
                		message: form,
                		backdrop: true,
                		keyboard: true,
                		title: "Ошибки",
                		buttons: {
                			main: {
                				label: "ОК",
                				className: "btn-primary btn-submit btn-sm",
                				callback: function () {
                					$('#privateData').find('.spinner').each(function () {
                						$(this).remove();
                					});
                				}
                			}
                		}
                	});
                }
            });
            return false;
        });

        $('#changePassword').handle('click', function () {
        	var that = this;

	        $("#changePasswordForm").show();
	        $("#errorForm").text("");

        	return false;
        });

        $("#savePassword").handle('click', function () {
        	var that = this;
        	$("#errorForm").text("");
        	if ($("#newPassword").val() !== $("#newPassword2").val()) {
		        $("#errorForm").text("Новый пароль и пароль подтверждения не совпадают.");
        		return false;
	        }

        	$.ajax({
        		url: "/Account/SavePassword",
        		type: "POST",
        		data: JSON.stringify({
        			"old": $("#oldpassword").val(),
        			"newPassword": $("#newPassword").val(),
        			"newPassword2": $("#newPassword2").val()
        		}),
        		contentType: "application/json; charset=utf-8",
        		dataType: "json",
        		success: function (data) {
			        $("#oldpassword").val("");
				    $("#newPassword").val("");
				    $("#newPassword2").val("");

        			if (data === false) {
				        $("#errorForm").text("Пароль не был изменен");
        			} else {
        				$("#errorForm").text("");
        				$("#changePasswordForm").hide();
        				alertify.success("Пароль успешно изменен.");
			        }
        		},
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