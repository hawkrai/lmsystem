angular.module('mainApp', ['mainApp.controllers', 'ngRoute', 'ui.bootstrap'])
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
angular.module('mainApp.controllers', ['ui.bootstrap'])
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
        $scope.groups = [];

        $scope.selectedGroup = [];

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
                        $scope.selectedGroup = $scope.groups[0];
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
            Date: "",
            Marks: []
        };

        $scope.init = function () {
            $scope.lectures = [];
            $scope.loadLectures();
            $scope.loadCalendar();
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

        $scope.addSheduleVisitingGraph = function() {
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
                    alertify.success(data.Message);
                }
            });
        };

        $scope.saveMarks = function() {

        };

        $scope.editMarks = function(calendar) {
            $('#dialogEditMarks').modal();
        };
    })
    .controller('LabsController', function ($scope, $http) {

        $scope.labs = [];

        $scope.UrlServiceLabs = '/Services/Labs/LabsService.svc/';

        $scope.editLabsData = {
            TitleForm: "",
            Theme: "",
            Duration: "",
            PathFile: "",
            ShortName: "",
            Id: 0
        };

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
    })
    .controller('PracticalsController', function ($scope, $http) {

        $scope.practicals = [];

        $scope.UrlServicePractical = '/Services/Practicals/PracticalService.svc/';

        $scope.editPracticalData = {
            TitleForm: "",
            Theme: "",
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
    });

