angular
    .module('btsApp.ctrl.bugs', ['ngTable'])
    .controller('bugsCtrl', [
        '$scope',
        '$routeParams',
        'bugsService',
        'projectsService',
        'PAGE_SIZE',
        'MIN_SEARCH_TEXT_LENGTH',
        'NgTableParams',
        function ($scope, $routeParams, bugsService, projectsService, PAGE_SIZE, MIN_SEARCH_TEXT_LENGTH, NgTableParams) {

            $scope.inputedSearchString = '';
            var searchString = '';
            $scope.isProjectBugsPage = false;

            function init() {
                $scope.setTitle('Управление ошибками');
                if ($routeParams.projectId !== null) {
                    $scope.isProjectBugsPage = true;
                    setProjectTitle();
                }   
            };

            function setProjectTitle() {
                projectsService.getProject($routeParams.projectId).then(function (response) {
                    $scope.projectTitle = response.data.Project.Title;
                });
            }

            $scope.onAddBug = function (projectId) {
                $.savingDialog("Документирование ошибки", "/BTS/AddBug", null, "primary", function (data) {
                    $scope.tableParams.reload();
                    alertify.success("Добавлена новая ошибка");
                });
            };

            function deleteBug(id) {
                bugsService.deleteBug(id).then(function () {
                    $scope.tableParams.reload();
                    alertify.success("Ошибка удалена");
                });
            };

            $scope.onDeleteBug = function (id) {
                bootbox.confirm({
                    title: 'Удаление ошибки',
                    message: 'Вы дествительно хотите удалить ошибку?',
                    buttons: {
                        'cancel': {
                            label: 'Отмена',
                            className: 'btn btn-default btn-sm'
                        },
                        'confirm': {
                            label: 'Удалить',
                            className: 'btn btn-primary btn-sm',
                        }
                    },
                    callback: function (result) {
                        if (result)
                            deleteBug(id);
                    }
                });
            };

            function needReloadPage() {
                return ($scope.inputedSearchString.length >= MIN_SEARCH_TEXT_LENGTH || $scope.inputedSearchString.length === 0) && searchString !== $scope.inputedSearchString;
            };

            $scope.onSearch = function () {
                if (needReloadPage()) {
                    searchString = $scope.inputedSearchString;
                    $scope.tableParams.reload();
                }
            };

            $scope.tableParams = new NgTableParams({
                sorting: { ModifyingDate: "desc" },
                count: PAGE_SIZE
            }, {
                getData: function (params) {
                    if ($scope.isProjectBugsPage) {
                        return bugsService.getBugs(params.page(), params.count(), searchString, params.orderBy(), $routeParams.projectId).then(function (response) {
                            params.total(response.data.TotalCount);
                            bugsService.addNumbering(response.data.Bugs, (params.page() - 1) * params.count());
                            return response.data.Bugs;
                        });
                    }
                    return bugsService.getBugs(params.page(), params.count(), searchString, params.orderBy()).then(function (response) {
                        params.total(response.data.TotalCount);
                        bugsService.addNumbering(response.data.Bugs, (params.page() - 1) * params.count());
                        return response.data.Bugs;
                    });
                }
            });

            init();
        }]);
