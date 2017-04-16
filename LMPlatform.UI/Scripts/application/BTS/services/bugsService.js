angular
    .module('btsApp.service.bugs', [])
    .factory('bugsService', [
        '$http',
        'MIN_SEARCH_TEXT_LENGTH',
        function ($http, MIN_SEARCH_TEXT_LENGTH) {

            var serviceUrl = '/Services/BTS/BugsService.svc';
            var bugsUrl = '/Index';
            var projectBugsUrl = '/Projects/';

            function addSortableParams(params, orderBy) {
                if (orderBy.length == 0)
                    return;
                params.sortingPropertyName = orderBy[0].substr(1);
                if (orderBy[0].substr(0, 1) == '-')
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
                getBugs: function (pageNumber, pageSize, searchString, orderBy, projectId) {
                    var resultUrl = serviceUrl + bugsUrl;
                    if (projectId != undefined) {
                        resultUrl = serviceUrl + projectBugsUrl + projectId + '/Index';
                    }
                    return $http({
                        method: 'GET',
                        url: resultUrl,
                        params: formParams(pageNumber, pageSize, searchString, orderBy)
                    });
                },

                deleteBug: function (bugId) {
                    return $http({
                        method: 'DELETE',
                        url: '/BTS/DeleteBug/' + bugId
                    });
                },

                addNumbering: function (bugs, indexFrom) {
                    var length = bugs.length;
                    for (var i = 0; i < length; i++) {
                        bugs[i].Number = i + 1 + indexFrom;
                    }
                }
            };
        }]);
