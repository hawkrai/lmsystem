$(document).ready(function () {
});

////functions
function initStudentManagement() {
    initManagement(".listButton", "Список предметов", "Предмет");
    initManagement(".editButton", "Редактировать", "Редактирование студента");
    initDeleteDialog(".deleteButton", "Удалить", "Удаление студента");
    initStatDialog(".statButton", "Статистика посещаемости");
};

function initLecturerManagement() {
    initManagement(".listButton", "Список группы", "Группа");
    initManagement(".editButton", "Редактировать", "Редактирование преподавателя");
    initManagement(".addButton", "", "Добавление преподавателя");
    initDeleteDialog(".deleteButton", "Удалить", "Удаление преподавателя");
    initStatDialog(".statButton", "Статистика посещаемости");
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
                          renderer: $.jqplot.DateAxisRenderer
                      },

                      yaxis: {
                          min: 0,
                          tickInterval: 1,
                          padMax: 1.5

                      },
                  },
  series: [{
                      seriesColors: ["#4bb2c5", "#EAA228", "#c5b47f", "#579575", "#839557", "#958c12", "#953579", "#4b5de4", "#d8b83f", "#ff5800", "#0085cc", "#c747a3", "#cddf54", "#FBD178", "#26B4E3", "#bd70c7"],
                      padding: 20,
                      sliceMargin: 0,
                      fill: true,
                      shadow: true,
                      startAngle: 0,
                      lineWidth: 2.5,
                      color: '#008CBA',
                      highlightColors: ["rgb(129,201,214)", "rgb(240,189,104)", "rgb(214,202,165)", "rgb(137,180,158)", "rgb(168,180,137)", "rgb(180,174,89)", "rgb(180,113,161)", "rgb(129,141,236)", "rgb(227,205,120)", "rgb(255,138,76)", "rgb(76,169,219)", "rgb(215,126,190)", "rgb(220,232,135)", "rgb(200,167,96)", "rgb(103,202,235)", "rgb(208,154,215)"]
                  }]
              });

              bootbox.dialog({
                  message: $('#chart'),
                  title: "Статистика посещаемости",
                  onEscape: function () {
                      plot.destroy();
                  },
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


