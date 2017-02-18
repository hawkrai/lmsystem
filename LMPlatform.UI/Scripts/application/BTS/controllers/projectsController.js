angular
    .module('btsApp.ctrl.projects', ['ngResource'])
    .controller('projectsCtrl', [
        '$scope',
        '$location',
        '$resource',
        function ($scope, $location, $resource) {

            $scope.Title = "Проекты";
        }]);
