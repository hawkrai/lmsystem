
angular
    .module('materialsApp.service.material', [])
    .factory('materialsService', [
        '$http',
        function ($http) {

            var url = '/Services/Materials/MaterialsService.svc/';

            return {
                getFolders: function (data) {
                    if (data == undefined) {
                        data = {
                            Pid: 0
                        };
                    } else if (data != undefined && data.Pid == undefined) {
                        data.Pid = 0;
                    }
                    return $http({
                        method: 'POST',
                        url: url + 'getFoldersMaterials',
                        data: data
                    });
                },
                backspaceFolder: function (data) {
                    if (data == undefined) {
                        data = {
                            id: 0
                        };
                    } else if (data != undefined && data.id == undefined) {
                        data.id = 0;
                    }
                    return $http({
                        method: 'POST',
                        url: url + 'backspaceFolderMaterials',
                        data: data
                    });
                },
                createFolder: function (data) {
                    return $http({
                        method: 'POST',
                        url: url + 'createFolderMaterials',
                        data: data
                    });
                },
                deleteFolder: function (data) {
                    return $http({
                        method: 'POST',
                        url: url + 'deleteFolderMaterials',
                        data: data
                    });
                },
                renameFolder: function (data) {
                    return $http({
                        method: 'POST',
                        url: url + 'renameFolderMaterials',
                        data: data
                    });
                },
                saveText: function (data) {
                    return $http({
                        method: 'POST',
                        url: url + 'saveTextMaterials',
                        data: data
                    });
                },
                getDocuments: function (data) {
                    if (data == undefined) {
                        data = {
                            Pid: 0
                        };
                    } else if (data != undefined && data.Pid == undefined) {
                        data.Pid = 0;
                    }
                    return $http({
                        method: 'POST',
                        url: url + 'getDocumentsMaterials',
                        data: data
                    });
                },
                getText: function (data) {
                    return $http({
                        method: 'POST',
                        url: url + 'getTextMaterials',
                        data: data
                    });
                }
            };
        }]);
