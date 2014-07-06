angular
    .module('dpApp.ctrl.percentages', ['ngTable', 'ngResource'])
    .controller('percentagesCtrl', [
        '$scope',
        '$modal',
        'ngTableParams',
        '$resource',
        '$location',
        function ($scope, $modal, ngTableParams, $resource, $location) {

            var percentages = $resource('api/Percentage');

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