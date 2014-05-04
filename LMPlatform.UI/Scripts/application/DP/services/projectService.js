
angular
    .module('dpApp.service.project', [])
    .factory('projectService', [
        '$http',
        function ($http) {

            var apiUrl = '/api/diplomProject/';

            return {
                getProjects: function (params) {
                    return $http({
                        method: 'GET',
                        url: apiUrl,
                        params: params
                    });
                },

                getProject: function (projectId) {
                    return $http({
                        method: 'GET',
                        url: apiUrl + projectId
                    });
                },

                createProject: function (project) {
                    return $http({
                        method: 'POST',
                        url: apiUrl,
                        data: project
                    });
                },

                updateProject: function (project) {
                    return $http({
                        method: 'PUT',
                        url: apiUrl,
                        data: project
                    });
                },

                deleteproject: function (projectId) {
                    return $http({
                        method: 'DELETE',
                        url: apiUrl + projectId
                    });
                }
            };
        }]);
