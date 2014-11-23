angular
    .module('dpApp', [
        'ngRoute',
        'frapontillo.bootstrap-duallistbox',
        'ui.bootstrap',
        'xeditable',
        'dpApp.ctrl.home',
        'ui.select'
    ])
    .config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {

        $routeProvider.when('/Projects', {
            templateUrl: '/Dp/Projects',
            controller: 'projectsCtrl',
            reloadOnSearch: false
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