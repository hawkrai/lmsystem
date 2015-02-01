
angular
    .module('dpApp.service.project', [])
    .factory('projectService', [
        '$http',
        function ($http) {

            var apiUrl = '/api/diplomProject/';
            var projectAssignmentUrl = '/api/diplomProjectAssignment/';
            var correlationApiUrl = '/api/correlation/';
            var studentApiUrl = '/api/student/';
            var downloadTaskSheetUrl = "/Dp/GetTasksSheetDocument?diplomProjectId=";
            var downloadTaskSheetHtmlUrl = "/Dp/GetTasksSheetHtml?diplomProjectId=";

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
                },

                assignProject: function (projectId, studentId) {
                    return $http({
                        method: 'POST',
                        url: projectAssignmentUrl,
                        data: { projectId: projectId, studentId: studentId }
                    });
                },

                deleteAssignment: function (projectId) {
                    return $http({
                        method: 'DELETE',
                        url: projectAssignmentUrl + projectId
                    });
                },


                getStudents: function (projectId, params) {
                    return $http({
                        method: 'GET',
                        url: studentApiUrl,
                        params: params
                    });
                },


                getGroupCorrelation: function () {
                    return $http({
                        method: 'GET',
                        url: correlationApiUrl,
                        params: { entity: 'Group' }
                    });
                },


                getLecturerDiplomGroupCorrelation: function () {
                    return $http({
                        method: 'GET',
                        url: correlationApiUrl,
                        params: { entity: 'LecturerDiplomGroup' }
                    });
                },


                getDiplomProjectCorrelation: function () {
                    return $http({
                        method: 'GET',
                        url: correlationApiUrl,
                        params: { entity: 'DiplomProject' }
                    });
                },


                getDiplomLecturerCorrelation: function () {
                    return $http({
                        method: 'GET',
                        url: correlationApiUrl,
                        params: { entity: 'DiplomLecturer' }
                    });
                },


                getDiplomProjectTaskSheetTemplateCorrelation: function () {
                    return $http({
                        method: 'GET',
                        url: correlationApiUrl,
                        params: { entity: 'DiplomProjectTaskSheetTemplate' }
                    });
                },

                downloadTaskSheetHtml: function (projectId) {
                    return $http({
                        method: 'GET',
                        url: downloadTaskSheetHtmlUrl + projectId,
                    });
                },

                downloadTaskSheet: function (projectId) {
                    location.href = downloadTaskSheetUrl + projectId;
                },
            };
        }]);
