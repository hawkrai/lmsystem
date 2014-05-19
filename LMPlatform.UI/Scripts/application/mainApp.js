var app = angular.module('mainApp', ['mainApp.controllers', 'ngRoute', 'ui.bootstrap', 'xeditable'])
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
            });
    });
app.run(function (editableOptions, editableThemes) {
    editableThemes.bs3.inputClass = 'input-sm text-center';
    editableThemes.bs3.buttonsClass = 'btn-sm';
    editableOptions.theme = 'bs3';
});
angular.module('mainApp.controllers', ['ui.bootstrap', 'xeditable'])
    .controller('MainCtrl', function ($scope) {

        $scope.today = function () {
            $scope.dt = new Date();
        };
        $scope.today();

        $scope.showWeeks = true;
        $scope.toggleWeeks = function () {
            $scope.showWeeks = !$scope.showWeeks;
        };

        $scope.clear = function () {
            $scope.dt = null;
        };

        // Disable weekend selection
        $scope.disabled = function (date, mode) {
            return (mode === 'day' && (date.getDay() === 0 || date.getDay() === 6));
        };

        $scope.toggleMin = function () {
            $scope.minDate = ($scope.minDate) ? null : new Date();
        };
        $scope.toggleMin();

        $scope.openDate = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();

            $scope.opened = true;
        };

        $scope.dateOptions = {
            'year-format': "'yy'",
            'starting-day': 1
        };

        $scope.formats = ['dd-MM-yyyy', 'yyyy/MM/dd', 'shortDate'];
        $scope.format = $scope.formats[0];


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

        $scope.init = function (subjectId) {
            $scope.subjectId = subjectId;
            $scope.loadGroups();
        };

        $scope.loadGroups = function () {
            $.ajax({
                type: 'GET',
                url: $scope.UrlServiceMain + "GetGroups/" + $scope.subjectId,
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
    }).controller('NewsController', function ($scope, $http) {

        $scope.news = [];
        $scope.UrlServiceNews = '/Services/News/NewsService.svc/';

        $scope.editNewsData = {
            TitleForm: "",
            Title: "",
            Body: "",
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
            $('#dialogAddNews').modal();
        };

        $scope.editNews = function (news) {
            $scope.editNewsData.TitleForm = "Редактирование новости";
            $scope.editNewsData.Title = news.Title;
            $scope.editNewsData.Body = news.Body;
            $scope.editNewsData.Id = news.NewsId;
            $('#dialogAddNews').modal();
        };

        $scope.saveNews = function () {
            $http({
                method: 'POST',
                url: $scope.UrlServiceNews + "Save",
                data: { subjectId: $scope.subjectId, id: $scope.editNewsData.Id, title: $scope.editNewsData.Title, body: $scope.editNewsData.Body },
                headers: { 'Content-Type': 'application/json' }
            }).success(function (data, status) {
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
            bootbox.confirm("Вы действительно хотите удалить новость?", function (isConfirmed) {
                if (isConfirmed) {
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
            });
        };
    })
    .controller('LecturesController', function ($scope, $http) {

        $scope.lectures = [];
        $scope.UrlServiceLectures = '/Services/Lectures/LecturesService.svc/';

        $scope.editLecturesData = {
            TitleForm: "",
            Theme: "",
            Duration: "",
            PathFile: "",
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

        $scope.init = function () {
            $scope.lectures = [];
            $scope.loadLectures();
            $scope.loadCalendar();
        };

        $scope.changeGroups = function (selectedGroup) {
            $scope.selectedGroupChange(selectedGroup.GroupId);
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
            $http({
                method: 'POST',
                url: $scope.UrlServiceLectures + "Save",
                data: {
                    subjectId: $scope.subjectId,
                    id: $scope.editLecturesData.Id,
                    theme: $scope.editLecturesData.Theme,
                    duration: $scope.editLecturesData.Duration,
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
        };

        $scope.deleteLectures = function (lectures) {
            bootbox.confirm("Вы действительно хотите удалить лекцию?", function (isConfirmed) {
                if (isConfirmed) {
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
            });
        };

        $scope.addSheduleVisitingGraph = function () {
            $('#dialogAddVisitData').modal();
        };

        $scope.addDate = function () {
            var dd = $scope.dt.getDate();
            var mm = $scope.dt.getMonth() + 1; //January is 0!
            var yyyy = $scope.dt.getFullYear();

            if (dd < 10) {
                dd = '0' + dd;
            }

            if (mm < 10) {
                mm = '0' + mm;
            }

            date = dd + '-' + mm + '-' + yyyy;

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
                    $scope.loadGroups();
                    alertify.success(data.Message);
                }
            });
        };

        $scope.saveMarks = function () {
            $http({
                method: 'POST',
                url: $scope.UrlServiceLectures + "SaveMarksCalendarData",
                data: {
                    dateId: $scope.editMarks.DateId,
                    subjectId: $scope.subjectId,
                    groupId: $scope.groupWorkingData.selectedGroup.groupId,
                    marks: $scope.editMarks.StudentMarkForDate
                },
                headers: { 'Content-Type': 'application/json' }
            }).success(function (data, status) {
                if (data.Code != '200') {
                    alertify.error(data.Message);
                } else {
                    alertify.success(data.Message);
                    $scope.loadGroups();
                    $('#dialogEditMarks').modal('hide');
                }
            });
        };

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
    })
    .controller('LabsController', function ($scope, $http) {

        $scope.labs = [];

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

        $scope.editMarksVisiting = [];

        $scope.init = function () {
            $scope.labs = [];
            $scope.loadLabs();
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
            $scope.editLabsData.Id = "0";
            $scope.editLabsData.Order = "0";

            $("#labsFile").empty();

            $.ajax({
                type: 'GET',
                url: "/Subject/GetFileLabs?id=0",
                contentType: "application/json",

            }).success(function (data, status) {
                $scope.$apply(function () {
                    $("#labsFile").append(data);
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

            $("#labsFile").empty();

            $.ajax({
                type: 'GET',
                url: "/Subject/GetFileLabs?id=" + lab.LabId,
                contentType: "application/json",

            }).success(function (data, status) {
                $scope.$apply(function () {
                    $("#labsFile").append(data);
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
                    $scope.loadGroups();
                    alertify.success(data.Message);
                }
                $("#dialogAddLabs").modal('hide');
            });
        };

        $scope.deleteLabs = function (lab) {
            bootbox.confirm("Вы действительно хотите удалить лабораторную работу?", function (isConfirmed) {
                if (isConfirmed) {
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
                            $scope.labs.splice($scope.labs.indexOf(lab), 1);
                        }
                    });
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

        };

        $scope.changeSubGroup = function (selectedSubGroup) {
            $scope.selectedGroupChange(null, selectedSubGroup.SubGroupId);
        };

        $scope.saveVisitingMark = function () {
            $http({
                method: 'POST',
                url: $scope.UrlServiceLabs + "SaveLabsVisitingData",
                data: {
                    students: $scope.groupWorkingData.selectedSubGroup.Students
                },
                headers: { 'Content-Type': 'application/json' }
            }).success(function (data, status) {
                if (data.Code != '200') {
                    alertify.error(data.Message);
                } else {
                    alertify.success(data.Message);
                    $scope.loadGroups();
                }
            });
        };

        $scope.managementDate = function () {
            $('#dialogmanagementData').modal();
        };

        $scope.addDate = function () {
            var dd = $scope.dt.getDate();
            var mm = $scope.dt.getMonth() + 1; //January is 0!
            var yyyy = $scope.dt.getFullYear();

            if (dd < 10) {
                dd = '0' + dd;
            }

            if (mm < 10) {
                mm = '0' + mm;
            }

            date = dd + '-' + mm + '-' + yyyy;

            $http({
                method: 'POST',
                url: $scope.UrlServiceLabs + "SaveScheduleProtectionDate",
                data: {
                    subGroupId: $scope.groupWorkingData.selectedSubGroup.SubGroupId,
                    date: date
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

        $scope.saveLabsMarks = function () {
            $http({
                method: 'POST',
                url: $scope.UrlServiceLabs + "SaveStudentLabsMark",
                data: {
                    students: $scope.groupWorkingData.selectedSubGroup.Students,
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
            bootbox.confirm("Вы действительно хотите удалить практическое занятие?", function (isConfirmed) {
                if (isConfirmed) {
                    $http({
                        method: 'POST',
                        url: $scope.UrlServicePractical + "Delete",
                        data: { id: practical.LabId, subjectId: $scope.subjectId },
                        headers: { 'Content-Type': 'application/json' }
                    }).success(function (data, status) {
                        if (data.Code != '200') {
                            alertify.error(data.Message);
                        } else {
                            alertify.success(data.Message);
                            $scope.practicals.splice($scope.practicals.indexOf(practical), 1);
                        }
                    });
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

            if (dd < 10) {
                dd = '0' + dd;
            }

            if (mm < 10) {
                mm = '0' + mm;
            }

            date = dd + '-' + mm + '-' + yyyy;

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
    });

