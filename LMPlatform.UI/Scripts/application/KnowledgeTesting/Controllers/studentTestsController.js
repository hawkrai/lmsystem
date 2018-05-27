'use strict';
studentsTestingApp.controller('studentTestsCtrl', function ($scope, $http, $modal) {

    $scope.init = function() {
        $scope.loadTests();
    };
 

    $scope.loadTests = function() {
        var subjectId = getUrlValue('subjectId');

        $http({ method: "GET", url: kt.actions.tests.getTests, dataType: 'json', params: { subjectId: subjectId } })
            .success(function(data) {
                $scope.tests = data.filter(x => !x.ForSelfStudy);
                $scope.testsForSelfStudy = data.filter(x => x.ForSelfStudy);
            })
            .error(function(data, status, headers, config) {
                alertify.error("Во время получения данных произошла ошибка");
            });
    };
});