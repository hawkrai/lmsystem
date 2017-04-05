angular
    .module('btsApp', [
        'ngRoute',
        'btsApp.ctrl.home',
        'btsApp.ctrl.projects',
        'btsApp.service.projects',
        'btsApp.directive.project',
        'btsApp.ctrl.bugs',
        'btsApp.service.bugs',
        'btsApp.directive.bug'
    ])
    .config(['$routeProvider', function ($routeProvider) {

        $routeProvider.when('/Projects', {
            templateUrl: '/BTS/Projects',
            controller: 'projectsCtrl'
        });

        $routeProvider.when('/Bugs', {
            templateUrl: '/BTS/bugs',
            controller: 'bugsCtrl'
        });

        $routeProvider.when('/Project/:projectId/Bugs', {
            templateUrl: '/BTS/bugs',
            controller: 'bugsCtrl'
        });

        $routeProvider.otherwise({
            redirectTo: '/Projects'
        });
    }]);