angular
    .module('cpApp.ctrl.taskSheet', [])
    .controller('taskSheetCtrl', [
        '$scope',
        'projectService',
        '$sce',
        '$modal',
        '$resource',
        function ($scope, projectService, $sce, $modal, $resource) {

            $scope.setTitle("Лист задания");

            $scope.projects = [];
            $scope.project = { Id: null };

            $scope.groups = [];
            $scope.group = {
                Id: null,
                Name: ""
            };

            $scope.taskSheetHtml = "";

            function getParameterByName(name) {
                name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
                var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
                    results = regex.exec(location.search);
                return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
            }

            var subjectId = getParameterByName("subjectId");

            projectService.getCourseProjectCorrelation(subjectId)
                .success(function (data) {
                    $scope.projects = data;
                });

            projectService.getGroups(subjectId)
                .success(function (data) {
                    $scope.groups = data;
                });

            $scope.selectProject = function (project) {
                if (project) {
                    $scope.selectedProjectId = project.Id;
                }
                projectService.downloadTaskSheetHtml($scope.selectedProjectId)
                    .success(function (data) {
                        $scope.taskSheetHtml = $sce.trustAsHtml(data);
                    });
            };

            $scope.downloadTaskSheet = function () {
                projectService.downloadTaskSheet($scope.selectedProjectId);
            };


            $scope.editTaskSheet = function () {
                $modal.open({
                    templateUrl: '/Cp/TaskSheetEdit',
                    controller: editTaskSheetController,
                    scope: $scope,
                    resolve: {
                        projectId: function () {
                            return $scope.selectedProjectId;
                        }
                    }
                }).result.then(function (result) {
                    $scope.selectProject();
                });
            };

            var editTaskSheetController = function ($scope, $modalInstance) {

                var taskSheets = $resource('api/CpTaskSheet');
                var taskSheetTemplates = $resource('api/CpTaskSheetTemplate');
                $scope.taskSheet = {};

                $scope.templates = [];
                $scope.template = { Id: null };

                $scope.tasks = [];

                $scope.multipleChoice = {}; // encapsulate your model inside an object.
                $scope.multipleChoice.selectedGroups = [];

                function updateTemplates() {
                    projectService.getDiplomProjectTaskSheetTemplateCorrelation(subjectId)
                        .success(function(data) {
                            $scope.templates = data;
                        });
                };

                

                updateTemplates();
                $scope.form = {};
                $scope.dateStartPickerOpen = function ($event) {
                    $event.preventDefault();
                    $event.stopPropagation();

                    $scope.form.dateStartPickerOpened = true;
                };

                $scope.dateEndPickerOpen = function ($event) {
                    $event.preventDefault();
                    $event.stopPropagation();

                    $scope.form.dateEndPickerOpened = true;
                };

                $scope.selectTemplate = function (template) {
                    $scope.selectedTemplateId = template.Id;
                    $scope.template.Name = template.Name;
                    taskSheetTemplates.get({ templateId: template.Id }, function (data) {
                        $scope.taskSheet.InputData = data.InputData;
                        $scope.taskSheet.RpzContent = data.RpzContent;
                        $scope.taskSheet.DrawMaterials = data.DrawMaterials;
                        $scope.taskSheet.Consultants = data.Consultants;
                        $scope.taskSheet.Faculty = data.Faculty;
                        $scope.taskSheet.HeadCathedra = data.HeadCathedra;
                        $scope.taskSheet.Univer = data.Univer;
                        $scope.taskSheet.DateEnd = data.DateEnd;
                        $scope.taskSheet.DateStart = data.DateStart;
                    });
                };

                taskSheets.get({ courseProjectId: $scope.selectedProjectId }, function (data) {
                    $scope.taskSheet = data;
                });

                taskSheets.query(function (data) {
                    $scope.tasks = data;
                });

                $scope.saveTaskSheetTemplate = function () {

                    var template = {};
                    angular.copy($scope.taskSheet, template);
                    template.Id = $scope.selectedTemplateId || 0;
                    template.Name = $scope.template.Name;

                    if (template.Id == 0 && !template.Name) {
                        alert('Выберите шаблон или введите название нового шаблона!' + $scope.templateName);//TODO
                        return;
                    }

                    taskSheetTemplates.save(template)
                        .$promise.then(function () {
                            alertify.success('Данные успешно сохранены');
                            updateTemplates();
                        }, $scope.handleError);
                };


                $scope.saveTaskSheet = function () {

                    taskSheets.save($scope.taskSheet)
                        .$promise.then(function (data, status, headers, config) {
                            $scope.selectProject();
                            alertify.success('Данные успешно сохранены');
                        }, $scope.handleError);

                    $modalInstance.close();
                };

                $scope.applyTaskSheetTemplate = function () {
                    if ($scope.multipleChoice.selectedGroups.length > 0) {
                        angular.forEach($scope.projects, function (value, key) {
                            var flag = $scope.multipleChoice.selectedGroups.findIndex(x => x.Id === value.GroupId) != -1
                            if (flag) {

                                var task = $scope.tasks.find(function (p) {
                                    return p.CourseProjectId === this.Id;
                                }, value);

                                task.InputData = $scope.taskSheet.InputData;
                                task.RpzContent = $scope.taskSheet.RpzContent;
                                task.DrawMaterials = $scope.taskSheet.DrawMaterials;
                                task.Consultants = $scope.taskSheet.Consultants;
                                task.Faculty = $scope.taskSheet.Faculty;
                                task.HeadCathedra = $scope.taskSheet.HeadCathedra;
                                task.Univer = $scope.taskSheet.Univer;
                                task.DateEnd = $scope.taskSheet.DateEnd;
                                task.DateStart = $scope.taskSheet.DateStart;

                                taskSheets.save(task);
                                $scope.selectProject();
                            };
                        });

                        alertify.success('Шаблон применен');
                        $modalInstance.close();
                    } else {
                        alertify.error('Выберите группу')
                    }
                };

                $scope.closeDialog = function () {
                    $modalInstance.close();
                };
            };
        }
    ]);