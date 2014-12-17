angular
    .module('materialsApp', [
        'ngRoute',
        'materialsApp.ctrl.home',
        'materialsApp.ctrl.catalog',
        'materialsApp.ctrl.new',
        'materialsApp.service.material'
    ])
    .config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {

        $routeProvider.when('/Catalog', {
            templateUrl: '/Materials/Catalog',
            controller: 'catalogCtrl',
            reloadOnSearch: false
        });
        $routeProvider.when('/New', {
            templateUrl: '/Materials/New',
            controller: 'newCtrl',
            reloadOnSearch: false
        });
        $routeProvider.otherwise({
            redirectTo: '/Catalog'
        });
    }]);