var msgApp = angular.module("msgApp", ['ngTable']);

msgApp
    .controller("msgController", [
        '$scope', '$http', '$filter', 'ngTableParams', function($scope, $http, $filter, ngTableParams) {
            $scope.activeTab = 'inbox';
            $scope.userId = 0;
            $scope.data = [];
            $scope.displayMessage = {};
            $scope.inboxMessages = [];
            $scope.outboxMessages = [];

            $scope.UrlServiceMessages = '/Services/Messages/MessagesService.svc/';

            $scope.recipients = [];
            $scope.selectedRecipients = [];
            $scope.recipientsList = [];

            $scope.init = function(userId) {
                $scope.userId = userId;
                $scope.loadMessages(true);
            };

            $scope.switchTabTo = function(tabName) {
                $scope.activeTab = tabName;
            };

            $scope.isInbox = function(item) {
                return (item.AthorId == $scope.userId & $scope.activeTab != 'inbox') ||
                (item.AthorId != $scope.userId & $scope.activeTab == 'inbox');
            };

            var selectRecipients = function () {
                $scope.recipients = _.filter($scope.recipientsList, function (item) {
                    return _.contains($scope.selectedRecipients, item.id);
                });
            };

            $scope.fetchRecipients = function () {
                var url = $scope.UrlServiceMessages + "GetRecipients/";
                $http.get(url).then(function (result) {
                    $scope.recipientsList = result.data.Recipients;
                   // selectRecipients();
                });
            };

            $scope.show = function(msg) {
                $.ajax({
                    type: 'GET',
                    url: $scope.UrlServiceMessages + "GetMessage/" + msg.Id,
                    dataType: "json",
                    contentType: "application/json",
                }).success(function(data, status) {
                    if (data.Code != '200') {
                        alertify.error(data.Message);
                    } else {
                        $scope.$apply(function() {
                            $scope.displayMessage = data.DisplayMessage;
                            msg.IsRead = true;
                        });

                        bootbox.dialog({
                            message: $('#displayMsg').html(),
                            title: " <i class='fa fa-envelope'></i> " + $scope.displayMessage.Subject,
                        });
                    }
                }).error(function() {
                    alertify.error("Ошибка сервиса");
                });

            };


            $scope.showForm = function(replayData) {
                bootbox.dialog({
                    message: $('#msgForm').html(),
                    title: " <i class='fa fa-envelope'></i> " + "Отпарвка сообщения",
                });

                $scope.fetchRecipients();

            };

            $scope.deleteMessage = function(msgId) {
                bootbox.confirm("Вы действительно хотите удалить сообщение?", function(isConfirmed) {
                    if (isConfirmed) {
                        $http({
                            method: 'POST',
                            url: $scope.UrlServiceMessages + "Delete",
                            data: { messageId: msgId },
                            headers: { 'Content-Type': 'application/json' }
                        }).success(function(data, status) {
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

            $scope.getData = function() {
                return $scope.activeTab === 'inbox' ? $scope.inboxMessages : $scope.outboxMessages;
            };

            $scope.loadMessages = function(initLoad) {
                $.ajax({
                    type: 'GET',
                    url: $scope.UrlServiceMessages + "GetMessages/",
                    dataType: "json",
                    contentType: "application/json",
                }).success(function(data, status) {
                    if (data.Code != '200') {
                        alertify.error(data.Message);
                    } else {
                        $scope.$apply(function() {
                            $scope.inboxMessages = data.InboxMessages;
                            $scope.outboxMessages = data.OutboxMessages;

                            if (initLoad) {

                                $scope.tableParams = new ngTableParams({
                                    page: 1,
                                    count: 10
                                },
                                {
                                    getData: function($defer, params) {
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

                        $scope.$watch("activeTab", function () {
                            $scope.tableParams.reload();
                        });
                    }
                }).error(function() {
                    alertify.error("Ошибка сервиса");
                });
            };
        }
    ])
    .directive('displayMessage', function() {
        return {
            restrict: 'E',
            templateUrl: 'Message/DisplayMessage'
        };
    })
    .directive('messageForm', function() {
        return {
            restrict: 'E',
            templateUrl: 'Message/MessageForm'
        };
    })
    .directive('chosen', function() {
        var linker = function(scope, element, attrs) {
            var list = attrs['chosen'];

            scope.$watch(list, function() {
                element.trigger('chosen:updated');
            });

            element.chosen({ width: "100%" });
        };

        return {
            restrict: 'A',
            link: linker
        };
    });




