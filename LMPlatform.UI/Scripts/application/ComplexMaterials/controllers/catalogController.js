angular
    .module('complexMaterialsApp.ctrl.catalog', ['ngResource', "angularSpinner"])
    .controller('catalogCtrl', [
        '$scope',
        '$route',
        "$rootScope",
        '$location',
        '$resource',
        "complexMaterialsDataService",
        "titleController",
        "$log",
        function ($scope, $route, $rootScope, $location, $resource, complexMaterialsDataService, titleController) {
            
            $scope.titleController = titleController;

            function getParameterByName(name) {
                name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
                var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
                    results = regex.exec(location.search);
                return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
            }

            var subjectId = getParameterByName("subjectId");
            var parentId = $location.search()["parent"];
            

            function updateRootConceptList() {
                $scope.startSpin();
                if (parentId && parentId>0) {
                    complexMaterialsDataService.getConcepts({ parentId: parentId }).success(function (data) {
                        $scope.folders = data.Concepts;
                        $scope.parent = data.Concept;
                        $scope.selectedItem = null;
                        $scope.titleController.setTitle($scope.parent.Name);
                    }).finally(function () {
                        $scope.stopSpin();
                    });
                }
                else {
                    complexMaterialsDataService.getRootConcepts({ subjectId: subjectId }).success(function (data) {
                        $scope.folders = data.Concepts;
                        $scope.parent = null;
                        $scope.titleController.setDefValue();
                        //$scope.$parent = null;
                    }).finally(function () {
                        $scope.stopSpin();
                    });
                }
            }

            function updateCurrentCatalog(parentId)
            {
                $scope.startSpin();
                complexMaterialsDataService.getConcepts({ parentId: parentId }).success(function (data) {
                    $scope.folders = data.Concepts;
                    titleController.setTitle($scope.parent.Name);
                    $scope.selectedItem = null;
                }).finally(function () {
                    $scope.stopSpin();
                });
            }

            function updateQueryParams(id) {
               
                if (id || id==0)
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

            
            updateRootConceptList();

            $scope.getHeaderValue = function () {
                if($scope.parent)
                    return $scope.parent.Name
                return "Модуль электронных учебно-методических комплексов";
            }

            $scope.isShowRootAddButton = function () {
                return $scope.parent == null;
            }

            $scope.isShowAddFolderButton = function () {
                return $scope.parent != null && $scope.parent.ParentId != 0;
            }

            $scope.isShowAddFileButton = function () {
                return $scope.parent != null && $scope.parent.IsGroup && $scope.parent.ParentId != 0;
            }

            $scope.isShowEditItemButton = function () {
                return $scope.selectedItem != null && ((!$scope.selectedItem.ReadOnly) || (!$scope.selectedItem.IsGroup));
            }

            $scope.isShowDeleteItemButton = function () {
                return $scope.selectedItem != null && !$scope.selectedItem.ReadOnly;
            }

            $scope.isShowOpenItemButton = function () {
                return $scope.selectedItem != null && !$scope.selectedItem.IsGroup && $scope.selectedItem.HasData;
            }

            $scope.isShowMapButton = function () {
                return $scope.parent != null || ($scope.selectedItem != null && $scope.selectedItem.ParentId==0);
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
                var pid = $scope.parent.ParentId;
                if (pid == 0) {
                    updateQueryParams(pid);
                    updateRootConceptList();
                }
                else {
                    $scope.startSpin()
                    complexMaterialsDataService.getConcepts({ parentId: pid }).success(function (data) {
                        $scope.folders = data.Concepts;
                        $scope.parent = data.Concept;
                        $scope.selectedItem = null;
                        if ($scope.parent != null)
                            $scope.titleController.setTitle($scope.parent.Name);
                        else
                            $scope.titleController.setDefValue();
                        updateQueryParams();
                    }).finally(function () {
                        $scope.stopSpin();
                    });
                }  
            };

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
                $.savingDialog("Добавление нового ЭУМК", "/ComplexMaterial/AddRootConcept", null, "primary", function (data) {
                    updateRootConceptList();
                    $scope.stopSpin();
                    alertify.success("Добавлен новый ЭУМК");
                }, function () {
                    $scope.startSpin();
                });
            };

            $scope.addNewComplexMaterial = function () {
                var data = {};
                data.parentId = $scope.parent.Id;
                $.savingDialog("Добавление нового концепт", "/ComplexMaterial/AddConcept", data, "primary", function (data) {
                    updateCurrentCatalog($scope.parent.Id);
                    alertify.success("Добавлен новый концепт");
                }, function () {
                    $scope.startSpin();
                    alertify.success("Добавляется новый концепт... Ожидайте");
                    var data = $scope.getFileAttachments();
                    angular.element('#FileData').val(data);
                });
            }

            $scope.addNewFolderComplexMaterial = function () {
                var data = {};
                data.parentId = $scope.parent.Id;
                $.savingDialog("Добавление новой папки", "/ComplexMaterial/AddFolderConcept", data, "primary", function (data) {
                    alertify.success("Добавлена новая папка");
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
                var title = 'Редактирование ЭУМК "' + $scope.selectedItem.Name+'"';
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
                var title = 'Редактирование концепта "' + $scope.selectedItem.Name+'"';
                $.savingDialog(title, "/ComplexMaterial/EditConcept", data, "primary", function (data) {
                    alertify.success("Концепт отредактирован");
                    updateCurrentCatalog($scope.parent.Id);
                    $scope.stopSpin();
                }, function ()
                {
                    $scope.startSpin();
                    var data = $scope.getFileAttachments();
                    $scope.selectedItem.HasData = data.length>0;
                    angular.element('#FileData').val(data);
                });
            };

            $scope.openMemo = function () {
                $.savingDialog("Памятка о ЭУМК", "/ComplexMaterial/ShowMemo", null, "primary", function (data) {
                }, null, { hideSaveButton: true });
            };

            $scope.openConcept = function (id) {
                var data = {};
                data.id = id || $scope.selectedItem.Id;
                var title = 'Просмотр концепта "' + $scope.selectedItem.Name + '"';
                $.savingDialog(title, "/ComplexMaterial/OpenConcept", data, "primary", function (data) {
                }, null, { hideSaveButton: true });
            };

            $scope.openFolder = function ($event) {
                var idFolder = angular.element($event.target).data("idfolder");
                var currentFolder = $scope.getById($scope.folders, idFolder);
                if (currentFolder.IsGroup) {
                    $scope.parent = currentFolder;
                    //angular.element("#material-header").html(currentFolder.Name);
                    angular.element(".catalog").attr("data-pid", idFolder);
                    updateCurrentCatalog(idFolder);
                    updateQueryParams();
                }  
            };

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

