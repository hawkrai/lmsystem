'use strict';
knowledgeTestingApp.controller('questionsCtrl', function ($scope, $http, $modal) {

    $scope.init = function() {
        $scope.loadQuestions();
    };

    $scope.onNewButtonClicked = function() {
        loadQuestion(0);
    };

    $scope.onEditButtonClicked = function (questionId) {
        loadQuestion(questionId);
    };

    $scope.onDeleteButtonClicked = function (questionId, questionName) {
        var context = {
            id: questionId
        };

        bootbox.confirm({
            title: 'Удаление вопроса',
            message: 'Вы дествительно хотите удалить вопрос "' + questionName + '"?',
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
            $http({ method: "DELETE", url: kt.actions.questions.deleteQuestion, dataType: 'json', params: { id: this.id } })
                .success(function() {
                    $scope.loadQuestions();
                    alertify.success('Вопрос успешно удалён');
                })
                .error(function(data, status, headers, config) {
                    alertify.error("Во время удаления произошла ошибка");
                });
        }
    };

    $scope.loadQuestions = function() {
        var testId = getHashValue('testId');

        $http({ method: "GET", url: kt.actions.questions.getQuestions, dataType: 'json', params: { testId: testId } })
            .success(function(data) {
                $scope.questions = data;
            })
            .error(function(data, status, headers, config) {
                alertify.error("Во время получения данных произошла ошибка");
            });
    };

    function loadQuestion(questionId) {
        var modalInstance = $modal.open({
            templateUrl: '/Content/KnowledgeTesting/questionDetails.html',
            controller: 'questionDetailsCtrl',
            scope: $scope,
            resolve: {
                id: function () {
                    return questionId;
                }
            }
        });
    }

});