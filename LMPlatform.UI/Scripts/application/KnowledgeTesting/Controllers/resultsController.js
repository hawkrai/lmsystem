'use strict';
knowledgeTestingApp.controller('resultsCtrl', function ($scope, $http) {
    $scope.subjectId = getUrlValue('subjectId');
    $scope.forSelfStudyFilter = function (item) {
        return item.ForSelfStudy;
    }

    $http({ method: "GET", url: kt.actions.groups.getGroupsForSubject, dataType: 'json', params: { subjectId: $scope.subjectId } })
            .success(function (data) {
                $scope.groups = data;
                if (data.length > 0) {
                    $scope.loadResults(data[0].Id);
                    $scope.selectedGroup = $scope.groups[0];
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
               $scope.subgroupsResults = [];
               if ($scope.results.some(x => x.SubGroup === "first")) {
                   $scope.subgroupsResults[0] = $scope.results.filter(x => x.SubGroup == "first");
               }
               if ($scope.results.some(x => x.SubGroup === "second")) {
                   $scope.subgroupsResults[1] = $scope.results.filter(x => x.SubGroup == "second");
               }
               if ($scope.results.some(x => x.SubGroup === "third")) {
                   $scope.subgroupsResults[2] = $scope.results.filter(x => x.SubGroup == "third");
               }
               $scope.drawChartBar();
           })
           .error(function (data, status, headers, config) {
               alertify.error("Во время получения результатов произошла ошибка");
           });
    };

    $scope.getFilteredTestsNumber = function(type, subgroupResults, res) {
    	var testsFiltered = [];
    	switch (type) {
    		case 0:
    			testsFiltered = subgroupResults[0].TestPassResults.filter(x => x.ForSelfStudy == false)
    			break;
    		case 1:
    			testsFiltered = subgroupResults[0].TestPassResults.filter(x => x.ForSelfStudy == true)
    			break;
    		case 2:
    			break;
    		case 3:
    			break;
    		case 4:
    			break;
    	}

    	return testsFiltered.indexOf(res) + 1;
    };

    $scope.drawChartBar = function() {
        var lines = Enumerable.From($scope.results)
            .Where(function (item) { return $scope.calcOverage(item, true) != null; })
            .OrderByDescending(function(item) { return $scope.calcOverage(item); })
            .Select(function(item) {return  [item.StudentShortName, $scope.calcOverage(item)];})
            .ToArray();

        $('#chartBarAverage').html("");
        var plotBar = $('#chartBarAverage').jqplot([lines], {
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
                },
                yaxis: {
                    tickOptions: { formatString: '%d&nbsp&nbsp&nbsp' }
                }
            }
        });

        //var lines2 = Enumerable.From($scope.results)
        //    .Where(function (item) { return $scope.calcOverageForSelf(item, true) != null; })
        //    .OrderByDescending(function (item) { return $scope.calcOverageForSelf(item); })
        //    .Select(function (item) { return [item.StudentShortName, $scope.calcOverageForSelf(item)]; })
        //    .ToArray();

        //$('#chartBarAverage2').html("");
        //var plotBar = $('#chartBarAverage2').jqplot([lines2], {
        //    title: 'Рейтинг студентов (для самоконтроля)',
        //    seriesColors: ['#007196', '#008cba'],
        //    seriesDefaults: {
        //        renderer: jQuery.jqplot.BarRenderer,
        //        rendererOptions: {
        //            varyBarColor: true,
        //            showDataLabels: true,
        //        }
        //    },
        //    axes: {
        //        xaxis: {
        //            renderer: $.jqplot.CategoryAxisRenderer,
        //            tickRenderer: $.jqplot.CanvasAxisTickRenderer,
        //            tickOptions: {
        //                angle: lines2.length > 3 ? -90 : 0,
        //            }
        //        },
        //        yaxis: {
        //            tickOptions: { formatString: '%d&nbsp&nbsp&nbsp' }
        //        }
        //    }
        //});
    };

    $scope.calcOverage = function (result, dontUseTestResult) {
        var results = result.TestPassResults;
        var empty = dontUseTestResult ? null : 'Студент не прошел ни одного теста';

        if (results.length == 0) {
            return empty;
        }

        var passed = Enumerable.From(results).Where(function (item) {
             return item.Points != null && !item.ForSelfStudy;
        });

        var passedPercent = Enumerable.From(results).Where(function (item) {
            return item.Percent != null;
        });

        if (passed.Count() > 0) {

            var markSum = passed.Sum(function(item) {
                return item.Points;
            }) / passed.Count();

            var percentSum = passedPercent.Sum(function (item) {
                return item.Percent;
            }) / passedPercent.Count();

            return Math.round(markSum);// + " (" + Math.round(percentSum) + "%)";
        } else {
            return empty;
        }
    };

    $scope.calcOverageForSelf = function (result, dontUseTestResult) {
        var results = result.TestPassResults;
        var empty = dontUseTestResult ? null : 'Студент не прошел ни одного теста';

        if (results.length == 0) {
            return empty;
        }

        var passed = Enumerable.From(results).Where(function (item) {
            return item.Points != null && item.ForSelfStudy;
        });

        var passedPercent = Enumerable.From(results).Where(function (item) {
            return item.Percent != null;
        });

        if (passed.Count() > 0) {

            var markSum = passed.Sum(function (item) {
                return item.Points;
            }) / passed.Count();

            var percentSum = passedPercent.Sum(function (item) {
                return item.Percent;
            }) / passedPercent.Count();

            return Math.round(markSum);// + " (" + Math.round(percentSum) + "%)";
        } else {
            return empty;
        }
    };

    $scope.calcTotal = function(result, index) {
        var sum = 0;
        var count = 0;
        
        $.each(result, function (key, value) {
            if (value.TestPassResults[index].Points != null) {
                count = count + 1;
                sum = sum + value.TestPassResults[index].Percent;
            }
        });

        var percent = Math.round(sum / count);

        return {
            Percent: Math.round(sum / count),
            Point: Math.round(percent / 10),
        };
    };

    $scope.loadAnswers = function (sId, tId) {
        $http({ method: "GET", url: kt.actions.results.getUserAnswers, dataType: 'json', params: { studentId: sId, testId: tId } })
            .success(function (data) {
                var str = "";
                for (var i = 0; i < data.length; i++) {
                    str += "Вопрос" + data[i].Number + ": " + data[i].QuestionTitle;
                    str += "\n";
                    if (data[i].QuestionDescription !== null) {
                        str += "Задание к вопросу: " + data[i].QuestionDescription;
                        str += "\n";
                    }
                    str += "Ответ: " + data[i].AnswerString;
                    str += "\n";

                    if (data[i].Points == 1) {
                        str += "Верно";
                    }
                    else {
                        str += "Неверно";
                    }
                    str += "\n";
                    str += "\n";
                }
                alert(str);
            })
            .error(function (data, status, headers, config) {
                alertify.error("Во время получения данных об ответах произошла ошибка");
            });
    };

    $scope.resultExport = function() {
        window.location.href = "/TestPassing/GetResultsExcel?groupId=" + $scope.gropId + "&subjectId=" + $scope.subjectId + "&forSelfStudy=false";
    };

    $scope.resultExport2 = function () {
        window.location.href = "/TestPassing/GetResultsExcel?groupId=" + $scope.gropId + "&subjectId=" + $scope.subjectId + "&forSelfStudy=true";
    };

    $scope.selectedGroup = null;
});