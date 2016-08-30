angular.module('studentManagementApp.controllers', [])
    .controller('StudentManagementController', function ($scope, $http) {

    	$scope.groups = [];

		$scope.students = [];

		$scope.selectedGroup = null;

    	$scope.init = function () {
		    $scope.loadGroups();
    	};

		$scope.loadGroups = function() {
			$.ajax({
				type: 'GET',
				url: "/Services/CoreService.svc/GetAllGroupsLite",
				dataType: "json",
				contentType: "application/json"

			}).success(function(data) {
				$scope.$apply(function() {
					$scope.groups = data.Groups;
				});
			});
		};

		$scope.viewStudent = function(group) {
			$.ajax({
				type: 'GET',
				url: "/Services/CoreService.svc/GetStudentsByGroupId/" + group.GroupId,
				dataType: "json",
				contentType: "application/json"

			}).success(function (data) {
				$scope.$apply(function () {
					$scope.students = data.Students;
				});
			});
		};

		$scope.confirmationStudent = function(id) {
			$.ajax({
				type: 'PUT',
				url: "/Services/CoreService.svc/СonfirmationStudent/" + id,
				dataType: "json",
				contentType: "application/json"

			}).success(function () {
				$scope.viewStudent($scope.selectedGroup);
			});
		};
	});