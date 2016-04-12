angular
    .module('cpApp.ctrl.project', [])
    .controller('projectCtrl', [
        '$scope',
        '$routeParams',
        '$location',
        'projectService',
        'projectId',
        '$modalInstance',
        function ($scope, $routeParams, $location, projectService, projectId, $modalInstance) {

            $scope.isNew = angular.isUndefined(projectId);

         function getParameterByName(name) {
                name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
                var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
                    results = regex.exec(location.search);
                return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
            }

            $scope.project = {
                Theme: '',
                Id: null,
                SelectedGroupsIds: [],
                SubjectId: null
            };

            

            $scope.groups = [];

            if (!$scope.isNew) {
                projectService
                    .getProject(projectId)
                    .success(function (data, status, headers, config) {
                        $scope.project = data;
                    });
            }

          /*  projectService
                .getGroupCorrelation()
                .success(function (data, status, headers, config) {
                    $scope.groups = data;
                });
                */

            var subjectId = getParameterByName("subjectId");
            projectService
                .getGroups(subjectId)
                .success(function (data, status, headers, config) {
                    $scope.groups = data;
                });

            $scope.saveProject = function () {
                $scope.submitted = true;
               
              $scope.project.SubjectId = subjectId;
                if ($scope.forms.projectForm.Theme.$error.required) return;

                var saveFunc = $scope.isNew ? projectService.createProject : projectService.updateProject;

                saveFunc($scope.project)
                    .success(function (data, status, headers, config) {
                        $modalInstance.close();
                        alertify.success('Тема курсового проекта (работы) успешно сохранена');
                    })
                    .error(function (data) {
                        $scope.handleError(data);
                        //$modalInstance.close();
                    });
            };


            $scope.closeDialog = function () {
                $modalInstance.close();
            };

        }]);