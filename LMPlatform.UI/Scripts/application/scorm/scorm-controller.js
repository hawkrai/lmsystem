angular.module('scormApp.controllers', [])
    .controller('ScormController', function ($scope, $http) {

        $scope.sco = [];
        $scope.viewScoClient = false;
        $scope.urlServiceCore = "";
        $scope.treeActivity = null;
        $scope.nameLoadSco = "";
        $scope.viewScoObject = null;

        $scope.init = function () {
            $scope.loadObjects();
        };

        $scope.closeSco = function() {
            $scope.viewScoClient = false;
            $scope.viewScoObject = null;
            $scope.urlServiceCore = "";
        };

        $scope.frameLoad = function(urlRes) {
            $scope.urlServiceCore = "/ScormObjects/" + $scope.viewScoObject.Path + "/" + urlRes;
        };

        $scope.viewSco = function (object) {
            $scope.viewScoClient = true;
            $scope.viewScoObject = object;
            
            $.ajax({
                type: 'GET',
                url: "/ScormMod/ViewSco?path=" + object.Path,
                dataType: "json",
                contentType: "application/json",

            }).success(function (data, status) {
                $scope.$apply(function () {
                    $scope.treeActivity = data;
                });
               
            });
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
        
        $scope.openSco = function () {
            $($.find('input[type=file][name=openSco]')[0]).trigger('click');
        };

        $scope.loadScoEventClick = function () {
            var formData = new FormData();


            formData.append("name", $scope.nameLoadSco);
            formData.append("file", $.find('input[type=file][name=openSco]')[0].files[0]);
            $.ajax({
                url: "/ScormMod/LoadObject",
                type: 'POST',
                data: formData,
                processData: false,
                contentType: false,
                success: function (data) {
                    $scope.$apply(function () {
                        $scope.loadObjects();
                    });
                    
                },
            });
            $('#dialogLoadSco').modal('hide');
        };

        $scope.loadSco = function () {
            $('#dialogLoadSco').modal();
        };
    });