angular
    .module('btsApp.service.projects', [])
    .factory('projectsService', [
        '$http',
        'MIN_SEARCH_TEXT_LENGTH',
        function ($http, MIN_SEARCH_TEXT_LENGTH) {

            var serviceUrl = '/Services/BTS/ProjectsService.svc';
            var projectUrl = '/Show';
            var projectsUrl = '/Index';

            function addSortableParams(params, orderBy) {
                if (orderBy.length === 0)
                    return;
                params.sortingPropertyName = orderBy[0].substr(1);
                if (orderBy[0].substr(0, 1) === '-')
                    params.desc = true;
            };

            function formParams(pageNumber, pageSize, searchString, orderBy) {
                params = {
                    pageNumber: pageNumber,
                    pageSize: pageSize
                };
                addSortableParams(params, orderBy);
                if (searchString.length >= MIN_SEARCH_TEXT_LENGTH)
                    params.searchString = searchString;
                return params;
            };

            return {
                getProject: function(id) {
                    return $http({
                        method: 'GET',
                        url: serviceUrl + projectUrl + '/' + id
                    });
                },

                getProjectWithBugsAndMembers: function (id) {
                    return $http({
                        method: 'GET',
                        url: serviceUrl + projectUrl + '/' + id + '?withDetails=true'
                    });
                },

                getProjects: function (pageNumber, pageSize, searchString, orderBy) {
                    return $http({
                        method: 'GET',
                        url: serviceUrl + projectsUrl,
                        params: formParams(pageNumber, pageSize, searchString, orderBy)
                    });
                },

                deleteProject: function (projectId) {
                    return $http({
                        method: 'DELETE',
                        url: '/BTS/DeleteProject/' + projectId
                    });
                },

                deleteProjectUser: function (id) {
                    return $http({
                        method: 'DELETE',
                        url: '/BTS/DeleteProjectUser/' + id
                    });
                },

                addNumbering: function (projects, indexFrom) {
                    var length = projects.length;
                    for (var i = 0; i < length; i++) {
                        projects[i].Number = i + 1 + indexFrom;
                    }
                },

                getProjectComments: function (id) {
                    return $http({
                        method: 'GET',
                        url: serviceUrl + '/Projects/' + id + '/Bugs'
                    });
                },

                sendProjectComment: function (id, text) {
                    return $http({
                        method: 'POST',
                        url: '/BTS/ProjectManagement/' + id,
                        data: {
                            comment: text
                        }
                    });
                }
            };
        }]);
