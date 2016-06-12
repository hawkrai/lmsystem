angular
    .module('dpApp', [
        'ngRoute',
        'frapontillo.bootstrap-duallistbox',
        'ui.bootstrap',
        'xeditable',
        'dpApp.ctrl.home',
        'dpApp.ctrl.projects',
        'dpApp.ctrl.project',
        'dpApp.ctrl.taskSheet',
        'dpApp.ctrl.percentages',
        'dpApp.ctrl.percentage',
        'dpApp.ctrl.percentageResults',
        'dpApp.ctrl.visitStats',
        'dpApp.ctrl.students',
        'textAngular',
        'dpApp.ctrl.news',
        'dpApp.service.project',
        'ui.select'
    ])
    .config(['$routeProvider', '$locationProvider', function($routeProvider, $locationProvider) {
       
        $routeProvider.when('/News', {
            templateUrl: '/Dp/News',
            controller: 'newsCtrl'
        });

        $routeProvider.when('/Projects', {
            templateUrl: '/Dp/Projects',
            controller: 'projectsCtrl',
            reloadOnSearch: false
        });
        
        $routeProvider.when('/TaskSheet', {
            templateUrl: '/Dp/TaskSheet',
            controller: 'taskSheetCtrl'
        });
        
        $routeProvider.when('/Percentages', {
            templateUrl: '/Dp/Percentages',
            controller: 'percentagesCtrl'
        });
        
        $routeProvider.when('/PercentageResults', {
            templateUrl: '/Dp/PercentageResults',
            controller: 'percentageResultsCtrl'
        });
        
        $routeProvider.when('/VisitStats', {
            templateUrl: '/Dp/VisitStats',
            controller: 'visitStatsCtrl'
        });
        
        $routeProvider.otherwise({
            redirectTo: '/News'
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
    }).run(function (editableOptions) {
        editableOptions.theme = 'bs3'; 
    });;