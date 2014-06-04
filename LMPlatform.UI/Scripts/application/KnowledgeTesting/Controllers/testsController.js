'use strict';
knowledgeTestingApp.controller('testsCtrl', function ($scope, $http) {

    var subjectId = getUrlValue('subjectId');

    $http({
        method: "GET",
        url: kt.actions.tests.getTests,
        dataType: 'json',
        params: { subjectId: subjectId }
    })
    .success(function (data) {
        $scope.tests = data;
    })
    .error(function (data, status, headers, config) {
        alertify.error("Во время получения данных произошла ошибка");
    });;

});