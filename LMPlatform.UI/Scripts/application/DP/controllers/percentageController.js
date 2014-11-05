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

            var now = new Date();
            $scope.percentage = {
                Theme: '',
                Id: null,
                SelectedGroupsIds: [],
                Date: new Date(Date.UTC(now.getFullYear(), now.getMonth(), now.getDate()))
        };
            $scope.groups = [];

            if (!$scope.isNew) {
                percentages.get({ id: percentageId }, function (data) {
                    $scope.percentage = data;
                });
            }

            projectService
                .getGroupCorrelation()
                .success(function (data, status, headers, config) {
                    $scope.groups = data;
                });

            $scope.savePercentage = function () {
                $scope.submitted = true;
                if ($scope.forms.percentageForm.Name.$invalid
                    || $scope.forms.percentageForm.Percentage.$invalid
                    || $scope.forms.percentageForm.Date.$invalid) return false;
                
                percentages.save($scope.percentage)
                    .$promise.then(function (data, status, headers, config) {
                        $modalInstance.close();
                        alertify.success('График успешно сохранен.');
                    },
                    function (respData) {
                        $modalInstance.close();
                        $scope.handleError(respData);
                    });
            };


            $scope.closeDialog = function () {
                $modalInstance.close();
            };

        }]);