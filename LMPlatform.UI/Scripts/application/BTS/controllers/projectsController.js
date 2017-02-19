angular
    .module('btsApp.ctrl.projects', [])
    .controller('projectsCtrl', [
        '$scope',
        'projectsService',
        function ($scope, projectsService) {

            projectsService.getProjects().then(function (response) {
                $scope.projects = response.data.Data.Data;
                projectsService.addNumbering($scope.projects);
            });
        }]);
