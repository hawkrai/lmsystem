angular
    .module('monitoringApp', [
        'ngRoute',
        'monitoringApp.ctrl.students',
        'monitoringApp.ctrl.studentInfo',
        'monitoringApp.service.monitoring'
    ])
    .config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {
        $routeProvider.when('/Students', {
            templateUrl: '/Monitoring/Students',
            controller: 'studentsCtrl',
            reloadOnSearch: true
        });
        $routeProvider.when('/Student/:studentId', {
            templateUrl: '/Monitoring/StudentInfo',
            controller: 'studentInfoCtrl',
            reloadOnSearch: true
        });
        $routeProvider.otherwise({
            redirectTo: '/Students'
        });

    }]);