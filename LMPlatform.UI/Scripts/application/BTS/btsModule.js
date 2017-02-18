angular
    .module('btsApp', [
        'ngRoute',
        'btsApp.ctrl.home',
        'btsApp.ctrl.projects'
    ])
    .config(['$routeProvider', function ($routeProvider) {

        $routeProvider.when('/Projects', {
            templateUrl: '/BTS/Projects',
            controller: 'projectsCtrl'
        });

        $routeProvider.otherwise({
            redirectTo: '/Projects'
        });
    }]);