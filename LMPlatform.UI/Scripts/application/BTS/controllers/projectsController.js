angular
    .module('btsApp.ctrl.projects', [])
    .constant('PAGE_SIZE', 30)
    .controller('projectsCtrl', [
        '$scope',
        'projectsService',
        'PAGE_SIZE',
        function ($scope, projectsService, PAGE_SIZE) {

            var pageNumber = 0;
            $scope.projects = [];
            var busy = false;

            $scope.loadProjects = function () {
                if (busy) return;
                busy = true;

                projectsService.getProjects(++pageNumber, PAGE_SIZE).then(function (response) {
                    if (response.data.Data.Data.length == 0) {
                        busy = true;
                        return;
                    }
                    response.data.Data.Data.forEach(function (item, i) {
                        $scope.projects.push(item);
                    });
                    projectsService.addNumbering($scope.projects, (pageNumber - 1) * PAGE_SIZE);
                    busy = false;
                });
            };
        }]);
