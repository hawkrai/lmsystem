var dataTables = {
	createDataTable: function (id, sAjaxSource, pagination, filter, sort, info, drawCallbackName,invisibleColumns) {
		var $table = $('#' + id);
		var oTable = $table.dataTable({
			"sPaginationType": "bootstrap",
			"bServerSide": true,
			"bStateSave": false,
			"bPaginate": JSON.parse(pagination),
			"bSort": JSON.parse(sort),
			"bInfo": JSON.parse(info),
			"bProcessing": true,
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
			"fnDrawCallback": function () {
				$(".dataTables_wrapper").css("margin-bottom", 50);
			
				shared.setFooter();
			},
			"fnInitComplete":function() {
			    dataTables.hideColumns(id, invisibleColumns);
			    if (drawCallbackName != null && drawCallbackName != "") {
			        $.executeFunctionByName(drawCallbackName);
			    }
			}
		});
		
		return oTable;
	},
	
	hideColumns: function (tableId, invisibleColumns) {
		var dataTable = $("#" + tableId).dataTable({ "bRetrieve": true });

		$.each(invisibleColumns, function(index,value) {
			dataTable.fnSetColumnVis(value, false, true);
		});
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
