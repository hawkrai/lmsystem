var msgApp = angular.module("adminApp", ['adminApp.controllers', 'ngRoute', 'ngTable'])
.config(function ($locationProvider) {
})
.config(function ($routeProvider, $locationProvider) {
    $routeProvider
        .when('/News', {
            templateUrl: 'Admistration/Students',
            controller: 'StudentController'
        })
        .when('/Lectures', {
            templateUrl: 'Admistration/Lectures',
            controller: 'LecturesController'
        })
        .when('/Labs', {
            templateUrl: 'Admistration/Groups',
            controller: 'GroupController'
        });
});




