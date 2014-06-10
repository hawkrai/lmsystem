var filesApp = angular.module("filesApp", ['ngTable', 'ui.bootstrap']);

filesApp
    .controller("filesController", [
        '$scope', '$http', '$modal', '$filter', 'ngTableParams', function ($scope, $http, $modal, $filter, ngTableParams) {

            $scope.UrlServiceMessages = '/Services/Files/FilesService.svc/';
            $scope.data = [];


            $scope.init = function () {
                $scope.loadData();
            };

            $scope.loadData = function () {
                $.ajax({
                    type: 'GET',
                    url: $scope.UrlServiceMessages + "GetFiles/",
                    dataType: "json",
                    contentType: "application/json",
                }).success(function (data, status) {
                    if (data.Code != '200') {
                        alertify.error(data.Message);
                    } else {
                        $scope.$apply(function () {

                            $scope.data = data.Files;
                            $scope.serverPath = data.ServerPath;

                            $scope.tableParams = new ngTableParams({
                                page: 1,
                                count: 100
                            },
                            {
                                getData: function ($defer, params) {
                                    var filteredData = $scope.data;
                                    params.total(filteredData.length);
                                    $defer.resolve(filteredData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                                },
                                counts: [],
                                $scope: { $data: {} }
                            });


                        });
                    }
                }).error(function () {
                    alertify.error("Ошибка сервиса");
                });
            };
        }
    ]);
