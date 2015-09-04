var appUserProfile = angular.module("appUserProfile", ["appUserProfile.controllers", "ui.bootstrap", "angularSpinner"]);

angular.module("appUserProfile.controllers", ["ui.bootstrap", "angularSpinner"])
	.controller("ProfileController", function ($scope, $sce, $http, usSpinnerService) {
		$scope.login = "";
		$scope.isMyProfile = false;

		$scope.loadProfileData = null;

		$scope.loading = false;
		$scope.init = function (userLogin, isMyProfile) {
			$scope.login = userLogin;
			$scope.isMyProfile = isMyProfile === "true" ? true : false;

			$scope.startSpin();

			$scope.loadProfileData();

			$scope.stopSpin();
		}

		$scope.loadProfileData = function () {
			$.ajax({
				type: 'POST',
				url: "/Profile/GetProfileInfo",
				dataType: "json",
				contentType: "application/json",
				async: false,
				data: JSON.stringify({ userLogin: $scope.login }),
			}).success(function (data, status) {
				$scope.loadProfileData = data;
			});
		}

		$scope.startSpin = function () {
			$(".loading").toggleClass('ng-hide', false);
			//usSpinnerService.spin('spinner-1');
		};

		$scope.stopSpin = function () {
			$(".loading").toggleClass('ng-hide', true);
			//usSpinnerService.stop('spinner-1');
		};
	});