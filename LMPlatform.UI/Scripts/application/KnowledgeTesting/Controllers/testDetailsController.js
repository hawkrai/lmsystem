'use strict';
knowledgeTestingApp.controller('testDetailsCtrl', function ($scope, $http, id, $modalInstance) {

    $http({method: "GET", url: kt.actions.tests.getTest, dataType: 'json', params: { id: id }})
    .success(function(data) {
        $scope.test = data;
    })
    .error(function(data, status, headers, config) {
        alertify.error("Во время получения данных произошла ошибка");
    });
    
    $scope.saveTest = function (testId) {
        alert('saveTest');
    };
    
    $scope.closeDialog = function () {
        $modalInstance.close();
    };
});