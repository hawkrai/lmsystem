angular
    .module('dpApp.ctrl.projects', ['ngTable'])
    .controller('projectsCtrl', [
        '$scope',
        '$timeout',
        '$location',
        'projectService',
        'ngTableParams',
        function ($scope, $timeout, $location, projectService, ngTableParams) {

            $scope.createProject = function () {
                $location.path("/Project").search({});
            };
            $scope.editProject = function (id) {
                $location.path("/Project/" + id).search({});
            };
            $scope.deleteProject = function (id) {
                bootbox.confirm("Вы действительно хотите удалить тему?", function (isConfirmed) {
                    if (isConfirmed) {
                        projectService.deleteproject(id).success(function() {
                            alertify.success("Тема успешно удалена.");
                        });
                    }
                });
            };
            
            $scope.tableParams = new ngTableParams(
                angular.extend({
                    page: 1,            // show first page
                    count: 10          // count per page
                }, $location.search()), {
                    total: 0,           // length of data
                    getData: function ($defer, params) {
                        $location.search(params.url());
                        projectService.getProjects(params.url())
                            .success(function (data) {
                                $defer.resolve(data.Data);
                                params.total(data.Total);
                                $scope.navigationManager.setListPage(params.url());
                            });
                    }
                });

            $scope.navigationManager.setListPage();

        }]);