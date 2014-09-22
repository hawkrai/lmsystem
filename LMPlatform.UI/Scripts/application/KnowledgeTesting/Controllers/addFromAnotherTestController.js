'use strict';
knowledgeTestingApp.controller('addFromAnotherTestCtrl', function ($scope, $http, id, $modalInstance) {
    //$scope.SelectedGroup = 'A';
    var subjectId = getUrlValue('subjectId');

    $scope.context = $scope;
    $scope.context.group = null;

    $http({ method: "GET", url: kt.actions.tests.getForLector, dataType: 'json' })
            .success(function (data) {
                $scope.tests = data;
        })
            .error(function (data, status, headers, config) {
                alertify.error("Во время получения данных о группах произошла ошибка");
            });

    $scope.loadQuestionsFromBase = function () {
        $http({ method: "GET", url: kt.actions.tests.getQuestionsFromAnotherTests, dataType: 'json', params: { testId: $scope.context.test.Id } })
            .success(function (data) {
                $scope.questionsFromBase = data;
            })
            .error(function (data, status, headers, config) {
                alertify.error("Во время получения данных о подгруппах произошла ошибка");
            });
    };

    $scope.add = function () {
        var questionIds = Enumerable.From($scope.questionsFromBase)
           .Where(function (question) {
               return question.Checked;
           })
            .Select(function(item) {
                return item.Id;
            })
            .ToArray();

        var model = {
            questionItems: questionIds,
            testId: id,
        };

        $.ajax({
            url: kt.actions.questions.addFromAnotherTest,
            type: "POST",
            data: JSON.stringify(model),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.ErrorMessage) {
                    alertify.error(data.ErrorMessage);
                } else {
                    $scope.closeDialog();
                    $scope.loadQuestions();
                }
            }
        });
    };
    
    $scope.closeDialog = function () {
        $modalInstance.close();
    };
});