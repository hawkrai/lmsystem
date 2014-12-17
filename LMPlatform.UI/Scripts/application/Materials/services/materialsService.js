
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
                createFolder: function (data) {
                    return $http({
                        method: 'POST',
                        url: url + 'createFolderMaterials',
                        data: data
                    });
                }
            };
        }]);
