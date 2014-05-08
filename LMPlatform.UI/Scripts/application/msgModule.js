var msgApp = angular.module("msgApp", []);

msgApp
    .controller("msgController", ["$scope", "$http", function ($scope, $http) {
        $scope.activeTab = 'inbox';
        $scope.userId = 0;
        $scope.messages = [];
        $scope.UrlServiceMessages = '/Services/Messages/MessagesService.svc/';

        $scope.init = function (userId) {
            $scope.userId = userId;
            $scope.messages = [];
            $scope.loadMessages();
        };

        $scope.switchTabTo = function(tabName) {
            $scope.activeTab = tabName;
        };

        $scope.loadMessages = function () {
            $.ajax({
                type: 'GET',
                url: $scope.UrlServiceMessages + "GetMessages/" + $scope.userId,
                dataType: "json",
                contentType: "application/json",

            }).success(function (data, status) {
                if (data.Code != '200') {
                    alertify.error(data.Message);
                } else {
                    $scope.$apply(function () {
                        $scope.messages = data.Messages;
                    });
                }
            }).error(function () {
                alertify.error("Ошибка сервиса");
            });
        }; 
    }]);
    


