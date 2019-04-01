'use strict';
studentsTestingApp.controller('studentResultsCtrl', function ($scope, $http) {
    $scope.subjectId = getUrlValue('subjectId');

    $scope.init = function() {
        $http({ method: "GET", url: kt.actions.results.getResults, dataType: 'json', params: { subjectId: $scope.subjectId } })
            .success(function(data) {
                $scope.results = data.filter(x => !x.ForSelfStudy);
                $scope.resultsForSelfStudy = data.filter(x => x.ForSelfStudy);
                $scope.drawChartBar();
            })
            .error(function(data, status, headers, config) {
                alertify.error("Во время получения результатов произошла ошибка");
            });
    };
        
        
    $scope.drawChartBar = function () {
        var lines = Enumerable.From($scope.results)
            .Where(function (item) { return item.Points != null; })
            .Select(function (item) { return [item.Title, item.Points]; })
            .ToArray();

        $('#chartBarAverage').html("");
        var plotBar = $('#chartBarAverage').jqplot([lines], {
            title: 'Пройденные тесты',
            seriesColors: ['#007196', '#008cba'],
            seriesDefaults: {
            	renderer: $.jqplot.BarRenderer,
            	rendererOptions: {
            		varyBarColor: true,
            		showDataLabels: true,
            	}
            },
            axes: {
            	xaxis: {
            		renderer: $.jqplot.CategoryAxisRenderer,
            		tickRenderer: $.jqplot.CanvasAxisTickRenderer	
            	},
            	yaxis: {
            		tickOptions: { formatString: '%d&nbsp&nbsp&nbsp' }
            	}
            }
        });
    };
});