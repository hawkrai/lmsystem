'use strict';
knowledgeTestingApp.controller('testDetailsCtrl', function ($scope, $http, id, $modalInstance) {

    $http({ method: 'GET', url: kt.actions.tests.getTest, dataType: 'json', params: { id: id } })
    .success(function (data) {
    	$scope.test = data;
    	$scope.testType = 0;

    	$scope.testType = $scope.test.ForSelfStudy === true ? 1 : $scope.test.BeforeEUMK ? 2 : $scope.test.ForEUMK ? 3 : $scope.test.ForNN ? 4 : 0;
    })
    .error(function (data, status, headers, config) {
        alertify.error('Во время получения данных произошла ошибка');
    });

    $scope.saveTest = function () {
        $scope.test.SubjectId = getUrlValue('subjectId');
		
        $scope.test.ForSelfStudy = $scope.testType === 1 ? true : false;
        $scope.test.BeforeEUMK = $scope.testType === 2 ? true : false;
        $scope.test.ForEUMK = $scope.testType === 3 ? true : false;
        $scope.test.ForNN = $scope.testType === 4 ? true : false;

        $http({ method: "POST", url: kt.actions.tests.saveTest, dataType: 'json', params: $scope.test })
        .success(function (data) {
            if (data.ErrorMessage) {
                alertify.error(data.ErrorMessage);
            } else {
                $scope.loadTests();
                $scope.closeDialog();
                alertify.success('Тест успешно сохранен');
            }
        })
        .error(function (data, status, headers, config) {
            alertify.error('Во время сохранения произошла ошибка');
        });
    };

    $scope.closeDialog = function () {
        $modalInstance.close();
    };
    
    function aveTest(test) {
        
    }
});