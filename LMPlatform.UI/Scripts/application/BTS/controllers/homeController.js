angular
    .module('btsApp.ctrl.home', ['ngResource'])
    .controller('homeCtrl', [
        '$scope',
        '$location',
        '$resource',
        function ($scope, $location, $resource) {

            $scope.Title = 'Проекты';

            $scope.init = function (isLector) {
                if (isLector === 1) {
                    $scope.isLector = true;
                } else {
                    $scope.isLector = false;
                }
            };
            
            $scope.isActive = function (href) {
                return href === $location.path();
            };
        }]);