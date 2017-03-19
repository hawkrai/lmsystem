angular
    .module('btsApp.service.projects', [])
    .factory('projectsService', [
        '$http',
        'MIN_SEARCH_TEXT_LENGTH',
        function ($http, MIN_SEARCH_TEXT_LENGTH) {

            var projectsUrl = '/Services/BTS/ProjectsService.svc/Index';

            function formParams(pageNumber, pageSize, searchString) {
                params = {
                    pageNumber: pageNumber,
                    pageSize: pageSize
                };
                if (searchString.length >= MIN_SEARCH_TEXT_LENGTH)
                    params.searchString = searchString;
                return params;
            };

            return {
                getProjects: function (pageNumber, pageSize, searchString) {
                    return $http({
                        method: 'GET',
                        url: projectsUrl,
                        params: formParams(pageNumber, pageSize, searchString)
                    });
                },

                addNumbering: function (projects, indexFrom) {
                    var length = projects.length;
                    for (var i = 0; i < length; i++) {
                        projects[i].Number = i + 1 + indexFrom;
                    }
                }
            };
        }]);
