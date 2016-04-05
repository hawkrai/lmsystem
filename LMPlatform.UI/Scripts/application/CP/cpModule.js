angular
    .module('cpApp', [
        'ngRoute',
        'frapontillo.bootstrap-duallistbox',
        'ui.bootstrap',
        'xeditable',
        'cpApp.ctrl.home',
        'cpApp.ctrl.projects',
        'cpApp.ctrl.project',
        'cpApp.ctrl.students',
        'cpApp.ctrl.percentages',
        'cpApp.ctrl.percentage',
        'cpApp.ctrl.percentageResults',
        'cpApp.ctrl.visitStats',
        'cpApp.service.project',
        'cpApp.ctrl.taskSheet',
        'ui.select'
    ])
    .config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {

        $routeProvider.when('/Projects', {
            templateUrl: '/Cp/Projects',
            controller: 'projectsCtrl',
            reloadOnSearch: false
        });

        $routeProvider.when('/Percentages', {
            templateUrl: '/Cp/Percentages',
            controller: 'percentagesCtrl'
        });

        $routeProvider.when('/TaskSheet', {
            templateUrl: '/Cp/TaskSheet',
            controller: 'taskSheetCtrl'
        });

        $routeProvider.when('/PercentageResults', {
            templateUrl: '/Cp/PercentageResults',
            controller: 'percentageResultsCtrl'
        });

        $routeProvider.when('/VisitStats', {
            templateUrl: '/Cp/VisitStats',
            controller: 'visitStatsCtrl'
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
    }).run(function (editableOptions) {
        editableOptions.theme = 'bs3';
    });;