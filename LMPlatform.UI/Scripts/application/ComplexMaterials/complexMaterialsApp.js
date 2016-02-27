angular
    .module('complexMaterialsApp', [
        'ngRoute',
        'complexMaterialsApp.ctrl.home',
        'complexMaterialsApp.ctrl.catalog',
        'complexMaterialsApp.ctrl.map',
        'complexMaterialsApp.service.material',
        'complexMaterialsApp.service.navigation'
    ])
    .config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {
        $routeProvider.when('/Catalog', {
            templateUrl: '/ComplexMaterial/Catalog',
            controller: 'catalogCtrl',
            reloadOnSearch: true
        });
        $routeProvider.when('/Map', {
            templateUrl: '/ComplexMaterial/Map',
            controller: 'mapCtrl',
            reloadOnSearch: false
        });
        $routeProvider.otherwise({
            redirectTo: '/Catalog'
        });

    }]);