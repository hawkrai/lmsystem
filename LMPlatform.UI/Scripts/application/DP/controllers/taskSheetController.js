angular
    .module('dpApp.ctrl.taskSheet', [])
    .controller('taskSheetCtrl', [
        '$scope',
        'projectService',
        '$sce',
        '$modal',
        '$resource',
        function ($scope, projectService, $sce, $modal, $resource) {

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
                    scope: $scope,
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

                var taskSheets = $resource('api/TaskSheet');
                $scope.taskSheet = {};

                taskSheets.get({ diplomProjectId: $scope.selectedProjectId }, function (data) {
                    $scope.taskSheet = data;
                });


                $scope.saveTaskSheet = function () {

                    taskSheets.save($scope.taskSheet)
                        .$promise.then(function (data, status, headers, config) {
                            $scope.selectProject();
                            alertify.success('Данные успешно сохранены.');
                        }, $scope.handleError);

                    $modalInstance.close();
                };

                $scope.closeDialog = function () {
                    $modalInstance.close();
                };
            };
        }
    ]);