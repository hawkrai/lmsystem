

angular
    .module('complexMaterialsApp.service.material', [])
    .factory('titleController', [function () {
        var defValue = '***ЭУМК не выбран***';
        var title = defValue;
        return {
            title: function () {
                return title;
            },
            setTitle: function (newTitle) {
                title = newTitle
            },
            setDefValue: function () {
                title = defValue;
            }
        };
    }])
    .factory('complexMaterialsDataService', [
        '$http',
        function ($http) {

            var url = '/Services/Concept/ConceptService.svc/';

            return {
                attachSiblings: function(data)
                {
                    return $http({
                        method: 'POST',
                        url: url + 'AttachSiblings',
                        data: data
                    });
                },
                getTree: function(data){
                    var id = data.id;
                    return $http({
                        method: 'GET',
                        url: url + 'GetConceptTree?elementId='+id,
                        data: data
                    });

                },
                getConceptById:function(data){
                    return $http({
                        method: 'POST',
                        url: url + 'GetConcepts',
                        data: data
                    });
                },
                getConcepts: function (data) {
                    if (data == undefined) {
                        data = {
                            Pid: 0
                        };
                    } else if (data != undefined && data.Pid == undefined) {
                        data.Pid = 0;
                    }
                    return $http({
                        method: 'POST',
                        url: url + 'GetConcepts',
                        data: data
                    });
                },
                getRootConcepts: function (data) {
                    return $http({
                        method: 'POST',
                        url: url + 'GetRootConcepts',
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
                        url: url + 'backspaceRootFolderMaterials',
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
                deleteConcept: function (data) {
                    return $http({
                        method: 'POST',
                        url: url + 'Remove',
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
                renameDocument: function (data) {
                    return $http({
                        method: 'POST',
                        url: url + 'renameDocumentMaterials',
                        data: data
                    })
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
