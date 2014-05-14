var msgApp = angular.module("msgApp", ['ngTable']);

msgApp
    .controller("msgController", ['$scope', '$http', '$filter', 'ngTableParams', function ($scope, $http, $filter, ngTableParams) {
        $scope.activeTab = 'inbox';
        $scope.userId = 0;
        $scope.data = [];
        $scope.displayMessage = {};
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

        $scope.$watch("activeTab", function () {
            $scope.tableParams.reload();
        });

        $scope.isInbox = function (item) {
            return (item.AthorId == $scope.userId & $scope.activeTab != 'inbox') ||
                (item.AthorId != $scope.userId & $scope.activeTab == 'inbox');
        };

        $scope.show = function (msg) {
            $.ajax({
                type: 'GET',
                url: $scope.UrlServiceMessages + "GetMessage/" + msg.Id,
                dataType: "json",
                contentType: "application/json",
            }).success(function (data, status) {
                if (data.Code != '200') {
                    alertify.error(data.Message);
                } else {
                    $scope.$apply(function () {
                        $scope.displayMessage = data.DisplayMessage;
                        msg.IsRead = true;
                    });

                    bootbox.dialog({
                        message: $('#displayMsg').html(),
                        title: " <i class='fa fa-envelope'></i> " + $scope.displayMessage.Subject,
                    });
                }
            }).error(function () {
                alertify.error("Ошибка сервиса");
            });

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

        $scope.getData = function () {
            return $scope.activeTab === 'inbox' ? $scope.inboxMessages : $scope.outboxMessages;
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
    }])
.directive('displayMessage', function () {
    return {
        restrict: 'E',
        template: '<div id="displayMsg">'
                        + '<div class="display-msg">'
                                + '<div class="author-name">{{displayMessage.AthorName}}</div>'
                                + '<div><label>Отправлено:</label>{{displayMessage.Date}}</div>'
                                + '<div><label>Кому:</label>'
                                    + '<span class="recip-name" ng-repeat="name in displayMessage.Recipients"><i class="fa fa-user"></i>{{name}}</span>'
                                + '</div>'
                                + '<div>'
                                    + '<span class="attach-link" ng-repeat="attach in displayMessage.Attachments">'
                                        + '<a  href="/api/Upload/DowloadFile?fileName={{attach.PathName}}/{{attach.FileName}}">'
                                            + '<span class="glyphicon glyphicon-paperclip"></span>'
                                            + '{{attach.Name}}'
                                        + '</a>'
                                    + '</span>'
                                + '</div>'
                                + '<div class="msg-body">'
                                    + '{{displayMessage.Body}}'
                                + '</div>'
                        + '</div>' +
                  '</div>'
    };
});




