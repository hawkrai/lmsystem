angular.module('dpApp.ctrl.news', ['ui.bootstrap', 'ui.bootstrap', 'xeditable'])
    .controller('newsCtrl', [ 
        '$scope',
        '$http',
        '$routeParams',
        '$location',
        '$resource',
        'projectService',
        '$sce',

        function ($scope, $http, $routeParams, $location,$resource, projectService, $sce) {

        $scope.renderHtml = function (htmlCode) {
            return $sce.trustAsHtml(htmlCode);
        };

        $scope.setTitle("Объявления по дипломному проектированию");
        $scope.news = [];
        $scope.UrlServiceNews = '/api/DiplomProjectNews/';
        $scope.UrlCpNews = '/Dp/';

        var a = projectService;

        $scope.newsDisabled = false;

        projectService.getNewses()
        .success(function (data) {
            $scope.news = data;
            for (var i = 0; i<$scope.news.length; i++) {
                if ($scope.news[i].Disabled) {
                    $scope.newsDisabled = true;
                    break;
                } else {
                    $scope.newsDisabled = false;
                }
            }
        });

        $scope.editNewsData = {
            TitleForm: "",
            Title: "",
            Body: "",
            IsOldDate: false,
            Disabled: false,
            PathFile: "",
            Id: 0
        };

        $scope.loadNews = function() {
            projectService.getNewses()
           .success(function (data) {
               $scope.news = data;
               for (var i = 0; i<$scope.news.length; i++) {
                   if ($scope.news[i].Disabled) {
                       $scope.newsDisabled = true;
                       break;
                   } else {
                       $scope.newsDisabled = false;
                   }
               }
           });
        }

        $scope.addNews = function () {
            $scope.editNewsData.TitleForm = "Создание объявления";
            $scope.editNewsData.Title = "";
            $scope.editNewsData.Body = "";
            $scope.editNewsData.Id = "0";
            $scope.editNewsData.IsOldDate = false;
            $scope.editNewsData.Disabled = false;
            $scope.editNewsData.PathFile = "";
            $("#newsIsOLd").attr('disabled', 'disabled');
            $("#newsFile").empty();

            $.ajax({
                type: 'GET',
                url: "/Dp/GetFileNews?id=0",
                contentType: "application/json",

            }).success(function (data, status) {
                $scope.$apply(function () {
                    $("#newsFile").append(data);
                });
            });
            $('#dialogAddNews').modal();
        };

        $scope.editNews = function (news) {
            $scope.editNewsData.TitleForm = "Редактирование объявления";
            $scope.editNewsData.Title = news.Title;
            $scope.editNewsData.Body = news.Body;
            $scope.editNewsData.Id = news.Id;
            $scope.editNewsData.Disabled = news.Disabled;
            $scope.editNewsData.IsOldDate = false;
            $scope.editNewsData.ShortName = news.ShortName;
            $("#newsIsOLd").removeAttr('disabled');
            $("#newsFile").empty();

            $.ajax({
                type: 'GET',
                url: "/Dp/GetFileNews?id=" + news.Id,
                contentType: "application/json",

            }).success(function (data, status) {
                $scope.$apply(function () {
                    $("#newsFile").append(data);
                });
            });
            $('#dialogAddNews').modal();
        };

        $scope.saveNews = function () {
        	if ($scope.editNewsData.Title == undefined || $scope.editNewsData.Title.length === 0) {
		        bootbox.dialog({
			        message: "Объявление имеет пустой заголовок, сохранить?",
			        title: "Сохранение объявления",
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


		$scope.fSaveNews = function() {
			$http({
				method: 'POST',
				url: $scope.UrlCpNews + "SaveNews",
				data: {
				    id: $scope.editNewsData.Id,
				    title: $scope.editNewsData.Title,
				    body: $scope.editNewsData.Body,
				    disabled: $scope.editNewsData.Disabled,
				    isOldDate: $scope.editNewsData.IsOldDate,
				    pathFile: $scope.editNewsData.PathFile,
				    attachments: $scope.getLecturesFileAttachments()
				},
				headers: { 'Content-Type': 'application/json' }
			}).success(function(data, status) {
			    alertify.success("Объявление успешно сохранено");
			    $scope.loadNews();
				$("#dialogAddNews").modal('hide');
			});
		};
		

        $scope.deleteNews = function (news) {
            bootbox.dialog({
                message: "Вы действительно хотите удалить объявление?",
                title: "Удаление объявления",
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
                                method: 'DELETE',
                                url: $scope.UrlServiceNews + "Delete",
                                data: { Id: news.Id, LecturerId: news.LecturerId },
                                headers: { 'Content-Type': 'application/json' }
                            }).success(function (data, status) {
                                    alertify.success("Объявление успешно удалено");
                                    $scope.news.splice($scope.news.indexOf(news), 1);
                                    $scope.loadNews();
                            });
                        }
                    }
                }
            });
        };
        
        $scope.disableNews = function () {
            $http({
                method: 'POST',
                url: $scope.UrlCpNews + "DisableNews",
                headers: { 'Content-Type': 'application/json' }
            }).success(function (data, status) {
                alertify.success("Все объявления скрыты");
                $scope.loadNews();
            });
        };

        $scope.enableNews = function () {
            $http({
                method: 'POST',
                url: $scope.UrlCpNews + "EnableNews",
                headers: { 'Content-Type': 'application/json' }
            }).success(function (data, status) {
                alertify.success("Все объявления активны");
                $scope.loadNews();
            });
        };

        }]).directive('showonhoverparent',
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
   });;
