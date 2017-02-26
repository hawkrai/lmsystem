angular
    .module('btsApp.ctrl.projects', [])
    .constant('PAGE_SIZE', 30)
    .controller('projectsCtrl', [
        '$scope',
        'projectsService',
        'PAGE_SIZE',
        function ($scope, projectsService, PAGE_SIZE) {

            var pageNumber = 0;
            $scope.projects = [];
            var busy = false;

            $scope.loadProjects = function () {
                if (busy) return;
                busy = true;

                projectsService.getProjects(++pageNumber, PAGE_SIZE).then(function (response) {
                    if (response.data.Data.Data.length == 0) {
                        busy = true;
                        return;
                    }
                    response.data.Data.Data.forEach(function (item, i) {
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
                            className: 'btn btn-primary btn-sm'
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
        }]);
