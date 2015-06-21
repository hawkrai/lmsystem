angular
    .module('dpApp.ctrl.percentage', [])
    .controller('percentageCtrl', [
        '$scope',
        '$routeParams',
        '$location',
        'projectService',
        'percentageId',
        'percentages',
        '$modalInstance',
        function ($scope, $routeParams, $location, projectService, percentageId, percentages, $modalInstance) {

            $scope.isNew = angular.isUndefined(percentageId);

            $scope.percentage = {
                Theme: '',
                Id: null,
                Date: $scope.todayIso()
            };


            if (!$scope.isNew) {
                percentages.get({ id: percentageId }, function (data) {
                    $scope.percentage = data;
                });
            }

            projectService
                .getGroupCorrelation()
                .success(function (data) {
                    $scope.groups = data;
                });

            $scope.savePercentage = function () {
                $scope.submitted = true;
                if ($scope.forms.percentageForm.Name.$invalid
                    || $scope.forms.percentageForm.Percentage.$invalid
                    || $scope.forms.percentageForm.Date.$invalid) return false;

                percentages.save($scope.percentage)
                    .$promise.then(function () {
                        $modalInstance.close();
                        alertify.success('График успешно сохранен');
                    },
                    function (respData) {
//                        $modalInstance.close();
                        $scope.handleError(respData);
                    });
            };


            $scope.closeDialog = function () {
                $modalInstance.close();
            };

            $scope.datePickerOpen = function ($event) {
                $event.preventDefault();
                $event.stopPropagation();

                $scope.datePickerOpened = true;
            };

        }]);