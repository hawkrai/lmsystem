angular
    .module('btsApp.service.participations', [])
    .factory('participationsService', [
        '$http',
        'MIN_SEARCH_TEXT_LENGTH',
        function ($http, MIN_SEARCH_TEXT_LENGTH) {

            var coreServiceUrl = '/Services/CoreService.svc';
            var projectsServiceUrl = '/Services/BTS/ProjectsService.svc';
            var lecturersUrl = '/GetLecturers/All';
            var groupsUrl = '/GetGroupsByUser/';
            var studentParticipationsUrl = '/StudentsParticipationsByGroup/';
            var userProjectParticipationsUrl = '/ProjectParticipationsByUser/';

            function formParams(pageNumber, pageSize, orderBy) {
                params = {
                    pageNumber: pageNumber,
                    pageSize: pageSize
                };
                if (orderBy != undefined) {
                    addSortableParams(params, orderBy);
                }
                return params;
            };

            function addSortableParams(params, orderBy) {
                if (orderBy.length == 0)
                    return;
                params.sortingPropertyName = orderBy[0].substr(1);
                if (orderBy[0].substr(0, 1) == '-')
                    params.desc = true;
            };

            return {
                getLecturers: function () {
                    return $http({
                        method: 'GET',
                        url: coreServiceUrl + lecturersUrl
                    });
                },

                getGroups: function (id) {
                    return $http({
                        method: 'GET',
                        url: coreServiceUrl + groupsUrl + id
                    });
                },

                getUserProjectsParticipations: function (userId, pageNumber, pageSize, orderBy) {
                    return $http({
                        method: 'GET',
                        url: projectsServiceUrl + userProjectParticipationsUrl + userId,
                        params: formParams(pageNumber, pageSize, orderBy)
                    });
                },

                geStudentsParticipations: function (groupId, pageNumber, pageSize) {
                    return $http({
                        method: 'GET',
                        url: projectsServiceUrl + studentParticipationsUrl + groupId,
                        params: formParams(pageNumber, pageSize)
                    });
                }
            };
        }]);
