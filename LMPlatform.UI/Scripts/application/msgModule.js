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
            $scope.loadMessages();
        };

        $scope.switchTabTo = function (tabName) {
            $scope.activeTab = tabName;
        };

        var getData = function () {
            return $scope.activeTab === 'inbox' ? $scope.inboxMessages : $scope.outboxMessages;
        };

        $scope.$watch("activeTab", function () {
            $scope.tableParams.reload();
        });

        $scope.isInbox = function (item) {
            return (item.AthorId == $scope.userId & $scope.activeTab != 'inbox') ||
                   (item.AthorId != $scope.userId & $scope.activeTab == 'inbox');
        };

        $scope.checkboxes = { 'checked': false, items: {} };

        // watch for check all checkbox
        $scope.$watch('checkboxes.checked', function (value) {
            var msgs = getData();
            angular.forEach(msgs, function (item) {
                if (angular.isDefined(item.id)) {
                    $scope.checkboxes.items[item.id] = value;
                }
            });
        });

        // watch for data checkboxes
        $scope.$watch('checkboxes.items', function (values) {
            var msgs = getData();
            if (!msgs) {
                return;
            }
            var checked = 0, unchecked = 0,
                total = msgs.length;
            angular.forEach(msgs, function (item) {
                checked += ($scope.checkboxes.items[item.id]) || 0;
                unchecked += (!$scope.checkboxes.items[item.id]) || 0;
            });
            if ((unchecked == 0) || (checked == 0)) {
                $scope.checkboxes.checked = (checked == total);
            }
            // grayed checkbox
            angular.element(document.getElementById("select_all")).prop("indeterminate", (checked != 0 && unchecked != 0));
        }, true);


        $scope.deleteMessage = function (msgId) {
            bootbox.confirm("Вы действительно хотите удалить выбранные сообщения?", function (isConfirmed) {
                if (isConfirmed) {
                    
                    //var messagesToDelete = [];
                    //for (var propName in $scope.checkboxes.items) {
                    //    messagesToDelete.push(propName);
                    //}

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
                            //$scope.news.splice($scope.news.indexOf(news), 1);
                        }
                    });
                }
            });
        };

        $scope.loadMessages = function () {
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

                        $scope.tableParams = new ngTableParams({
                            page: 1,
                            count: 10
                        },
                        {
                            getData: function ($defer, params) {
                                var filteredData = getData();
                                params.total(filteredData.length);
                                $defer.resolve(filteredData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                            },

                            $scope: { $data: {} }
                        });

                    });
                }
            }).error(function () {
                alertify.error("Ошибка сервиса");
            });
        };
    }]);



