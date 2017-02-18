angular
    .module('btsApp.ctrl.home', ['ngResource'])
    .controller('homeCtrl', [
        '$scope',
        '$location',
        '$resource',
        function ($scope, $location, $resource) {

            $scope.Title = 'Проекты';
            
            $scope.isActive = function (href) {
                return href === $location.path();
            };
        }]);