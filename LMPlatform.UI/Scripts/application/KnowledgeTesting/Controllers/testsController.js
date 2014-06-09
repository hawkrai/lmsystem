'use strict';
knowledgeTestingApp.controller('testsCtrl', function ($scope, $http, $modal) {

    var subjectId = getUrlValue('subjectId');

    $scope.onEditButtonClicked = function (testId) {
        loadTest(testId);
    };
    
    $scope.onQuestionButtonClicked = function(testId) {
    };

    $http({ method: "GET", url: kt.actions.tests.getTests, dataType: 'json', params: { subjectId: subjectId }})
        .success(function (data) {
            $scope.tests = data;
            initializeTooltips();
        })
        .error(function (data, status, headers, config) {
            alertify.error("Во время получения данных произошла ошибка");
        });
    
    function initializeTooltips() {
        $(".fa-edit").tooltip({ title: "Редактировать тест", placement: 'left' });
        $(".fa-trash-o").tooltip({ title: "Удалить тест", placement: 'left' });
        $(".fa-question ").tooltip({ title: "Перейти к вопросам", placement: 'left' });
        $(".fa-rocket").tooltip({ title: "Пройти тест", placement: 'left' });
        $('.fa-unlock').tooltip({ title: "Доступность теста", placement: 'left' });
        $('.fa-lock').tooltip({ title: "Доступность теста", placement: 'left' });
    }
    
    function loadTest(testId) {
        var modalInstance = $modal.open({
            templateUrl: '/Content/KnowledgeTesting/testDetails.html',
            controller: 'testDetailsCtrl',
            resolve: {
                id: function () {
                    return testId;
                }
            }
        });
    }

});