'use strict';
knowledgeTestingApp.controller('questionsCtrl', function ($scope, $http, $modal) {

    $scope.init = function() {
        $scope.loadQuestions();
    };

    $scope.onEditButtonClicked = function (testId) {
        //loadTest(testId);
    };

    $scope.onDeleteButtonClicked = function (testId, testName) {
        //var context = {
        //    id: testId
        //};

        //bootbox.confirm({
        //    title: 'Удаление теста',
        //    message: 'Вы дествительно хотите удалить тест "' + testName + '"?',
        //    buttons: {
        //        'cancel': {
        //            label: 'Отмена',
        //            className: 'btn btn-primary btn-sm'
        //        },
        //        'confirm': {
        //            label: 'Удалить',
        //            className: 'btn btn-primary btn-sm',
        //        }
        //    },
        //    callback: $.proxy($scope.onDeleteConfirmed, context)
        //});
    };

    $scope.onDeleteConfirmed = function (result) {
        if (result) {
            $http({ method: "DELETE", url: kt.actions.tests.deleteTest, dataType: 'json', params: { id: this.id } })
                .success(function() {
                    $scope.loadQuestions();
                    alertify.success('Тест успешно удалён');
                })
                .error(function(data, status, headers, config) {
                    alertify.error("Во время удаления произошла ошибка");
                });
        }
    };
    
    $scope.onNewButtonClicked = function () {
        //loadTest(0);
    };

    $scope.loadQuestions = function() {
        var testId = 1;

        $http({ method: "GET", url: kt.actions.questions.getQuestions, dataType: 'json', params: { testId: testId } })
            .success(function(data) {
                $scope.questions = data;
            })
            .error(function(data, status, headers, config) {
                alertify.error("Во время получения данных произошла ошибка");
            });
    };

    function loadQuestion(questionId) {
        //var modalInstance = $modal.open({
        //    templateUrl: '/Content/KnowledgeTesting/testDetails.html',
        //    controller: 'testDetailsCtrl',
        //    scope: $scope,
        //    resolve: {
        //        id: function () {
        //            return testId;
        //        }
        //    }
        //});
    }

});