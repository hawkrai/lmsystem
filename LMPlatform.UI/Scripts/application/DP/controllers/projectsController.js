﻿angular
    .module('dpApp.ctrl.projects', ['ngTable'])
    .controller('projectsCtrl', [
        '$scope',
        '$timeout',
        '$location',
        '$modal',
        'projectService',
        'ngTableParams',
        function ($scope, $timeout, $location, $modal, projectService, ngTableParams) {

            $scope.setTitle("Темы дипломных проектов");

            $scope.forms = {};

            $scope.editProject = function (projectId) {
                var modalInstance = $modal.open({
                    templateUrl: '/Dp/Project',
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
                    templateUrl: '/Dp/Students',
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
                    title: "Удаление темы",
                    message: "Вы действительно хотите удалить тему дипломного проекта?",
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
                            className: 'btn btn-default btn-sm'
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
                    title: "Удаление",
                    message: "Вы действительно хотите удалить назначение дипломного проекта?",
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
                            className: 'btn btn-default btn-sm'
                        },
                        'confirm': {
                            label: 'Удалить',
                            className: 'btn btn-primary btn-sm'
                        }
                    }
                });
            };

            var hasChosenDp = false;
            $scope.userHasChosenDiplomProject = function() {
                return $scope.user.HasChosenDiplomProject || hasChosenDp;
            };

            $scope.chooseProject = function (id) {
                bootbox.confirm({
                    title: "Выбор темы",
                    message: "Вы действительно хотите выбрать данную тему дипломного проекта?",
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
                            className: 'btn btn-default btn-sm'
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
                    title: "Подтверждение темы",
                    message: "Вы действительно хотите подтвердить данную тему дипломного проекта?",
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
                            className: 'btn btn-default btn-sm'
                        },
                        'confirm': {
                            label: 'Подтвердить',
                            className: 'btn btn-primary btn-sm'
                        }
                    }
                });
            };

            $scope.downloadTaskSheet = function(id) {
                projectService.downloadTaskSheet(id);
            };
            
            $scope.searchString = "";
            $scope.search = function () {
                $scope.tableParams.filter.searchString = $scope.searchString;
                $scope.tableParams.reload();
            }

            $scope.tableParams = new ngTableParams(
                angular.extend({
                    page: 1,
                    filter: {
                        searchString: $scope.searchString
                    },
                    sorting: {
                        Theme: 'asc'
                    }
                }, {}
                )
                , {
                    total: 0,
                    getData: function ($defer, params) {
                        projectService.getProjects(angular.extend(params.url(), {
                            filter:
                            {
                                searchString: $scope.searchString
                            }
                        }))
                            .success(function (data) {
                                $defer.resolve(data.Items);
                                params.total(data.Total);
                                $scope.navigationManager.setListPage(params.url());
                            });
                    }
                });

            $scope.navigationManager.setListPage();

        }]);