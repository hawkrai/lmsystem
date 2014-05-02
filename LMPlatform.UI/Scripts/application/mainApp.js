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
            });
    });
angular.module('mainApp.controllers', [])
    .controller('MainCtrl', function ($scope) {

        $scope.subjectId = 0;

        $scope.init = function (subjectId) {
            $scope.subjectId = subjectId;
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

        $scope.init = function () {
            $scope.lectures = [];
            $scope.loadLectures();
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
                $scope.$apply(function() {
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

        $scope.getLecturesFileAttachments = function() {
            var itemAttachmentsTable = $('#fileupload').find('table').find('tbody tr');
            var data = $.map(itemAttachmentsTable, function (e) {
                var newObject = null;
                if (e.className === "template-download fade in") {
                    if (e.id === "-1") {
                        newObject = { Id: 0, Title: "", Name: $(e).find('td a').text(), AttachmentType: $(e).find('td.type').text(), FileName: $(e).find('td.guid').text() };
                    } 
                    else {
                        newObject = { Id: e.id, Title: "", Name: $(e).find('td a').text(), AttachmentType: $(e).find('td.type').text(), FileName: $(e).find('td.guid').text() };
                    }
                }
                return newObject;
            });
            var dataAsString = JSON.stringify(data);
            return dataAsString;
        },

        $scope.saveLectures = function () {
            var day = $scope.getLecturesFileAttachments();
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
                            $scope.lectures.splice($scope.lectures.indexOf(news), 1);
                        }
                    });
                }
            });
        };
    })
    .controller('LabsController', function ($scope) {

    });

