
angular
    .module('dpApp.service.project', [])
    .factory('projectService', [
        '$http',
        function ($http) {

            var apiUrl = '/api/diplomProject/';
            var correlationApiUrl = '/api/correlation/';

            return {
                getProjects: function (params) {
                    return $http({
                        method: 'GET',
                        url: apiUrl,
                        params: params
                    });
                },
                
                getGroupCorrelation: function () {
                    return $http({
                        method: 'GET',
                        url: correlationApiUrl,
                        params: {entity: 'Group'}
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
