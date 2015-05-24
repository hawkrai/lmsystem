angular.module('scormApp.controllers', [])
    .controller('ScormController', function ($scope, $http) {

        $scope.sco = [];
        $scope.viewSco = false;
        $scope.urlServiceCore = "";
        $scope.init = function () {
            $scope.loadObjects();
        };

        $scope.closeSco = function() {
            $scope.viewSco = false;
            $scope.urlServiceCore = "";
        };

        $scope.openSco = function(url) {
            $scope.viewSco = true;
            $scope.urlServiceCore = url;
        };

        $scope.loadObjects = function() {
            $.ajax({
                type: 'GET',
                url: "/ScormMod/GetObjects",
                dataType: "json",
                contentType: "application/json",

            }).success(function (data, status) {
                $scope.$apply(function () {
                        $scope.sco = data;
                    });

            });
        };
    });