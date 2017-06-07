
angular
    .module('complexMaterialsApp.ctrl.studentInfo', ['ngResource'])
    .controller('studentInfoCtrl', [
        '$scope',
        '$rootScope',
        '$location',
        '$resource',
        '$routeParams',
        'monitoringDataService',
        'navigationService',
        function ($scope, $rootScope, $location, $resource, $routeParams, monitoringDataService, navigationService) {
            $scope.data = {
                fullname: null,
                groupnumber: null,
            };

            function getParameterByName(name) {
                name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
                var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
                    results = regex.exec(location.search);
                return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
            }

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
                window.location.href = "#/Catalog?parent=" + monitoringDataService.getRootId();
            }

            $rootScope.goToHome = function () {
                window.location.href = "#/Catalog";
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
                $('#concept-tree').on("changed.jstree", function (e, args) {

                    var data = {};
                    data.id = args.node.data.ConceptId;
                    var title = 'Просмотр файла "' + args.node.data.Name + '"';
                    $.savingDialog(title, "/ComplexMaterial/OpenConcept", data, "primary", function (data) {

                    }, null, { hideSaveButton: true });

                    console.log(args.node);
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
                result.data = obj;
                return result;
            }

            navigationService.updateTitle(getParameterByName("subjectId"));
            
            $rootScope.getConceptName = function () {
                monitoringDataService.getConcept().success(function (data) {
                    $rootScope.conceptName = data.Name;
                });
            }

            $rootScope.getConceptName();

            monitoringDataService.getConcepts({ id: monitoringDataService.getSubjectId() }).success(function (data) {
                $scope.data.fullname = data.StudentFullName;
                $scope.data.groupnumber = data.GroupNumber;
                initTree(data.Tree);
            });

            $rootScope.isGoToConceptRootActive = function ($event) {
                return false;
            }

            $rootScope.isGoMonitoring = function ($event) {
                return true;
            }
        }]);
