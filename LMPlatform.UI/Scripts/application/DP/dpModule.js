angular
    .module('dpApp', [
        'ngRoute',
        'frapontillo.bootstrap-duallistbox',
        'dpApp.ctrl.home',
        'dpApp.ctrl.projects',
        'dpApp.ctrl.project',
        'dpApp.service.project'
    ])
    .config(['$routeProvider', '$locationProvider', function($routeProvider, $locationProvider) {
       
        $routeProvider.when('/Projects', {
            templateUrl: '/Dp/Projects',
            controller: 'projectsCtrl',
            reloadOnSearch: false
        });
        $routeProvider.when('/Project', {
            templateUrl: '/Dp/Project',
            controller: 'projectCtrl'
        });
        $routeProvider.when('/Project/:id', {
            templateUrl: '/Dp/Project',
            controller: 'projectCtrl'
        });
        
        $routeProvider.when('/ProjectAssignment', {
            templateUrl: '/Dp/ProjectAssignment',
            controller: 'projectAssignmentsCtrl'
        });
        
        $routeProvider.otherwise({
            redirectTo: '/Projects'
        });
    }]).directive('loadingContainer', function () {
        return {
            restrict: 'A',
            scope: false,
            link: function (scope, element, attrs) {
                var loadingLayer = angular.element('<div class="loading"></div>');
                element.append(loadingLayer);
                element.addClass('loading-container');
                scope.$watch(attrs.loadingContainer, function (value) {
                    loadingLayer.toggleClass('ng-hide', !value);
                });
            }
        };
    });