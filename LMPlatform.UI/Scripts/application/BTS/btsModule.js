angular
    .module('btsApp', [
        'ngRoute',
        'ui.bootstrap',
        'btsApp.ctrl.home',
        'btsApp.ctrl.projects',
        'btsApp.ctrl.participations',
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

        $routeProvider.when('/ProjectParticipation', {
            templateUrl: '/BTS/ProjectParticipation',
            controller: 'participationsCtrl'
        });

        $routeProvider.otherwise({
            redirectTo: '/Projects'
        });
    }]);