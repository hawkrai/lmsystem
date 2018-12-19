angular.module('tincanApp.controllers', [])
    .controller('TinCanController', function ($scope, $window) {
        $scope.endpoint = $window.lrsEndpoint;
        $scope.auth = $window.auth;
        $scope.actor = $window.actor;
        $scope.statements = [];
        $scope.editTinCanObject = null;
		$scope.viewTinCanClient = false;
        $scope.urlServiceCore = "";
        $scope.treeActivity = null;
		$scope.nameLoadTinCan = "";
		$scope.viewTinCanObject = null;

        $scope.init = function () {
            $scope.loadObjects();
        };

		$scope.closeTinCan = function () {
			$scope.viewTinCanClient = false;
			$scope.viewTinCanObject = null;
            $scope.urlServiceCore = "";
			$("#tinCanNameView").text("Просмотр TinCan объектов");
        };

        $scope.frameLoad = function (urlRes) {
            $scope.urlServiceCore = "/" + urlRes + "?"
                + "endpoint=" + $window.encodeURIComponent($scope.endpoint)
                + "&auth=" + $window.encodeURIComponent($scope.auth)
                + "&actor=" + $window.encodeURIComponent(JSON.stringify($scope.actor));
            document.getElementById('tin').src = document.getElementById('tin').src;
        };
		$scope.viewTinCan = function (object) {
			$scope.viewTinCanClient = true;
			$scope.viewTinCanObject = object;

			$("#tinCanNameView").text(object.StatementJson);
            
            $.ajax({
                type: 'GET',
				url: "/TinCanMod/ViewTinCan?id=" + object.Id,
                dataType: "json",
                contentType: "application/json",

            }).success(function (data, status) {
                $scope.$apply(function () {
                    $scope.treeActivity = data;
                    $scope.frameLoad(data);
                });
            });
        };

        $scope.loadObjects = function () {
            $.ajax({
                type: 'GET',
                url: "/TinCanMod/GetObjects",
                dataType: "json",
                contentType: "application/json",

            }).success(function (data, status) {
                $scope.$apply(function () {
                    $scope.statements = data;
                });

            });
        };

		$scope.openTinCan = function () {
			$($.find('input[type=file][name=openTinCan]')[0]).trigger('click');
        };
        
        $scope.requiredName = false;
		$scope.loadTinCanEventClick = function () {
			if ($scope.nameLoadTinCan === null || $scope.nameLoadTinCan.length === 0) {
                $scope.requiredName = true;
                return false;
            }
            $scope.requiredName = false;
            var formData = new FormData();


			formData.append("name", $scope.nameLoadTinCan);
			formData.append("file", $.find('input[type=file][name=openTinCan]')[0].files[0]);
            $.ajax({
                url: "/TinCanMod/LoadObject",
                type: 'POST',
                data: formData,
                processData: false,
                contentType: false,
                success: function (data) {
                    if (data.error !== undefined) {
                        alertify.error(data.error);
                    } else {
                        $scope.$apply(function () {
                            $scope.loadObjects();
                        });
                    }

                },
            });
			$scope.nameLoadTinCan = "";
            $('#dialogLoadTinCan').modal('hide');
        };
		$scope.loadTinCan = function () {
			$scope.nameLoadTinCan = "";
			$('#dialogLoadTinCan').modal();
        };

		$scope.deleteTinCan = function (object) {
            bootbox.dialog({
                message: "Вы действительно хотите удалить курс?",
				title: "Удаление TinCan курса",
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
                            $.ajax({
                                type: 'GET',
								url: "/TinCanMod/DeleteTinCan?id=" + object.Id,
                                dataType: "json",
                                contentType: "application/json",
                            }).success(function (data, status) {
                                $scope.$apply(function () {
                                    $scope.statements.splice($scope.statements.indexOf(object), 1);
                                });

                            });
                        }
                    }
                }
            });


        };

        $scope.editTinCan = function (object) {
            $scope.nameLoadTinCan = object.Name;
            $scope.editTinCanObject = object;
            $('#dialogChangeTinCan').modal();
        };

        $scope.changeTinCanName = function () {

            if ($scope.nameLoadTinCan === null || $scope.nameLoadTinCan.length === 0) {
                $scope.requiredName = true;
                return false;
            }
            $scope.requiredName = false;

            var formData = new FormData();

            formData.append("name", $scope.nameLoadTinCan);
            formData.append("path", $scope.editTinCanObject.Path);
            $.ajax({
                url: "/TinCanMod/EditObject",
                type: 'POST',
                data: formData,
                processData: false,
                contentType: false,
                success: function (data) {
                    $scope.$apply(function () {
                        $scope.loadObjects();
                    });
                },
            });
            $scope.editTinCanObject = null;
            $scope.nameLoadTinCan = "";
            $('#dialogChangeTinCan').modal('hide');
        };

        $scope.lock = function (object) {
            $.ajax({
                type: 'GET',
                url: "/TinCanMod/UpdateObjects?enable=true&id=" + object.Id,
                dataType: "json",
                contentType: "application/json",

            }).success(function (data, status) {
                $scope.$apply(function () {
                    $scope.loadObjects();
                });

            });
        };

        $scope.unlock = function (object) {
            $.ajax({
                type: 'GET',
                url: "/TinCanMod/UpdateObjects?enable=false&id=" + object.Id,
                dataType: "json",
                contentType: "application/json",

            }).success(function (data, status) {
                $scope.$apply(function () {
                    $scope.loadObjects();
                });

            });
        };
    });