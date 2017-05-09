angular
    .module('complexMaterialsApp.service.monitoring', [])
    .factory('monitoringDataService', [
        '$http',
        '$routeParams',
        function ($http, $routeParams) {
            var url = '/Services/CoreService.svc/';

            function gup(name, url) {
                if (!url) url = location.href;
                name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
                var regexS = "[\\?&]" + name + "=([^&#]*)";
                var regex = new RegExp(regexS);
                var results = regex.exec(url);
                return results == null ? null : results[1];
            }

            return {
                getGroups: function (data) {
                    var id = data.id;
                    return $http({
                        method: 'GET',
                        url: url + 'GetOnlyGroups/' + id,
                        data: data
                    });
                },
                getStudents: function (data) {
                    var id = data.id;
                    return $http({
                        method: 'GET',
                        url: url + 'GetStudentsByStudentGroupId/' + gup("subjectId", window.location.href) + '/' + id,
                    });
                },
                getConcepts: function (data) {
                    var id = data.id;
                    return $http({
                        method: 'GET',
                        url: '/api/WatchingTime/' + id + "?root=" + $routeParams.rootId + "&studentId=" + window.location.href.substr(window.location.href.lastIndexOf('/') + 1),
                    });
                },
                getConcept: function () {
                    return $http({
                        method: 'GET',
                        url: '/Services/Concept/ConceptService.svc/GetConcept?elementId=' + $routeParams.rootId,
                    });
                },
                getRootId: function () {
                    return $routeParams.rootId;
                },
                getSubjectId: function () {
                    return gup("subjectId", window.location.href);
                }
            };
        }]);
