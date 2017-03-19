
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
            };

            $scope.getTime = function (time) {
                if (!isNaN(parseFloat(time)) && isFinite(time)) {
                    var mins = Math.floor(time / 60);
                    var str = mins < 1 ? "" : mins + "m ";
                    var secs = (time - mins * 60);
                    str += secs < 1 ? "" : secs + "s";
                    return str;
                } else {
                    return " ";
                }
            }           

            $scope.IsVeryLong = function (concept) {
                if (concept.Estimated == 0 || concept.Estimated == undefined)
                    return false;
                var coeff = Math.abs(concept.view.Time - concept.Estimated) / concept.Estimated;
                if (coeff > 0.5)
                    return true;
                else
                    return false;
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
