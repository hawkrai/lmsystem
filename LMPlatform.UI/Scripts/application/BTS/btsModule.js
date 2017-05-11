angular
    .module('btsApp', [
        'ngRoute',
        'ui.bootstrap',
        'btsApp.ctrl.home',
        'btsApp.ctrl.projects',
        'btsApp.ctrl.project',
        'btsApp.ctrl.participations',
        'btsApp.ctrl.bugs',
    ])
    .config(['$routeProvider', function ($routeProvider) {

        $routeProvider.when('/Projects', {
            templateUrl: '/BTS/Projects',
            controller: 'projectsCtrl'
        });

        $routeProvider.when('/Projects/:id', {
            templateUrl: '/BTS/Project',
            controller: 'projectCtrl'
        });

        $routeProvider.when('/Bugs', {
            templateUrl: '/BTS/bugs',
            controller: 'bugsCtrl'
        });

        $routeProvider.when('/Projects/:projectId/Bugs', {
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