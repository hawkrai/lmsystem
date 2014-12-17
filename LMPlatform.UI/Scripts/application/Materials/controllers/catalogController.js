
angular
    .module('materialsApp.ctrl.catalog', ['ngResource'])
    .controller('catalogCtrl', [
        '$scope',
        '$location',
        '$resource',
        "materialsService",
        "$log",
        function ($scope, $location, $resource, materialsService) {

            materialsService.getFolders().success(function (data) {
                $scope.folders = data.Folders;
            });


            $scope.createFolder = function () {
                var pid = angular.element(".catalog").data("pid");
                materialsService.createFolder({ Pid: pid }).success(function (data) {
                    $scope.folders = data.Folders;
                });
            };

            $scope.openFolder = function () {
                var pidFolder = angular.element(".folder").data("pidfolder");
                console.log(pidFolder);
                materialsService.getFolders({ Pid: pidFolder }).success(function (data) {
                    $scope.folders = data.Folders;
                });
            }

        }])
    .directive('contextMenuFolder', function ($compile){
        return {
            priority:1,
            link: function(scope,element,attrs){
                element.bind('contextmenu', function (el) {
                    angular.element('#context_menu').detach();
                    angular.element('body').append($compile("<ul ng-model='items_menu' class='dropdown-menu' id='context_menu'>"
                        + "<li><a class='iteammenue' ng-click='createFolder()'>Создать папку</a></li>"
                        + "<li><a class='iteammenue' ng-click='deleteFolder()'>Удалить папку</a></li>"
                        + "<li><a class='iteammenue' ng-click='property_setting()'>Свойства и настройки</a></li>"
                        + "</ul>")(scope));

                    angular.element('#context_menu').css({ "display": "block", "position": "absolute", "top": el.pageY, "left": el.pageX });
                    el.preventDefault();
                })
            },
           
        }
    })
    .directive('deleteContextMenu', function () {
        return {
            priority:2,
            link: function (scope, element, attrs) {
                element.bind('click', function (el) {
                    angular.element('#context_menu').detach();
                });

            }
        }
    })
    
