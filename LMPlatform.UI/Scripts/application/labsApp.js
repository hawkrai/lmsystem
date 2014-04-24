//angular.module('mainApp', []).controller("LabsCtrl", function ($scope) {

//    $scope.labs = [];
//    $scope.subjectId = 0;
//    $scope.init = function (subjectId) {
//        $scope.constDomainService = 'api/Labs';
//        $scope.subjectId = subjectId;
//        $scope.loadLabs();
//        alert("ergerg");
//    };
    
//    $scope.loadLabs = function () {

//        $.ajax({
//            type: 'GET',
//            url: $scope.constDomainService + "/GetLabs?subjectId=" + $scope.subjectId,
//            dataType: "json",
//            contentType: "application/json",

//        }).success(function (data, status) {
//            if (data.Error) {
//                //bootbox.alert(data.Message);
//                alertify.error(data.Message);
//            } else {
//                $scope.$apply(function () {
//                    $scope.schemaes = JSON.parse(data.Data);
//                });
//            }
//        });
//    };
//});
angular.module('mainApp', ['ngRoute'])
    .config([
        '$routeProvider', '$locationProvider',
        function($routeProvider, $locationProvider) {
            $routeProvider.when('/Subject/GetModuleData', { templateUrl: '/Subject/GetModuleData', controller: 'MainCtrl' });
            $locationProvider.html5Mode(true);
        }
    ])
    .controller('MainCtrl', [
        '$route', '$routeParams', '$location',
        function($route, $routeParams, $location) {
            alert($routeParams);
        }
    ]);




//var myApp = angular.module('mainApp', [], function ($scope) {

//    $scope.when('/Subject/GetModuleData', { templateUrl: 'home.html' });

//});

//function LabsCtrl($scope) {
//    $scope.labs = [];
//    $scope.subjectId = 0;
//    $scope.init = function (subjectId) {
//        $scope.constDomainService = 'api/Labs';
//        $scope.subjectId = subjectId;
//        $scope.loadLabs();
//        alert("ergerg");
//    };

//    $scope.loadLabs = function () {

//        $.ajax({
//            type: 'GET',
//            url: $scope.constDomainService + "/GetLabs?subjectId=" + $scope.subjectId,
//            dataType: "json",
//            contentType: "application/json",

//        }).success(function (data, status) {
//            if (data.Error) {
//                //bootbox.alert(data.Message);
//                alertify.error(data.Message);
//            } else {
//                $scope.$apply(function () {
//                    $scope.schemaes = JSON.parse(data.Data);
//                });
//            }
//        });
//    };
//}

