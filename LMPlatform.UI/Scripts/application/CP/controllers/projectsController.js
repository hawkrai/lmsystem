angular
    .module('cpApp.ctrl.projects', ['ngTable'])
    .controller('projectsCtrl', [
        '$scope',
        '$timeout',
        '$location',
        '$modal',
        'projectService',
        'ngTableParams',
        function ($scope, $timeout, $location, $modal, projectService, ngTableParams) {

            $scope.setTitle("Темы курсовых проектов (работ)");

           

            $scope.forms = {};

            $scope.editProject = function (projectId) {
                var modalInstance = $modal.open({
                    templateUrl: '/Cp/Project',
                    controller: 'projectCtrl',
                    scope: $scope,
                    resolve: {
                        projectId: function () {
                            return projectId;
                        }
                    }
                });

                modalInstance.result.then(function (result) {
                    $scope.tableParams.reload();
                });

            };

            $scope.assignProject = function (projectId) {
                var modalInstance = $modal.open({
                    templateUrl: '/Cp/Students',
                    controller: 'studentsCtrl',
                    scope: $scope,
                    resolve: {
                        projectId: function () {
                            return projectId;
                        }
                    }
                });

                modalInstance.result.then(function () {
                    $scope.tableParams.reload();
                });

            };

            $scope.deleteProject = function (id) {
                bootbox.confirm({
                    title: "Удаление темы курсового проекта (работы)",
                    message: "Вы действительно хотите удалить тему курсового проекта (работы)?",
                    callback: function (isConfirmed) {
                        if (isConfirmed) {
                            projectService.deleteproject(id).success(function () {
                                $scope.tableParams.reload();
                                alertify.success("Тема успешно удалена");
                            }).error(function (error) {
                                $scope.handleError(error);
                            });
                        }
                    },
                    buttons: {
                        'cancel': {
                            label: 'Отмена',
                            className: 'btn btn-sm'
                        },
                        'confirm': {
                            label: 'Удалить',
                            className: 'btn btn-primary btn-sm'
                        }
                    }
                });
            };

            $scope.deleteAssignment = function (id) {
                bootbox.confirm({
                    title: "Удаление назначения курсового проекта (работы)",
                    message: "Вы действительно хотите удалить назначение курсового проекта (работы)?",
                    callback: function (isConfirmed) {
                        if (isConfirmed) {
                            projectService.deleteAssignment(id).success(function () {
                                $scope.tableParams.reload();
                                alertify.success("Назначение успешно удалено");
                            }).error(function (error) {
                                $scope.handleError(error);
                            });
                        }
                    },
                    buttons: {
                        'cancel': {
                            label: 'Отмена',
                            className: 'btn btn-sm'
                        },
                        'confirm': {
                            label: 'Удалить',
                            className: 'btn btn-primary btn-sm'
                        }
                    }
                });
            };

            var hasChosenDp = false;
            $scope.userHasChosenDiplomProject = function () {
                return $scope.user.HasChosenDiplomProject || hasChosenDp;
            };

            $scope.chooseProject = function (id) {
                bootbox.confirm({
                    title: "Выбор темы курсового проекта (работы)",
                    message: "Вы действительно хотите выбрать данную тему курсового проекта (работы)?",
                    callback: function (isConfirmed) {
                        if (isConfirmed) {
                            projectService.assignProject(id).success(function () {
                                hasChosenDp = true;
                                $scope.tableParams.reload();
                                alertify.success("Тема успешно выбрана");
                            }).error(function (error) {
                                $scope.handleError(error);
                            });
                        }
                    },
                    buttons: {
                        'cancel': {
                            label: 'Отмена',
                            className: 'btn btn-sm'
                        },
                        'confirm': {
                            label: 'Выбрать',
                            className: 'btn btn-primary btn-sm'
                        }
                    }
                });
            };



            $scope.confirmProject = function (id) {
                bootbox.confirm({
                    title: "Подтверждение выбора темы курсового проекта (работы)",
                    message: "Вы действительно хотите подтвердить выбор данной темы курсового проекта (работы)?",
                    callback: function (isConfirmed) {
                        if (isConfirmed) {
                            projectService.assignProject(id).success(function () {
                                $scope.tableParams.reload();
                                alertify.success("Тема успешно подтверждена");
                            }).error(function (error) {
                                $scope.handleError(error);
                            });
                        }
                    },
                    buttons: {
                        'cancel': {
                            label: 'Отмена',
                            className: 'btn btn-sm'
                        },
                        'confirm': {
                            label: 'Подтвердить',
                            className: 'btn btn-primary btn-sm'
                        }
                    }
                });
            };

            $scope.downloadTaskSheet = function (id) {
                projectService.downloadTaskSheet(id);
            };

            function getParameterByName(name) {
                name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
                var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
                    results = regex.exec(location.search);
                return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
            }

            var subjectId = getParameterByName("subjectId");

            $scope.tableParams = new ngTableParams(
              /*  angular.extend({
                    page: 1,
                    sorting: {
                        Theme: 'asc'
                    }
                }, {}// $location.search()} uncomment in order to save params in the url
                )*/
                {
                    page: 1,
                    filter: {
                        subjectId: subjectId
                    }
                }
                , {
                    total: 0,
                    getData: function ($defer, params) {
                        //                        $location.search(params.url());
                        projectService.getProjects(subjectId, params.url())
                            .success(function (data) {
                                $defer.resolve(data.Items);
                                params.total(data.Total);
                                $scope.navigationManager.setListPage(params.url());
                            });
                    }
                });

            $scope.navigationManager.setListPage();

        }]);