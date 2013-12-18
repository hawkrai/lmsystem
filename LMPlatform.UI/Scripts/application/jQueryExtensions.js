$.extend({
	isIE: function () {
		var myNav = navigator.userAgent.toLowerCase();
		return (myNav.indexOf('msie') != -1) ? parseInt(myNav.split('msie')[1]) : false;
	},
	generateUniqID: function (idlength) {
		var charstoformid = '_0123456789ABCDEFGHIJKLMNOPQRSTUVWXTZabcdefghiklmnopqrstuvwxyz'.split('');
		if (idlength == null) {
			idlength = 5;
		}
		var uniqid = '';
		for (var i = 0; i < idlength; i++) {
			uniqid += charstoformid[Math.floor(Math.random() * charstoformid.length)];
		}

		return uniqid;
	},
	existsObject: function (chekingObject) {
		return chekingObject != null && chekingObject.length > 0;
	},
	executeFunctionByName: function (functionName) {
		var context = window;
		var args = Array.prototype.slice.call(arguments, 2);
		var namespaces = functionName.split(".");
		var func = namespaces.pop();
		for (var i = 0; i < namespaces.length; i++) {
			context = context[namespaces[i]];
		}
		return context[func].apply(context, args);
	},
	savingDialog: function (header, url, data, color, saveCallback) {
	    var that = this;
	    var dialogId = that.generateUniqID();
	    if (color == null) {
	        color = "primary";
	    }
	    $("body").append("<div id=\"" + dialogId + "\" class=\"modal\"></div>");
	    var dialogContainer = $("#" + dialogId);
	    
	    var bootstrapDialogMarkup = "<div class=\"modal-dialog panel panel-" + color + "\" style=\"padding:0px\">" +
	        "<div class=\"panel-heading\">" +
	        "<button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\">&times;</button>" +
	        "<h4 class=\"modal-title\" id=\"myModalLabel\">" + header + "</h4>" +
	        "</div>" +
	        "<div class=\"modal-body\">" +
	        "</div>" +
	        " <div class=\"modal-footer\">" +
	        "<a href=\"#\" class=\"btn btn-" + color + " btn-sm\" id=\"cancelButton\" data-dismiss=\"modal\">Отменить</a>" +
	        "<a href=\"#\" class=\"btn btn-" + color + " btn-sm\" id=\"saveButton\" data-dismiss=\"modal\">Сохранить</a></div>" +
	        "</div>" +
	        "</dvi>";
	    
	    $(dialogContainer).html(bootstrapDialogMarkup);
	    
		$("#" + dialogId + " .modal-body").load(url, data, function () {
			var form = $($.elementId(dialogId) + " .modal-body").find("form");

			$.validator.unobtrusive.parse(form);
			
			$(document).on("click","#saveButton", function () {
				if ($(form).valid()) {
					$.post($(form).attr("action"), $(form).serialize(), function(result) {
						saveCallback(result);
						that.closeDialog(dialogId);
						var element = $("#" + dialogId);
						if ($(element).hasClass("modal")) {
						    $("#" + dialogId).removeData('modal');
						    $("#" + dialogId).remove();
						}
					});
				}
				return false;
			});

			$(form).submit(function() {
				if ($(form).valid()) {
					$.post($(form).attr("action"), $(form).serialize(), function (result) {
						saveCallback(result);
						that.closeDialog(dialogId);
						var element = $("#" + dialogId);
						if ($(element).hasClass("modal")) {
						    $("#" + dialogId).removeData('modal');
						    $("#" + dialogId).remove();
						}
					});
				}
				return false;
			});
		});

		$("#" + dialogId).on('hiden', function (element) {
			if ($(element.target).hasClass("modal")) {
			    $("#" + dialogId).removeData('modal');
			    $("#" + dialogId).remove();
			}
		});

		$("#" + dialogId).on('shown', function(element) {
			setTimeout(function () { $(element.target).find("input[type=text]").focus(); }, 1);
		});

		$(dialogContainer).modal();

		return dialogId;
	},
	closeDialog: function (dialogId) {
	    $("#" + dialogId).modal('hide');
	},
	elementId: function(id) {
		return "#" + id;
	},
	showValidationErrorMessage: function (element, isValid) {
		var validatingElements = [];

		if (element != null) {
			validatingElements.push(element);;
		} else {
			validatingElements = $("body .input-validation-error");
		}

		$.each(validatingElements, function (index, validatingElement) {
		    $(validatingElement).tooltip('destroy');
			if (!isValid || $(validatingElement).hasClass(".input-validation-error")) {
				var validationMessage = $(element).attr($(validatingElement).getValidationAttribute());
				$(validatingElement).tooltip({title: validationMessage, placement: 'right' });
			}
		});
	}
});
$.fn.extend({
	handle: function (eventName, handler) {
		$(this).off(eventName).on(eventName, handler);
	},
	getValidationAttribute: function () {
		var result = [];
		$(this[0].attributes).each(function (index, attribute) {
			if (attribute.name.match("^data-val-")) {
				result.push(attribute.name);
			}
		});

		return result[0];
	},
	showWaiting: function (options) {
		$(this[0]).waitingIndicator(options);
		var waitingIndicator = $(this[0]).data("omertex-waitingIndicator");
		waitingIndicator.show(true);
	},
	hideWaiting: function () {
		var waitingIndicator = $(this[0]).data("omertex-waitingIndicator");
		waitingIndicator.hide(true);
	},
	addCloseButtonInAlert: function () {
	    var closeButton = $('<a href="#"><i class="icon-white icon-remove close"></i></a>');
		closeButton.click(function (e) {
			e.preventDefault();
			$(this).parent('div').slideUp(function () {
				$(this).hide();
				shared.bodyHeightChanged();
			});
		});

		$(this[0]).prepend(closeButton);
	}
});