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
                    }
                });
            };

            $scope.onChangeLecturer = function () {
                $scope.lecturerProjectsTableParams.reload();
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

            init();
        }]);
