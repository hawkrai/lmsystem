
angular
    .module('complexMaterialsApp.ctrl.students', ['ngResource'])
    .controller('studentsCtrl', [
        '$scope',
        "$rootScope",
        '$location',
        '$resource',
        'monitoringDataService',
        'navigationService',
        function ($scope, $rootScope, $location, $resource, monitoringDataService, navigationService) {
            $scope.data = {
                groups: null,
                selectedGroup: null,
                subGroups: []
            };

            $scope.getStudentInfoLink = function (studentId) {
                return window.location.href + "/Student/" + studentId;
            }

            $rootScope.isBackspaceShow = function () {
                return true;
            }

            function loadStudents(groupId) {
                $scope.data.subGroups = [];
                monitoringDataService.getStudents({ id: groupId }).success(function (data) {
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

            $rootScope.goToConceptRoot = function () {
                window.location.href = "/ComplexMaterial/?subjectId=" + monitoringDataService.getSubjectId() + "&parent=" + monitoringDataService.getRootId();
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
