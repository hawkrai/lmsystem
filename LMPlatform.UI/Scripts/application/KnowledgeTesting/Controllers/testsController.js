'use strict';
knowledgeTestingApp.controller('testsCtrl', function ($scope, $http, $modal) {

    

    $scope.orderTests = function (newOrder) {

        $.ajax({
            url: "/Tests/OrderTests/",
            type: "PATCH",
            data: JSON.stringify({ newOrder: newOrder }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {

            },
        });
    };

    function gup(name, url) {
        if (!url) url = location.href;
        name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
        var regexS = "[\\?&]" + name + "=([^&#]*)";
        var regex = new RegExp(regexS);
        var results = regex.exec(url);
        return results == null ? null : results[1];
    }

    $scope.init = function() {
    	$scope.loadTests();
	    $("#sortable").sortable({
		    update: function (event, ui) {
			    var newOrder = {};
			    $(this).children().each(function (index) {
				    $(this).find('td').first().html(index + 1);
				    var testId = $(this).find('td').first().attr("testId");
				    var testNumber = index + 1;
				    newOrder[testId] = testNumber;
			    });
			    $scope.orderTests(newOrder);
		    }
	    });
	    $("#sortableForSelfStudy").sortable({
		    update: function (event, ui) {
			    var newOrder = {};
			    $(this).children().each(function (index) {
				    $(this).find('td').first().html(index + 1);
				    var testId = $(this).find('td').first().attr("testId");
				    var testNumber = index + 1;
				    newOrder[testId] = testNumber;
			    });
			    $scope.orderTests(newOrder);
		    }
	    });
    };

    $scope.onEditButtonClicked = function (testId) {
        loadTest(testId);
    };

    $scope.subjectId = gup("subjectId", window.location.href);
    
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
            .success(function (data) {
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