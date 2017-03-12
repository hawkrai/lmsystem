angular
    .module('btsApp.service.projects', [])
    .factory('projectsService', [
        '$http',
        function ($http) {

            var projectsUrl = '/Services/BTS/ProjectsService.svc/Index';

            function formParams(pageNumber, pageSize) {
                return {
                    pageNumber: pageNumber,
                    pageSize: pageSize
                };
            };

            return {
                getProjects: function (pageNumber, pageSize) {
                    return $http({
                        method: 'GET',
                        url: projectsUrl,
                        params: formParams(pageNumber, pageSize)
                    });
                },

                addNumbering: function (projects, indexFrom) {
                    var length = projects.length;
                    for (var i = indexFrom; i < length; i++) {
                        projects[i].Number = i + 1;
                    }
                }
            };
        }]);
