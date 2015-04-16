'use strict';
knowledgeTestingApp.controller('resultsCtrl', function ($scope, $http) {
    $scope.subjectId = getUrlValue('subjectId');

    $http({ method: "GET", url: kt.actions.groups.getGroupsForSubject, dataType: 'json', params: { subjectId: $scope.subjectId } })
            .success(function (data) {
                $scope.groups = data;
                if (data.length > 0) {
                    $scope.loadResults(data[0].Id);
                }
            })
            .error(function (data, status, headers, config) {
                alertify.error("Во время получения данных о группах произошла ошибка");
            });

    $scope.loadResults = function (groupId) {
        $scope.gropId = groupId;
        $http({ method: "GET", url: kt.actions.results.getResults, dataType: 'json', params: { subjectId: $scope.subjectId, groupId: groupId} })
           .success(function (data) {
               $scope.results = data;
               $scope.drawChartBar();
           })
           .error(function (data, status, headers, config) {
               alertify.error("Во время получения результатов произошла ошибка");
           });
    };

    $scope.drawChartBar = function() {
        var lines = Enumerable.From($scope.results)
            .Where(function (item) { return $scope.calcOverage(item, true) != null; })
            .OrderByDescending(function(item) { return $scope.calcOverage(item); })
            .Select(function(item) {return  [item.StudentName, $scope.calcOverage(item)];})
            .ToArray();

        $('#chartBarAverage').html("");
        var plotBar = $('#chartBarAverage').jqplot([lines], {
            animate: !$.jqplot.use_excanvas,
            title: 'Рейтинг студентов',
            seriesColors: ['#007196', '#008cba'],
            seriesDefaults: {
                renderer: jQuery.jqplot.BarRenderer,
                rendererOptions: {
                    varyBarColor: true,
                    showDataLabels: true,
                }
            },
            axes: {
                xaxis: {
                    renderer: $.jqplot.CategoryAxisRenderer,
                    tickRenderer: $.jqplot.CanvasAxisTickRenderer,
                    tickOptions: {
                        angle: lines.length > 3 ? -90 : 0,
                    }
                }
            }
        });
    };

    $scope.calcOverage = function (result, dontUseTestResult) {
        var results = result.TestPassResults;
        var empty = dontUseTestResult ? null : 'Студент не прошел ни одного теста';

        if (results.length == 0) {
            return empty;
        }

        var passed = Enumerable.From(results).Where(function (item) {
             return item.Points != null;
        });

        if (passed.Count() > 0) {
            return passed.Sum(function (item) {
                return item.Points;
            }) / passed.Count();
        } else {
            return empty;
        }
    };
});