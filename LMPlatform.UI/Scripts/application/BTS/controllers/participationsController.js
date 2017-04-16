angular
    .module('btsApp.ctrl.participations', ['ngTable', 'btsApp.service.participations'])
    .controller('participationsCtrl', [
        '$scope',
        'PAGE_SIZE',
        'NgTableParams',
        'participationsService',
        function ($scope, PAGE_SIZE, NgTableParams, participationsService) {
            $scope.lecturers = [];
            $scope.selectedLecturer = null;
            $scope.groups = [];
            $scope.selectedGroup = null;

            function init() {
                $scope.setTitle('Занятость на проектах');
                setLecturers();
                setGroups();
            };

            function setLecturers() {
                participationsService.getLecturers().then(function (response) {
                    $scope.lecturers = response.data.Lectors;
                    if ($scope.lecturers.length != 0) {
                        $scope.selectedLecturer = $scope.lecturers[0];
                        $scope.lecturerProjectsTableParams.reload();
                    }
                });
            };

            function setGroups() {
                participationsService.getGroups($scope.userId).then(function (response) {
                    $scope.groups = response.data.Groups;
                    if ($scope.groups.length != 0) {
                        $scope.selectedGroup = $scope.groups[0];
                        $scope.studentsParticipationsTableParams.reload();
                    }
                });
            };

            $scope.onLecturerChange = function () {
                $scope.lecturerProjectsTableParams.reload();
            };

            $scope.onGroupChange = function () {
                $scope.studentsParticipationsTableParams.reload();
            };

            $scope.lecturerProjectsTableParams = new NgTableParams({
                count: PAGE_SIZE
            }, {
                getData: function (params) {
                    if ($scope.selectedLecturer == null)
                        return;
                    return participationsService.getUserProjectsParticipations($scope.selectedLecturer.LectorId, params.page(), params.count(), params.orderBy()).then(function (response) {
                        params.total(response.data.TotalCount);
                        return response.data.Projects;
                    });
                }
            });

            $scope.studentsParticipationsTableParams = new NgTableParams({
                count: PAGE_SIZE
            }, {
                getData: function (params) {
                    if ($scope.selectedGroup == null)
                        return;
                    return participationsService.geStudentsParticipations($scope.selectedGroup.GroupId, params.page(), params.count()).then(function (response) {
                        params.total(response.data.TotalCount);
                        return response.data.ProjectsStudents;
                    });
                }
            });

            init();
        }]);
