var msgApp = angular.module("msgApp", ['ngTable', 'ui.bootstrap']);

msgApp
    .controller("msgController", ['$scope', '$http', '$modal', '$filter', 'ngTableParams', function ($scope, $http, $modal, $filter, ngTableParams) {
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

                        modalInstance.result.then(function (dataFromModal) {
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
                    saveMessage(dataFromModal);
                }, function () {
                });
            };

            var saveMessage = function (formMsg) {
                $http({
                    method: 'POST',
                    url: $scope.UrlServiceMessages + "Save",
                    data: {
                        subject: formMsg.subject,
                        body: formMsg.body,
                        recipients: JSON.stringify(formMsg.recipients),
                        attachments: formMsg.attachments
                    } ,
                    headers: { 'Content-Type': 'application/json' }
        }).success(function(data, status) {
            if (data.Code != '200') {
                alertify.error(data.Message);
            } else {
                $scope.loadMessages();
                alertify.success(data.Message);
            }
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

        $scope.replay = function() {
            $scope.showForm(displayMsg);
        };

        $scope.ok = function () {
            $modalInstance.close($scope.displayMessage.Id);
        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };
    }])
    .controller("WriteMsgCtrl", ["$scope", "$http", "$modalInstance", "replayData", function ($scope, $http, $modalInstance, replayData) {

        $scope.formMsg = {};
        $scope.formMsg.recipients = [];

        if (replayData) {
            $scope.formMsg.subject = "Re: " + replayData.Subject;
            $scope.formMsg.recipients.add(replayData.AthorId);
            alert($scope.formMsg.recipients);
        }

        $scope.getAttachments = function () {
            var itemAttachmentsTable = $('#fileupload').find('table').find('tbody tr');
            var data = $.map(itemAttachmentsTable, function (e) {
                var newObject = null;
                if (e.className === "template-download fade in") {
                    if (e.id === "-1") {
                        newObject = { Id: 0, Title: "", Name: $(e).find('td a').text(), AttachmentType: $(e).find('td.type').text(), FileName: $(e).find('td.guid').text() };
                    } else {
                        newObject = { Id: e.id, Title: "", Name: $(e).find('td a').text(), AttachmentType: $(e).find('td.type').text(), FileName: $(e).find('td.guid').text() };
                    }
                }
                return newObject;
            });
            var dataAsString = JSON.stringify(data);
            return dataAsString;
        };

        $scope.ok = function () {
            $scope.formMsg.attachments = $scope.getAttachments();
            $modalInstance.close($scope.formMsg);
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
                lookingForMsg: "Поиск..."
            },
                function (data) {

                    $.each(data, function (i, val) {
                        recipientsList[i] = val;
                    });

                    return recipientsList;
                },
              {
                  no_results_text: "Пользователь не найден...",
                  width: '100%'
              }
            ).change(function () {
                scope.formMsg.recipients = $(element).val();
            });
        };

        return {
            restrict: 'A',
            link: linker
        };
    });




