'use strict';
knowledgeTestingApp.controller('testsCtrl', function ($scope, $http, $modal) {

    $scope.init = function() {
        $scope.loadTests();
    };

    $scope.onEditButtonClicked = function (testId) {
        loadTest(testId);
    };
    

    $scope.onUnlockButtonClicked = function (testId) {
        var modalInstance = $modal.open({
            scope: $scope,
            templateUrl: '/Content/KnowledgeTesting/testUnlocks.html',
            controller: 'testUnlocksCtrl',
            resolve: {
                id: function () {
                    return testId;
                }
            }
        });
    };

    $scope.onDeleteButtonClicked = function (testId, testName) {
        var context = {
            id: testId
        };

        bootbox.confirm({
            title: 'Удаление теста',
            message: 'Вы дествительно хотите удалить тест "' + testName + '"?',
            buttons: {
                'cancel': {
                    label: 'Отмена',
                    className: 'btn btn-primary btn-sm'
                },
                'confirm': {
                    label: 'Удалить',
                    className: 'btn btn-primary btn-sm',
                }
            },
            callback: $.proxy($scope.onDeleteConfirmed, context)
        });
    };

    $scope.onDeleteConfirmed = function (result) {
        if (result) {
            $http({ method: "DELETE", url: kt.actions.tests.deleteTest, dataType: 'json', params: { id: this.id } })
                .success(function() {
                    $scope.loadTests();
                    alertify.success('Тест успешно удалён');
                })
                .error(function() {
                    alertify.error("Во время удаления произошла ошибка");
                });
        }
    };
    
    $scope.onNewButtonClicked = function () {
        loadTest(0);
    };

    $scope.loadTests = function() {
        var subjectId = getUrlValue('subjectId');

        $http({ method: "GET", url: kt.actions.tests.getTests, dataType: 'json', params: { subjectId: subjectId } })
            .success(function(data) {
                $scope.tests = data;
            })
            .error(function(data, status, headers, config) {
                alertify.error("Во время получения данных произошла ошибка");
            });
    };
    
    function loadTest(testId) {
        var modalInstance = $modal.open({
            templateUrl: '/Content/KnowledgeTesting/testDetails.html',
            controller: 'testDetailsCtrl',
            scope: $scope,
            resolve: {
                id: function () {
                    return testId;
                }
            }
        });
    };

    $scope.onUploadImage = function() {
        var modalInstance = $modal.open({
            templateUrl: '/Content/KnowledgeTesting/content.html',
            controller: 'contentCtrl',
            scope: $scope,
        });
    };
});