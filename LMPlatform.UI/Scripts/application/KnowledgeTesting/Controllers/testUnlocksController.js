'use strict';
knowledgeTestingApp.controller('testUnlocksCtrl', function ($scope, $http, $modalInstance) {
    //$scope.SelectedGroup = 'A';
    var subjectId = getUrlValue('subjectId');
    
    $http({ method: "GET", url: kt.actions.groups.getGroupsForSubject, dataType: 'json', params: { subjectId: subjectId } })
            .success(function (data) {
                $scope.groups = data;
                //$scope.selectedGroup = data[0];
                $scope.loadStudents();
            })
            .error(function (data, status, headers, config) {
                alertify.error("Во время получения данных о группах произошла ошибка");
            });

    $scope.loadStudents = function () {
        if ($scope.SelectedGroup) {
            $http({ method: "GET", url: kt.actions.groups.getSubgroups, dataType: 'json', params: { groupId: $scope.selectedGroup.Id, subjectId: subjectId } })
                .success(function(data) {
                    $scope.subGroups = data;
                })
                .error(function(data, status, headers, config) {
                    alertify.error("Во время получения данных о подгруппах произошла ошибка");
                });
        }
    };
    
    $scope.closeDialog = function () {
        $modalInstance.close();
    };
});