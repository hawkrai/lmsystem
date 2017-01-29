
angular
    .module('monitoringApp.ctrl.students', ['ngResource'])
    .controller('studentsCtrl', [
        '$scope',
        '$location',
        '$resource',
        'monitoringDataService',
        function ($scope, $location, $resource, monitoringDataService) {
            $scope.data = {
                groups: null,
                selectedGroup: null,
                students: null
            };

            function loadStudents(groupId) {
                monitoringDataService.getStudents({ id: groupId }).success(function (data) {
                    $scope.data.students = data.Students
                });
            }

            $scope.changeGroup = function () {
                loadStudents($scope.data.selectedGroup);
            }

            monitoringDataService.getGroups({ id: monitoringDataService.getSubjectId() }).success(function (data) {
                $scope.data.groups = data.Groups
                $scope.data.selectedGroup = data.Groups[0].GroupId;
                loadStudents($scope.data.selectedGroup);
            });

        }]);
