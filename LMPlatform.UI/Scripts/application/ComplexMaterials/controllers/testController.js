
angular
    .module('complexMaterialsApp.ctrl.test', ['ngResource'])
    .controller('testCtrl', [
        '$scope',
        '$route',
        "$rootScope",
        '$location',
        '$resource',
        "complexMaterialsDataService",
        "$log",
        function ($scope, $route, $rootScope, $location, $resource, complexMaterialsDataService) {

            }])