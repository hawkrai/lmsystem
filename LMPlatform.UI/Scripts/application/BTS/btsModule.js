angular
    .module('btsApp', [
        'ngRoute',
        'btsApp.ctrl.home',
        'btsApp.ctrl.projects'
    ])
    .config(['$routeProvider', function ($routeProvider) {

        $routeProvider.when('/Projects', {
            template: 'AZAZA',
            //templateUrl: '/Dp/Projects',
            controller: 'projectsCtrl'
        });

        $routeProvider.otherwise({
            redirectTo: '/Projects'
        });
    }]);