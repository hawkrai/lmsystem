
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
                var pid = angular.element(".catalog").attr("data-pid");
                materialsService.createFolder({ Pid: pid }).success(function (data) {
                    $scope.folders = data.Folders;
                    angular.element('#context_menu').detach();
                });
            };

            $scope.backspaceFolder = function ($event) {
                var pid = angular.element(".catalog").attr("data-pid");
                //alert(angular.element(".catalog").attr("data-pid"));
                //angular.element(".catalog").attr("data-pid", idFolder);
                materialsService.backspaceFolder({ Pid: pid }).success(function (data) {
                    $scope.folders = data.Folders;
                    angular.element(".catalog").attr("data-pid", data.Pid);
                });
            };

            $scope.openFolder = function ($event) {
                var idFolder = angular.element($event.target).data("idfolder");
                angular.element(".catalog").attr("data-pid", idFolder);
                materialsService.getFolders({ Pid: idFolder }).success(function (data) {
                    $scope.folders = data.Folders;
                });
            };

            $scope.deleteFolder = function ($event) {
                var idFolder = $scope.actionFolder.data("idfolder");
                var nameFolder = $scope.actionFolder.find('.nameFolder').text();
                angular.element('#context_menu').detach();
                bootbox.confirm({
                    title: 'Удаление',
                    message: 'Вы дествительно хотите удалить папку "'+nameFolder+'" ?',
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
                    callback: function (isConfirmed) {
                        if (isConfirmed) {
                            materialsService.deleteFolder({ IdFolder: idFolder }).success(function (data) {
                                $scope.actionFolder.remove();
                            });
                        } else {

                        }
                    },
                });

            };

            $scope.renameFolder = function ($event) {
                var idFolder = $scope.actionFolder.data("idfolder");
                var pid = angular.element(".catalog").data("pid");

                var nameFolder = $scope.actionFolder.find('.nameFolder').attr('contenteditable', 'true').focus();
                var oldName = $scope.actionFolder.find('.nameFolder').text();


                function placeCaretAtEnd(el) {
                    el.focus();
                    if (typeof window.getSelection != "undefined"
                            && typeof document.createRange != "undefined") {
                        var range = document.createRange();
                        range.selectNodeContents(el);
                        range.collapse(false);
                        var sel = window.getSelection();
                        sel.removeAllRanges();
                        sel.addRange(range);
                    } else if (typeof document.body.createTextRange != "undefined") {
                        var textRange = document.body.createTextRange();
                        textRange.moveToElementText(el);
                        textRange.collapse(false);
                        textRange.select();
                    }
                }


                placeCaretAtEnd($scope.actionFolder.find('.nameFolder')[0]);
                angular.element('#context_menu').detach();

                $scope.actionFolder.find('.nameFolder').bind('blur', function () {
                    $scope.actionFolder.find('.nameFolder').attr('contenteditable', 'false');
                });

                $scope.actionFolder.find('.nameFolder').bind('keypress', function (e) {
                    if (e.keyCode == 13) {
                        $scope.actionFolder.find('.nameFolder').attr('contenteditable', 'false');
                        var newName = $scope.actionFolder.find('.nameFolder').text();
                        materialsService.renameFolder({ id: idFolder, pid: pid,  newName: newName }).success(function (data) {
                            $scope.folders = data.Folders;
                            angular.element('#context_menu').detach();
                        });

                    } else if (e.keyCode == 27) {
                        $scope.actionFolder.find('.nameFolder').text(oldName);
                        $scope.actionFolder.find('.nameFolder').attr('contenteditable', 'false');
                    }
                });
            }



        }])
    .directive('contextMenuFolder', function ($compile){
        return {
            priority: 2,
            controller: function ($scope, $element, $attrs) {
                $element.bind('contextmenu', function (el) {
                    $scope.$parent.actionFolder = $element;
                    //$scope.$parent.$apply();
                    angular.element('#context_menu').detach();
                    angular.element('body').append($compile("<ul ng-model='items_menu' class='dropdown-menu' id='context_menu'>"
                        + "<li><a class='iteammenue' ng-click='createFolder()'>Создать папку</a></li>"
                        + "<li><a class='iteammenue' ng-click='renameFolder()'>Переименовать</a></li>"
                        + "<li><a class='iteammenue' ng-click='deleteFolder()'>Удалить папку</a></li>"
                        + "<li><a class='iteammenue' ng-click='property_setting()'>Свойства и настройки</a></li>"
                        + "</ul>")($scope));

                    angular.element('#context_menu').css({ "display": "block", "position": "absolute", "top": el.pageY, "left": el.pageX });
                    if (el.stopPropagation) el.stopPropagation();
                    el.cancelBubble = true;
                    el.returnValue = false;
                    el.preventDefault();
                })
            },
            link: function (scope, element, attrs) {
                element.bind('click', function (el) {
                    angular.element('#context_menu').detach();
                })
            },
           
        }
    })
    .directive('deleteContextMenu', function ($compile) {
        return {
            priority:1,
            link: function (scope, element, attrs) {
                element.bind('click', function (el) {
                    angular.element('#context_menu').detach();
                })
                element.bind('contextmenu', function (el) {
                    angular.element('#context_menu').detach();
                    angular.element('body').append($compile("<ul ng-model='items_menu' class='dropdown-menu' id='context_menu'>"
                         + "<li><a class='iteammenue' ng-click='createFolder()'>Создать папку</a></li>"
                         + "<li><a class='iteammenue' ng-click='property_setting()'>Свойства и настройки</a></li>"
                         + "</ul>")(scope));

                    angular.element('#context_menu').css({ "display": "block", "position": "absolute", "top": el.pageY, "left": el.pageX });
                    el.preventDefault();
                });

            }
        }
    })
    
