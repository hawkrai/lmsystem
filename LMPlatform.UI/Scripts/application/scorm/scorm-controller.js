angular.module('scormApp.controllers', [])
    .controller('ScormController', function ($scope, $http) {

    	$scope.scoObjects = [];
    	$scope.editScoObject = null;
    	$scope.viewScoClient = false;
    	$scope.urlServiceCore = "";
    	$scope.treeActivity = null;
    	$scope.nameLoadSco = "";
    	$scope.viewScoObject = null;

    	$scope.init = function () {
    		$scope.loadObjects();
    	};

    	$scope.closeSco = function () {
    		$scope.viewScoClient = false;
    		$scope.viewScoObject = null;
    		$scope.urlServiceCore = "";
    		$("#scoNameView").text("Просмотр SCO объектов");
    	};

    	$scope.frameLoad = function (urlRes) {
    		$scope.urlServiceCore = "/ScormObjects/" + $scope.viewScoObject.Path + "/" + urlRes;
    	};

    	$scope.viewSco = function (object) {
    		$scope.viewScoClient = true;
    		$scope.viewScoObject = object;

		    $("#scoNameView").text(object.Name);

    		$.ajax({
    			type: 'GET',
    			url: "/ScormMod/ViewSco?path=" + object.Path,
    			dataType: "json",
    			contentType: "application/json",

    		}).success(function (data, status) {
    			$scope.$apply(function () {
    				$scope.treeActivity = data;
    			});
    		});
    	};

    	$scope.loadObjects = function () {
    		$.ajax({
    			type: 'GET',
    			url: "/ScormMod/GetObjects",
    			dataType: "json",
    			contentType: "application/json",

    		}).success(function (data, status) {
    			$scope.$apply(function () {
    				$scope.scoObjects = data;
    			});

    		});
    	};

    	$scope.openSco = function () {
    		$($.find('input[type=file][name=openSco]')[0]).trigger('click');
    	};

    	$scope.requiredName = false;

    	$scope.loadScoEventClick = function () {
			if ($scope.nameLoadSco == null || $scope.nameLoadSco.length == 0) {
				$scope.requiredName = true;
				return false;
			}
			$scope.requiredName = false;
    		var formData = new FormData();


    		formData.append("name", $scope.nameLoadSco);
    		formData.append("file", $.find('input[type=file][name=openSco]')[0].files[0]);
    		$.ajax({
    			url: "/ScormMod/LoadObject",
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
    		$scope.nameLoadSco = "";
    		$('#dialogLoadSco').modal('hide');
    	};

    	$scope.loadSco = function () {
    		$scope.nameLoadSco = "";
    		$('#dialogLoadSco').modal();
    	};


    	$scope.deleteSco = function (object) {
    		bootbox.dialog({
    			message: "Вы действительно хотите удалить курс?",
    			title: "Удаление SCORM курса",
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
    							url: "/ScormMod/DeleteSco?path=" + object.Path,
    							dataType: "json",
    							contentType: "application/json",
    						}).success(function (data, status) {
    							$scope.$apply(function () {
    								$scope.scoObjects.splice($scope.scoObjects.indexOf(object), 1);
    							});

    						});
    					}
    				}
    			}
    		});


    	};

    	$scope.editSco = function (object) {
    		$scope.nameLoadSco = object.Name;
    		$scope.editScoObject = object;
    		$('#dialogChangeSco').modal();
    	};

    	$scope.changeScoName = function () {

    		if ($scope.nameLoadSco == null || $scope.nameLoadSco.length == 0) {
    			$scope.requiredName = true;
    			return false;
    		}
    		$scope.requiredName = false;

    		var formData = new FormData();

    		formData.append("name", $scope.nameLoadSco);
    		formData.append("path", $scope.editScoObject.Path);
    		$.ajax({
    			url: "/ScormMod/EditObject",
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
    		$scope.editScoObject = null;
		    $scope.nameLoadSco = "";
    		$('#dialogChangeSco').modal('hide');
    	};

    	$scope.lock = function (object) {
    		$.ajax({
    			type: 'GET',
    			url: "/ScormMod/UpdateObjects?enable=true&path=" + object.Path,
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
    			url: "/ScormMod/UpdateObjects?enable=false&path=" + object.Path,
    			dataType: "json",
    			contentType: "application/json",

    		}).success(function (data, status) {
    			$scope.$apply(function () {
    				$scope.loadObjects();
    			});

    		});
    	};
    });