'use strict';
studentsTestingApp.controller('studentResultsCtrl', function ($scope, $http) {
    $scope.subjectId = getUrlValue('subjectId');

    $scope.init = function() {
        $http({ method: "GET", url: kt.actions.results.getResults, dataType: 'json', params: { subjectId: $scope.subjectId } })
            .success(function(data) {
            	$scope.resultPlot = data.filter(x => x.ForSelfStudy || x.ForNN || (!x.BeforeEUMK && !x.ForEUMK));
            	$scope.results = data.filter(x => !x.ForSelfStudy && !x.ForNN && !x.BeforeEUMK && !x.ForEUMK);
            	$scope.resultsForSelfStudy = data.filter(x => x.ForSelfStudy);
		        $scope.resultsForNN = data.filter(x => x.ForNN);
		        $scope.drawChartBar();
	        })
            .error(function(data, status, headers, config) {
                alertify.error("Во время получения результатов произошла ошибка");
            });
    };
        
        
    $scope.drawChartBar = function () {
    	var lines = Enumerable.From($scope.resultPlot)
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
	$scope.drawChartBar1 = function () {
		var lines = Enumerable.From($scope.resultsForSelfStudy)
			.Where(function (item) { return item.Points != null; })
			.Select(function (item) { return [item.Title, item.Points]; })
			.ToArray();

		$('#chartBarAverage1').html("");
		var plotBar = $('#chartBarAverage1').jqplot([lines], {
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
	$scope.drawChartBar2 = function () {
		var lines = Enumerable.From($scope.resultsForNN)
			.Where(function (item) { return item.Points != null; })
			.Select(function (item) { return [item.Title, item.Points]; })
			.ToArray();

		$('#chartBarAverage2').html("");
		var plotBar = $('#chartBarAverage2').jqplot([lines], {
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