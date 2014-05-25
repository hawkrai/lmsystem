var testsResultsList = {
    init: function () {
        $('.groupButton').on('click', $.proxy(this._onGroupClicked, this));
    },
    
    _webServiceUrl: '/TestPassing/',
    _getResultsForGroupMethodName: 'GetTestResultsForGroup',
    

    _onGroupClicked: function(eventArgs) {
        var groupId = $(eventArgs.target).children().first().val();
        this._getResultsForGroup(groupId);
    },
    
    _getResultsForGroup: function (groupId) {
        $.ajax({
            url: this._webServiceUrl + this._getResultsForGroupMethodName,
            type: "GET",
            data: {
                groupId: groupId
            },
            dataType: "html",
            success: $.proxy(this._onResultsTableLoaded, this)
        });
    },
    
    _onResultsTableLoaded: function(content) {
        $('#testReultsTable').html(content);
            var data = {
                'Пройдено тестов': 80,
                'не пройдено тестов': 20,
                'В процессе прохождения': 7
            };

            var line = [];
            for (var propName in data) {
                line.push([propName, data[propName]]);
            }

            var plot1 = jQuery.jqplot('chart1', [line],
              {
                  seriesDefaults: {
                      // Make this a pie chart.
                      renderer: jQuery.jqplot.PieRenderer,
                      rendererOptions: {
                          // Put data labels on the pie slices.
                          // By default, labels show the percentage of the slice.
                          showDataLabels: true,
                          sliceMargin: 5
                      }
                  },
                  grid: {
                      drawGridLines: true,        // wether to draw lines across the grid or not.
                      gridLineColor: '#cccccc',    // *Color of the grid lines.
                      background: '#fafafa',      // CSS color spec for background color of grid.
                      borderWidth: 0,           // pixel width of border around grid.
                      shadow: false,               // draw a shadow for grid.
                      renderer: $.jqplot.CanvasGridRenderer,  // renderer to use to draw the grid.
                      rendererOptions: {}         // options to pass to the renderer.  Note, the default
                      // CanvasGridRenderer takes no additional options.
                  },
                  legend: { show: true, location: 'e' }
              }
            );
    }
};

function initTestResultsList() {
}

$(document).ready(function () {
    testsResultsList.init();
});
