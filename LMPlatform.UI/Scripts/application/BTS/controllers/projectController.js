angular
    .module('btsApp.ctrl.project', [])
    .controller('projectCtrl', [
        '$scope',
        '$routeParams',
        'projectsService',
        function ($scope, $routeParams, projectsService) {
            $scope.project = {};

            var projectManagerRoleName = 'Руководитель проекта';

            function init() {
                setProject();
            }

            function setProject() {
                projectsService.getProjectWithBugsAndMembers($routeParams.id).then(function (response) {
                    $scope.setTitle(response.data.Project.Title);
                    $scope.project = response.data.Project;
                });
            }

            $scope.isProjectManager = function () {
                if ($scope.project.Members) {
                    return $scope.project.Members.some(function (elem) {
                        return elem.UserId === $scope.$parent.userId && elem.Role === projectManagerRoleName;
                    });
                }
            }

            $scope.onAddStudent = function () {
                $.savingDialog("Добавление участника к проекту", "/BTS/AssignStudentOnProject/" + $scope.project.Id,
                    null, "primary", function (data) {
                        setProject();
                        alertify.success("Добавлен новый участник");
                    });
            }

            $scope.onAddLecturer = function () {
                $.savingDialog("Добавление участника к проекту", "/BTS/AssignLecturerOnProject/" + $scope.project.Id,
                    null, "primary", function (data) {
                        setProject();
                        alertify.success("Добавлен новый участник");
                    });
            }

            $scope.onClearProject = function () {
                bootbox.confirm("Вы действительно хотите очистить проект (удалить участников, ошибки и комментарии)?", function (isConfirmed) {
                    if (isConfirmed) {
                        $.post("/BTS/ClearProject/" + $scope.project.Id, null, function () {
                        });
                        location.reload();
                    }
                });
            }

            function deleteProjectUser(id) {
                projectsService.deleteProjectUser(id).then(function (response) {
                    setProject();
                    alertify.success("Участник проекта удален");
                });
            }

            $scope.onDeleteProjectUser = function (id) {
                bootbox.confirm({
                    title: 'Удаление участника проекта',
                    message: 'Вы дествительно хотите удалить участника проекта?',
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
                        if (result) {
                            deleteProjectUser(id);
                        }
                    }
                });
            };

            init();
        }]);
