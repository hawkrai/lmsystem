﻿var msgApp = angular.module("msgApp", ['ngTable', 'ui.bootstrap']);

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
    					$scope.displayMessage.currentUserId = $scope.userId;

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

    				modalInstance.result.then(function (replayMsg) {
    					if (replayMsg) {
    						$scope.showForm(replayMsg);
    					}

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
    		if (formMsg.body == undefined || formMsg.body.length == 0) {


			    bootbox.dialog({
				    message: "Вы действительно хотите отправить пустое сообщение?",
				    title: "Отправка сообщения",
				    buttons: {
					    danger: {
						    label: "Отмена",
						    className: "btn-default btn-sm",
						    callback: function() {

						    }
					    },
					    success: {
						    label: "Да",
						    className: "btn-primary btn-sm",
						    callback: function(isConfirmed) {
							    if (isConfirmed) {

								    $http({
									    method: 'POST',
									    url: $scope.UrlServiceMessages + "Save",
									    data: {
										    subject: formMsg.subject,
										    body: formMsg.body,
										    recipients: JSON.stringify(formMsg.recipients),
										    attachments: formMsg.attachments
									    },
									    headers: { 'Content-Type': 'application/json' }
								    }).success(function(data, status) {
									    if (data.Code != '200') {
										    alertify.error(data.Message);
									    } else {
										    $scope.loadMessages();
										    alertify.success(data.Message);
									    }
								    });
							    }
						    }
					    }
				    }
			    });
		    } else {
    			$http({
    				method: 'POST',
    				url: $scope.UrlServiceMessages + "Save",
    				data: {
    					subject: formMsg.subject,
    					body: formMsg.body,
    					recipients: JSON.stringify(formMsg.recipients),
    					attachments: formMsg.attachments
    				},
    				headers: { 'Content-Type': 'application/json' }
    			}).success(function (data, status) {
    				if (data.Code != '200') {
    					alertify.error(data.Message);
    				} else {
    					$scope.loadMessages();
    					alertify.success(data.Message);
    				}
    			});
		    }

    	};

    	$scope.deleteMessage = function (msgId) {
    		bootbox.confirm({
    			title: 'Подтверждение удаления',
    			message: 'Вы действительно хотите удалить выбранное сообщение?',
    			buttons: {
    				'cancel': {
    					label: 'Отмена',
    				},
    				'confirm': {
    					label: 'Удалить',
    				}
    			},
    			callback: function (isConfirmed) {
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
                                total: $scope.getData().length,
                            	getData: function ($defer, params) {
                            		var filteredData = $scope.getData();
                            		params.total(filteredData.length);
                            		$defer.resolve(filteredData);//TODO rollback
                            	},
                            	counts: [],
                            	fideDataTablesInfo: true,
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
    }])
    .controller("DisplayMsgCtrl", function ($scope, $modalInstance, displayMsg) {

    	$scope.displayMessage = displayMsg;

    	$scope.replay = function () {
    		$modalInstance.close($scope.displayMessage);
    	};

    	$scope.ok = function () {
    		$modalInstance.close();
    	};

    	$scope.cancel = function () {
    		$modalInstance.dismiss('cancel');
    	};
    })
    .controller("WriteMsgCtrl", function ($scope, $http, $modalInstance, replayData) {
    	$scope.recipientsList = [];
    	$scope.replayData = replayData;

    	if (replayData) {
    		$scope.recipientsList.push({ value: replayData.AthorId, text: replayData.AthorName });
    	}

    	$scope.cancelModal = function () {
    		$modalInstance.dismiss('cancel');
    	};

    	$scope.submitModal = function (formData) {
    		$modalInstance.close(formData);
    	};
    })
    .controller("FormMsgCtrl", function ($scope) {

    	$scope.formData = {};
    	$scope.formData.recipients = [];

    	if ($scope.replayData) {
    		$scope.formData.subject = "Re: " + $scope.replayData.Subject;
    		$scope.formData.recipients.push($scope.replayData.AthorId);
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


    	$scope.send = function () {
    		$scope.submitted = true;

    		if ($scope.msgForm.$valid) {
    			$scope.formData.attachments = $scope.getAttachments();
    			$scope.submitModal($scope.formData);
    		}

    	};
    })

    .directive('ajaxChosen', function () {

    	return {
    		restrict: 'A',
    		link: function (scope, element, attrs) {
    			var recipientsList = [];
    			$.each(scope.recipientsList, function (i, val) {
    				recipientsList.push({ text: val.text, value: val.value });
    				$(element).append("<option value = '" + val.value + "' selected='selected'>" + val.text + "</option>");
    			});

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
                	scope.formData.recipients = $(element).val();
                });
    		}
    	};
    });




