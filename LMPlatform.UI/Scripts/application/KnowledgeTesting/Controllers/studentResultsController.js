'use strict';
studentsTestingApp.controller('studentResultsCtrl', function ($scope, $http) {
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
           })
           .error(function (data, status, headers, config) {
               alertify.error("Во время получения результатов произошла ошибка");
           });
    };

    $scope.calcOverage = function (results) {
        var empty = 'Студент не прошел ни одного теста';

        if (results.length == 0) {
            return empty;
        }

        var passed = Enumerable.From(results).Where(function (item) {
             return item.Points > 0;
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