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

            $scope.taskSheetHtml = "";

            projectService.getDiplomProjectCorrelation()
                .success(function (data) {
                    $scope.projects = data;
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

                function updateTemplates() {
                    projectService.getDiplomProjectTaskSheetTemplateCorrelation()
                        .success(function(data) {
                            $scope.templates = data;
                        });
                };

                updateTemplates();

                $scope.selectTemplate = function (template) {
                    $scope.selectedTemplateId = template.Id;
                    $scope.template.Name = template.Name;
                    taskSheetTemplates.get({ templateId: template.Id }, function (data) {
                        $scope.taskSheet.InputData = data.InputData;
                        $scope.taskSheet.RpzContent = data.RpzContent;
                        $scope.taskSheet.DrawMaterials = data.DrawMaterials;
                        $scope.taskSheet.Consultants = data.Consultants;
                    });
                };

                taskSheets.get({ courseProjectId: $scope.selectedProjectId }, function (data) {
                    $scope.taskSheet = data;
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

                $scope.closeDialog = function () {
                    $modalInstance.close();
                };
            };
        }
    ]);