var msgApp = angular.module("msgApp", ['ngTable']);

msgApp
    .controller("msgController", ['$scope', '$http', '$filter', 'ngTableParams', function ($scope, $http, $filter, ngTableParams) {
        $scope.activeTab = 'inbox';
        $scope.userId = 0;
        $scope.data = [];
        $scope.inboxMessages = [];
        $scope.outboxMessages = [];

        $scope.UrlServiceMessages = '/Services/Messages/MessagesService.svc/';

        $scope.init = function (userId) {
            $scope.userId = userId;
            $scope.loadMessages(true);
        };

        $scope.switchTabTo = function (tabName) {
            $scope.activeTab = tabName;
        };

        $scope.getData = function () {
            return $scope.activeTab === 'inbox' ? $scope.inboxMessages : $scope.outboxMessages;
        };

        $scope.$watch("activeTab", function () {
            $scope.tableParams.reload();
        });

        $scope.isInbox = function (item) {
            return (item.AthorId == $scope.userId & $scope.activeTab != 'inbox') ||
                   (item.AthorId != $scope.userId & $scope.activeTab == 'inbox');
        };

        $scope.deleteMessage = function (msgId) {
            bootbox.confirm("Вы действительно хотите удалить сообщение?", function (isConfirmed) {
                if (isConfirmed) {
                    $http({
                        method: 'POST',
                        url: $scope.UrlServiceMessages + "Delete",
                        data: { messageId: msgId },
                        headers: { 'Content-Type': 'application/json' }
                    }).success(function (data, status) {
                        if (data.Code != '200') {
                            alertify.error(data.Message);
                        } else {
                            alertify.success(data.Message);
                            $scope.loadMessages();
                        }
                    });
                }
            });
        };

        $scope.loadMessages = function (initLoad) {
            $.ajax({
                type: 'GET',
                url: $scope.UrlServiceMessages + "GetMessages/",
                dataType: "json",
                contentType: "application/json",

            }).success(function (data, status) {
                if (data.Code != '200') {
                    alertify.error(data.Message);
                } else {
                    $scope.$apply(function () {
                        $scope.inboxMessages = data.InboxMessages;
                        $scope.outboxMessages = data.OutboxMessages;

                        if (initLoad) {

                            $scope.tableParams = new ngTableParams({
                                page: 1,
                                count: 10
                            },
                                {
                                    getData: function ($defer, params) {
                                        var filteredData = $scope.getData();
                                        params.total(filteredData.length);
                                        $defer.resolve(filteredData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                                    },

                                    $scope: { $data: {} }
                                });
                        } else {
                            $scope.tableParams.reload();
                        }

                    });
                }
            }).error(function () {
                alertify.error("Ошибка сервиса");
            });
        };
    }]);



