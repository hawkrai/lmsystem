angular.module('cpApp.ctrl.news', ['ui.bootstrap', 'ui.bootstrap', 'xeditable'])
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

        $scope.setTitle("Объявления по курсовому проектированию");
        $scope.news = [];
        $scope.UrlServiceNews = '/api/CourseProjectNews/';
        

        function getParameterByName(name) {
            name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
            var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
                results = regex.exec(location.search);
            return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
        }

        var a = projectService;

        var subjectId = getParameterByName("subjectId");

            projectService.getNewses(subjectId)
            .success(function (data) {
                $scope.news = data;
            });

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
            $scope.editNewsData.TitleForm = "Создание объявления";
            $scope.editNewsData.Title = "";
            $scope.editNewsData.Body = "";
            $scope.editNewsData.Id = "0";
            $scope.editNewsData.IsOldDate = false;
            $scope.editNewsData.Disabled = false;
            $("#newsIsOLd").attr('disabled', 'disabled');
            $('#dialogAddNews').modal();
        };

        $scope.editNews = function (news) {
            $scope.editNewsData.TitleForm = "Редактирование объявления";
            $scope.editNewsData.Title = news.Title;
            $scope.editNewsData.Body = news.Body;
            $scope.editNewsData.Id = news.Id;
            $scope.editNewsData.Disabled = news.Disabled;
            $scope.editNewsData.IsOldDate = false;
            $("#newsIsOLd").removeAttr('disabled');
            $('#dialogAddNews').modal();
        };

        $scope.saveNews = function () {
        	if ($scope.editNewsData.Title == undefined || $scope.editNewsData.Title.length === 0) {
		        bootbox.dialog({
			        message: "Объявление имеет пустой заголовок, сохранить?",
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
				data: { subjectId: subjectId, id: $scope.editNewsData.Id, title: $scope.editNewsData.Title, body: $scope.editNewsData.Body, disabled: $scope.editNewsData.Disabled, isOldDate: $scope.editNewsData.IsOldDate },
				headers: { 'Content-Type': 'application/json' }
			}).success(function(data, status) {
			    alertify.success("Объявление успешно сохранено");
			    projectService.getNewses(subjectId)
                    .success(function (data) {
                    $scope.news = data;
                });
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
                                data: { Id: news.Id, SubjectId: subjectId },
                                headers: { 'Content-Type': 'application/json' }
                            }).success(function (data, status) {
                                    alertify.success("Объявление успешно удалено");
                                    $scope.news.splice($scope.news.indexOf(news), 1);
                            });
                        }
                    }
                }
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
