
angular
    .module('complexMaterialsApp.ctrl.studentInfo', ['ngResource'])
    .controller('studentInfoCtrl', [
        '$scope',
        '$rootScope',
        '$location',
        '$resource',
        '$routeParams',
        'monitoringDataService',
        function ($scope, $rootScope, $location, $resource, $routeParams, monitoringDataService) {
            $scope.data = {
                fullname: null,
                groupnumber: null,
            };

            $scope.treeConfig = {
                core: {
                    multiple: false,
                    animation: true,
                    error: function (error) {
                        $log.error('treeCtrl: error from js tree - ' + angular.toJson(error));
                    },
                    check_callback: true,
                    worker: true
                },
                version: 1
            };

            $rootScope.isBackspaceShow = function () {
                return true;
            }

            $rootScope.goToConceptRoot = function () {
                window.location.href = "/ComplexMaterial/?subjectId=" + monitoringDataService.getSubjectId() + "&parent=" + monitoringDataService.getRootId();
            }

            $rootScope.goToHome = function () {
                window.location.href = "/ComplexMaterial/?subjectId=" + monitoringDataService.getSubjectId();
            }

            function getTime(time) {
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

            function isVeryLong (concept) {
                if (concept.Estimated == 0 || concept.Estimated == undefined || concept.ViewTime == undefined)
                    return false;
                var coeff = Math.abs(concept.ViewTime - concept.Estimated) / concept.Estimated;
                if (coeff > 0.5)
                    return true;
                else
                    return false;
            }

            function initTree(data) {
                var tree = data.map(function (item) {
                    return convertFormat(item);
                });
                $.jstree.defaults.core.themes.variant = "large"; 
                $('#concept-tree').jstree({
                    'core': {
                        'data': tree
                    }
                });
            }

            function convertFormat(obj) {
                var result = {};
                result.text = obj.Name;
                if (obj.ViewTime != null)
                    result.text += " <b style=\"color:" + (isVeryLong(obj) ? "red" : "green") + ";\">" + getTime(obj.ViewTime) + "</b>";
                if (obj.Children != undefined) {
                    result.children = obj.Children.map(function (item) {
                        return convertFormat(item);
                    });
                }
                if (obj.IsFile) {
                    result.icon = "jstree-file";
                    result.text += " (ожидается " + getTime(obj.Estimated) + ")</b>";
                } else {
                    result.icon = "jstree-folder";
                }
                return result;
            }

            monitoringDataService.getConcepts({ id: monitoringDataService.getSubjectId() }).success(function (data) {
                $scope.data.fullname = data.StudentFullName;
                $scope.data.groupnumber = data.GroupNumber;
                initTree(data.Tree);
            });
        }]);
