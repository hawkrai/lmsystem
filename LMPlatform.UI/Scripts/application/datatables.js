var dataTables = {
	createDataTable: function (id, sAjaxSource, pagination, filter, sort, info, drawCallbackName, displayLength) {
		var $table = $('#' + id);
		var oTable = $table.dataTable({
			"sPaginationType": "bootstrap",
			"bServerSide": true,
			"bStateSave": false,
			"bPaginate": JSON.parse(pagination),
			"bSort": JSON.parse(sort),
			"bInfo": JSON.parse(info),
			"bProcessing": true,
			'iDisplayLength': displayLength,
			'bLengthChange': true,
			"oLanguage": {
				"oPaginate": {
					"sPrevious": "Предыдущая ",
					"sNext": "Следующая",
					"sLast": "Последняя",
					"sFirst": "Первая"
				},
				"sProcessing": "<div class=\"fill-partial mini center\">",
				"sSearch": "Поиск",
				"sLengthMenu": 'Отображать <select>' +
					'<option value="5">5</option>' +
					'<option value="10">10</option>' +
					'<option value="20">20</option>' +
					'<option value="30">30</option>' +
					'<option value="40">40</option>' +
					'<option value="-1">Все</option>' +
					'</select> записей',
				"sInfo": "Отображается _START_ - _END_ из _TOTAL_ записей",
				"sInfoEmpty": "Отображается 0 записей из 0",
				"sInfoEmpty ": "Нет данных для отображения",
				"sEmptyTable": "Ничего не найдено"
			},
			"bFilter": JSON.parse(filter),
			"sDom": '<f>rt<"bottom"ilp><"clear">',
			"aLengthMenu": [[5, 10, 20, 30, 40, -1], [5, 10, 20, 30, 40, "Все"]],
			"bAutoWidth": false,
			"sAjaxSource": sAjaxSource,
			"fnServerData": function (sSource, aoData, fnCallback) {
				$.ajax({
					"dataType": 'json',
					"type": "POST",
					"url": sSource,
					"data": aoData,
					"success": fnCallback
				});
            },
			"fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
			    if (aData[5] == "Удален") {
			        $('td', nRow).css('background-color', '#FDC1C1');
			    }
			},
			"fnDrawCallback": function () {
				$(".dataTables_wrapper").css("margin-bottom", 50);
			    
				if (drawCallbackName != null && drawCallbackName != "") {
				    $.executeFunctionByName(drawCallbackName);
				}

				shared.setFooter();
			},
			"fnInitComplete": function (oSettings, json) {
			    var currentId = $(this).attr('id');
			    console.log(currentId);
			    if (currentId) {

			        var thisLength = $('#' + currentId + '_length');
			        var thisLengthLabel = $('#' + currentId + '_length');
			        var thisLengthSelect = $('#' + currentId + '_length select');

			        var thisFilter = $('#' + currentId + '_filter');
			        var thisFilterLabel = $('#' + currentId + '_filter label');
			        var thisFilterInput = $('#' + currentId + '_filter label input');

			        // Re-arrange the records selection for a form-horizontal layout
			        thisLength.addClass('form-group');
			        thisLengthSelect.addClass('form-control input-sm').attr('id', currentId + '_length_select');
			        $(thisLengthLabel).attr('style', 'float:left');
			        // Re-arrange the search input for a form-horizontal layout
			        thisFilter.addClass('form-group');
			        thisFilterInput.addClass('form-control  input-group-sm').attr('id', currentId + '_filter_input');
			    }
			}

		});
		
		return oTable;
	},

	deleteRow: function (tableId, url) {
		var dataTable = $("#" + tableId).dataTable({ "bRetrieve": true });
	    var that = this;
		$.ajax({
			type: "POST",
			url: url,
			success: function () {
			    var row = $(that).closest('tr');
				dataTable.fnDeleteRow(row);
			}
		});
	}
};
