angular
    .module('btsApp.ctrl.home', ['ngResource'])
    .controller('homeCtrl', [
        '$scope',
        '$location',
        '$resource',
        function ($scope, $location, $resource) {
            $scope.Title = 'Проекты';

            $scope.setTitle = function (title) {
                $scope.Title = title;
            };

            $scope.init = function (isLector, userId) {
                if (isLector === 1) {
                    $scope.isLector = true;
                } else {
                    $scope.isLector = false;
                }
                $scope.userId = userId;
            };
            
            $scope.isActive = function (href) {
                return href === $location.path();
            };
        }]);