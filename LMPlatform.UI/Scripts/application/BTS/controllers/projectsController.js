angular
    .module('btsApp.ctrl.projects', [])
    .constant('PAGE_SIZE', 30)
    .constant('MIN_SEARCH_TEXT_LENGTH', 3)
    .controller('projectsCtrl', [
        '$scope',
        'projectsService',
        'PAGE_SIZE',
        'MIN_SEARCH_TEXT_LENGTH',
        function ($scope, projectsService, PAGE_SIZE, MIN_SEARCH_TEXT_LENGTH) {

            $scope.inputedSearchString = '';
            var searchString = '';
            var pageNumber = 0;
            $scope.projects = [];
            var busy = false;
            var nothingToLoad = false;

            $scope.loadProjects = function () {
                if (busy || nothingToLoad) return;
                busy = true;

                projectsService.getProjects(++pageNumber, PAGE_SIZE, searchString).then(function (response) {
                    if (response.data.Projects.length == 0) {
                        nothingToLoad = true;
                        busy = false;
                        return;
                    }
                    response.data.Projects.forEach(function (item, i) {
                        $scope.projects.push(item);
                    });
                    projectsService.addNumbering($scope.projects, (pageNumber - 1) * PAGE_SIZE);
                    busy = false;
                });
            };

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
                    $scope.projects = [];
                    pageNumber = 0;
                    nothingToLoad = false;
                    $scope.loadProjects();
                }
            };
        }]);
