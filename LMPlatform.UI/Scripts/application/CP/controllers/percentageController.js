angular
    .module('cpApp.ctrl.percentage', [])
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

            function getParameterByName(name) {
                name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
                var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
                    results = regex.exec(location.search);
                return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
            }



            var subjectId = getParameterByName("subjectId");

            $scope.percentage = {
                Theme: '',
                Id: null,
                Date: $scope.todayIso(),
                SubjectId: subjectId
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