var msgApp = angular.module("msgApp", ['ngTable', 'ui.bootstrap']);

msgApp
    .controller("msgController", [
        '$scope', '$http', '$modal', '$filter', 'ngTableParams', function ($scope, $http, $modal, $filter, ngTableParams) {
            $scope.activeTab = 'inbox';
            $scope.userId = 0;
            $scope.data = [];
            $scope.displayMessage = {};
            $scope.inboxMessages = [];
            $scope.outboxMessages = [];

            $scope.UrlServiceMessages = '/Services/Messages/MessagesService.svc/';

            $scope.recipients = [];
            $scope.selectedRecipients = [];


            $scope.init = function (userId) {
                $scope.userId = userId;
                $scope.loadMessages(true);
            };

            $scope.switchTabTo = function (tabName) {
                $scope.activeTab = tabName;
            };

            $scope.isInbox = function (item) {
                return (item.AthorId == $scope.userId & $scope.activeTab != 'inbox') ||
                (item.AthorId != $scope.userId & $scope.activeTab == 'inbox');
            };

            $scope.openMsg = function (msg) {
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
                        var modalInstance = $modal.open({
                            templateUrl: 'Message/DisplayMessage',
                            controller: 'DisplayMsgCtrl',
                            //size: size,
                            resolve: {
                                displayMsg: function () {
                                    return data.DisplayMessage;
                                }
                            }
                        });

                        modalInstance.result.then(function (selectedItem) {
                            //$scope.selected = selectedItem;
                            //alert('ok!');
                        }, function () {
                            //alert('dismissed!');
                        });
                    }
                }).error(function () {
                    alertify.error("Ошибка сервиса");
                });
            };


            $scope.showForm = function (replayData) {
                var modalInstance = $modal.open({
                    templateUrl: 'Message/MessageForm',
                    controller: 'WriteMsgCtrl',
                    resolve: {
                        replayData: function () {
                            return replayData;
                        }
                    }
                });

                modalInstance.result.then(function (dataFromModal) {
                    //alert('ok!');
                }, function () {
                    //alert('dismissed!');
                });
                //$scope.fetchRecipients();
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

                        $scope.$watch("activeTab", function () {
                            $scope.tableParams.reload();
                        });
                    }
                }).error(function () {
                    alertify.error("Ошибка сервиса");
                });
            };
        }
    ])
    .controller("DisplayMsgCtrl", ["$scope", "$modalInstance", "displayMsg", function ($scope, $modalInstance, displayMsg) {

        $scope.displayMessage = displayMsg;

        $scope.ok = function () {
            $modalInstance.close($scope.displayMessage.Id);
        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };
    }])
    .controller("WriteMsgCtrl", ["$scope", "$http", "$modalInstance", "replayData", function ($scope, $http, $modalInstance, replayData) {

        $scope.recipientsList = [];
        $scope.formMsg = {};

        var selectRecipients = function () {
            $scope.recipients = _.filter($scope.recipientsList, function (item) {
                return _.contains($scope.selectedRecipients, item.id);
            });
        };

        $scope.fetchRecipients = function () {
            var url = '/Services/Messages/MessagesService.svc/' + 'GetRecipients/';
            $http.get(url).then(function (result) {
                $scope.recipientsList = result.data.Recipients;
                // selectRecipients();
            });
        };

        $scope.fetchRecipients();

        $scope.ok = function () {
            $modalInstance.close();
        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };
    }])
    .directive('ajaxChosen', function () {
        var linker = function (scope, element, attrs) {

            var recipientsList = [];

            $(element).ajaxChosen({
                type: 'GET',
                url: '/Message/GetSelectListOptions',
                dataType: 'json',
                keepTypingMsg: "Продолжайте печатать...",
                lookingForMsg: "Поиск"
            },
                function (data) {

                    //recipientsList = data;
                    //var terms = {};

                    $.each(data, function (i, val) {
                        recipientsList[i] = val;
                    });

                    return recipientsList;
                },
              {
                  no_results_text: "Пользователь не найден...",
                  width: '100%'
              }
            );


            //var list = attrs['chosen'];

            //scope.$watch(list, function () {
            //    element.trigger('chosen:updated');
            //});

            //element.chosen({ width: "100%" });
        };

        return {
            restrict: 'A',
            link: linker
        };
    });




