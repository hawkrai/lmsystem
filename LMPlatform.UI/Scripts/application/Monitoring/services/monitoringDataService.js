angular
    .module('monitoringApp.service.monitoring', [])
    .factory('monitoringDataService', [
        '$http',
        function ($http) {
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
                        url: '/api/WatchingTime/' + id + "?root=" + gup("root", window.location.href),
                        data: data
                    });
                },
                getConcept: function () {
                    return $http({
                        method: 'GET',
                        url: '/Services/Concept/ConceptService.svc/GetConcept?elementId=' + gup("root", window.location.href),
                    });
                },
                getRootId: function () {
                    return gup("root", window.location.href);
                },
                getSubjectId: function () {
                    return gup("subjectId", window.location.href);
                }
            };
        }]);
