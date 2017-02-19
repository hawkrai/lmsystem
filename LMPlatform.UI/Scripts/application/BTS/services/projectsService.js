angular
    .module('btsApp.service.projects', [])
    .factory('projectsService', [
        '$http',
        function ($http) {

            var projectsUrl = '/api/BtsProjects';

            return {
                getProjects: function (params) {
                    return $http({
                        method: 'GET',
                        url: projectsUrl,
                        params: params
                    });
                },

                addNumbering: function (projects) {
                    projects.forEach(function (item, i) {
                        item.Number = i + 1;
                    });
                }
            };
        }]);
