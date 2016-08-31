angular.module('joinLectorApp.controllers', [])
    .controller('JoinLectorController', function ($scope, $http) {

    	$scope.subjects = [];

    	$scope.lectors = [];

		$scope.joinLectors = [];

    	$scope.selectedSubject = null;

    	$scope.selectedLector = null;

    	$scope.init = function () {
    		$scope.loadSubjects();
    	};

    	$scope.loadSubjects = function () {
    		$.ajax({
    			type: 'GET',
    			url: "/Services/CoreService.svc/GetSubjectsByOwnerUser",
    			dataType: "json",
    			contentType: "application/json"

    		}).success(function (data) {
    			$scope.$apply(function () {
    				$scope.subjects = data.Subjects;
    			});
    		});
    	};

    	$scope.loadLectors = function () {
		    if ($scope.selectedSubject != null) {
			    $.ajax({
				    type: 'GET',
				    url: "/Services/CoreService.svc/GetNoAdjointLectors/" + $scope.selectedSubject.Id,
				    dataType: "json",
				    contentType: "application/json"

			    }).success(function(data) {
				    $scope.$apply(function() {
					    $scope.lectors = data.Lectors;
				    });
			    });
		    }
	    };

    	$scope.joinedLectors = function (subject) {
    		if ($scope.selectedSubject != null) {
    			$.ajax({
    				type: 'GET',
    				url: "/Services/CoreService.svc/GetJoinedLector/" + $scope.selectedSubject.Id,
    				dataType: "json",
    				contentType: "application/json"

    			}).success(function (data) {
    				$scope.$apply(function () {
    					$scope.joinLectors = data.Lectors;
    				});
    			});

    			$scope.loadLectors();
    		}
    	};

    	$scope.join = function () {
    		if ($scope.selectedSubject != null && $scope.selectedLector != null) {
    			$http({
    				method: 'POST',
    				url: "/Services/CoreService.svc/JoinLector",
    				data: { subjectId: $scope.selectedSubject.Id, lectorId: $scope.selectedLector.LectorId },
    				headers: { 'Content-Type': 'application/json' }
    			}).success(function () {
    				$scope.selectedLector = null;
    				$scope.joinedLectors($scope.selectedSubject);
    			});
		    }
    	};

		$scope.disjoin = function(lectorId) {
			if ($scope.selectedSubject != null) {
				$http({
					method: 'POST',
					url: "/Services/CoreService.svc/DisjoinLector",
					data: { subjectId: $scope.selectedSubject.Id, lectorId: lectorId },
					headers: { 'Content-Type': 'application/json' }
				}).success(function () {
					$scope.selectedLector = null;
					$scope.joinedLectors($scope.selectedSubject);
				});
			}
		};
	});