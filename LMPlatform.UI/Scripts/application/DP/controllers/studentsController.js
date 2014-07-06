angular
    .module('dpApp.ctrl.students', ['ngTable'])
    .controller('studentsCtrl', [
        '$scope',
        '$modalInstance',
        'projectService',
        'ngTableParams',
        'projectId',
        function ($scope, $modalInstance, projectService, ngTableParams, projectId) {
            
            $scope.assignProject = function (studentId) {
//                bootbox.confirm("Вы действительно хотите назначить проект?", function (isConfirmed) {
//                    if (isConfirmed) {
                        projectService.assignProject(projectId, studentId).success(function () {
                            $modalInstance.close();
                            alertify.success("Проект успешно назначен.");
                        }).error(function (data) { 
                            $modalInstance.close();
                            alertify.error(data.ExceptionMessage);
                        });
//                    }
//                });
            };

            $scope.tableParams = new ngTableParams(
                {
                    page: 1,
                    count: 10,
                    filter: {
                        diplomProjectId: projectId
                    }
                }, {
                    total: 0,
                    counts: [],
                    hideDataTablesInfo: true,
                    getData: function ($defer, params) {
                        projectService.getStudents(projectId, params.url())
                            .success(function (data) {
                                $defer.resolve(data.Items);
                                params.total(data.Total);
                            });
                    }
                });

            $scope.closeDialog = function () {
                $modalInstance.close();
            };
        }]);