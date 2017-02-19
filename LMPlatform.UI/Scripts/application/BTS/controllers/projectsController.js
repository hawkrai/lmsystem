angular
    .module('btsApp.ctrl.projects', [])
    .controller('projectsCtrl', [
        '$scope',
        'projectsService',
        function ($scope, projectsService) {

            var initialize = function () {
                $(".editProject").tooltip({ title: "Редактировать проект", placement: 'left' });
                //$(".deleteProject").tooltip({ title: "Удалить проект", placement: 'right' });
                $(".deleteButton").tooltip({ title: "Удалить проект", placement: 'top' });
            };

            projectsService.getProjects().then(function (response) {
                $scope.projects = response.data.Data.Data;
                projectsService.addNumbering($scope.projects);
            });

            initialize();
        }]);
