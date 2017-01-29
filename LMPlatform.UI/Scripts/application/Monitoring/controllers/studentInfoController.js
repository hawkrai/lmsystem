
angular
    .module('monitoringApp.ctrl.studentInfo', ['ngResource'])
    .controller('studentInfoCtrl', [
        '$scope',
        '$location',
        '$resource',
        '$routeParams',
        'monitoringDataService',
        function ($scope, $location, $resource, $routeParams, monitoringDataService) {
            $scope.data = {
                concepts: null,
                view: null
            }

            monitoringDataService.getConcepts({ id: monitoringDataService.getSubjectId() }).success(function (data) {
                $scope.data.concepts = data
                $scope.data.concepts.forEach(function (concept, i, arr) {
                    concept.view = concept.Views.filter(function (item, i, arr) {
                        console.log(item.UserId + " " + $routeParams.studentId);
                        return item.UserId == $routeParams.studentId;
                    })[0];
                });
            });
        }]);
