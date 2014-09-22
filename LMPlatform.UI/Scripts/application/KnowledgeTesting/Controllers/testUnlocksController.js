'use strict';
knowledgeTestingApp.controller('testUnlocksCtrl', function ($scope, $http, id, $modalInstance) {
    //$scope.SelectedGroup = 'A';
    var subjectId = getUrlValue('subjectId');

    $scope.context = $scope;
    $scope.context.group = null;

    $http({ method: "GET", url: kt.actions.groups.getGroupsForSubject, dataType: 'json', params: { subjectId: subjectId } })
            .success(function (data) {
                $scope.groups = data;
            if (data.length > 0) {
                $scope.group = data[0];
                $scope.loadStudents();
            }
        })
            .error(function (data, status, headers, config) {
                alertify.error("Во время получения данных о группах произошла ошибка");
            });

    $scope.loadStudents = function () {
        if ($scope.group) {
            $http({ method: "GET", url: kt.actions.groups.getSubgroups, dataType: 'json', params: { groupId: $scope.group.Id, subjectId: subjectId, testId: id } })
                .success(function(data) {
                    $scope.subGroups = data;
                })
                .error(function(data, status, headers, config) {
                    alertify.error("Во время получения данных о подгруппах произошла ошибка");
                });
        }
    };

    $scope.lockAll = function (subGroup, unlock) {
        var studentIds = Enumerable.From(subGroup.Students)
           .Select(function (student) {
               return student.Id;
           }).ToArray();

        var model = {
            studentIds: studentIds,
            testId: id,
            unlock: unlock
        };

        $.ajax({
            url: kt.actions.tests.unlock,
            type: "POST",
            data: JSON.stringify(model),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function() {
                $scope.loadStudents();
                $scope.loadTests();
            }
        });
    };

    $scope.lockOne = function(studentId, unlocked) {
        var model = {
            testId: id,
            studentId: studentId,
            unlocked: unlocked
        };

        $.ajax({
            url: kt.actions.tests.unlockOne,
            type: "POST",
            data: JSON.stringify(model),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function() {
                $scope.loadStudents();
                $scope.loadTests();
            }
        });
    };
    
    $scope.closeDialog = function () {
        $modalInstance.close();
    };
});