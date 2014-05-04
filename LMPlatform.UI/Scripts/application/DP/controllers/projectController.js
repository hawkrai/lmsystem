angular
    .module('dpApp.ctrl.project', [])
    .controller('projectCtrl', [
        '$scope',
        '$routeParams',
        '$location',
        'projectService',
        function ($scope, $routeParams, $location, projectService) {

            $scope.isNew = angular.isUndefined($routeParams.id);

            $scope.project = {
                Theme: '',
                Id: null,
                SelectedGroupsIds: []
            };
            $scope.groups = [];
            
            if (!$scope.isNew) {
                projectService
                    .getProject($routeParams.id)
                    .success(function (data, status, headers, config) {
                        $scope.project = data.Project;
                        $scope.groups = data.Groups;
                    });
            }


            $scope.saveProject = function () {

                if ($scope.isNew) {
                    projectService
                        .createProject($scope.project)
                        .success(function (data, status, headers, config) {
                            $scope.navigationManager.goToListPage();
                        })
                        .error(function (data, status, headers, config) {
                            $scope.errorMessage = (data || { message: "Create operation failed." }).message + (' [HTTP-' + status + ']');
                        });
                } else {
                    projectService
                        .updateProject($scope.project)
                        .success(function (data, status, headers, config) {
                            $scope.navigationManager.goToListPage();
                        })
                        .error(function (data, status, headers, config) {
                            $scope.errorMessage = (data || { message: "Update operation failed." }).message + (' [HTTP-' + status + ']');
                        });
                }
            };


            $scope.returnToList = function () {
                $scope.navigationManager.goToListPage();
            };
      
        }]);