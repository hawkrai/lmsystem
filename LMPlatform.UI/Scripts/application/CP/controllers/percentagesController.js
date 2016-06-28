angular
    .module('cpApp.ctrl.percentages', ['ngTable', 'ngResource'])
    .controller('percentagesCtrl', [
        '$scope',
        '$modal',
        'ngTableParams',
        '$resource',
        '$location',
        'projectService',
        function ($scope, $modal, ngTableParams, $resource, $location, projectService) {

            $scope.setTitle("График процентовки");

            var percentages = $resource('api/CpPercentage');

            function getParameterByName(name) {
                name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
                var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
                    results = regex.exec(location.search);
                return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
            }



            var subjectId = getParameterByName("subjectId");

            $scope.forms = {};

            $scope.groups = [];
            $scope.selectGroup = function (group) {
                $scope.selectedGroupId = group.Id;
                $scope.setLecturerSelectedSecretaryId(group.Id);
                $scope.tableParams.reload();
            };

            projectService.getLecturerDiplomGroupCorrelation(subjectId)
                .success(function (data) {
                    $scope.groups = data;
                    var selectedSecretaries = data.filter(function (elt) {
                        return $scope.getLecturerSelectedSecretaryId() == elt.Id ? elt : null;
                    });
                    if (selectedSecretaries.length == 1) {
                        $scope.group = selectedSecretaries[0];
                        $scope.selectGroup($scope.group);
                    } else {
                        $scope.group = { Id: null, Name: "Выберите секретаря" };
                    }
                });

            $scope.editPercentage = function (percentageId) {
                var modalInstance = $modal.open({
                    templateUrl: '/Cp/Percentage',
                    controller: 'percentageCtrl',
                    keyboard: false,
                    scope: $scope,
                    resolve: {
                        
                        percentageId: function () { return percentageId; },
                        percentages: function () { return percentages; }
                    }
                });

                modalInstance.result.then(function (result) {
                    $scope.tableParams.reload();
                });

            };

            $scope.deletePercentage = function (id) {
                bootbox.confirm({
                    title: "Удаление этапа процентовки",
                    message: "Вы действительно хотите удалить этап?",
                    callback: function (isConfirmed) {
                        if (isConfirmed) {
                            percentages.delete({ id: id }).$promise.then(function () {
                                $scope.tableParams.reload();
                                alertify.success("Этап успешно удален");
                            }, function (error) {
                                $scope.handleError(error);
                            });
                        }
                    },
                    buttons: {
                        'cancel': {
                            label: 'Отмена',
                            className: 'btn-default btn-sm'
                        },
                        'confirm': {
                            label: 'Удалить',
                            className: 'btn btn-primary btn-sm',
                        }
                    },
                });
            };

            $scope.tableParams = new ngTableParams(
                {
                    page: 1,
                    count: 1000
                }, {
                    total: 0,
                    getData: function ($defer, params) {
                        percentages.get(angular.extend(params.url(), {
                            filter:
                            {
                                subjectId: subjectId,
                                groupId: $scope.selectedGroupId
                            }
                        }),
                            function (data) {
                                $defer.resolve(data.Items);
                                params.total(data.Total);
                            });
                    }
                });
        }]);