'use strict';
knowledgeTestingApp.controller('testDetailsCtrl', function ($scope, $http, id, $modalInstance) {

    $http({ method: 'GET', url: kt.actions.tests.getTest, dataType: 'json', params: { id: id } })
    .success(function (data) {
        $scope.test = data;
    })
    .error(function (data, status, headers, config) {
        alertify.error('Во время получения данных произошла ошибка');
    });

    $scope.saveTest = function () {
        $scope.test.SubjectId = getUrlValue('subjectId');
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