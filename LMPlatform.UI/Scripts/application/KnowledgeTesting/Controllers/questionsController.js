'use strict';
knowledgeTestingApp.controller('questionsCtrl', function ($scope, $http, $modal) {
 
    $scope.init = function () {
    	$scope.testId = getHashValue('testId');
    	$scope.ForNN = getHashValue('forNN');
    	$scope.loadQuestions();

	    $("#sortable").sortable({
		    update: function (event, ui) {
			    var newOrder = {};
			    $(this).children().each(function (index) {
				    $(this).find('td').first().html(index + 1);
				    var questionId = $(this).find('td').first().attr("questionid");
				    var questionNumber = index + 1;
				    newOrder[questionId] = questionNumber;
			    });
			    $scope.orderQuestions(newOrder);
		    }
	    });
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
    
    $scope.addFromAnotherTestClicked = function () {
        var testId = getHashValue('testId');
        var modalInstance = $modal.open({
            templateUrl: '/Content/KnowledgeTesting/addQuestionFromAnotherTest.html',
            controller: 'addFromAnotherTestCtrl',
            scope: $scope,
            resolve: {
                id: function () {
                    return $scope.testId;
                }
            }
        });
    };

    $scope.createNeuralNetwork = function () {
    	var modalInstance = $modal.open({
    		templateUrl: '/Content/NeuralNetwork/createNeuralNetwork.html',
    		controller: 'createNeuralNetworkCtrl',
    		scope: $scope,
    		resolve: {
    			data: function () {
    				return $scope.questions;
    			}
    		}
    	});
    };

    $scope.orderQuestions = function (newOrder) {

        $.ajax({
            url: "/Tests/OrderQuestions/",
            type: "PATCH",
            data: JSON.stringify({ newOrder: newOrder }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {

            },
        });
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
        var subjectId = getUrlValue("subjectId");
        var modalInstance = $modal.open({
            templateUrl: '/Content/KnowledgeTesting/questionDetails.html',
            controller: 'questionDetailsCtrl',
            scope: $scope,
            resolve: {
                id: function () {
                    return questionId;
                },
                subjectId: function () {
                    return subjectId;
                },
            }
        });
    }
});