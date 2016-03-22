
angular
    .module('cpApp.service.project', [])
    .factory('projectService', [
        '$http',
        function ($http) {

            var apiUrl = '/api/courseProject/';
            var projectAssignmentUrl = '/api/courseProjectAssignment/';
            var correlationApiUrl = '/api/cpcorrelation/';
            var studentApiUrl = '/api/courseStudent/';
            var downloadTaskSheetUrl = "/Cp/GetTasksSheetDocument?courseProjectId=";
            var downloadTaskSheetHtmlUrl = "/Cp/GetTasksSheetHtml?courseProjectId=";

            return {
                getProjects: function (subjectId, params) {
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
                        params: { entity: 'LecturerCourseGroup' }
                    });
                },


                getDiplomProjectCorrelation: function () {
                    return $http({
                        method: 'GET',
                        url: correlationApiUrl,
                        params: { entity: 'CourseProject' }
                    });
                },


                getDiplomLecturerCorrelation: function () {
                    return $http({
                        method: 'GET',
                        url: correlationApiUrl,
                        params: { entity: 'CourseLecturer' }
                    });
                },


                getDiplomProjectTaskSheetTemplateCorrelation: function () {
                    return $http({
                        method: 'GET',
                        url: correlationApiUrl,
                        params: { entity: 'CourseProjectTaskSheetTemplate' }
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
