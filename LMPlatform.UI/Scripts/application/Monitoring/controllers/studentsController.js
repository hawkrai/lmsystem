
angular
    .module('monitoringApp.ctrl.students', ['ngResource'])
    .controller('studentsCtrl', [
        '$scope',
        "$rootScope",
        '$location',
        '$resource',
        'monitoringDataService',
        function ($scope, $rootScope, $location, $resource, monitoringDataService) {
            $scope.data = {
                groups: null,
                selectedGroup: null,
                subGroups: []
            };

            function loadStudents(groupId) {
                $scope.data.subGroups = [];
                monitoringDataService.getStudents({ id: groupId }).success(function (data) {
                    //$scope.data.students = data.Students;
                    data.Students.forEach(function (item, i, arr) {
                        if ($scope.data.subGroups[item.SubgroupId] === undefined)
                            $scope.data.subGroups[item.SubgroupId] = [];
                        $scope.data.subGroups[item.SubgroupId].push(item);
                    });
                });
            }

            $scope.changeGroup = function () {
                loadStudents($scope.data.selectedGroup);
            }

            $rootScope.goToHome = function () {
                window.location.href = "/ComplexMaterial/?subjectId=" + monitoringDataService.getSubjectId();
            }

            $rootScope.getConceptName = function () {
                monitoringDataService.getConcept().success(function (data) {
                    $rootScope.conceptName = data.Name;
                });
            }

            monitoringDataService.getGroups({ id: monitoringDataService.getSubjectId() }).success(function (data) {
                $scope.data.groups = data.Groups
                $scope.data.selectedGroup = data.Groups[0].GroupId;
                loadStudents($scope.data.selectedGroup);
            });

            $rootScope.getConceptName();

        }]);
