angular
    .module('dpApp.ctrl.taskSheet', [])
    .controller('taskSheetCtrl', [
        '$scope',
        'projectService',
        '$sce',
        '$modal',
        function ($scope, projectService, $sce, $modal) {

            $scope.setTitle("Лист задания");

            $scope.projects = [{ Name: "proj1", Id: 1 }, { Name: "proj3", Id: 24 }];

            $scope.taskSheetHtml = "";

            projectService.getDiplomProjectCorrelation()
                .success(function (data) {
                    $scope.projects = data;
                });

            $scope.selectProject = function () {
                projectService.downloadTaskSheetHtml($scope.selectedProjectId)
                    .success(function (data) {
                        $scope.taskSheetHtml = $sce.trustAsHtml(data);
                    });
            };

            $scope.downloadTaskSheet = function () {
                projectService.downloadTaskSheet($scope.selectedProjectId);
            };


            $scope.editTaskSheet = function () {
                $modal.open({
                    templateUrl: '/Dp/TaskSheetEdit',
                    controller: editTaskSheetController,
                    resolve: {
                        projectId: function () {
                            return $scope.selectedProjectId;
                        }
                    }
                }).result.then(function (result) {
                    $scope.selectProject();
                });
            };

            var editTaskSheetController = function ($scope, $modalInstance) {

                $scope.taskSheet = {};

                $scope.saveTaskSheet = function () {
                    $modalInstance.close();
                };

                $scope.closeDialog = function () {
                    $modalInstance.close();
                };
            };
        }
    ]);