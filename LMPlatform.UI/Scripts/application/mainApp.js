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
            $scope.constDomainService = 'api/';
            $scope.subjectId = subjectId;
        };
    })
    .controller('NewsController', function ($scope) {

        $scope.news = [];

        $scope.editNewsData = {
            TitleForm: "",
            Title: "",
            Body: "",
            Id : 0
        };

        $scope.init = function () {
            $scope.news = [];
            $scope.loadNews();
            //$('#newsHtml').wysihtml5();
        };

        $scope.loadNews = function () {
            $.ajax({
                type: 'GET',
                url: $scope.constDomainService + "News/GetNews?subjectId=" + $scope.subjectId,
                dataType: "json",
                contentType: "application/json",

            }).success(function (data, status) {
                if (data.Data.Error) {
                    alertify.error(data.Data.Message);
                } else {
                    $scope.$apply(function () {
                        $scope.news = data.Data.Data;
                    });
                }
            });
        };

        $scope.addNews = function () {
            $scope.editNewsData.TitleForm = "Создание новости";
            $scope.editNewsData.Title = "";
            $scope.editNewsData.Body = "";
            $scope.editNewsData.Id = 0;
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
            $.post($scope.constDomainService + "News/Save", { subjectId: $scope.subjectId, Id: $scope.editNewsData.Id, title: $scope.editNewsData.Title, body: $scope.editNewsData.Body }, function (data) {
                if (data.Data.Error) {
                    spinner.stop();
                    alertify.error(data.Data.Message);
                } else {
                    $scope.$apply(function() {
                        $scope.loadNews();
                    });
                    alertify.success(data.Data.Message);
                }
                $("#dialogAddNews").modal('hide');
            });
        };

        $scope.deleteNews = function(news) {
            bootbox.confirm("Вы действительно хотите удалить новость?", function (isConfirmed) {
                if (isConfirmed) {
                    $.post($scope.constDomainService + "News/Delete?Id=" + news.NewsId + "&SubjectId=" + $scope.subjectId, null, function (data) {
                        if (data.Data.Error) {
                            alertify.error(data.Data.Message);
                        } else {
                            alertify.success(data.Data.Message);
                            $scope.$apply(function () {
                                $scope.news.splice($scope.news.indexOf(news), 1);
                            });

                        }
                    }, null);
                }
            });
        };
    })


    .controller('LecturesController', function ($scope) {

    })
    .controller('LabsController', function ($scope) {

    });

