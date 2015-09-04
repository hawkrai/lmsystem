var shared = {
	bodyHeight: 0,
	refreshInterval: null,
	countTrying: 0,

	setFooter: function () {
	    if ($(window).scrollTop() == 0) {
	        $(window).scrollTop(1);
	        var scrollTop = $(window).scrollTop();
	        if (scrollTop > 0) {
	            $('footer').css("position", "relative");
	        } else {
	            $('footer').css("position", "fixed");
	        }
	    } else {
	        scrollTop = $(window).scrollTop();
	        if (scrollTop > 0) {
	            $('footer').css("position", "relative");
	        } else {
	            $('footer').css("position", "fixed");
	        }
	    }
	},

	init: function () {
		this.setFooter();
		this.initTooltipButtons();
		this.applyCss();
		this.initPlugins();

		this.bodyHeight = $("body").height();

		this.initEventHandlers();


	},

	initEventHandlers: function () {
		var that = this;
		$(document).on('click', function () {
			that.countTrying = 0;
			that.refreshInterval = setInterval("shared.bodyHeightChanged();", 50);
		});

		//$(document).ajaxError(function (e, jqxhr, settings) {
		//	e.stopPropagation();
		//	var response = { message: "В результате выполнения запроса возникла ошибка. Обновите страницу и свяжитесь с администратором." };
		//	if (jqxhr.responseText != null) {
		//	    try {
		//	        response = JSON.parse(jqxhr.responseText);
		//	    }
		//	    catch (e) {
		//	        $("body").html(jqxhr.responseText);
		//	    }
		//	}

		//	bootbox.dialog(response.message, [{
		//		"label": "ОК",
		//		"class": "btn btn-primary",
		//	}], { "header": "Ошибка в результате запроса." });
		//});
	},

	applyCss: function () {
	},

	errorAlert: function () {
		$('.alert-error').show();
	},

	initTooltipButtons: function () {
		var tooltipButtons = $("body button.tooltip-button");
		$("body .required-marker").tooltip();

		tooltipButtons.each(function (index, button) {
			$(button).tooltip({ title: $(button).parent("div").find(".helperMessage").text(), placement: 'right' });
		});
	},

	initPlugins: function (container) {
		if (container == null) {
			container = $('body');
		}
		container.find(".datetimepicker").each(function (index, value) {
			var dateFormat = $(value).attr('data-date-format') != "" ? $(value).attr('data-date-format') : "dd.mm.yyyy";
			var viewMode = dateFormat == "yyyy" ? "years" : "days";

			$(value).datepicker({
				format: $(value).attr('data-date-format') != "" ? $(value).attr('data-date-format') : "dd.mm.yyyy",
				language: "ru",
				autoclose: true,
				viewMode: viewMode,
				minViewMode: viewMode
			});
		});
	    
		container.find(".multiselect").each(function (index, element) {
		    $(element).multiselect({
				buttonClass: 'btn',
				buttonWidth: '100%',
				buttonContainer: '<div class="btn-group" style="width:100%" />',
				maxHeight: 200,
				buttonText: function (options) {
					if (options.length == 0) {
						return "Ничего не выбрано <b class=\"caret\"></b>";
					} else if (options.length > 3) {
						return options.length + " Выбрано  <b class=\"caret\"></b>";
					} else {
						var selected = '';
						options.each(function () {
							selected += $(this).text() + ', ';
						});
						return selected.substr(0, selected.length - 2) + " <b class=\"caret\"></b>";
					}
				}
			});
		});
	},

	bodyHeightChanged: function () {
		var currentHeight = $("body").height();

		if (currentHeight != shared.bodyHeight) {
			this.setFooter();
			clearInterval(shared.refreshInterval);
			this.bodyHeight = currentHeight;
			this.countTrying = 0;
		} else {
			this.countTrying++;
			if (this.countTrying == 10) {
				clearInterval(shared.refreshInterval);
				this.countTrying = 0;
			}
		}
	}
};

function getUrlValue(varSearch) {
    var searchString = window.location.search.substring(1);
    var variableArray = searchString.split('&');
    for (var i = 0; i < variableArray.length; i++) {
        var keyValuePair = variableArray[i].split('=');
        if (keyValuePair[0] == varSearch) {
            return keyValuePair[1];
        }
    }
}

function getHashValue(varSearch) {
    var searchString = window.location.hash.substring(window.location.hash.indexOf('?') + 1);
    var variableArray = searchString.split('&');
    for (var i = 0; i < variableArray.length; i++) {
        var keyValuePair = variableArray[i].split('=');
        if (keyValuePair[0] == varSearch) {
            return keyValuePair[1];
        }
    }
}

$(document).ready(function () {
	shared.init();
});