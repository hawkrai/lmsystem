angular
    .module('complexMaterialsApp', [
        'ngRoute',
        'complexMaterialsApp.ctrl.home',
        'complexMaterialsApp.ctrl.catalog',
        'complexMaterialsApp.ctrl.map',
        'complexMaterialsApp.ctrl.test',
        'complexMaterialsApp.service.material',
        'complexMaterialsApp.service.navigation',
        'complexMaterialsApp.ctrl.students',
        'complexMaterialsApp.ctrl.studentInfo',
        'complexMaterialsApp.service.monitoring'
    ])
    .config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {
        $routeProvider.when('/Catalog', {
            templateUrl: '/ComplexMaterial/Catalog',
            controller: 'catalogCtrl',
            reloadOnSearch: true
        });
        $routeProvider.when('/Map', {
            templateUrl: '/ComplexMaterial/Map',
            controller: 'mapCtrl',
            reloadOnSearch: false
        });
        $routeProvider.when('/Tests', {
            templateUrl: '/Content/KnowledgeTesting/passing.html',
            controller: 'testCtrl',
            reloadOnSearch: false
        });
        $routeProvider.when('/MonitoringStudents/root/:rootId', {
            templateUrl: '/ComplexMaterial/MonitoringStudents',
            controller: 'studentsCtrl',
            reloadOnSearch: true
        });
        $routeProvider.when('/MonitoringStudents/root/:rootId/Student/:studentId', {
            templateUrl: '/ComplexMaterial/MonitoringStudentInfo',
            controller: 'studentInfoCtrl',
            reloadOnSearch: true
        });
        $routeProvider.otherwise({
            redirectTo: '/Catalog'
        });

    }]);