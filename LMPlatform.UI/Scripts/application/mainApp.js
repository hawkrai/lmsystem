var app = angular.module('mainApp', ['mainApp.controllers', 'ngRoute', 'ui.bootstrap', 'xeditable', 'ui.chart', 'angularSpinner'])
    .config(function ($locationProvider) {
    })
    .config(function ($routeProvider, $locationProvider) {
        $routeProvider
            .when('/News', {
                templateUrl: 'Subject/News',
                controller: 'NewsController'
            })
            .when('/Lectures', {
                templateUrl: 'Subject/Lectures',
                controller: 'LecturesController'
            })
            .when('/Labs', {
                templateUrl: 'Subject/Labs',
                controller: 'LabsController'
            })
            .when('/Practical', {
                templateUrl: 'Subject/Practicals',
                controller: 'PracticalsController'
            })
            .when('/Repo', {
                templateUrl: 'Subject/Repo',
                controller: 'RepoController'
            })
            .when('/SubjectAttachments', {
                templateUrl: 'Subject/SubjectAttachments',
                controller: 'SubjectAttachmentsController'
            });

			$routeProvider.otherwise({
        		redirectTo: '/News'
			});

    }).value('charting', {
        pieChartOptions: {
            seriesDefaults: {
                renderer: jQuery.jqplot.PieRenderer,
                rendererOptions: {
                    showDataLabels: true
                }
            },
            legend: { show: true, location: 'e' }
        }
    });

app.run(function (editableOptions, editableThemes) {
    editableThemes.bs3.inputClass = 'input-sm text-center';
    editableThemes.bs3.buttonsClass = 'btn-sm';
    editableOptions.theme = 'bs3';
});

app.directive('showonhoverparent',
   function () {
   	return {
   		link: function (scope, element, attrs) {
   			element.parent().bind('mouseenter', function () {
   				element.show();
   			});
   			element.parent().bind('mouseleave', function () {
   				element.hide();
   			});
   		}
   	};
   });

angular.module('mainApp.controllers', ['ui.bootstrap', 'xeditable', 'textAngular', 'angularSpinner'])
    .controller('MainCtrl', function ($rootScope, $scope, $sce, usSpinnerService) {
        $scope.renderHtml = function (htmlCode) {
            return $sce.trustAsHtml(htmlCode);
        };
        
        $scope.today = function () {
        	$scope.dt = new Date();
        };
        $scope.today();

        $scope.clear = function () {
        	$scope.dt = null;
        };

    	// Disable weekend selection
        $scope.disabled = function (date, mode) {
        	return (mode === 'day' && (date.getDay() === 0 || date.getDay() === 6));
        };

        $scope.toggleMin = function () {
        	$scope.minDate = $scope.minDate ? null : new Date();
        };
        $scope.toggleMin();

        $scope.open = function () {
        	//$event.preventDefault();
        	//$event.stopPropagation();

        	$scope.opened = true;
        };
        
        $scope.dateOptions = {
        	formatYear: 'yyyy',
        	startingDay: 1
        };

        $scope.format = "dd/MM/yyyy";

        $scope.showWeeks = true;
        $scope.toggleWeeks = function () {
            $scope.showWeeks = !$scope.showWeeks;
        };

        $scope.subjectId = 0;
        $scope.UrlServiceMain = '/Services/CoreService.svc/';

        //groups
        $scope.groups = [];
        $scope.subGroups = [];

        $scope.groupWorkingData = {
            selectedSubGroup: null,
            selectedGroup: null,
            selectedGroupId: 0,
            selectedSubGroupId: 0
        };

        $scope.startSpin = function () {
        	$(".loading").toggleClass('ng-hide', false);
        	usSpinnerService.spin('spinner-1');
        };

        $scope.stopSpin = function () {
        	$(".loading").toggleClass('ng-hide', true);
        	usSpinnerService.stop('spinner-1');
        };

        $scope.selectedGroupChange = function (groupId, subGroupId) {
            if (groupId != null) {
                $scope.groupWorkingData.selectedGroupId = groupId;
                $scope.groupWorkingData.selectedSubGroupId = 0;
            }
            if (subGroupId != null) {
                $scope.groupWorkingData.selectedSubGroupId = subGroupId;
            }

            $scope.groupWorkingData.selectedGroup = null;
            $.each($scope.groups, function (index, value) {
                if (value.GroupId == $scope.groupWorkingData.selectedGroupId) {
                    $scope.groupWorkingData.selectedGroup = value;
                    return;
                }
            });

            if ($scope.groupWorkingData.selectedSubGroupId == 0) {
                if ($scope.groupWorkingData.selectedGroup.SubGroupsOne != null) {
                    $scope.groupWorkingData.selectedSubGroupId = $scope.groupWorkingData.selectedGroup.SubGroupsOne.SubGroupId;
                }
                else if ($scope.groupWorkingData.selectedGroup.SubGroupsTwo != null) {
                    $scope.groupWorkingData.selectedSubGroupId = $scope.groupWorkingData.selectedGroup.SubGroupsTwo.SubGroupId;
                }
            }

            if ($scope.groupWorkingData.selectedGroup.SubGroupsOne != null && $scope.groupWorkingData.selectedGroup.SubGroupsTwo != null) {
                $scope.subGroups = [$scope.groupWorkingData.selectedGroup.SubGroupsOne, $scope.groupWorkingData.selectedGroup.SubGroupsTwo];
            } else {
                $scope.subGroups = [];
            }

            $scope.groupWorkingData.selectedSubGroup = null;
            if ($scope.groupWorkingData.selectedSubGroupId != 0)

                if ($scope.subGroups[0] != null && $scope.groupWorkingData.selectedSubGroupId == $scope.subGroups[0].SubGroupId) {
                    $scope.groupWorkingData.selectedSubGroup = $scope.subGroups[0];
                } else if ($scope.subGroups[1] != null && $scope.groupWorkingData.selectedSubGroupId == $scope.subGroups[1].SubGroupId) {
                    $scope.groupWorkingData.selectedSubGroup = $scope.subGroups[1];
                }
        };
        $scope.userRole = 0;
		$scope.userId = 0;
        $scope.init = function (subjectId, userRole, userId) {
        	$scope.subjectId = subjectId;
        	$scope.userRole = userRole;
        	$scope.userId = userId;
        	$scope.loadGroups();
            bootbox.setDefaults({
                locale: "ru"
            });
        };

        $scope.loadGroups = function () {
            $.ajax({
                type: 'GET',
                //url: $scope.UrlServiceMain + "GetGroups/" + $scope.subjectId,
                url: $scope.UrlServiceMain + "GetGroupsV2/" + $scope.subjectId,
                dataType: "json",
                contentType: "application/json",

            }).success(function (data, status) {
                if (data.Code != '200') {
                    alertify.error(data.Message);
                } else {
                    $scope.$apply(function () {
                        $scope.groups = data.Groups;

                        if ($scope.groupWorkingData.selectedGroupId == 0) {
                            if ($scope.groups[0].SubGroupsOne != null) {
                                $scope.selectedGroupChange($scope.groups[0].GroupId, $scope.groups[0].SubGroupsOne.SubGroupId);
                            }
                            else if ($scope.groups[0].SubGroupsTwo != null) {
                                $scope.selectedGroupChange($scope.groups[0].GroupId, $scope.groups[0].SubGroupsTwo.SubGroupId);
                            } else {
                                $scope.selectedGroupChange($scope.groups[0].GroupId, null);
                            }

                        } else if ($scope.groupWorkingData.selectedSubGroupId == 0) {
                            if ($scope.groupWorkingData.selectedGroup.SubGroupsOne != null) {
                                $scope.selectedGroupChange(null, $scope.groupWorkingData.selectedGroup.SubGroupsOne.SubGroupId);
                            }
                            else if ($scope.groupWorkingData.selectedGroup.SubGroupsTwo != null) {
                                $scope.selectedGroupChange(null, $scope.groupWorkingData.selectedGroup.SubGroupsTwo.SubGroupId);
                            } else {
                                $scope.selectedGroupChange(null, null);
                            }
                        } else {
                            $scope.selectedGroupChange(null, null);
                        }

                        $scope.$broadcast('groupLoaded', '');
                    });
                }
            });
        };

        $scope.getLecturesFileAttachments = function () {
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
    })
    .controller('NewsController', function ($scope, $http) {

        $scope.news = [];
        $scope.UrlServiceNews = '/Services/News/NewsService.svc/';

        $scope.editNewsData = {
            TitleForm: "",
            Title: "",
            Body: "",
            IsOldDate: false,
			Disabled: false,
            Id: 0
        };

        $scope.init = function () {
            $scope.news = [];
            $scope.loadNews();
	        //$('#newsHtml').wysihtml5();
        };

        $scope.loadNews = function () {
            $.ajax({
                type: 'GET',
                url: $scope.UrlServiceNews + "GetNews/" + $scope.subjectId,
                dataType: "json",
                contentType: "application/json",

            }).success(function (data, status) {
                if (data.Code != '200') {
                    alertify.error(data.Message);
                } else {
                    $scope.$apply(function () {
                        $scope.news = data.News;
                    });
                }
            });
        };

        $scope.addNews = function () {
            $scope.editNewsData.TitleForm = "Создание новости";
            $scope.editNewsData.Title = "";
            $scope.editNewsData.Body = "";
            $scope.editNewsData.Id = "0";
            $scope.editNewsData.IsOldDate = false;
            $scope.editNewsData.Disabled = false;
            $("#newsIsOLd").attr('disabled', 'disabled');
            $('#dialogAddNews').modal();
        };

        $scope.editNews = function (news) {
            $scope.editNewsData.TitleForm = "Редактирование новости";
            $scope.editNewsData.Title = news.Title;
            $scope.editNewsData.Body = news.Body;
            $scope.editNewsData.Id = news.NewsId;
            $scope.editNewsData.Disabled = news.Disabled;
            $scope.editNewsData.IsOldDate = false;
            $("#newsIsOLd").removeAttr('disabled');
            $('#dialogAddNews').modal();
        };

        $scope.saveNews = function () {
        	if ($scope.editNewsData.Title == undefined || $scope.editNewsData.Title.length === 0) {
		        bootbox.dialog({
			        message: "Новость имеет пустой заголовок, сохранить?",
			        title: "Сохранение новости",
			        buttons: {
				        danger: {
					        label: "Отмена",
					        className: "btn-default btn-sm",
					        callback: function() {

					        }
				        },
				        success: {
					        label: "Сохранить",
					        className: "btn-primary btn-sm",
					        callback: function() {
					        	$scope.fSaveNews();
					        }
				        }
			        }
		        });
	        } else {
		        $scope.fSaveNews();
	        }
            
        };

		$scope.fSaveNews = function() {
			$http({
				method: 'POST',
				url: $scope.UrlServiceNews + "Save",
				data: { subjectId: $scope.subjectId, id: $scope.editNewsData.Id, title: $scope.editNewsData.Title, body: $scope.editNewsData.Body, disabled: $scope.editNewsData.Disabled, isOldDate: $scope.editNewsData.IsOldDate },
				headers: { 'Content-Type': 'application/json' }
			}).success(function(data, status) {
				if (data.Code != '200') {
					alertify.error(data.Message);
				} else {
					$scope.loadNews();
					alertify.success(data.Message);
				}
				$("#dialogAddNews").modal('hide');
			});
		};
		

        $scope.deleteNews = function (news) {
            bootbox.dialog({
                message: "Вы действительно хотите удалить новость?",
                title: "Удаление новости",
                buttons: {
                    danger: {
                        label: "Отмена",
                        className: "btn-default btn-sm",
                        callback: function () {

                        }
                    },
                    success: {
                        label: "Удалить",
                        className: "btn-primary btn-sm",
                        callback: function () {
                            $http({
                                method: 'POST',
                                url: $scope.UrlServiceNews + "Delete",
                                data: { id: news.NewsId, subjectId: $scope.subjectId },
                                headers: { 'Content-Type': 'application/json' }
                            }).success(function (data, status) {
                                if (data.Code != '200') {
                                    alertify.error(data.Message);
                                } else {
                                    alertify.success(data.Message);
                                    $scope.news.splice($scope.news.indexOf(news), 1);
                                }
                            });
                        }
                    }
                }
            });
        };

		$scope.disableNews = function() {
			$http({
				method: 'POST',
				url: $scope.UrlServiceNews + "DisableNews",
				data: { subjectId: $scope.subjectId },
				headers: { 'Content-Type': 'application/json' }
			}).success(function (data, status) {
				if (data.Code != '200') {
					alertify.error(data.Message);
				} else {
					alertify.success(data.Message);
					$scope.loadNews();
				}
			});
		};

		$scope.enableNews = function () {
			$http({
				method: 'POST',
				url: $scope.UrlServiceNews + "EnableNews",
				data: { subjectId: $scope.subjectId },
				headers: { 'Content-Type': 'application/json' }
			}).success(function (data, status) {
				if (data.Code != '200') {
					alertify.error(data.Message);
				} else {
					alertify.success(data.Message);
					$scope.loadNews();
				}
			});
		};

	})
    .controller('LecturesController', function ($scope, charting, $http, $filter) {

        $scope.lectures = [];
        $scope.UrlServiceLectures = '/Services/Lectures/LecturesService.svc/';

        $scope.editLecturesData = {
            TitleForm: "",
            Theme: "",
            Duration: "",
            PathFile: "",
            Order: "",
            Id: 0
        };

        $scope.editVisitData = {
            Date: ""
        };

        $scope.lecturesCalendar = [];

        $scope.editMarks = {
            DateId: "",
            Date: "",
            StudentMarkForDate: [],
        };

        $scope.ticks = [];
        $scope.barvalues = [];

        $scope.init = function () {
            $scope.lectures = [];
            $scope.loadLectures();
            $scope.loadCalendar();

            $scope.ticks = ['1', '2', '3'];
            $scope.barvalues = [2, 4, 6];

            var $table = $("#tableformVisiting");
            var $fixedColumn = $table.clone().insertBefore($table).addClass("fixed-column");

            $fixedColumn.find("th:not(:first-child),td:not(:first-child)").remove();

            $fixedColumn.find("tr").each(function (i, elem) {
            	$(this).height($table.find("tr:eq(" + i + ")").height());
            });

			if ($scope.groups.length > 0) {
				$scope.startSpin();
				$scope.loadLecturesMarkVisiting();
			}

            //setTimeout(function() {
            //	$scope.startSpin();
            //	$scope.loadLecturesMarkVisiting();
            //}, 1000);

            //$scope.setBarChart();
        };

		$scope.$on('groupLoaded', function (event, arg) {
			$scope.startSpin();
			$scope.loadLecturesMarkVisiting();
		});

        $scope.changeGroups = function (selectedGroup) {
        	$scope.selectedGroupChange(selectedGroup.GroupId);
	        $scope.startSpin();
	        $scope.loadLecturesMarkVisiting();
        };

        $scope.loadLecturesMarkVisiting = function () {
        	$.ajax({
        		type: 'GET',
        		url: $scope.UrlServiceMain + "GetLecturesMarkVisitingV2?subjectId=" + $scope.subjectId + "&groupId=" + $scope.groupWorkingData.selectedGroupId,
        		dataType: "json",
        		contentType: "application/json"
        	}).success(function (data, status) {
        		if (data.Code != '200') {
        			alertify.error(data.Message);
        		} else {
        			$scope.$apply(function () {
        				$scope.groupWorkingData.selectedGroup.LecturesMarkVisiting = data.GroupsVisiting[0].LecturesMarksVisiting;
        			});
        		}
		        $scope.stopSpin();
	        });
        };

        $scope.loadCalendar = function () {
            $.ajax({
                type: 'GET',
                url: $scope.UrlServiceLectures + "GetCalendar/" + $scope.subjectId,
                dataType: "json",
                contentType: "application/json",

            }).success(function (data, status) {
                if (data.Code != '200') {
                    alertify.error(data.Message);
                } else {
                    $scope.$apply(function () {
                        $scope.lecturesCalendar = data.Calendar;
                    });
                }
            });
        };

		$scope.visitingLExport = function() {
			window.location.href = "/Statistic/GetVisitLecture?subjectId=" + $scope.subjectId + "&groupId=" + $scope.groupWorkingData.selectedGroup.GroupId;
		};

        $scope.loadLectures = function () {
            $.ajax({
                type: 'GET',
                url: $scope.UrlServiceLectures + "GetLectures/" + $scope.subjectId,
                dataType: "json",
                contentType: "application/json",

            }).success(function (data, status) {
                if (data.Code != '200') {
                    alertify.error(data.Message);
                } else {
                    $scope.$apply(function () {
                        $scope.lectures = data.Lectures;
                    });
                }
            });
        };

        $scope.addLectures = function () {
            $scope.editLecturesData.TitleForm = "Создание лекции";
            $scope.editLecturesData.Theme = "";
            $scope.editLecturesData.Duration = "";
            $scope.editLecturesData.PathFile = "";
            $scope.editLecturesData.Order = "";
            $scope.editLecturesData.Id = "0";

            $("#lecturesFile").empty();

            $.ajax({
                type: 'GET',
                url: "/Subject/GetFileLectures?id=0",
                contentType: "application/json",

            }).success(function (data, status) {
                $scope.$apply(function () {
                    $("#lecturesFile").append(data);
                });
            });

            $('#dialogAddLectures').modal();
        };

        $scope.editLectures = function (lectures) {
            $scope.editLecturesData.TitleForm = "Редактирование лекции";
            $scope.editLecturesData.Theme = lectures.Theme;
            $scope.editLecturesData.Duration = lectures.Duration;
            $scope.editLecturesData.PathFile = lectures.PathFile;
            $scope.editLecturesData.Id = lectures.LecturesId;
            $scope.editLecturesData.Order = lectures.Order;

            $("#lecturesFile").empty();

            $.ajax({
                type: 'GET',
                url: "/Subject/GetFileLectures?id=" + lectures.LecturesId,
                contentType: "application/json",

            }).success(function (data, status) {
                $scope.$apply(function () {
                    $("#lecturesFile").append(data);
                });
            });
            $('#dialogAddLectures').modal();
        };

        $scope.saveLectures = function () {
            var isError = false;
            $("#dialogAddLectures .alert-error").empty();
            if ($scope.editLecturesData.Theme == "") {
                $("#dialogAddLectures .alert-error").append("<p style=\"font-\">Необходимо заполнить поле Тема лекции</p>");
                isError = true;
            }

            if ($scope.editLecturesData.Duration == "") {
                $("#dialogAddLectures .alert-error").append("<p>Необходимо заполнить поле Количество часов</p>");
                isError = true;
            }

            if (parseInt($scope.editLecturesData.Duration) < 1 || parseInt($scope.editLecturesData.Duration) > 100) {
                $("#dialogAddLectures .alert-error").append("<p>Количество часов допускается в диапазоне [1..99]</p>");
                isError = true;
            }

            if (isError) {
                $("#dialogAddLectures .alert-error").attr("style", "");
            } else {
                $("#dialogAddLectures .alert-error").attr("style", "display: none");
                $("#dialogAddLectures .alert-error").empty();
                $http({
                    method: 'POST',
                    url: $scope.UrlServiceLectures + "Save",
                    data: {
                        subjectId: $scope.subjectId,
                        id: $scope.editLecturesData.Id,
                        theme: $scope.editLecturesData.Theme,
                        duration: $scope.editLecturesData.Duration,
                        order: $scope.editLecturesData.Order,
                        pathFile: $scope.editLecturesData.PathFile,
                        attachments: $scope.getLecturesFileAttachments()
                    },
                    headers: { 'Content-Type': 'application/json' }
                }).success(function (data, status) {
                    if (data.Code != '200') {
                        alertify.error(data.Message);
                    } else {
                        $scope.loadLectures();
                        alertify.success(data.Message);
                    }
                    $("#dialogAddLectures").modal('hide');
                });
            }


        };

        $scope.deleteLectures = function (lectures) {
            bootbox.dialog({
                message: "Вы действительно хотите удалить лекцию?",
                title: "Удаление лекции",
                buttons: {
                    danger: {
                        label: "Отмена",
                        className: "btn-default btn-sm",
                        callback: function () {

                        }
                    },
                    success: {
                        label: "Удалить",
                        className: "btn-primary btn-sm",
                        callback: function () {
                            $http({
                                method: 'POST',
                                url: $scope.UrlServiceLectures + "Delete",
                                data: { id: lectures.LecturesId, subjectId: $scope.subjectId },
                                headers: { 'Content-Type': 'application/json' }
                            }).success(function (data, status) {
                                if (data.Code != '200') {
                                    alertify.error(data.Message);
                                } else {
                                    alertify.success(data.Message);
                                    $scope.lectures.splice($scope.lectures.indexOf(lectures), 1);
                                }
                            });
                        }
                    }
                }
            });
        };

        $scope.addSheduleVisitingGraph = function () {
            $('#dialogAddVisitData').modal();
        };

        $scope.addDate = function () {

	        date = $filter('date')($scope.dt, "dd/MM/yyyy");

            var isDate = false;
            $.each($scope.lecturesCalendar, function (key, value) {
                if (value.Date == date) {
                    isDate = true;
                }
            });

            if (isDate) {
                bootbox.dialog({
                    message: "Данная дата уже добавлена. Добавить еще одну такую дату?",
                    title: "Добавление даты",
                    buttons: {
                        danger: {
                            label: "Отмена",
                            className: "btn-default btn-sm",
                            callback: function () {

                            }
                        },
                        success: {
                            label: "Добавить",
                            className: "btn-primary btn-sm",
                            callback: function () {
                                $http({
                                    method: 'POST',
                                    url: $scope.UrlServiceLectures + "SaveDateLectures",
                                    data: {
                                        subjectId: $scope.subjectId,
                                        date: date
                                    },
                                    headers: { 'Content-Type': 'application/json' }
                                }).success(function (data, status) {
                                    if (data.Code != '200') {
                                        alertify.error(data.Message);
                                    } else {
                                        $scope.loadCalendar();
                                        $scope.loadLecturesMarkVisiting();
                                        alertify.success(data.Message);
                                    }
                                });
                            }
                        }
                    }
                });

            } else {
                $http({
                    method: 'POST',
                    url: $scope.UrlServiceLectures + "SaveDateLectures",
                    data: {
                        subjectId: $scope.subjectId,
                        date: date
                    },
                    headers: { 'Content-Type': 'application/json' }
                }).success(function (data, status) {
                    if (data.Code != '200') {
                        alertify.error(data.Message);
                    } else {
                        $scope.loadCalendar();
                        $scope.loadLecturesMarkVisiting();
                        alertify.success(data.Message);
                    }
                });
            }

        };

        $scope.saveVisitingMark = function () {
            $http({
                method: 'POST',
                url: $scope.UrlServiceLectures + "SaveMarksCalendarData",
                data: {
                    lecturesMarks: $scope.groupWorkingData.selectedGroup.LecturesMarkVisiting
                },
                headers: { 'Content-Type': 'application/json' }
            }).success(function (data, status) {
                if (data.Code != '200') {
                    alertify.error(data.Message);
                } else {
                    alertify.success(data.Message);
                    $scope.loadLecturesMarkVisiting();
                }
            });
        };

        //$scope.saveMarks = function () {
        //    $http({
        //        method: 'POST',
        //        url: $scope.UrlServiceLectures + "SaveMarksCalendarData",
        //        data: {
        //            dateId: $scope.editMarks.DateId,
        //            subjectId: $scope.subjectId,
        //            groupId: $scope.groupWorkingData.selectedGroup.groupId,
        //            marks: $scope.editMarks.StudentMarkForDate
        //        },
        //        headers: { 'Content-Type': 'application/json' }
        //    }).success(function (data, status) {
        //        if (data.Code != '200') {
        //            alertify.error(data.Message);
        //        } else {
        //            alertify.success(data.Message);
        //            $scope.loadGroups();
        //            $('#dialogEditMarks').modal('hide');
        //        }
        //    });
        //};

        $scope.editMarks = function (calendar) {
            var id = $scope.groupWorkingData.selectedGroup.GroupId;
            $http({
                method: 'POST',
                url: $scope.UrlServiceLectures + "GetMarksCalendarData",
                data: {
                    dateId: calendar.Id,
                    subjectId: $scope.subjectId,
                    groupId: $scope.groupWorkingData.selectedGroup.GroupId
                },
                headers: { 'Content-Type': 'application/json' }
            }).success(function (data, status) {
                if (data.Code != '200') {
                    alertify.error(data.Message);
                } else {
                    $scope.editMarks.Date = data.Date;
                    $scope.editMarks.DateId = data.DateId;
                    $scope.editMarks.StudentMarkForDate = data.StudentMarkForDate;
                    $('#dialogEditMarks').modal();
                }
            });

        };

        $scope.deleteVisitData = function (idDate) {
            bootbox.dialog({
                message: "Вы действительно хотите удалить дату и все связанные с ней данные?",
                title: "Удаление даты",
                buttons: {
                    danger: {
                        label: "Отмена",
                        className: "btn-default btn-sm",
                        callback: function () {

                        }
                    },
                    success: {
                        label: "Удалить",
                        className: "btn-primary btn-sm",
                        callback: function () {
                            $http({
                                method: 'POST',
                                url: $scope.UrlServiceLectures + "DeleteVisitingDate",
                                data: { id: idDate },
                                headers: { 'Content-Type': 'application/json' }
                            }).success(function (data, status) {
                                if (data.Code != '200') {
                                    alertify.error(data.Message);
                                } else {
                                    alertify.success(data.Message);
                                    $scope.loadCalendar();
                                    $scope.loadLecturesMarkVisiting();
                                }
                            });
                        }
                    }
                }
            });
        };

        $scope.deleteAllDates = function () {
        	bootbox.dialog({
        		message: "Вы действительно хотите удалить даты и все связанные с ними данные?",
        		title: "Удаление даты",
        		buttons: {
        			danger: {
        				label: "Отмена",
        				className: "btn-default btn-sm",
        				callback: function () {

        				}
        			},
        			success: {
        				label: "Удалить",
        				className: "btn-primary btn-sm",
        				callback: function () {
        					$http({
        						method: 'POST',
        						url: $scope.UrlServiceLectures + "DeleteVisitingDates",
        						data: { dateIds: $scope.lecturesCalendar.map(function (a) { return a.Id; }) },
        						headers: { 'Content-Type': 'application/json' }
        					}).success(function (data, status) {
        						if (data.Code != '200') {
        							alertify.error(data.Message);
        						} else {
        							alertify.success(data.Message);
        							$scope.loadCalendar();
        							$scope.loadLecturesMarkVisiting();
        						}
        					});
        				}
        			}
        		}
        	});
        };

        //$scope.setBarChart = function () {
        //    $.jqplot('barchart', [$scope.barvalues], {
        //        seriesDefaults: {
        //            renderer: $.jqplot.BarRenderer,

        //            pointLabels: { show: true, location: 'e', edgeTolerance: -15 },

        //        },
        //        axes: {
        //            xaxis: {
        //                renderer: $.jqplot.CategoryAxisRenderer,
        //                ticks: $scope.ticks
        //            }
        //        }
        //    });
        //};

    })
    .controller('LabsController', function ($scope, $http, $filter, usSpinnerService) {

        $scope.labs = [];

        $scope.studentId = 0;
        $scope.fileId = 0;
        $scope.file = null;
        $scope.IsRet = false;

        $scope.UrlServiceLabs = '/Services/Labs/LabsService.svc/';

        $scope.editLabsData = {
            TitleForm: "",
            Theme: "",
            Duration: "",
            Order: 0,
            PathFile: "",
            ShortName: "",
            Id: 0
        };

        $scope.editFileSend = {
            Comments: "",
            PathFile: "",
            Id: 0
        };

        $scope.labFilesUser = [];

        $scope.editMarksVisiting = [];

        $scope.init = function () {
            $scope.labs = [];
            $scope.loadLabs();
            if ($scope.userRole == "1") {
                $scope.loadFilesLabUser();
            };

            if ($scope.groups.length > 0) {
                $scope.reload();
            }
        };

        $scope.resultPlagiatium = [];

        $scope.checkPlagiarism = function (id, files, userName) {
            $scope.userFileIdCheck = id;
            $scope.plagiat = {
                FileName: files.Attachments[0].Name,
                UserName: userName
            };

            $('#dialogPlagiarism').modal();

            $(".loadingP").toggleClass('ng-hide', false);
            usSpinnerService.spin('spinner-1');

            $http({
                method: 'POST',
                url: $scope.UrlServiceLabs + "CheckPlagiarism",
                data: {
                    userFileId: id,
                    subjectId: $scope.subjectId
                },
                headers: { 'Content-Type': 'application/json' }
            }).success(function (data, status) {
                if (data.Code != '200') {
                    alertify.error(data.Message);
                } else {
                    alertify.success(data.Message);

                    $scope.resultPlagiatium = data.DataD;

                }
                usSpinnerService.stop('spinner-1');
                $(".loadingP").toggleClass('ng-hide', true);

            });
        };

        $scope.checkPlagiarismSubject = function () {
            $scope.resultPlagiatiumSybject = [];
            $('#dialogPlagiarismSubject').modal();

        };

        $scope.loadPlagiarismSubject = function () {
            $('.number-spinner').find('button').prop("disabled", true);
            $(".loadingPSubject").toggleClass('ng-hide', false);
            usSpinnerService.spin('spinner-1');
            $scope.resultPlagiatiumSybject = [];
            $http({
                method: 'POST',
                url: $scope.UrlServiceLabs + "CheckPlagiarismSubjects",
                data: {
                    subjectId: $scope.subjectId,
                    type: $("input[name=typePlagiarism]:checked").val(),
                    threshold: $("#threshold").val()
                },
                headers: { 'Content-Type': 'application/json' }
            }).success(function (data, status) {
                if (data.Code != '200') {
                    alertify.error(data.Message);
                } else {
                    alertify.success(data.Message);

                    $scope.resultPlagiatiumSybject = data.DataD;

                }
                usSpinnerService.stop('spinner-1');
                $(".loadingPSubject").toggleClass('ng-hide', true);
                $('.number-spinner').find('button').prop("disabled", false);
            });
        };

        $scope.exportPlagiarism = function () {
            window.location.href = "/Statistic/ExportPlagiarism?subjectId=" + $scope.subjectId + "&type=" + $("input[name=typePlagiarism]:checked").val() + "&threshold=" + $("#threshold").val();
        },

        $scope.exportPlagiarismStudent = function () {
                window.location.href = "/Statistic/ExportPlagiarismStudent?userFileId=" + $scope.userFileIdCheck + "&subjectId=" + $scope.subjectId;
            },

        $scope.receivedLabFile = function (id, files) {
                $http({
                    method: 'POST',
                    url: $scope.UrlServiceLabs + "ReceivedLabFile",
                    data: {
                        userFileId: id
                    },
                    headers: { 'Content-Type': 'application/json' }
                }).success(function (data, status) {
                    if (data.Code != '200') {
                        alertify.error(data.Message);
                    } else {
                        //$scope.$apply(function () {
                        //$scope.reloadFiles();
                        files.IsReceived = true;
                        files.IsReturned = false;
                        //});
                        alertify.success(data.Message);
                    }
                });
            };

        $scope.cancelReceivedLabFile = function (id, files) {
            $http({
                method: 'POST',
                url: $scope.UrlServiceLabs + "CancelReceivedLabFile",
                data: {
                    userFileId: id
                },
                headers: { 'Content-Type': 'application/json' }
            }).success(function (data, status) {
                if (data.Code != '200') {
                    alertify.error(data.Message);
                } else {
                    //$scope.$apply(function () {
                    //$scope.reloadFiles();
                    files.IsReceived = false;
                    //});
                    alertify.success(data.Message);
                }
            });
        };

        $scope.reload = function () {
            $scope.startSpin();
            // performance issue
            $scope.loadLabsV2();
            $scope.loadFilesV2();
        };

        $scope.$on('groupLoaded', function (event, arg) {
            $scope.reload();
        });

        //(student.LabsMarkTotal * student.TestMark) / 2
        $scope.ratingMark = function (student) {
            if ((student.LabsMarkTotal == null || student.LabsMarkTotal.length == 0) && (student.TestMark == null || student.TestMark.length == 0)) return "";

            if ((student.LabsMarkTotal == null || student.LabsMarkTotal.length == 0) && (student.TestMark != null && student.TestMark.length != 0)) {
                return student.TestMark;
            }
            else if ((student.LabsMarkTotal != null && student.LabsMarkTotal.length != 0) && (student.TestMark == null || student.TestMark.length == 0)) {
                return student.LabsMarkTotal;
            }

            return ((parseFloat(student.LabsMarkTotal) + parseFloat(student.TestMark)) / 2).toFixed(1);
        };

        $scope.visitingLabsExport = function () {
            window.location.href = "/Statistic/GetVisitLabs?subjectId=" + $scope.subjectId + "&groupId=" + $scope.groupWorkingData.selectedGroup.GroupId + "&subGroupOneId=" + $scope.groupWorkingData.selectedGroup.SubGroupsOne.SubGroupId + "&subGroupTwoId=" + $scope.groupWorkingData.selectedGroup.SubGroupsTwo.SubGroupId;
        };

        $scope.LabMarkExport = function () {
            window.location.href = "/Statistic/GetLabsMarks?subjectId=" + $scope.subjectId + "&groupId=" + $scope.groupWorkingData.selectedGroup.GroupId;
        };

        $scope.loadFilesLabUser = function () {
            $http({
                method: 'POST',
                url: $scope.UrlServiceLabs + "GetFilesLab",
                data: {
                    userId: $scope.userId,
                    subjectId: $scope.subjectId
                },
                headers: { 'Content-Type': 'application/json' }
            }).success(function (data, status) {
                if (data.Code != '200') {
                    alertify.error(data.Message);
                } else {
                    //$scope.$apply(function () {
                    $scope.labFilesUser = data.UserLabFiles;
                    //});
                    alertify.success(data.Message);
                }
            });
        };

        $scope.addLabFiles = function () {
            window.maxNumberOfFiles = 1;
            $scope.editFileSend.Comments = "";
            $scope.editFileSend.PathFile = "";
            $scope.editFileSend.Id = "0";

            $("#labsFile").empty();

            $.ajax({
                type: 'GET',
                url: "/Subject/GetUserFilesLab?id=0",
                contentType: "application/json",

            }).success(function (data, status) {
                $scope.$apply(function () {
                    $("#labsFile").append(data);
                });
            });

            $('#dialogAddFiles').modal();
        };

        $scope.editLabFiles = function (file) {
            $scope.editFileSend.Comments = file.Comments;
            $scope.editFileSend.PathFile = file.PathFile;
            $scope.editFileSend.Id = file.Id;

            $("#labsFile").empty();

            $.ajax({
                type: 'GET',
                url: "/Subject/GetUserFilesLab?id=" + file.Id,
                contentType: "application/json",

            }).success(function (data, status) {
                $scope.$apply(function () {
                    $("#labsFile").append(data);
                });
            });
            $('#dialogAddFiles').modal();
        };

        $scope.saveLabFiles = function (id) {

            if ($scope.userRole == "1" && ($scope.editFileSend.Comments == null || $scope.editFileSend.Comments.length == 0 || JSON.parse($scope.getLecturesFileAttachments()).length == 0)) {
                bootbox.alert("Необходимо заполнить поля и прикрепить файлы.");
                return false;
            }
            if ($scope.userRole == "0" && JSON.parse($scope.getLecturesFileAttachments()).length == 0) {
                bootbox.alert("Необходимо прикрепить файлы.");
                return false;
            }
            if (typeof id === 'undefined')
                id = $scope.userId;
            $http({
                method: 'POST',
                url: $scope.UrlServiceLabs + "SendFile",
                data: {
                    subjectId: $scope.subjectId,
                    userId: id,
                    id: $scope.editFileSend.Id,
                    comments: $scope.editFileSend.Comments,
                    pathFile: $scope.editFileSend.PathFile,
                    attachments: $scope.getLecturesFileAttachments(),
                    isCp: false,
                    isRet: $scope.IsRet
                },
                headers: { 'Content-Type': 'application/json' }
            }).success(function (data, status) {
                if (data.Code != '200') {
                    alertify.error(data.Message);
                    $scope.IsRet = false;
                } else {
                    $scope.loadFilesLabUser();
                    alertify.success(data.Message);
                }
                $("#dialogAddFiles").modal('hide');
            });
        };

        $scope.replaceLabFiles = function (userId) {
            if ($scope.userRole == "0" && JSON.parse($scope.getLecturesFileAttachments()).length == 0) {
                bootbox.alert("Необходимо прикрепить файлы.");
                return false;
            }
            $http({
                method: 'POST',
                url: $scope.UrlServiceLabs + "DeleteUserFile",
                data: { id: $scope.fileId },
                headers: { 'Content-Type': 'application/json' }
            }).success(function (data, status) {
                if (data.Code != '200') {
                    alertify.error(data.Message);
                } else {
                    alertify.success("Работа возвращена");
                    $scope.labFilesUser.splice($scope.labFilesUser.indexOf($scope.file), 1);
                    $scope.IsRet = true;
                    $scope.file.IsReturned = true;
                    $scope.saveLabFiles(userId);

                }
            });
        };

        $scope.returnFile = function (id, file, studentId) {
            $scope.fileId = id;
            $scope.file = file;
            $scope.studentId = studentId;
            $scope.addLabFiles();
        };

        $scope.loadLabs = function () {
            $.ajax({
                type: 'GET',
                url: $scope.UrlServiceLabs + "GetLabs/" + $scope.subjectId,
                dataType: "json",
                contentType: "application/json",

            }).success(function (data, status) {
                if (data.Code != '200') {
                    alertify.error(data.Message);
                } else {
                    $scope.$apply(function () {
                        $scope.labs = data.Labs;
                    });
                }
            });
        };

        $scope.addLabs = function () {
            $scope.editLabsData.TitleForm = "Создание лабораторной работы";
            $scope.editLabsData.Theme = "";
            $scope.editLabsData.Duration = "";
            $scope.editLabsData.PathFile = "";
            $scope.editLabsData.ShortName = "";
            $scope.editLabsData.Id = 0;
            $scope.editLabsData.Order = 0;

            $("#attachedFiles").empty();

            $.ajax({
                type: 'GET',
                url: "/Subject/GetFileLabs?id=0",
                contentType: "application/json",

            }).success(function (data, status) {
                $scope.$apply(function () {
                    $("#attachedFiles").append(data);
                });
            });

            $('#dialogAddLabs').modal();
        };

        $scope.editLabs = function (lab) {
            $scope.editLabsData.TitleForm = "Редактирование лабораторной работы";
            $scope.editLabsData.Theme = lab.Theme;
            $scope.editLabsData.Duration = lab.Duration;
            $scope.editLabsData.PathFile = lab.PathFile;
            $scope.editLabsData.ShortName = lab.ShortName;
            $scope.editLabsData.Id = lab.LabId;
            $scope.editLabsData.Order = lab.Order;

            $("#attachedFiles").empty();

            $.ajax({
                type: 'GET',
                url: "/Subject/GetFileLabs?id=" + lab.LabId,
                contentType: "application/json",

            }).success(function (data, status) {
                $scope.$apply(function () {
                    $("#attachedFiles").append(data);
                });
            });
            $('#dialogAddLabs').modal();
        };

        $scope.saveLabs = function () {

            $http({
                method: 'POST',
                url: $scope.UrlServiceLabs + "Save",
                data: {
                    subjectId: $scope.subjectId,
                    id: $scope.editLabsData.Id,
                    theme: $scope.editLabsData.Theme,
                    duration: $scope.editLabsData.Duration,
                    order: $scope.editLabsData.Order,
                    shortName: $scope.editLabsData.ShortName,
                    pathFile: $scope.editLabsData.PathFile,
                    attachments: $scope.getLecturesFileAttachments()
                },
                headers: { 'Content-Type': 'application/json' }
            }).success(function (data, status) {
                if (data.Code != '200') {
                    alertify.error(data.Message);
                } else {
                    $scope.loadLabs();
                    $scope.loadLabsV2();
                    alertify.success(data.Message);
                }
                $("#dialogAddLabs").modal('hide');
            });
        };

        $scope.deleteLabs = function (lab) {
            bootbox.dialog({
                message: "Вы действительно хотите удалить лабораторную работу?",
                title: "Удаление лабораторной работы",
                buttons: {
                    danger: {
                        label: "Отмена",
                        className: "btn-default btn-sm",
                        callback: function () {

                        }
                    },
                    success: {
                        label: "Удалить",
                        className: "btn-primary btn-sm",
                        callback: function () {
                            $http({
                                method: 'POST',
                                url: $scope.UrlServiceLabs + "Delete",
                                data: { id: lab.LabId, subjectId: $scope.subjectId },
                                headers: { 'Content-Type': 'application/json' }
                            }).success(function (data, status) {
                                if (data.Code != '200') {
                                    alertify.error(data.Message);
                                } else {
                                    alertify.success(data.Message);
                                    $scope.loadLabsV2();
                                    $scope.labs.splice($scope.labs.indexOf(lab), 1);
                                }
                            });
                        }
                    }
                }
            });
        };

        $scope.deleteUserFile = function (file) {
            bootbox.dialog({
                message: "Вы действительно хотите удалить работу?",
                title: "Удаление работы",
                buttons: {
                    danger: {
                        label: "Отмена",
                        className: "btn-default btn-sm",
                        callback: function () {

                        }
                    },
                    success: {
                        label: "Удалить",
                        className: "btn-primary btn-sm",
                        callback: function () {
                            $http({
                                method: 'POST',
                                url: $scope.UrlServiceLabs + "DeleteUserFile",
                                data: { id: file.Id },
                                headers: { 'Content-Type': 'application/json' }
                            }).success(function (data, status) {
                                if (data.Code != '200') {
                                    alertify.error(data.Message);
                                } else {
                                    alertify.success(data.Message);
                                    $scope.labFilesUser.splice($scope.labFilesUser.indexOf(file), 1);
                                }
                            });
                        }
                    }
                }
            });
        };

        $scope.changeGroups = function (selectedGroup) {
            if (selectedGroup.SubGroupsOne != null) {
                $scope.selectedGroupChange(selectedGroup.GroupId, selectedGroup.SubGroupsOne.SubGroupId);
            }
            else if (selectedGroup.SubGroupsTwo != null) {
                $scope.selectedGroupChange(selectedGroup.GroupId, selectedGroup.SubGroupsTwo.SubGroupId);
            } else {
                $scope.selectedGroupChange(selectedGroup.GroupId, null);
            }
            $scope.startSpin();
            // performance issue
            $scope.loadLabsV2();
            $scope.loadFilesV2();
            // end performance issue

        };

        $scope.reloadFiles = function () {
            $scope.startSpin();
            $scope.loadFilesV2();
        };

        $scope.loadFilesV2 = function () {

            $.ajax({
                type: 'GET',
                url: $scope.UrlServiceLabs + "GetFilesV2?subjectId=" + $scope.subjectId + "&groupId=" + $scope.groupWorkingData.selectedGroupId,
                dataType: "json",
                contentType: "application/json",

            }).success(function (data, status) {
                if (data.Code != '200') {
                    alertify.error(data.Message);
                } else {
                    $scope.$apply(function () {
                        $scope.groupWorkingData.selectedGroup.StudentsFilesV2_1 = $filter("filter")(data.Students, function (r) {
                            return r.SubGroup === 1;
                        });

                        $scope.groupWorkingData.selectedGroup.StudentsFilesV2_2 = $filter("filter")(data.Students, function (r) {
                            return r.SubGroup === 2;
                        });

                        $scope.groupWorkingData.selectedGroup.StudentsFilesV2_3 = $filter("filter")(data.Students, function (r) {
                            return r.SubGroup === 3;
                        });

                    });
                }
                $scope.stopSpin();
            });
        };

        $scope.loadLabsV2 = function () {

            $.ajax({
                type: 'GET',
                url: $scope.UrlServiceLabs + "GetLabsV2?subjectId=" + $scope.subjectId + "&groupId=" + $scope.groupWorkingData.selectedGroupId,
                dataType: "json",
                contentType: "application/json",

            }).success(function (data, status) {
                if (data.Code != '200') {
                    alertify.error(data.Message);
                } else {
                    $scope.$apply(function () {
                        $scope.groupWorkingData.selectedGroup.SubGroupsOne.LabsV2 = $filter("filter")(data.Labs, function (r) {
                            return r.SubGroup === 1;
                        });
                        $scope.groupWorkingData.selectedGroup.SubGroupsTwo.LabsV2 = $filter("filter")(data.Labs, function (r) {
                            return r.SubGroup === 2;
                        });

                        $scope.groupWorkingData.selectedGroup.SubGroupsThird.LabsV2 = $filter("filter")(data.Labs, function (r) {
                            return r.SubGroup === 3;
                        });

                        $scope.groupWorkingData.selectedGroup.SubGroupsOne.ScheduleProtectionLabsV2 = $filter("filter")(data.ScheduleProtectionLabs, function (r) {
                            return r.SubGroup === 1;
                        });
                        $scope.groupWorkingData.selectedGroup.SubGroupsTwo.ScheduleProtectionLabsV2 = $filter("filter")(data.ScheduleProtectionLabs, function (r) {
                            return r.SubGroup === 2;
                        });

                        $scope.groupWorkingData.selectedGroup.SubGroupsThird.ScheduleProtectionLabsV2 = $filter("filter")(data.ScheduleProtectionLabs, function (r) {
                            return r.SubGroup === 3;
                        });
                    });
                    $scope.loadStudentResults();
                }
            });
        };

        $scope.loadStudentResults = function () {
            $.ajax({
                type: 'GET',
                url: $scope.UrlServiceLabs + "GetMarksV2?subjectId=" + $scope.subjectId + "&groupId=" + $scope.groupWorkingData.selectedGroupId,
                dataType: "json",
                contentType: "application/json",

            }).success(function (data, status) {
                if (data.Code != '200') {
                    alertify.error(data.Message);
                } else {
                    $scope.$apply(function () {
                        $scope.groupWorkingData.selectedGroup.SubGroupsOne.StudentsV2 = $filter("filter")(data.Students, function (r) {
                            return r.SubGroup === 1;
                        });
                        $scope.groupWorkingData.selectedGroup.SubGroupsTwo.StudentsV2 = $filter("filter")(data.Students, function (r) {
                            return r.SubGroup === 2;
                        });

                        $scope.groupWorkingData.selectedGroup.SubGroupsThird.StudentsV2 = $filter("filter")(data.Students, function (r) {
                            return r.SubGroup === 3;
                        });

                        $scope.groupWorkingData.selectedGroup.StudentsV2 = $filter("filter")(data.Students, function (r) {
                            return r.SubGroup !== 4;
                        });
                    });
                    $scope.stopSpin();
                }
            });
        }

        $scope.changeSubGroup = function (selectedSubGroup) {
            $scope.groupWorkingData.selectedSubGroup = selectedSubGroup;
            $scope.selectedGroupChange(null, selectedSubGroup.SubGroupId);
        };

        $scope.saveVisitingMarkSubOne = function (key, markId, commentId, studId) {
            var index = document.getElementById(key).value;
            var arrMark = new Array(index); var arrComment = new Array(index); var arrStudentId = new Array(index); var arrId = new Array(index);
            var mark, comment, id, studentId;
            var dateId = document.getElementById('dateVisitingMarkOne').value;

            for (var i = 0; i <= index; i++) {
                id = markId + i;
                mark = document.getElementById(id).value;
                id = commentId + i;
                comment = document.getElementById(id).value;
                id = studId + i;
                studentId = document.getElementById(id).value;
                arrMark[i] = mark;
                arrComment[i] = comment;
                arrStudentId[i] = studentId;

                $.each($scope.groupWorkingData.selectedGroup.SubGroupsOne.StudentsV2, function (key, student) {
                    if (student.StudentId == studentId) {
                        $.each(student.LabVisitingMark, function (key, labVisiting) {
                            if (labVisiting.ScheduleProtectionLabId == dateId) {
                                arrId[i] = labVisiting.LabVisitingMarkId;
                            }
                        })
                    }
                })
            }

            $http({
                method: 'POST',
                url: $scope.UrlServiceLabs + "SaveLabsVisitingData",
                data: {
                    dateId: dateId,
                    marks: arrMark,
                    comments: arrComment,
                    studentsId: arrStudentId,
                    Id: arrId,
                    students: $scope.groupWorkingData.selectedGroup.SubGroupsOne.StudentsV2
                },
                headers: { 'Content-Type': 'application/json' }
            }).success(function (data, status) {
                if (data.Code != '200') {
                    alertify.error(data.Message);
                } else {
                    alertify.success(data.Message);
                    $scope.loadLabsV2();
                }
            });
        };


        $scope.saveVisitingMarkSubTwo = function (key, markId, commentId, studId) {
            var index = document.getElementById(key).value;
            var arrMark = new Array(index); var arrComment = new Array(index); var arrStudentId = new Array(index); var arrId = new Array(index);
            var mark, comment, id, studentId;
            var dateId = document.getElementById('dateVisitingMarkTwo').value;

            for (var i = 0; i <= index; i++) {
                id = markId + i;
                mark = document.getElementById(id).value;
                id = commentId + i;
                comment = document.getElementById(id).value;
                id = studId + i;
                studentId = document.getElementById(id).value;
                arrMark[i] = mark;
                arrComment[i] = comment;
                arrStudentId[i] = studentId;

                $.each($scope.groupWorkingData.selectedGroup.SubGroupsTwo.StudentsV2, function (key, student) {
                    if (student.StudentId == studentId) {
                        $.each(student.LabVisitingMark, function (key, labVisiting) {
                            if (labVisiting.ScheduleProtectionLabId == dateId) {
                                arrId[i] = labVisiting.LabVisitingMarkId;
                            }
                        })
                    }
                })
            }

            $http({
                method: 'POST',
                url: $scope.UrlServiceLabs + "SaveLabsVisitingData",
                data: {
                    dateId: dateId,
                    marks: arrMark,
                    comments: arrComment,
                    studentsId: arrStudentId,
                    Id: arrId,
                    students: $scope.groupWorkingData.selectedGroup.SubGroupsTwo.StudentsV2
                },
                headers: { 'Content-Type': 'application/json' }
            }).success(function (data, status) {
                if (data.Code != '200') {
                    alertify.error(data.Message);
                } else {
                    alertify.success(data.Message);
                    $scope.loadLabsV2();
                }
            });
        };

        $scope.saveVisitingMarkSubThird = function (key, markId, commentId, studId) {
            var index = document.getElementById(key).value;
            var arrMark = new Array(index); var arrComment = new Array(index); var arrStudentId = new Array(index); var arrId = new Array(index);
            var mark, comment, id, studentId;
            var dateId = document.getElementById('dateVisitingMarkThird').value;

            for (var i = 0; i <= index; i++) {
                id = markId + i;
                mark = document.getElementById(id).value;
                id = commentId + i;
                comment = document.getElementById(id).value;
                id = studId + i;
                studentId = document.getElementById(id).value;
                arrMark[i] = mark;
                arrComment[i] = comment;
                arrStudentId[i] = studentId;

                $.each($scope.groupWorkingData.selectedGroup.SubGroupsThird.StudentsV2, function (key, student) {
                    if (student.StudentId == studentId) {
                        $.each(student.LabVisitingMark, function (key, labVisiting) {
                            if (labVisiting.ScheduleProtectionLabId == dateId) {
                                arrId[i] = labVisiting.LabVisitingMarkId;
                            }
                        })
                    }
                })
            }

            $http({
                method: 'POST',
                url: $scope.UrlServiceLabs + "SaveLabsVisitingData",
                data: {
                    dateId: dateId,
                    marks: arrMark,
                    comments: arrComment,
                    studentsId: arrStudentId,
                    Id: arrId,
                    students: $scope.groupWorkingData.selectedGroup.SubGroupsThird.StudentsV2
                },
                headers: { 'Content-Type': 'application/json' }
            }).success(function (data, status) {
                if (data.Code != '200') {
                    alertify.error(data.Message);
                } else {
                    alertify.success(data.Message);
                    $scope.loadLabsV2();
                }
            });
        };

        $scope.commentImage = function (comment, studentIndex, markIndex, name) {
            var id = name + studentIndex + markIndex;
            var elem = document.getElementById(id);

            if (elem !== null) {
                if (comment != "" & comment != null) {
                    elem.style.display = 'block';
                    elem.title = comment;
                }
                else {
                    elem.style.display = 'none';
                    elem.title = null;
                }
            }
        };

        $scope.NumberControl = function (object, errorName) {
            var error = document.getElementById(errorName);
            if (object.value === "") {
                object.value = "";
                error.style.display = 'none';
            } else {
                if (parseInt(object.value) < object.min) {
                    object.value = object.min;
                    error.style.display = 'block';
                } else {
                    if (parseInt(object.value) > object.max) {
                        object.value = object.max;
                        error.style.display = 'block';
                    }
                    else { error.style.display = 'none'; }
                }
            }
        };


        $scope.dotImageTwo = function (labId, studentId, studentIndex, markIndex, name) {
            var id = name + studentIndex + markIndex;
            var elem = document.getElementById(id);
            var labsDate = new Array();
            var sum = 0;
            var missedDates = new Array();
            if (elem === null) return;
            $.each($scope.groupWorkingData.selectedGroup.SubGroupsTwo.LabsV2, function (key, labs) {
                if (labs.LabId === labId) {
                    $.each(labs.ScheduleProtectionLabsRecomend, function (key, recom) {
                        if (recom.Mark === '10') {
                            labsDate.push(recom.ScheduleProtectionId);
                        }
                    })
                }
            })

            $.each($scope.groupWorkingData.selectedGroup.SubGroupsTwo.StudentsV2, function (key, student) {
                if (student.StudentId == studentId) {
                    $.each(student.LabVisitingMark, function (key, lVisiting) {
                        for (var i = 0; i < labsDate.length; i++) {
                            if (lVisiting.ScheduleProtectionLabId == labsDate[i]) {
                                if (lVisiting.Mark !== "") {
                                    sum = sum + parseInt(lVisiting.Mark);
                                    $.each($scope.groupWorkingData.selectedGroup.SubGroupsTwo.ScheduleProtectionLabsV2, function (key, date) {
                                        if (date.ScheduleProtectionLabId === lVisiting.ScheduleProtectionLabId) {
                                            missedDates.push(date.Date);
                                        }
                                    })
                                }
                            }
                        }
                    })

                }
            })

            if (missedDates.length !== 0) {
                var text = "";
                for (var i = 0; i < missedDates.length; i++) {

                    if (i === 0) {
                        text = text + missedDates[i];
                    }
                    else {
                        text = text + ", " + missedDates[i];
                    }

                }

                elem.style.display = 'block';
                elem.title = "Пропустил(a) " + sum + " часа(ов): " + text;
            }
        };

        $scope.dotImageOne = function (labId, studentId, studentIndex, markIndex, name) {
            var id = name + studentIndex + markIndex;
            var elem = document.getElementById(id);
            var labsDate = new Array();
            var sum = 0;
            var missedDates = new Array();
            if (elem === null) return;
            $.each($scope.groupWorkingData.selectedGroup.SubGroupsOne.LabsV2, function (key, labs) {
                if (labs.LabId === labId) {
                    $.each(labs.ScheduleProtectionLabsRecomend, function (key, recom) {
                        if (recom.Mark === '10') {
                            labsDate.push(recom.ScheduleProtectionId);
                        }
                    })
                }
            })

            $.each($scope.groupWorkingData.selectedGroup.SubGroupsOne.StudentsV2, function (key, student) {
                if (student.StudentId == studentId) {
                    $.each(student.LabVisitingMark, function (key, lVisiting) {
                        for (var i = 0; i < labsDate.length; i++) {
                            if (lVisiting.ScheduleProtectionLabId === labsDate[i]) {
                                if (lVisiting.Mark !== "") {
                                    sum = sum + parseInt(lVisiting.Mark);
                                    $.each($scope.groupWorkingData.selectedGroup.SubGroupsOne.ScheduleProtectionLabsV2, function (key, date) {
                                        if (date.ScheduleProtectionLabId === lVisiting.ScheduleProtectionLabId) {
                                            missedDates.push(date.Date);
                                        }
                                    })
                                }
                            }
                        }
                    })

                }
            })

            if (missedDates.length !== 0) {
                var text = "";
                for (var i = 0; i < missedDates.length; i++) {

                    if (i === 0) {
                        text = text + missedDates[i];
                    }
                    else {
                        text = text + ", " + missedDates[i];
                    }

                }

                elem.style.display = 'block';
                elem.title = "Пропустил(a) " + sum + " часа(ов): " + text;
            }
        };

        $scope.dotImageThird = function (labId, studentId, studentIndex, markIndex, name) {
            var id = name + studentIndex + markIndex;
            var elem = document.getElementById(id);
            var labsDate = new Array();
            var sum = 0;
            var missedDates = new Array();
            if (elem === null) return;
            $.each($scope.groupWorkingData.selectedGroup.SubGroupsThird.LabsV2, function (key, labs) {
                if (labs.LabId === labId) {
                    $.each(labs.ScheduleProtectionLabsRecomend, function (key, recom) {
                        if (recom.Mark === '10') {
                            labsDate.push(recom.ScheduleProtectionId);
                        }
                    })
                }
            })

            $.each($scope.groupWorkingData.selectedGroup.SubGroupsThird.StudentsV2, function (key, student) {
                if (student.StudentId == studentId) {
                    $.each(student.LabVisitingMark, function (key, lVisiting) {
                        for (var i = 0; i < labsDate.length; i++) {
                            if (lVisiting.ScheduleProtectionLabId === labsDate[i]) {
                                if (lVisiting.Mark !== "") {
                                    sum = sum + parseInt(lVisiting.Mark);
                                    $.each($scope.groupWorkingData.selectedGroup.SubGroupsThird.ScheduleProtectionLabsV2, function (key, date) {
                                        if (date.ScheduleProtectionLabId === lVisiting.ScheduleProtectionLabId) {
                                            missedDates.push(date.Date);
                                        }
                                    })
                                }
                            }
                        }
                    })

                }
            })

            if (missedDates.length !== 0) {
                var text = "";
                for (var i = 0; i < missedDates.length; i++) {

                    if (i === 0) {
                        text = text + missedDates[i];
                    }
                    else {
                        text = text + ", " + missedDates[i];
                    }

                }

                elem.style.display = 'block';
                elem.title = "Пропустил(a) " + sum + " часа(ов): " + text;
            }
        };

        $scope.managementDateSubOne = function () {
            $('#dialogmanagementDataSubOne').modal();
        };


        $scope.managementDateSubTwo = function () {
            $('#dialogmanagementDataSubTwo').modal();
        };

        $scope.managementDateSubThird = function () {
            $('#dialogmanagementDataSubThird').modal();
        };

        $scope.exposeMarkSubOne = function (mark, comment, date, labId, studentId, Id, studentIndex, markIndex) {
            $("#errorOneMark").hide();
            if (mark) {
                $('#markInputOne').val(mark);
                $('#markInputOneSpan').text("");
            }
            else {
                var cell = document.getElementById("markOne" + studentIndex + markIndex);
                var recomendedMark = cell.innerHTML;
                $('#markInputOneSpan').text("Рекомендованная оценка - " + recomendedMark);
                $('#markInputOne').val("");
            }

            $('#commentInputOne').val(comment);
            if (date == '' || date == null) {
                $('#dateInputOne').val($scope.getCurentDate());
            }
            else {
                $('#dateInputOne').val(date);
            }

            $('#inputLabIdOne').val(labId);
            $('#inputStudentIdOne').val(studentId);
            $('#inputIdOne').val(Id);
            $('#dialogexposeMarkOne').modal();
        };

        $scope.exposeMarkSubTwo = function (mark, comment, date, labId, studentId, Id, studentIndex, markIndex) {
            $("#errorTwoMark").hide();
            if (mark) {
                $('#markInputTwo').val(mark);
                $('#markInputTwoSpan').text("");
            }
            else {
                var cell = document.getElementById("markTwo" + studentIndex + markIndex);
                var recomendedMark = cell.innerHTML;
                $('#markInputOne').val("");
                $('#markInputTwoSpan').text("Рекомендованная оценка - " + recomendedMark);
            }

            $('#commentInputTwo').val(comment);
            if (date == '' || date == null) {
                $('#dateInputTwo').val($scope.getCurentDate());
            }
            else {
                $('#dateInputTwo').val(date);
            }

            $('#inputLabIdTwo').val(labId);
            $('#inputStudentIdTwo').val(studentId);
            $('#inputIdTwo').val(Id);
            $('#dialogexposeMarkTwo').modal();
        };

        $scope.exposeMarkSubThird = function (mark, comment, date, labId, studentId, Id, studentIndex, markIndex) {
            $("#errorThirdMark").hide();
            if (mark) {
                $('#markInputThird').val(mark);
                $('#markInputThirdSpan').text("");
            }
            else {
                var cell = document.getElementById("markThird" + studentIndex + markIndex);
                var recomendedMark = cell.innerHTML;

                $('#markInputThirdSpan').text("Рекомендованная оценка - " + recomendedMark);
                $('#markInputOne').val("");
            }

            $('#commentInputThird').val(comment);
            if (date == '' || date == null) {
                $('#dateInputThird').val($scope.getCurentDate());
            }
            else {
                $('#dateInputThird').val(date);
            }

            $('#inputLabIdThird').val(labId);
            $('#inputStudentIdThird').val(studentId);
            $('#inputIdThird').val(Id);
            $('#dialogexposeMarkThird').modal();
        };

        $scope.exposeMarkVisitingOne = function (dateId) {
            var elem = document.getElementById('textDateOne');
            $.each($scope.groupWorkingData.selectedGroup.SubGroupsOne.ScheduleProtectionLabsV2, function (key, date) {
                if (date.ScheduleProtectionLabId == dateId) {
                    elem.innerHTML = date.Date;
                }
            })

            $('#dateVisitingMarkOne').val(dateId);
            $('#dialogexposeVisitingMarkOne').modal();
        };

        $scope.exposeMarkVisitingTwo = function (dateId) {
            var elem = document.getElementById('textDateTwo');
            $.each($scope.groupWorkingData.selectedGroup.SubGroupsTwo.ScheduleProtectionLabsV2, function (key, date) {
                if (date.ScheduleProtectionLabId == dateId) {
                    elem.innerHTML = date.Date;
                }
            })
            $('#dateVisitingMarkTwo').val(dateId);
            $('#dialogexposeVisitingMarkTwo').modal();
        };

        $scope.exposeMarkVisitingThird = function (dateId) {
            var elem = document.getElementById('textDateThird');
            $.each($scope.groupWorkingData.selectedGroup.SubGroupsThird.ScheduleProtectionLabsV2, function (key, date) {
                if (date.ScheduleProtectionLabId == dateId) {
                    elem.innerHTML = date.Date;
                }
            })

            $('#dateVisitingMarkThird').val(dateId);
            $('#dialogexposeVisitingMarkThird').modal();
        };

        $scope.visitingMarkOne = function (studentId, name, index) {
            var id = name + index;
            var elem = document.getElementById(id);
            var dateId = document.getElementById('dateVisitingMarkOne').value;

            $.each($scope.groupWorkingData.selectedGroup.SubGroupsOne.StudentsV2, function (key, student) {
                if (student.StudentId == studentId) {
                    $.each(student.LabVisitingMark, function (key, labVisitingMark) {
                        if (labVisitingMark.ScheduleProtectionLabId == dateId) {
                            elem.value = labVisitingMark.Mark;
                        }
                    })
                }
            })
        };
        $scope.visitingCommentOne = function (studentId, name, index) {
            var id = name + index;
            var elem = document.getElementById(id);
            var dateId = document.getElementById('dateVisitingMarkOne').value;

            $.each($scope.groupWorkingData.selectedGroup.SubGroupsOne.StudentsV2, function (key, student) {
                if (student.StudentId == studentId) {
                    $.each(student.LabVisitingMark, function (key, labVisitingMark) {
                        if (labVisitingMark.ScheduleProtectionLabId == dateId) {
                            elem.value = labVisitingMark.Comment;
                        }
                    })
                }
            })
            elem = document.getElementById("keyOne");
            elem.value = index;
        };

        $scope.checkSubTotal = function (marks) {
            var coutLabs = marks.length;
            var countMarks = $scope.groupWorkingData.selectedGroup.SubGroupsOne.StudentsV2[0].Marks.filter(function (a) { return a.Mark !== ""; }).length;
            return (coutLabs - countMarks) === 0;

        };

        $scope.setTitleSubTotal = function (marks) {
            var coutLabs = marks.length;
            var countMarks = $scope.groupWorkingData.selectedGroup.SubGroupsOne.StudentsV2[0].Marks.filter(function (a) { return a.Mark !== ""; }).length;
            return "Защищено " + countMarks + " работ из " + coutLabs;

        };

        $scope.visitingMarkTwo = function (studentId, name, index) {
            var id = name + index;
            var elem = document.getElementById(id);
            var dateId = document.getElementById('dateVisitingMarkTwo').value;

            $.each($scope.groupWorkingData.selectedGroup.SubGroupsTwo.StudentsV2, function (key, student) {
                if (student.StudentId == studentId) {
                    $.each(student.LabVisitingMark, function (key, labVisitingMark) {
                        if (labVisitingMark.ScheduleProtectionLabId == dateId) {
                            elem.value = labVisitingMark.Mark;
                        }
                    })
                }
            })
        };
        $scope.visitingCommentTwo = function (studentId, name, index) {
            var id = name + index;
            var elem = document.getElementById(id);
            var dateId = document.getElementById('dateVisitingMarkTwo').value;

            $.each($scope.groupWorkingData.selectedGroup.SubGroupsTwo.StudentsV2, function (key, student) {
                if (student.StudentId == studentId) {
                    $.each(student.LabVisitingMark, function (key, labVisitingMark) {
                        if (labVisitingMark.ScheduleProtectionLabId == dateId) {
                            elem.value = labVisitingMark.Comment;
                        }
                    })
                }
            })
            elem = document.getElementById("keyTwo");
            elem.value = index;
        };

        $scope.visitingMarkThird = function (studentId, name, index) {
            var id = name + index;
            var elem = document.getElementById(id);
            var dateId = document.getElementById('dateVisitingMarkThird').value;

            $.each($scope.groupWorkingData.selectedGroup.SubGroupsThird.StudentsV2, function (key, student) {
                if (student.StudentId == studentId) {
                    $.each(student.LabVisitingMark, function (key, labVisitingMark) {
                        if (labVisitingMark.ScheduleProtectionLabId == dateId) {
                            elem.value = labVisitingMark.Mark;
                        }
                    })
                }
            })
        };
        $scope.visitingCommentThird = function (studentId, name, index) {
            var id = name + index;
            var elem = document.getElementById(id);
            var dateId = document.getElementById('dateVisitingMarkThird').value;

            $.each($scope.groupWorkingData.selectedGroup.SubGroupsThird.StudentsV2, function (key, student) {
                if (student.StudentId == studentId) {
                    $.each(student.LabVisitingMark, function (key, labVisitingMark) {
                        if (labVisitingMark.ScheduleProtectionLabId == dateId) {
                            elem.value = labVisitingMark.Comment;
                        }
                    })
                }
            })
            elem = document.getElementById("keyThird");
            elem.value = index;
        };

        $scope.getCurentDate = function () {
            var dt = new Date();
            var month = dt.getMonth() + 1;
            if (month < 10) month = '0' + month;
            var day = dt.getDate();
            if (day < 10) day = '0' + day;
            var year = dt.getFullYear();
            return day + '.' + month + '.' + year;
        };

        $scope.StrToDate = function (Dat) {
            var year = Number(Dat.split(".")[2])
            var month = Number(Dat.split(".")[1])
            var day = Number(Dat.split(".")[0])
            var dat = new Date(year, month, day)
            return dat
        };

        $scope.setMarkSubOne = function (labId, studentId, studentIndex, markIndex) {
            var date = $scope.getCurentDate();
            var dateId;
            var cell = document.getElementById("markOne" + studentIndex + markIndex);
            if (cell === null) return;
            $.each($scope.groupWorkingData.selectedGroup.SubGroupsOne.StudentsV2, function (key, studentValue) {
                $.each(studentValue.Marks, function (key, studentLabMarksValue) {
                    if (studentLabMarksValue.LabId == labId) {
                        if (studentLabMarksValue.StudentId == studentId) {
                            if (studentLabMarksValue.Mark != "") {
                                cell.innerHTML = studentLabMarksValue.Mark;
                            }
                            else {
                                $.each($scope.groupWorkingData.selectedGroup.SubGroupsOne.ScheduleProtectionLabsV2, function (key, scheduleProtectionLabsValue) {
                                    if (scheduleProtectionLabsValue.Date == date) {
                                        dateId = scheduleProtectionLabsValue.ScheduleProtectionLabId;
                                        $.each($scope.groupWorkingData.selectedGroup.SubGroupsOne.LabsV2, function (key, valueLabs) {
                                            if (valueLabs.LabId == labId) {
                                                $.each(valueLabs.ScheduleProtectionLabsRecomend, function (key, valueRecomend) {
                                                    if (valueRecomend.ScheduleProtectionId == dateId) {
                                                        cell.innerHTML = valueRecomend.Mark;
                                                        cell.style.color = '#e6e6e6';
                                                        cell.title = "Рекоммендуемая отметка"
                                                    }
                                                })
                                            }
                                        })
                                    }
                                    else if ($scope.StrToDate(scheduleProtectionLabsValue.Date) < $scope.StrToDate(date)) {
                                        dateId = scheduleProtectionLabsValue.ScheduleProtectionLabId;
                                        $.each($scope.groupWorkingData.selectedGroup.SubGroupsOne.LabsV2, function (key, valueLabs) {
                                            if (valueLabs.LabId == labId) {
                                                $.each(valueLabs.ScheduleProtectionLabsRecomend, function (key, valueRecomend) {
                                                    if (valueRecomend.ScheduleProtectionId == dateId) {
                                                        cell.innerHTML = valueRecomend.Mark;
                                                        cell.style.color = '#e6e6e6';
                                                        cell.title = "Рекоммендуемая отметка"
                                                    }
                                                })
                                            }
                                        })
                                    }
                                })
                            }
                        }
                    }
                })
            })
        };

        $scope.setMarkSubTwo = function (labId, studentId, studentIndex, markIndex) {
            var date = $scope.getCurentDate();
            var dateId;
            var cell = document.getElementById("markTwo" + studentIndex + markIndex);
            if (cell === null) return;
            $.each($scope.groupWorkingData.selectedGroup.SubGroupsTwo.StudentsV2, function (key, studentValue) {
                $.each(studentValue.Marks, function (key, studentLabMarksValue) {
                    if (studentLabMarksValue.LabId == labId) {
                        if (studentLabMarksValue.StudentId == studentId) {
                            if (studentLabMarksValue.Mark != "") {
                                cell.innerHTML = studentLabMarksValue.Mark;
                            }
                            else {
                                $.each($scope.groupWorkingData.selectedGroup.SubGroupsTwo.ScheduleProtectionLabsV2, function (key, scheduleProtectionLabsValue) {
                                    if (scheduleProtectionLabsValue.Date == date) {
                                        dateId = scheduleProtectionLabsValue.ScheduleProtectionLabId;
                                        $.each($scope.groupWorkingData.selectedGroup.SubGroupsTwo.LabsV2, function (key, valueLabs) {
                                            if (valueLabs.LabId == labId) {
                                                $.each(valueLabs.ScheduleProtectionLabsRecomend, function (key, valueRecomend) {
                                                    if (valueRecomend.ScheduleProtectionId == dateId) {
                                                        cell.innerHTML = valueRecomend.Mark;
                                                        cell.style.color = '#e6e6e6';
                                                        cell.title = "Рекоммендуемая отметка"
                                                    }
                                                })
                                            }
                                        })
                                    }
                                    else if ($scope.StrToDate(scheduleProtectionLabsValue.Date) < $scope.StrToDate(date)) {
                                        dateId = scheduleProtectionLabsValue.ScheduleProtectionLabId;
                                        $.each($scope.groupWorkingData.selectedGroup.SubGroupsTwo.LabsV2, function (key, valueLabs) {
                                            if (valueLabs.LabId == labId) {
                                                $.each(valueLabs.ScheduleProtectionLabsRecomend, function (key, valueRecomend) {
                                                    if (valueRecomend.ScheduleProtectionId == dateId) {
                                                        cell.innerHTML = valueRecomend.Mark;
                                                        cell.style.color = '#e6e6e6';
                                                        cell.title = "Рекоммендуемая отметка"
                                                    }
                                                })
                                            }
                                        })
                                    }
                                })
                            }
                        }
                    }
                })
            })
        };

        $scope.setMarkSubThird = function (labId, studentId, studentIndex, markIndex) {
            var date = $scope.getCurentDate();
            var dateId;
            var cell = document.getElementById("markThird" + studentIndex + markIndex);
            if (cell === null) return;
            $.each($scope.groupWorkingData.selectedGroup.SubGroupsThird.StudentsV2, function (key, studentValue) {
                $.each(studentValue.Marks, function (key, studentLabMarksValue) {
                    if (studentLabMarksValue.LabId == labId) {
                        if (studentLabMarksValue.StudentId == studentId) {
                            if (studentLabMarksValue.Mark != "") {
                                cell.innerHTML = studentLabMarksValue.Mark;
                            }
                            else {
                                $.each($scope.groupWorkingData.selectedGroup.SubGroupsThird.ScheduleProtectionLabsV2, function (key, scheduleProtectionLabsValue) {
                                    if (scheduleProtectionLabsValue.Date == date) {
                                        dateId = scheduleProtectionLabsValue.ScheduleProtectionLabId;
                                        $.each($scope.groupWorkingData.selectedGroup.SubGroupsThird.LabsV2, function (key, valueLabs) {
                                            if (valueLabs.LabId == labId) {
                                                $.each(valueLabs.ScheduleProtectionLabsRecomend, function (key, valueRecomend) {
                                                    if (valueRecomend.ScheduleProtectionId == dateId) {
                                                        cell.innerHTML = valueRecomend.Mark;
                                                        cell.style.color = '#e6e6e6';
                                                        cell.title = "Рекоммендуемая отметка"
                                                    }
                                                })
                                            }
                                        })
                                    }
                                    else if ($scope.StrToDate(scheduleProtectionLabsValue.Date) < $scope.StrToDate(date)) {
                                        dateId = scheduleProtectionLabsValue.ScheduleProtectionLabId;
                                        $.each($scope.groupWorkingData.selectedGroup.SubGroupsThird.LabsV2, function (key, valueLabs) {
                                            if (valueLabs.LabId == labId) {
                                                $.each(valueLabs.ScheduleProtectionLabsRecomend, function (key, valueRecomend) {
                                                    if (valueRecomend.ScheduleProtectionId == dateId) {
                                                        cell.innerHTML = valueRecomend.Mark;
                                                        cell.style.color = '#e6e6e6';
                                                        cell.title = "Рекоммендуемая отметка"
                                                    }
                                                })
                                            }
                                        })
                                    }
                                })
                            }
                        }
                    }
                })
            })
        };

        $scope.addDateSubOne = function () {
            date = $filter('date')($scope.dt, "dd/MM/yyyy");
            var isDate = false;
            $.each($scope.groupWorkingData.selectedGroup.SubGroupsOne.ScheduleProtectionLabsV2, function (key, value) {
                if (value.Date == date) {
                    isDate = true;
                }
            });

            if (isDate) {
                bootbox.dialog({
                    message: "Данная дата уже добавлена. Добавить еще одну такую дату?",
                    title: "Добавление даты",
                    buttons: {
                        danger: {
                            label: "Отмена",
                            className: "btn-default btn-sm",
                            callback: function () {

                            }
                        },
                        success: {
                            label: "Добавить",
                            className: "btn-primary btn-sm",
                            callback: function () {
                                $http({
                                    method: 'POST',
                                    url: $scope.UrlServiceLabs + "SaveScheduleProtectionDate",
                                    data: {
                                        subGroupId: $scope.groupWorkingData.selectedGroup.SubGroupsOne.SubGroupId,
                                        date: date
                                    },
                                    headers: { 'Content-Type': 'application/json' }
                                }).success(function (data, status) {
                                    if (data.Code != '200') {
                                        alertify.error(data.Message);
                                    } else {
                                        $scope.loadLabsV2();
                                        alertify.success(data.Message);
                                    }
                                });
                            }
                        }
                    }
                });
            } else {
                $http({
                    method: 'POST',
                    url: $scope.UrlServiceLabs + "SaveScheduleProtectionDate",
                    data: {
                        subGroupId: $scope.groupWorkingData.selectedGroup.SubGroupsOne.SubGroupId,
                        date: date
                    },
                    headers: { 'Content-Type': 'application/json' }
                }).success(function (data, status) {
                    if (data.Code != '200') {
                        alertify.error(data.Message);
                    } else {
                        $scope.loadLabsV2();
                        alertify.success(data.Message);
                    }
                });
            }
        };

        $scope.addDateSubTwo = function () {
            date = $filter('date')($scope.dt, "dd/MM/yyyy");
            var isDate = false;
            $.each($scope.groupWorkingData.selectedGroup.SubGroupsTwo.ScheduleProtectionLabsV2, function (key, value) {
                if (value.Date == date) {
                    isDate = true;
                }
            });

            if (isDate) {
                bootbox.dialog({
                    message: "Данная дата уже добавлена. Добавить еще одну такую дату?",
                    title: "Добавление даты",
                    buttons: {
                        danger: {
                            label: "Отмена",
                            className: "btn-default btn-sm",
                            callback: function () {

                            }
                        },
                        success: {
                            label: "Добавить",
                            className: "btn-primary btn-sm",
                            callback: function () {
                                $http({
                                    method: 'POST',
                                    url: $scope.UrlServiceLabs + "SaveScheduleProtectionDate",
                                    data: {
                                        subGroupId: $scope.groupWorkingData.selectedGroup.SubGroupsTwo.SubGroupId,
                                        date: date
                                    },
                                    headers: { 'Content-Type': 'application/json' }
                                }).success(function (data, status) {
                                    if (data.Code != '200') {
                                        alertify.error(data.Message);
                                    } else {
                                        $scope.loadLabsV2();
                                        alertify.success(data.Message);
                                    }
                                });
                            }
                        }
                    }
                });
            } else {
                $http({
                    method: 'POST',
                    url: $scope.UrlServiceLabs + "SaveScheduleProtectionDate",
                    data: {
                        subGroupId: $scope.groupWorkingData.selectedGroup.SubGroupsTwo.SubGroupId,
                        date: date
                    },
                    headers: { 'Content-Type': 'application/json' }
                }).success(function (data, status) {
                    if (data.Code != '200') {
                        alertify.error(data.Message);
                    } else {
                        $scope.loadLabsV2();
                        alertify.success(data.Message);
                    }
                });
            }
        };

        $scope.addDateSubThird = function () {
            date = $filter('date')($scope.dt, "dd/MM/yyyy");
            var isDate = false;
            $.each($scope.groupWorkingData.selectedGroup.SubGroupsThird.ScheduleProtectionLabsV2, function (key, value) {
                if (value.Date == date) {
                    isDate = true;
                }
            });

            if (isDate) {
                bootbox.dialog({
                    message: "Данная дата уже добавлена. Добавить еще одну такую дату?",
                    title: "Добавление даты",
                    buttons: {
                        danger: {
                            label: "Отмена",
                            className: "btn-default btn-sm",
                            callback: function () {

                            }
                        },
                        success: {
                            label: "Добавить",
                            className: "btn-primary btn-sm",
                            callback: function () {
                                $http({
                                    method: 'POST',
                                    url: $scope.UrlServiceLabs + "SaveScheduleProtectionDate",
                                    data: {
                                        subGroupId: $scope.groupWorkingData.selectedGroup.SubGroupsThird.SubGroupId,
                                        date: date
                                    },
                                    headers: { 'Content-Type': 'application/json' }
                                }).success(function (data, status) {
                                    if (data.Code != '200') {
                                        alertify.error(data.Message);
                                    } else {
                                        $scope.loadLabsV2();
                                        alertify.success(data.Message);
                                    }
                                });
                            }
                        }
                    }
                });
            } else {
                $http({
                    method: 'POST',
                    url: $scope.UrlServiceLabs + "SaveScheduleProtectionDate",
                    data: {
                        subGroupId: $scope.groupWorkingData.selectedGroup.SubGroupsThird.SubGroupId,
                        date: date
                    },
                    headers: { 'Content-Type': 'application/json' }
                }).success(function (data, status) {
                    if (data.Code != '200') {
                        alertify.error(data.Message);
                    } else {
                        $scope.loadLabsV2();
                        alertify.success(data.Message);
                    }
                });
            }
        };

        $scope.saveLabsMarksSubOne = function () {
            $("#errorOneMark").hide();
            var labId = document.getElementById('inputLabIdOne').value;
            var studentId = document.getElementById('inputStudentIdOne').value;
            var mark = document.getElementById('markInputOne').value;
            var comment = document.getElementById('commentInputOne').value;
            var date = document.getElementById('dateInputOne').value;
            var Id = document.getElementById('inputIdOne').value;

            var markF = parseInt(mark);
            if (mark !== "" && (!markF || (markF < 0 || markF > 10))) {
                $("#errorOneMark").show();
            }
            else {
                $("#dialogexposeMarkOne").modal('hide');
                $http({
                    method: 'POST',
                    url: $scope.UrlServiceLabs + "SaveStudentLabsMark",
                    data: {
                        studentId: studentId,
                        labId: labId,
                        mark: mark,
                        comment: comment,
                        date: date,
                        id: Id,
                        students: $scope.groupWorkingData.selectedGroup.SubGroupsOne.StudentsV2,
                    },
                    headers: { 'Content-Type': 'application/json' }
                }).success(function (data, status) {
                    if (data.Code != '200') {
                        alertify.error(data.Message);
                    } else {
                        $scope.loadLabsV2();
                        alertify.success(data.Message);
                    }
                });
            }
        };


        $scope.saveLabsMarksSubTwo = function () {
            $("#errorTwoMark").hide();
            var labId = document.getElementById('inputLabIdTwo').value;
            var studentId = document.getElementById('inputStudentIdTwo').value;
            var mark = document.getElementById('markInputTwo').value;
            var comment = document.getElementById('commentInputTwo').value;
            var date = document.getElementById('dateInputTwo').value;
            var Id = document.getElementById('inputIdTwo').value;

            var markF = parseInt(mark);
            if (mark !== "" && (!markF || (markF < 0 || markF > 10))) {
                $("#errorTwoMark").show();
            }
            else {
                $("#dialogexposeMarkTwo").modal('hide');
                $http({
                    method: 'POST',
                    url: $scope.UrlServiceLabs + "SaveStudentLabsMark",
                    data: {
                        studentId: studentId,
                        labId: labId,
                        mark: mark,
                        comment: comment,
                        date: date,
                        id: Id,
                        students: $scope.groupWorkingData.selectedGroup.SubGroupsTwo.StudentsV2,
                    },
                    headers: { 'Content-Type': 'application/json' }
                }).success(function (data, status) {
                    if (data.Code != '200') {
                        alertify.error(data.Message);
                    } else {
                        $scope.loadLabsV2();
                        alertify.success(data.Message);
                    }
                });
            }
        };

        $scope.saveLabsMarksSubThird = function () {
            $("#errorThirdMark").hide();

            var labId = document.getElementById('inputLabIdThird').value;
            var studentId = document.getElementById('inputStudentIdThird').value;
            var mark = document.getElementById('markInputThird').value;
            var comment = document.getElementById('commentInputThird').value;
            var date = document.getElementById('dateInputThird').value;
            var Id = document.getElementById('inputIdThird').value;

            var markF = parseInt(mark);
            if (mark !== "" && (!markF || (markF < 0 || markF > 10))) {
                $("#errorThirdMark").show();
            }
            else {
                $("#dialogexposeMarkThird").modal('hide');
                $http({
                    method: 'POST',
                    url: $scope.UrlServiceLabs + "SaveStudentLabsMark",
                    data: {
                        studentId: studentId,
                        labId: labId,
                        mark: mark,
                        comment: comment,
                        date: date,
                        id: Id,
                        students: $scope.groupWorkingData.selectedGroup.SubGroupsThird.StudentsV2,
                    },
                    headers: { 'Content-Type': 'application/json' }
                }).success(function (data, status) {
                    if (data.Code != '200') {
                        alertify.error(data.Message);
                    } else {
                        $scope.loadLabsV2();
                        alertify.success(data.Message);
                    }
                });
            }
        };

        $scope.deleteVisitingDate = function (idDate) {
            bootbox.dialog({
                message: "Вы действительно хотите удалить дату и все связанные с ней данные?",
                title: "Удаление даты",
                buttons: {
                    danger: {
                        label: "Отмена",
                        className: "btn-default btn-sm",
                        callback: function () {

                        }
                    },
                    success: {
                        label: "Удалить",
                        className: "btn-primary btn-sm",
                        callback: function () {
                            $http({
                                method: 'POST',
                                url: $scope.UrlServiceLabs + "DeleteVisitingDate",
                                data: { id: idDate },
                                headers: { 'Content-Type': 'application/json' }
                            }).success(function (data, status) {
                                if (data.Code != '200') {
                                    alertify.error(data.Message);
                                } else {
                                    alertify.success(data.Message);
                                    $scope.loadLabsV2();
                                }
                            });
                        }
                    }
                }
            });
        };

        $scope.saveZip = function () {
            document.location.href = "/Subject/GetZipLabs?id=" + $scope.groupWorkingData.selectedGroup.GroupId + "&subjectId=" + $scope.subjectId;
            //$.ajax({
            //	type: 'GET',
            //	url: "/Subject/GetZipLabs?id=" + subGroupId + "&subjectId=" + $scope.subjectId,
            //	contentType: "application/zip",
            //}).success(function (data, status) {
            //});
        };

        $scope.getZip = function (userId) {
            var subGroupId = $scope.groupWorkingData.selectedSubGroup.SubGroupId;
            document.location.href = "/Subject/GetStudentZipLabs?id=" + subGroupId + "&subjectId=" + $scope.subjectId + "&userId=" + userId;
            //$.ajax({
            //	type: 'GET',
            //	url: "/Subject/GetZipLabs?id=" + subGroupId + "&subjectId=" + $scope.subjectId,
            //	contentType: "application/zip",
            //}).success(function (data, status) {
            //});
        };
    })
    .controller('RepoController', function ($scope, $http, $filter) {

        $scope.UrlServiceLabs = '/Services/Labs/LabsService.svc/';

        $scope.groupWorkingData = {
            selectedSubGroup: null,
            selectedGroup: null,
            selectedGroupId: 0,
            selectedSubGroupId: 0
        };

        
        $scope.editFileSend = {
            Comments: "",
            PathFile: "",
            Id: 0
        };

        $scope.labFilesUser = [];


        $scope.groups = [];
        $scope.subGroups = [];

        function getParameterByName(name) {
            name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
            var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
                results = regex.exec(location.search);
            return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
        }

        var subjectId = getParameterByName("subjectId");

        $scope.userRole = 0;
        $scope.userId = 0;
        $scope.subjectId = 0;
        $scope.initRepo = function (userRole, userId) {
            $scope.subjectId = subjectId;
            $scope.loadExGroups();
            $scope.userRole = userRole;
            $scope.userId = userId;
            
            bootbox.setDefaults({
                locale: "ru"
            });
            
        };

        $scope.resultPlagiatium = [];

        $scope.getLecturesFileAttachments = function () {
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



        $scope.loadFilesLabUser = function () {
            $http({
                method: 'POST',
                url: $scope.UrlServiceLabs + "GetFilesLab",
                data: {
                    userId: '1',
                    subjectId: $scope.subjectId,
                    isCoursPrj: false
                },
                headers: { 'Content-Type': 'application/json' }
            }).success(function (data, status) {
                if (data.Code != '200') {
                    alertify.error(data.Message);
                } else {
                    $scope.labFilesUser = data.UserLabFiles;
                    alertify.success(data.Message);
                }
            });
        };

        $scope.loadExGroups = function () {
            $.ajax({
                type: 'GET',
                url: "/Services/CoreService.svc/GetGroupsV3/" + $scope.subjectId,
                dataType: "json",
                contentType: "application/json",

            }).success(function (data, status) {
                if (data.Code != '200') {
                    alertify.error(data.Message);
                } else {
                    $scope.$apply(function () {
                        $scope.groups = data.Groups;
                        
                        if ($scope.groupWorkingData.selectedGroupId == 0) {
                            $scope.loadFilesV2($scope.groups[0]);
                        }
                        
                    });
                }
            });
        };

        


        $scope.changeGroups = function (selectedGroup) {
            
            $scope.groupWorkingData.selectedGroupId = selectedGroup.GroupId;
            $scope.startSpin();

            $scope.groupWorkingData.selectedGroup = null;
            $.each($scope.groups, function (index, value) {
                if (value.GroupId == $scope.groupWorkingData.selectedGroupId) {
                    $scope.groupWorkingData.selectedGroup = value;
                    return;
                }
            });
            
           

            //$scope.startSpin();
            //$scope.loadFilesV2();

            //$scope.reloadFiles();


        };

        $scope.reloadFiles = function () {
            $scope.startSpin();
            $scope.loadFilesV2();
        };

        $scope.loadFilesV2 = function (groupId) {

            $.ajax({
                type: 'GET',
                url: $scope.UrlServiceLabs + "GetFilesV2?subjectId=" + $scope.subjectId + "&groupId=" + groupId.GroupId + "&isCp=false",
                dataType: "json",
                contentType: "application/json",

            }).success(function (data, status) {
                if (data.Code != '200') {
                    alertify.error(data.Message);
                } else {
                    $scope.$apply(function() {
                         $scope.groupWorkingData.selectedGroup.StudentsFiles = data.Students;
                    });

                }
                $scope.stopSpin();
                });
            $scope.changeGroups(groupId);
            
            
        };

        $scope.loadLabsV2 = function () {

            $.ajax({
                type: 'GET',
                url: $scope.UrlServiceLabs + "GetLabsV2?subjectId=" + $scope.subjectId + "&groupId=" + $scope.groupWorkingData.selectedGroupId,
                dataType: "json",
                contentType: "application/json",

            }).success(function (data, status) {
                if (data.Code != '200') {
                    alertify.error(data.Message);
                } else {
                    $scope.$apply(function () {
                        $scope.groupWorkingData.selectedGroup.SubGroupsOne.LabsV2 = $filter("filter")(data.Labs, function (r) {
                            return r.SubGroup === 1;
                        });
                        $scope.groupWorkingData.selectedGroup.SubGroupsTwo.LabsV2 = $filter("filter")(data.Labs, function (r) {
                            return r.SubGroup === 2;
                        });

                        $scope.groupWorkingData.selectedGroup.SubGroupsThird.LabsV2 = $filter("filter")(data.Labs, function (r) {
                            return r.SubGroup === 3;
                        });

                        $scope.groupWorkingData.selectedGroup.SubGroupsOne.ScheduleProtectionLabsV2 = $filter("filter")(data.ScheduleProtectionLabs, function (r) {
                            return r.SubGroup === 1;
                        });
                        $scope.groupWorkingData.selectedGroup.SubGroupsTwo.ScheduleProtectionLabsV2 = $filter("filter")(data.ScheduleProtectionLabs, function (r) {
                            return r.SubGroup === 2;
                        });

                        $scope.groupWorkingData.selectedGroup.SubGroupsThird.ScheduleProtectionLabsV2 = $filter("filter")(data.ScheduleProtectionLabs, function (r) {
                            return r.SubGroup === 3;
                        });
                    });
                }
            });
        };



        $scope.commentImage = function (comment, studentIndex, markIndex, name) {
            var id = name + studentIndex + markIndex;
            var elem = document.getElementById(id);

            if (elem !== null) {
                if (comment != "" & comment != null) {
                    elem.style.display = 'block';
                    elem.title = comment;
                }
                else {
                    elem.style.display = 'none';
                    elem.title = null;
                }
            }
        };

        $scope.NumberControl = function (object, errorName) {
            var error = document.getElementById(errorName);
            if (object.value === "") {
                object.value = "";
                error.style.display = 'none';
            } else {
                if (parseInt(object.value) < object.min) {
                    object.value = object.min;
                    error.style.display = 'block';
                } else {
                    if (parseInt(object.value) > object.max) {
                        object.value = object.max;
                        error.style.display = 'block';
                    }
                    else { error.style.display = 'none'; }
                }
            }
        };

        $scope.getCurentDate = function () {
            var dt = new Date();
            var month = dt.getMonth() + 1;
            if (month < 10) month = '0' + month;
            var day = dt.getDate();
            if (day < 10) day = '0' + day;
            var year = dt.getFullYear();
            return day + '.' + month + '.' + year;
        };

        $scope.StrToDate = function (Dat) {
            var year = Number(Dat.split(".")[2])
            var month = Number(Dat.split(".")[1])
            var day = Number(Dat.split(".")[0])
            var dat = new Date(year, month, day)
            return dat
        };

        $scope.saveZip = function () {
            document.location.href = "/Subject/GetZipLabs?id=" + $scope.groupWorkingData.selectedGroup.GroupId + "&subjectId=" + $scope.subjectId;
            //$.ajax({
            //	type: 'GET',
            //	url: "/Subject/GetZipLabs?id=" + subGroupId + "&subjectId=" + $scope.subjectId,
            //	contentType: "application/zip",
            //}).success(function (data, status) {
            //});
        };

        $scope.getZip = function (userId) {
            var subGroupId = $scope.groupWorkingData.selectedSubGroup.SubGroupId;
            document.location.href = "/Subject/GetStudentZipLabs?id=" + subGroupId + "&subjectId=" + $scope.subjectId + "&userId=" + userId;
            //$.ajax({
            //	type: 'GET',
            //	url: "/Subject/GetZipLabs?id=" + subGroupId + "&subjectId=" + $scope.subjectId,
            //	contentType: "application/zip",
            //}).success(function (data, status) {
            //});
        };


    })
    .controller('PracticalsController', function ($scope, $http) {

        $scope.practicals = [];

        $scope.UrlServicePractical = '/Services/Practicals/PracticalService.svc/';

        $scope.editPracticalData = {
            TitleForm: "",
            Theme: "",
            Order: "",
            Duration: "",
            PathFile: "",
            ShortName: "",
            Id: 0
        };

        $scope.init = function () {
            $scope.practicals = [];
            $scope.loadPracticals();
        };

        $scope.loadPracticals = function () {
            $.ajax({
                type: 'GET',
                url: $scope.UrlServicePractical + "GetPracticals/" + $scope.subjectId,
                dataType: "json",
                contentType: "application/json",

            }).success(function (data, status) {
                if (data.Code != '200') {
                    alertify.error(data.Message);
                } else {
                    $scope.$apply(function () {
                        $scope.practicals = data.Practicals;
                    });
                }
            });
        };

        $scope.addPracticals = function () {
            $scope.editPracticalData.TitleForm = "Создание практического занятия";
            $scope.editPracticalData.Theme = "";
            $scope.editPracticalData.Duration = "";
            $scope.editPracticalData.Order = "";
            $scope.editPracticalData.PathFile = "";
            $scope.editPracticalData.ShortName = "";
            $scope.editPracticalData.Id = "0";

            $("#practicalsFile").empty();

            $.ajax({
                type: 'GET',
                url: "/Subject/GetFilePracticals?id=0",
                contentType: "application/json",

            }).success(function (data, status) {
                $scope.$apply(function () {
                    $("#practicalsFile").append(data);
                });
            });

            $('#dialogAddPracticals').modal();
        };

        $scope.editPracticals = function (practical) {
            $scope.editPracticalData.TitleForm = "Редактирование практического занятия";
            $scope.editPracticalData.Theme = practical.Theme;
            $scope.editPracticalData.Duration = practical.Duration;
            $scope.editPracticalData.PathFile = practical.PathFile;
            $scope.editPracticalData.ShortName = practical.ShortName;
            $scope.editPracticalData.Order = practical.Order;
            $scope.editPracticalData.Id = practical.PracticalId;

            $("#practicalsFile").empty();

            $.ajax({
                type: 'GET',
                url: "/Subject/GetFilePracticals?id=" + practical.PracticalId,
                contentType: "application/json",

            }).success(function (data, status) {
                $scope.$apply(function () {
                    $("#practicalsFile").append(data);
                });
            });
            $('#dialogAddPracticals').modal();
        };

        $scope.savePracticals = function () {
            $http({
                method: 'POST',
                url: $scope.UrlServicePractical + "Save",
                data: {
                    subjectId: $scope.subjectId,
                    id: $scope.editPracticalData.Id,
                    theme: $scope.editPracticalData.Theme,
                    duration: $scope.editPracticalData.Duration,
                    order: $scope.editPracticalData.Order,
                    shortName: $scope.editPracticalData.ShortName,
                    pathFile: $scope.editPracticalData.PathFile,
                    attachments: $scope.getLecturesFileAttachments()
                },
                headers: { 'Content-Type': 'application/json' }
            }).success(function (data, status) {
                if (data.Code != '200') {
                    alertify.error(data.Message);
                } else {
                    $scope.loadPracticals();
                    alertify.success(data.Message);
                }
                $("#dialogAddPracticals").modal('hide');
            });
        };

        $scope.deletePracticals = function (practical) {
            bootbox.dialog({
                message: "Вы действительно хотите удалить практическое занятие?",
                title: "Удаление практического занятия",
                buttons: {
                    danger: {
                        label: "Отмена",
                        className: "btn-default btn-sm",
                        callback: function () {

                        }
                    },
                    success: {
                        label: "Удалить",
                        className: "btn-primary btn-sm",
                        callback: function () {
                            $http({
                                method: 'POST',
                                url: $scope.UrlServicePractical + "Delete",
                                data: { id: practical.PracticalId, subjectId: $scope.subjectId },
                                headers: { 'Content-Type': 'application/json' }
                            }).success(function (data, status) {
                                if (data.Code != '200') {
                                    alertify.error(data.Message);
                                } else {
                                    alertify.success(data.Message);
                                    $scope.practicals.splice($scope.practicals.indexOf(practical), 1);
                                    $scope.loadGroups();
                                }
                            });
                        }
                    }
                }
            });
        };

        $scope.changeGroups = function (selectedGroup) {
            $scope.selectedGroupChange(selectedGroup.GroupId);
        };

        $scope.addVisitingMarks = function (visitingDate) {

        };

        $scope.addDate = function () {
            var dd = $scope.dt.getDate();
            var mm = $scope.dt.getMonth() + 1; //January is 0!
            var yyyy = $scope.dt.getFullYear();

            date = dd + '/' + mm + '/' + yyyy;

            var isDate = false;
            $.each($scope.groupWorkingData.selectedGroup.ScheduleProtectionPracticals, function (key, value) {
                if (value.Date == date) {
                    isDate = true;
                }
            });

            if (isDate) {
                bootbox.dialog({
                    message: "Данная дата уже добавлена. Добавить еще одну такую дату?",
                    title: "Добавление даты",
                    buttons: {
                        danger: {
                            label: "Отмена",
                            className: "btn-default btn-sm",
                            callback: function () {

                            }
                        },
                        success: {
                            label: "Добавить",
                            className: "btn-primary btn-sm",
                            callback: function () {
                                $http({
                                    method: 'POST',
                                    url: $scope.UrlServicePractical + "SaveScheduleProtectionDate",
                                    data: {
                                        groupId: $scope.groupWorkingData.selectedGroup.GroupId,
                                        date: date,
                                        subjectId: $scope.subjectId
                                    },
                                    headers: { 'Content-Type': 'application/json' }
                                }).success(function (data, status) {
                                    if (data.Code != '200') {
                                        alertify.error(data.Message);
                                    } else {
                                        $scope.loadGroups();
                                        alertify.success(data.Message);
                                    }
                                });
                            }
                        }
                    }
                });
            } else {
                $http({
                    method: 'POST',
                    url: $scope.UrlServicePractical + "SaveScheduleProtectionDate",
                    data: {
                        groupId: $scope.groupWorkingData.selectedGroup.GroupId,
                        date: date,
                        subjectId: $scope.subjectId
                    },
                    headers: { 'Content-Type': 'application/json' }
                }).success(function (data, status) {
                    if (data.Code != '200') {
                        alertify.error(data.Message);
                    } else {
                        $scope.loadGroups();
                        alertify.success(data.Message);
                    }
                });
            }

        };

        $scope.dateVisitingManagement = function () {
            $('#dialogManagementData').modal();
        };

        $scope.saveVisitingMark = function () {
            $http({
                method: 'POST',
                url: $scope.UrlServicePractical + "SavePracticalsVisitingData",
                data: {
                    students: $scope.groupWorkingData.selectedGroup.Students,
                },
                headers: { 'Content-Type': 'application/json' }
            }).success(function (data, status) {
                if (data.Code != '200') {
                    alertify.error(data.Message);
                } else {
                    $scope.loadGroups();
                    alertify.success(data.Message);
                }
            });
        };

        $scope.savePracticalsMarks = function () {
            $http({
                method: 'POST',
                url: $scope.UrlServicePractical + "SaveStudentPracticalsMark",
                data: {
                    students: $scope.groupWorkingData.selectedGroup.Students,
                },
                headers: { 'Content-Type': 'application/json' }
            }).success(function (data, status) {
                if (data.Code != '200') {
                    alertify.error(data.Message);
                } else {
                    $scope.loadGroups();
                    alertify.success(data.Message);
                }
            });
        };

        $scope.deleteVisitingDate = function (idDate) {
            bootbox.dialog({
                message: "Вы действительно хотите удалить дату и все связанные с ней данные?",
                title: "Удаление даты",
                buttons: {
                    danger: {
                        label: "Отмена",
                        className: "btn-default btn-sm",
                        callback: function () {

                        }
                    },
                    success: {
                        label: "Удалить",
                        className: "btn-primary btn-sm",
                        callback: function () {
                            $http({
                                method: 'POST',
                                url: $scope.UrlServicePractical + "DeleteVisitingDate",
                                data: { id: idDate },
                                headers: { 'Content-Type': 'application/json' }
                            }).success(function (data, status) {
                                if (data.Code != '200') {
                                    alertify.error(data.Message);
                                } else {
                                    alertify.success(data.Message);
                                    $scope.loadGroups();
                                }
                            });
                        }
                    }
                }
            });
        };
    })
    .controller('SubjectAttachmentsController', function ($scope, $http) {
    $scope.init = function () {
        $.ajax({
            type: 'GET',
            url: "/Subject/GetFileSubject?subjectId=" + $scope.subjectId,
            contentType: "application/json",

        }).success(function (data, status) {
            $scope.$apply(function () {
                $(".lecturesF").append(data.Lectures);
                $(".labsF").append(data.Labs);
                $(".practF").append(data.Practicals);
            });
        });
    };
});

