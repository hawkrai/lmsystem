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

            function init() {
                setLecturers();
            };

            function setLecturers() {
                participationsService.getLecturers().then(function (response) {
                    $scope.lecturers = response.data.Lectors;
                    $scope.selectedLecturer = $scope.lecturers[0];
                });
            };

            $scope.onChangeGroup = function () {
                
            };

            init();
        }]);
