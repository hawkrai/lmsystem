angular
    .module('complexMaterialsApp.ctrl.catalog', ['ngResource', "angularSpinner"])
    .controller('catalogCtrl', [
        '$scope',
        '$window',
        '$route',
        "$rootScope",
        '$location',
        '$resource',
        "complexMaterialsDataService",
        "navigationService",
        "$log",
        function ($scope, $window, $route, $rootScope, $location, $resource, complexMaterialsDataService, navigationService) {

            $scope.navigationService = navigationService;
            function getParameterByName(name) {
                name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
                var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
                    results = regex.exec(location.search);
                return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
            }

            var adjustment;
            var subjectId = getParameterByName("subjectId");
            $scope.navigationService.currentSubjectId = subjectId;

            var parentId = $location.search()["parent"]
            if (parentId === undefined)
                parentId = getParameterByName("parent");
            var tree;
            function attachDragableActionToConceptList() {
                $(".bs-glyphicons-list").sortable({
                    group: 'bs-glyphicons-list',
                    pullPlaceholder: false,
                    update: function (ev, ui) {
                        if (!ui.item.data().$scope.isShowAddFolderButton()) {
                            $(this).sortable('cancel');
                            alertify.error("Изменение корневой структуры ЭУМК запрещено");
                            return;
                        }
                        var rightId = 0;
                        var leftId = 0;
                        var currentId = ui.item[0].id;
                        if (ui.item[0].nextElementSibling)
                            rightId = ui.item[0].nextElementSibling.id;
                        if (ui.item[0].previousElementSibling)
                            leftId = ui.item[0].previousElementSibling.id;

                        $scope.attachSiblings(currentId, rightId, leftId);
                    },
                    onDrop: function ($item, container, _super) {
                        alert($item.nextElementSibling);
                        var $clonedItem = $('<li/>').css({ height: 0 });
                        $item.before($clonedItem);
                        $clonedItem.animate({ 'height': $item.height() });

                        $item.animate($clonedItem.position(), function () {
                            $clonedItem.detach();
                            _super($item, container);
                        });
                    },

                    // set $item relative to cursor position
                    onDragStart: function ($item, container, _super) {
                        var offset = $item.offset(),
                            pointer = container.rootGroup.pointer;

                        adjustment = {
                            left: pointer.left - offset.left,
                            top: pointer.top - offset.top
                        };

                        _super($item, container);
                    },
                    onDrag: function ($item, position) {
                        $item.css({
                            left: position.left - adjustment.left,
                            top: position.top - adjustment.top
                        });
                    }
                });
            }

            function updateRootConceptList(action) {
                $scope.startSpin();
                if (parentId && parentId > 0) {
                    complexMaterialsDataService.getConcepts({ parentId: parentId }).success(function (data) {
                        $scope.folders = data.Concepts;
                        $scope.parent = data.Concept;
                        $scope.selectedItem = null;
                        $scope.navigationService.setNavigation($scope.parent, action ? action : "inc");
                    }).finally(function () {
                        $scope.stopSpin();
                    });
                }
                else {
                    complexMaterialsDataService.getRootConcepts({ subjectId: subjectId }).success(function (data) {
                        $scope.folders = data.Concepts;
                        $scope.parent = null;
                        $scope.navigationService.setHomeNavigation(data);
                    }).finally(function () {
                        $scope.stopSpin();
                    });
                }
            }

            function loadNavigationTree() {
                if (parentId < 1) {
                    updateRootConceptList();
                    return;
                }
                $scope.startSpin();
                complexMaterialsDataService.getTree({ id: parentId }).success(function (data) {
                    $scope.navigationService.setTree(data);
                }).error(function (e) {
                    alertify.error(e)
                }).finally(function () {
                    updateRootConceptList();
                });
            }

            function updateCurrentCatalog(parentId) {
                $scope.startSpin();
                complexMaterialsDataService.getConcepts({ parentId: parentId }).success(function (data) {
                    $scope.folders = data.Concepts;
                    $scope.navigationService.setNavigation($scope.parent, "inc");
                    $scope.selectedItem = null;
                }).finally(function () {
                    $scope.stopSpin();
                });
            }

            function updateQueryParams(id) {

                if (id || id == 0)
                    $location.search({ "parent": id });
                else if ($scope.parent)
                    $location.search({ "parent": $scope.parent.Id });
                parentId = $location.search()["parent"];
            }

            $scope.startSpin = function () {
                $(".loading").toggleClass('ng-hide', false);
            };

            $scope.stopSpin = function () {
                $(".loading").toggleClass('ng-hide', true);
            };

            loadNavigationTree();

            attachDragableActionToConceptList();

            $scope.attachSiblings = function (sourceId, rightId, leftId) {
                if (rightId == 0 && leftId == 0 || sourceId == 0)
                    return;
                $scope.startSpin();
                complexMaterialsDataService.attachSiblings({ source: sourceId, left: leftId, right: rightId }).success(function (data) {
                    updateRootConceptList()
                }).finally(function () {
                    $scope.stopSpin();
                });
            }

            $scope.getHeaderValue = function () {
                if ($scope.parent)
                    return $scope.parent.SubjectName
                return "Модуль электронных учебно-методических комплексов";
            }

            $scope.isShowRootAddButton = function () {
                return $scope.parent == null;
            }

            $scope.isShowAddFileButton = function () {
                
                return $scope.parent != null && $scope.parent.IsGroup && $scope.parent.ParentId != 0&& !$scope.isTestModule($scope.parent.Name);
            }

            $scope.isShowEditItemButton = function () {
                return $scope.selectedItem != null && ((!$scope.selectedItem.ReadOnly) || (!$scope.selectedItem.IsGroup)) && !$scope.isTestModule($scope.selectedItem.Container);
            }

            $scope.isShowDeleteItemButton = function () {
                return $scope.selectedItem != null && !$scope.selectedItem.ReadOnly && !$scope.isTestModule($scope.selectedItem.Container);
            }

            $scope.isShowOpenItemButton = function () {
                return $scope.selectedItem != null && !$scope.selectedItem.IsGroup && $scope.selectedItem.HasData;
            }

            $scope.isShowMapButton = function () {
                return $scope.parent != null || ($scope.selectedItem != null && $scope.selectedItem.ParentId == 0);
            }

            $scope.selectElement = function ($event) {
                var idFolder = angular.element($event.target).data("idfolder");
                var selectedItem = $scope.getById($scope.folders, idFolder);
                selectedItem.selected = !selectedItem.selected;
                if (selectedItem.selected)
                    $scope.selectedItem = selectedItem;
                else
                    $scope.selectedItem = null;
                var i = 0, len = $scope.folders.length;
                for (; i < len; i++)
                    if ($scope.folders[i].Id != idFolder)
                        $scope.folders[i].selected = false;
            }

            $scope.isSelected = function (folder) {
                var idFolder = angular.element($event.target).data("idfolder");
                var selectedItem = $scope.getById($scope.folders, idFolder);
                return selectedItem.selected ? "selected-elem" : "";
            }

            $rootScope.backspaceFolder = function ($event) {
                if (!$scope.parent)
                    return;
                var pid = $scope.parent.ParentId;
                if (pid == 0) {
                    updateQueryParams(pid);
                    updateRootConceptList("dec");
                }
                else {
                    $scope.startSpin()
                    complexMaterialsDataService.getConcepts({ parentId: pid }).success(function (data) {
                        $scope.folders = data.Concepts;
                        $scope.parent = data.Concept;
                        $scope.selectedItem = null;
                        if ($scope.parent != null)
                            $scope.navigationService.setNavigation($scope.parent, "dec");
                        else
                            $scope.navigationService.setHomeNavigation(data);
                        updateQueryParams();
                    }).finally(function () {
                        $scope.stopSpin();
                    });
                }
            };

            $rootScope.goToHome = function ($event) {
                updateQueryParams(0)
                updateRootConceptList();
            }

            $rootScope.goMonitoring = function ($event) {
                window.location.href = "/Monitoring/?subjectId=" + subjectId;
            }

            $rootScope.isBackspaceShow = function () {
                return $scope.parent != undefined;
            }

            $scope.isShowAddFolderButton = function () {
                return $scope.parent != null && $scope.parent.ParentId != 0 && !$scope.isTestModule($scope.parent.Name);
            }

            $scope.getById = function (input, id) {
                var i = 0, len = input.length;
                for (; i < len; i++) {
                    if (input[i].Id == id) {
                        return input[i];
                    }
                }
                return null;
            }

            $scope.getFileAttachments = function () {
                var itemAttachmentsTable = $('#fileupload').find('table').find('tbody tr');
                var data = $.map(itemAttachmentsTable, function (e) {
                    var newObject = null;
                    if (e.className === "template-download fade in") {
                        if (e.id === "-1") {
                            newObject = { Id: 0, Title: "", Name: $(e).find('td a').text(), AttachmentType: $(e).find('td.type').text(), FileName: $(e).find('td.guid').text() };
                        } else {
                            newObject = { Id: e.id, Title: "", Name: $(e).find('td a').text(), AttachmentType: $(e).find('td.type').text(), FileName: $(e).find('td.guid').text() };
                        }
                    }
                    return newObject;
                });
                var dataAsString = JSON.stringify(data);
                return dataAsString;
            };

            $scope.addNewRootComplexMaterial = function () {
                var url = "/ComplexMaterial/AddRootConcept";
                if (subjectId)
                    url = url + "/?subjectId=" + subjectId;
                $.savingDialog("Добавление нового ЭУМК", url, null, "primary", function (data) {
                    updateRootConceptList();
                    alertify.success("Добавлен новый ЭУМК");
                }, function () {
                    $scope.startSpin();
                });
            };

            $scope.addNewComplexMaterial = function () {
                var data = {};
                data.parentId = $scope.parent.Id;
                $.savingDialog("Добавление нового модуля", "/ComplexMaterial/AddConcept", data, "primary", function (data) {
                    updateCurrentCatalog($scope.parent.Id);
                    alertify.success("Добавлен новый модуль");
                }, function () {
                    $scope.startSpin();
                    var data = $scope.getFileAttachments();
                    angular.element('#FileData').val(data);
                });
            }

            $scope.addNewFolderComplexMaterial = function () {
                var data = {};
                data.parentId = $scope.parent.Id;
                $.savingDialog("Добавление нового составного модуля", "/ComplexMaterial/AddFolderConcept", data, "primary", function (data) {
                    alertify.success("Добавлен новый составной модуль");
                    updateCurrentCatalog($scope.parent.Id);
                    $scope.stopSpin();
                }, function () {
                    $scope.startSpin();
                });
            };

            $scope.editRootComplexMaterial = function () {
                if ($scope.selectedItem.ParentId > 0) {
                    $scope.editComplexMaterial();
                    return;
                }

                var data = {};
                data.id = $scope.selectedItem.Id;
                var title = 'Редактирование ЭУМК "' + $scope.selectedItem.Name + '"';
                $.savingDialog(title, "/ComplexMaterial/EditRootConcept", data, "primary", function (data) {
                    updateRootConceptList();
                    alertify.success("ЭУМК отредактирован");
                    $scope.stopSpin();
                }, function () {
                    $scope.startSpin();
                });
            };

            $scope.editComplexMaterial = function () {
                var data = {};
                data.id = $scope.selectedItem.Id;
                data.parentId = $scope.selectedItem.ParentId;
                var title = 'Редактирование модуля "' + $scope.selectedItem.Name + '"';
                $.savingDialog(title, "/ComplexMaterial/EditConcept", data, "primary", function (data) {
                    alertify.success("Модуль отредактирован");
                    updateCurrentCatalog($scope.parent.Id);
                    $scope.stopSpin();
                }, function () {
                    $scope.startSpin();
                    var data = $scope.getFileAttachments();
                    $scope.selectedItem.HasData = data.length > 0;
                    angular.element('#FileData').val(data);
                });
            };

            $scope.openMemo = function () {
                $.savingDialog("Положение об УМК", "/ComplexMaterial/ShowMemo", null, "primary", function (data) {
                }, null, { hideSaveButton: true });
            };

            $scope.openConceptInner = function (id, name, container) {
                if ($scope.isTestModule(container))
                    $scope.openTest(id);
                else {
                    var data = {};
                    data.id = id;
                    var title = 'Просмотр файла "' + name + '"';
                    $.savingDialog(title, "/ComplexMaterial/OpenConcept", data, "primary", function (data) {

                    }, null, { hideSaveButton: true });
                }
            };

            $scope.isTestModule = function (container) {
                return container == "test" || container == 'Блок контроля знаний';
            }

            $scope.openTest = function (id) {
                var returnUrl = encodeURIComponent($location.absUrl());
                var url = '/Tests/KnowledgeTesting?subjectId=' + subjectId + '#/passing?testId=' + id + '&return=' + returnUrl;
                $window.open(url, '_blank')
            }

            $scope.openConcept = function (id) {
                $scope.openConceptInner($scope.selectedItem.Id, $scope.selectedItem.Name, $scope.selectedItem.Container);
            };

            $scope.openConceptViews = function (id) {
                var data = {};
                data.id = id || $scope.selectedItem.Id;
                var title = 'Просмотры "' + $scope.selectedItem.Name + '"';
                $.savingDialog(title, "/ComplexMaterial/OpenViewsConcept", data, "primary", function (data) {
                }, null, { hideSaveButton: true });
            };

            var prevTime;
            $scope.openFolder = function ($event) {
                if (prevTime && $event.timeStamp - prevTime < 2)
                    return;
                prevTime = $event.timeStamp;
                var idFolder = angular.element($event.target).data("idfolder");
                var currentFolder = $scope.getById($scope.folders, idFolder);
                if (currentFolder.IsGroup) {
                    $scope.parent = currentFolder;
                    angular.element(".catalog").attr("data-pid", idFolder);
                    $scope.openFolderInner(idFolder);
                }
                else
                    $scope.openConceptInner(idFolder, currentFolder.Name, currentFolder.Container);
            };

            $scope.openFolderInner = function (folderId) {
                updateCurrentCatalog(folderId);
                updateQueryParams();
            }

            $scope.goToBredCrumb = function ($event) {
                var idFolder = angular.element($event.target).data("idfolder");
                angular.element(".catalog").attr("data-pid", idFolder);
                var item = $scope.navigationService.buildBreadCrumbs(idFolder);
                updateQueryParams(item.Id);
                updateRootConceptList();
            }

            $scope.openMap = function () {
                $scope.$parent.mapForItem = ($scope.selectedItem && $scope.selectedItem.ParentId == 0) ? $scope.selectedItem : $scope.parent;
                $location.url("/Map?elementId=" + $scope.$parent.mapForItem.Id);
            }

            $scope.deleteConcept = function ($event) {
                var data = {};
                data.id = $scope.selectedItem.Id;
                bootbox.confirm({
                    title: 'Удаление',
                    message: 'Вы дествительно хотите удалить "' + $scope.selectedItem.Name + '" ?',
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
                            $scope.startSpin();
                            complexMaterialsDataService.deleteConcept(data).success(function (data) {
                                $scope.selectedItem.ParentId > 0 ? updateCurrentCatalog($scope.selectedItem.ParentId) : updateRootConceptList();
                                $scope.stopSpin();
                                $scope.selectedItem = null;
                            });
                        } else {

                        }
                    },
                });

            };
        }])

