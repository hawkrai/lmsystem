$(document).ready(function () {
});

////functions
function initStudentManagement() {
    initManagement(".listButton", "Список предметов", "Изучаемые предметы");
    initManagement(".editButton", "Редактировать", "Редактирование студента");
    initDeleteDialog(".deleteButton", "Удалить", "Удаление студента");
    initStatDialog(".statButton", "Статистика авторизации в систему");
};

function initLecturerManagement() {
    initManagement(".listButton", "Список предметов", "Список предметов");
    initManagement(".editButton", "Редактировать", "Редактирование преподавателя");
    initManagement(".addButton", "", "Добавление преподавателя");
    initDeleteDialog(".deleteButton", "Удалить", "Удаление преподавателя");
    initStatDialog(".statButton", "Статистика авторизации в систему");
};

function initGroupManagement() {
    initManagement(".listButton", "Список группы", "Группа");
    initManagement(".editButton", "Редактировать", "Редактирование группы");
    initManagement(".addButton", "Добавить группу", "Добавление группы");
    initDeleteDialog(".deleteButton", "Удалить", "Удаление группы");
};

function initManagement(btnSelector, btnTooltipTitle, dialogTitle) {
    var btn = $(btnSelector);
    if (btnTooltipTitle) {
        btn.tooltip({ title: btnTooltipTitle, placement: 'right' });
    }

    btn.handle("click", function () {
        var actionUrl = $(this).attr('href');
        showForm(actionUrl, dialogTitle);
        return false;
    });
};

function initStatDialog(btnSelector, btnTooltipTitle) {
    var btn = $(btnSelector);
    btn.tooltip({ title: btnTooltipTitle, placement: 'right' });
    btn.handle("click", function () {
        $('body').append("<div id='chart' style='width: 550px;'></div>");
        var actionUrl = $(this).attr('href');
        $.get(actionUrl, {},
          function (data) {

              var line = [];

              if (data.attendance) {
                  data.attendance.forEach(function (val) {
                      line.push([val.day, val.count]);
                  });
              } else {
                  var today = new Date();
                  line = [[today, 0]];
              }

              var plot = $.jqplot('chart', [line], {
                  title: data.resultMessage,

                  axes: {
                      xaxis: {
  renderer: $.jqplot.DateAxisRenderer,
                          tickRenderer: $.jqplot.CanvasAxisTickRenderer,
                          tickOptions: { angle:-30 ,formatString: "%#d/%#m/%y" }
                      },

                      yaxis: {
                          autoscale: true,
                          min: 0,
                          renderer: $.jqplot.LogAxisRenderer,
                          tickInterval: 1,
                          padMax: 1.5

                      },
                  },
                cursor: {
                      show: true,
                      zoom: true,
                      looseZoom: true,
                      constrainOutsideZoom: false
                  },
  highlighter: {
                      show: true,
                      sizeAdjust: 7.5
                  },
series: [{
                      rendererOptions: {
                      smooth: true
                      },
                      neighborThreshold: 1, color: '#008CBA', lineWidth: 4
                  }]
              });
              

              bootbox.dialog({
                  message: $('#chart'),
                  title: "Статистика авторизации в систему",
                  onEscape: function () {
                      plot.destroy();
                  }
              });
          });
    });
}


function initDeleteDialog(btnSelector, btnTooltipTitle) {
    var btn = $(btnSelector);
    btn.tooltip({ title: btnTooltipTitle, placement: 'right' });

    btn.handle("click", function () {
        var actionUrl = $(this).attr('href');
        bootbox.confirm({
            message: "Вы действительно хотите удалить?",
            title: "Подтверждение удаления",
            buttons: {
                'cancel': {
                    label: "Отмена",
                    className: 'btn btn-sm'
                },
                'confirm': {
                    label: 'Удалить',
                    className: 'btn btn-primary btn-sm'
                }
            },

            callback: function (result) {
                if (result) {
                    $.post(actionUrl, function (data) {
                        updateDataTable();
                        if (data.resultMessage)
                            alertify.success(data.resultMessage);
                    }).fail(function () {
                        alertify.error("Произошла ошибка");
                    });
                }
            }
        });
    });
};

function showForm(formUrl, formTitle) {
    $.ajax({
        url: formUrl,
        cache: false
    }).done(function (data) {
        bootbox.dialog({
            message: data,
            title: formTitle
        });
    });
}

function successAjaxForm(result) {
    var box = $('#adminModal').parents(".modal");
    if (result.resultMessage) {
        $(box).modal('hide');
        updateDataTable();
        alertify.success(result.resultMessage);
    } else {
        $('#adminModal').html(result);
        $('form').removeData('validator');
        $('form').removeData('unobtrusiveValidation');
        $.validator.unobtrusive.parse('form');
    }
}

function updateDataTable(updateTableId) {
    if (updateTableId) {
        $(updateTableId).dataTable().fnDraw();
    } else {
        $(".dataTable").dataTable().fnDraw();
    }
};


