'use strict';
knowledgeTestingApp.controller('controlCtrl', function ($scope, $http, $modal) {

    $scope.subjectId = getUrlValue('subjectId');

    $http({ method: "GET", url: kt.actions.results.getControl, dataType: 'json', params: { subjectId: $scope.subjectId } })
        .success(function (data) {
            $scope.items = data;
            $scope.repeat();
        })
        .error(function (data, status, headers, config) {
            alertify.error("Во время получения данных произошла ошибка");
        });

    //$scope.repeat = function() {
    //    window.setInterval(function() {

    //        $http({ method: "GET", url: kt.actions.results.getControl, dataType: 'json', params: { subjectId: $scope.subjectId } })
    //            .success(function(data) {
    //                $scope.items = data;
    //                $scope.repeat();
    //            })
    //            .error(function(data, status, headers, config) {
    //                alertify.error("Во время получения данных произошла ошибка");
    //            });
    //    }, 1000);
    //};
});