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

            function clearBugs() {
                $scope.bugs = {
                    totalCount: 0,
                    types: [
                        {
                            name: 'Низкая',
                            count: 0,
                            style: { width: '0%' }
                        },
                        {
                            name: 'Средняя',
                            count: 0,
                            style: { width: '0%' }
                        },
                        {
                            name: 'Высокая',
                            count: 0,
                            style: { width: '0%' }
                        },
                        {
                            name: 'Критическая',
                            count: 0,
                            style: { width: '0%' }
                        }
                    ],
                    statuses: []
                };
            }

            function setProject() {
                clearBugs();
                projectsService.getProjectWithBugsAndMembers($routeParams.id).then(function (response) {
                    $scope.setTitle(response.data.Project.Title);
                    $scope.project = response.data.Project;
                    setBugs();
                });
            }

            function setBugs() {
                $scope.bugs.totalCount = $scope.project.Bugs.length;
                $scope.project.Bugs.forEach(function (bug) {
                    $scope.bugs.types.forEach(function (bugType) {
                        if (bugType.name === bug.Severity) {
                            bugType.count = bugType.count + 1;
                        }
                    });
                });
                $scope.bugs.types.forEach(function (bugType) {
                    var percantage = bugType.count * 100.0 / $scope.bugs.totalCount;
                    bugType.style.width =  percantage + '%';
                });

                setTimeout(function () {
                    setBugsStatuses();
                    setGraph();
                }, 100);
            }

            function setBugsStatuses() {
                $scope.project.Bugs.forEach(function (bug) {
                    var existingElem;
                    var exist = $scope.bugs.statuses.some(function (elem) {
                        existingElem = elem;
                        return elem.label === bug.Status;
                    });
                    if (exist) {
                        existingElem.count = existingElem.count + 1;
                    } else {
                        $scope.bugs.statuses.push({ label: bug.Status, count: 1 });
                    }
                });

                $scope.bugs.statuses.forEach(function (elem) {
                    elem.value = elem.count * 100.0 / $scope.bugs.totalCount;
                });
            }

            function setGraph() {

                Morris.Donut({
                    element: 'graph',
                    data: $scope.bugs.statuses,
                    formatter: function (x) { return x + "%" }
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

            $scope.getBugCount = function (name) {
                return $scope.project.Members.filter(function (elem) {
                    return elem.Name === name;
                }).length;
            }

            $scope.getBugPercentage = function (name) {

            }

            init();
        }]);
