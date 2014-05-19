var parentalApp = angular.module("parentalApp", ['ngRoute', 'ui.bootstrap']);
//.config(function ($routeProvider, $locationProvider) {
//    $routeProvider
//        .when('/Statistcs', {
//            templateUrl: 'Parental/Statistics',
//            controller: 'StatisticsController'
//        })
//        .when('/Plan', {
//            templateUrl: 'Parental/Plan',
//            controller: 'PlanController'
//        })
//        .when('/', {
//            templateUrl: 'Parental/'
//        });
//});

parentalApp.controller("StatCtrl", ['$scope', '$http', '$modal', function ($scope, $http, $modal) {

    $scope.UrlServiceParental = '/Services/Parental/ParentalService.svc/';
    $scope.UrlServiceCore = '/Services/CoreService.svc/';

    $scope.Subjects = [];
    $scope.subject = { Name: "Все предметы", Id: -1, ShortName: "" };
    $scope.data = {};
    $scope.data.Students = {};

    $scope.init = function (groupId) {
        $scope.groupId = groupId;
        $scope.loadSubjects();
    };

    $scope.$watch("subject", function (newValue, oldValue) {
        if (newValue.Id < 0) {

        } else {
            $scope.loadData($scope.subject.Id);
        }
    });

    $scope.loadSubjects = function () {
        $http({
            method: 'Get',
            url: $scope.UrlServiceParental + "GetGroupSubjects/" + $scope.groupId,
            headers: { 'Content-Type': 'application/json' }
        }).success(function (data, status) {
            if (data.Code != '200') {
                alertify.error(data.Message);
            } else {

                if (data.Subjects.length > 0) {
                    $scope.Subjects = data.Subjects;
                    $scope.Subjects.splice(0, 0, $scope.subject);
                }

                alertify.success(data.Message);
            }
        });
    };

    $scope.loadData = function (subjectId) {
        $http({
            method: 'Get',
            url: $scope.UrlServiceCore + "GetGroups/" + subjectId,
            headers: { 'Content-Type': 'application/json' }
        }).success(function (data, status) {
            if (data.Code != '200') {
                alertify.error(data.Message);
            } else {
                
                var queryData = Enumerable.From(data.Groups).First(function (x) { return x.GroupId == $scope.groupId; });
            
                    $scope.data = queryData;
                

                alertify.success(data.Message);
            }
        });
    };

}]);




