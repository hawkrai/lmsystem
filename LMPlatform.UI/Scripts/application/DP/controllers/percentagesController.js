angular
    .module('dpApp.ctrl.percentages', ['ngTable', 'ngResource'])
    .controller('percentagesCtrl', [
        '$scope',
        '$modal',
        'ngTableParams',
        '$resource',
        '$location',
        function ($scope, $modal, ngTableParams, $resource, $location) {

            $scope.setTitle("График процентовки");

            var percentages = $resource('api/Percentage');


            $scope.forms = {};

            $scope.editPercentage = function (percentageId) {
                var modalInstance = $modal.open({
                    templateUrl: '/Dp/Percentage',
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
                    title: "Удаление",
                    message: "Вы действительно хотите удалить запись?",
                    callback: function (isConfirmed) {
                        if (isConfirmed) {
                            percentages.delete({ id: id }).$promise.then(function () {
                                $scope.tableParams.reload();
                                alertify.success("Запись успешно удалена.");
                            }, function (error) {
                                alertify.error(error);
                            });
                        }
                    },
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
                });
            };


            $scope.tableParams = new ngTableParams(
                angular.extend({
                    page: 1,
                    count: 10
                }, $location.search()), {
                    total: 0,
                    getData: function ($defer, params) {
                        $location.search(params.url());
                        percentages.get(params.url(),
                            function (data) {
                                $defer.resolve(data.Items);
                                params.total(data.Total);
                            });
                    }
                });
        }]);