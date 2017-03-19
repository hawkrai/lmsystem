angular
    .module('btsApp.ctrl.projects', ['ngTable'])
    .constant('PAGE_SIZE', 25)
    .constant('MIN_SEARCH_TEXT_LENGTH', 3)
    .controller('projectsCtrl', [
        '$scope',
        'projectsService',
        'PAGE_SIZE',
        'MIN_SEARCH_TEXT_LENGTH',
        'NgTableParams',
        function ($scope, projectsService, PAGE_SIZE, MIN_SEARCH_TEXT_LENGTH, NgTableParams) {

            $scope.inputedSearchString = '';
            var searchString = '';

            $scope.onAddProject = function () {
                $.savingDialog("Добавление проекта", "/BTS/AddProject", null, "primary", function (data) {
                    alertify.success("Добавлен новый проект");
                })
            };

            $scope.onEditProject = function (id) {
                $.savingDialog("Редактирование проекта", "/BTS/Editproject/" + id, null, "primary", function (data) {
                    alertify.success("Проект успешно изменен");
                });
            };

            function deleteProject(id) {
                console.log(id);
            };

            $scope.onDeleteProject = function (id) {
                bootbox.confirm({
                    title: 'Удаление проекта',
                    message: 'Вы дествительно хотите удалить проект?',
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
                    callback: function () {
                        deleteProject(id);
                    }
                });
            };

            function needReloadPage() {
                return ($scope.inputedSearchString.length >= MIN_SEARCH_TEXT_LENGTH || $scope.inputedSearchString.length == 0) && searchString != $scope.inputedSearchString;
            };

            $scope.onSearch = function () {
                if (needReloadPage()) {
                    searchString = $scope.inputedSearchString;
                    $scope.tableParams.reload();
                }
            };

            $scope.tableParams = new NgTableParams({
                sorting: { CreationDate: "desc" },
                count: PAGE_SIZE
            }, {
                getData: function (params) {
                    return projectsService.getProjects(params.page(), params.count(), searchString, params.orderBy()).then(function (response) {
                        params.total(response.data.TotalCount);
                        projectsService.addNumbering(response.data.Projects, (params.page() - 1) * params.count());
                        return response.data.Projects;
                    });
                }
            });
        }]);
