
angular
    .module('materialsApp.ctrl.catalog', ['ngResource'])
    .controller('catalogCtrl', [
        '$scope',
        "$rootScope",
        '$location',
        '$resource',
        "materialsService",
        "$log",
        function ($scope, $rootScope, $location, $resource, materialsService) {

            function getParameterByName(name) {
                name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
                var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
                    results = regex.exec(location.search);
                return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
            }

            var subjectId = getParameterByName("subjectId");

            $scope.$parent.subjectId = subjectId;

            materialsService.getFolders({ Pid: $scope.$parent.folder, subjectId: subjectId }).success(function (data) {
                $scope.folders = data.Folders;
            });

            materialsService.getDocuments({ Pid: $scope.$parent.folder, subjectId: subjectId }).success(function (data) {
                $scope.documents = data.Documents;
            });


            $scope.createFolder = function () {
                var pid = angular.element(".catalog").attr("data-pid");
                materialsService.createFolder({ Pid: pid, subjectId: subjectId }).success(function (data) {
                    $scope.folders = data.Folders;
                    angular.element('#context_menu').detach();
                });
            };

            $rootScope.backspaceFolder = function ($event) {
                var pid = angular.element(".catalog").attr("data-pid");
                //alert(angular.element(".catalog").attr("data-pid"));
                //angular.element(".catalog").attr("data-pid", idFolder);
                materialsService.backspaceFolder({ Pid: pid, subjectId: subjectId }).success(function (data) {
                    $scope.folders = data.Folders;
                    pid = data.Pid;
                    $scope.$parent.folder = pid;
                    angular.element(".catalog").attr("data-pid", data.Pid);
                    materialsService.getDocuments({ Pid: pid, subjectId: subjectId }).success(function (data) {
                        $scope.documents = data.Documents;
                    });
                });
            };

            $scope.openFolder = function ($event) {
                var idFolder = angular.element($event.target).data("idfolder");
                $scope.$parent.folder = idFolder;
                angular.element(".catalog").attr("data-pid", idFolder);
                materialsService.getFolders({ Pid: idFolder, subjectId: subjectId }).success(function (data) {
                    $scope.folders = data.Folders;
                });
                materialsService.getDocuments({ Pid: idFolder, subjectId: subjectId }).success(function (data) {
                    $scope.documents = data.Documents;
                });
            };

            $scope.openDocument = function ($event) {
                var idDocument = angular.element($event.target).data("iddocument"),
                    nameDocument = angular.element($event.target).data("namedocument");
                $scope.$parent.document = idDocument;
                $location.url("/New");
                
            }

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
                            className: 'btn btn-default btn-sm'
                        },
                        'confirm': {
                            label: 'Удалить',
                            className: 'btn btn-primary btn-sm',
                        }
                    },
                    callback: function (isConfirmed) {
                        if (isConfirmed) {
                            materialsService.deleteFolder({ IdFolder: idFolder, subjectId: subjectId }).success(function (data) {
                                $scope.actionFolder.remove();
                            });
                        } else {

                        }
                    },
                });

            };
            // для документа
            $scope.deleteDocument = function ($event) {
                var idDocument = $scope.actionDocument.data("iddocument");
                var nameDocument = $scope.actionDocument.find('.nameDocument').text();
                var pid = angular.element(".catalog").attr("data-pid");
                angular.element('#context_menu').detach();
                bootbox.confirm({
                    title: 'Удаление',
                    message: 'Вы дествительно хотите удалить документ "' + nameDocument + '" ?',
                    buttons: {
                        'cancel': {
                            label: 'Отмена',
                            className: 'btn btn-default btn-sm'
                        },
                        'confirm': {
                            label: 'Удалить',
                            className: 'btn btn-primary btn-sm',
                        }
                    },
                    callback: function (isConfirmed) {
                        if (isConfirmed) {
                            materialsService.deleteDocument({ IdDocument: idDocument, pid: pid, subjectId: subjectId }).success(function (data) {
                                $scope.actionDocument.remove();
                            });
                        } else {

                        }
                    },
                });

            };

            // для папаки
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
                    var newName = $scope.actionFolder.find('.nameFolder').text();
                    materialsService.renameFolder({ id: idFolder, pid: pid, newName: newName, subjectId: subjectId }).success(function (data) {
                        $scope.folders = data.Folders;
                        angular.element('#context_menu').detach();
                    });
                });

                $scope.actionFolder.find('.nameFolder').bind('keypress', function (e) {
                    if (e.keyCode == 13) {
                        $scope.actionFolder.find('.nameFolder').attr('contenteditable', 'false');
                        var newName = $scope.actionFolder.find('.nameFolder').text();
                        materialsService.renameFolder({ id: idFolder, pid: pid, newName: newName, subjectId: subjectId }).success(function (data) {
                            $scope.folders = data.Folders;
                            angular.element('#context_menu').detach();
                        });

                    } else if (e.keyCode == 27) {
                        $scope.actionFolder.find('.nameFolder').text(oldName);
                        $scope.actionFolder.find('.nameFolder').attr('contenteditable', 'false');
                    }
                });
            }

            // для документа
            $scope.renameDocument = function ($event) {
                var idDocument = $scope.actionDocument.data("iddocument"),
                    pid = angular.element(".catalog").data("pid");
                //    nameDocument = $scope.actionDocument.find('.nameDocument').data("namedocument");
                //    alert(nameDocument);
                    $scope.actionDocument.find('.nameDocument').attr('contenteditable', 'true').focus(),
                    oldName = $scope.actionDocument.data("namedocument");

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


                placeCaretAtEnd($scope.actionDocument.find('.nameDocument')[0]);
                angular.element('#context_menu').detach();

                $scope.actionDocument.find('.nameDocument').bind('blur', function () {
                    $scope.actionDocument.find('.nameDocument').attr('contenteditable', 'false');
                });

                $scope.actionDocument.find('.nameDocument').bind('keypress', function (e) {
                    if (e.keyCode == 13) {
                        $scope.actionDocument.find('.nameDocument').attr('contenteditable', 'false');
                        var newName = $scope.actionDocument.find(".nameDocument").text();
                        materialsService.renameDocument({ id: idDocument, pid: pid, newName: newName, subjectId: subjectId }).success(function (data) {
                            if(data.Code == 500)
                                alert("Ошибка");
                            console.log(data);
                            $scope.documents = data.Documents;
                            angular.element('#context_menu').detach();
                        });

                    } else if (e.keyCode == 27) {
                        $scope.actionDocument.find('.nameDocument').text(oldName);
                        $scope.actionDocument.find('.nameDocument').attr('contenteditable', 'false');
                    }
                });
            }

            $scope.createMaterial = function ($event) {
                $scope.$parent.idFolder = angular.element(".catalog").attr("data-pid");
                angular.element('#context_menu').detach();
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
                    if ($scope.user == 'lector') {
                        angular.element('body').append($compile("<ul ng-model='items_menu' class='dropdown-menu' id='context_menu'>"
                            + "<li><a class='iteammenue' ng-click='createFolder()'>Создать папку</a></li>"
                            + "<li><a class='iteammenue' href='#New' ng-click='createMaterial()' >Создать новый материал</a></li>"
                            + "<li><a class='iteammenue' ng-click='renameFolder()'>Переименовать</a></li>"
                            + "<li><a class='iteammenue' ng-click='deleteFolder()'>Удалить папку</a></li>"
                            + "<li><a class='iteammenue' ng-click='property_setting()'>Свойства и настройки</a></li>"
                            + "</ul>")($scope));
                    } else if ($scope.user == 'student'){
                        angular.element('body').append($compile("")($scope));
                    }

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
    .directive('contextMenuDocument', function ($compile) {
        return {
            priority: 3,
            controller: function ($scope, $element, $attrs) {
                $element.bind('contextmenu', function (el) {
                    $scope.$parent.actionDocument = $element;
                    //$scope.$parent.$apply();
                    angular.element('#context_menu').detach();
                    if ($scope.user == 'lector') {
                        angular.element('body').append($compile("<ul ng-model='items_menu' class='dropdown-menu' id='context_menu'>"
                            + "<li><a class='iteammenue' ng-click='createFolder()'>Создать папку</a></li>"
                            + "<li><a class='iteammenue' href='#New' ng-click='createMaterial()' >Создать новый материал</a></li>"
                            + "<li><a class='iteammenue' ng-click='renameDocument()'>Переименовать</a></li>"
                            + "<li><a class='iteammenue' ng-click='deleteDocument()'>Удалить документ</a></li>"
                            + "<li><a class='iteammenue' ng-click='property_setting()'>Свойства и настройки</a></li>"
                            + "</ul>")($scope));
                    }
                    else if ($scope.user == 'student')
                    {
                        angular.element('body').append($compile("")($scope));
                    }
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
            link: function ($scope, element, attrs) {
                element.bind('click', function (el) {
                    angular.element('#context_menu').detach();
                })
                element.bind('contextmenu', function (el) {
                    angular.element('#context_menu').detach();
                    if ($scope.user == 'lector') {
                        angular.element('body').append($compile("<ul ng-model='items_menu' class='dropdown-menu' id='context_menu'>"
                             + "<li><a class='iteammenue' ng-click='createFolder()'>Создать папку</a></li>"
                             + "<li><a class='iteammenue' href='#New' ng-click='createMaterial()' >Создать новый материал</a></li>"
                             + "<li><a class='iteammenue' ng-click='property_setting()'>Свойства и настройки</a></li>"
                             + "</ul>")($scope));
                    }
                    else if ($scope.user == 'student')
                    {
                        angular.element('body').append($compile("")($scope));
                    }

                    angular.element('#context_menu').css({ "display": "block", "position": "absolute", "top": el.pageY, "left": el.pageX });
                    el.preventDefault();
                });

            }
        }
    })
    
