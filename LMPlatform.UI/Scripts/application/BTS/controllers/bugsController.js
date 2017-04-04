angular
    .module('btsApp.ctrl.bugs', ['ngTable'])
    .controller('bugsCtrl', [
        '$scope',
        'bugsService',
        'PAGE_SIZE',
        'NgTableParams',
        function ($scope, bugsService, PAGE_SIZE, NgTableParams) {

            $scope.inputedSearchString = '';
            var searchString = '';

            $scope.tableParams = new NgTableParams({
                sorting: { ModifyingDate: "desc" },
                count: PAGE_SIZE
            }, {
                getData: function (params) {
                    return bugsService.getBugs(params.page(), params.count(), searchString, params.orderBy()).then(function (response) {
                        params.total(response.data.TotalCount);
                        bugsService.addNumbering(response.data.Bugs, (params.page() - 1) * params.count());
                        return response.data.Bugs;
                    });
                }
            });
        }]);
