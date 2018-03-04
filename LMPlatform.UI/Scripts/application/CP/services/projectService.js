
angular
    .module('cpApp.service.project', [])
    .factory('projectService', [
        '$http',
        function ($http) {

            var apiUrl = '/api/courseProject/';
            var apiUrlSubject = '/api/CourseProjectSubject/';
            var apiUrlGroups = "/api/CourseProjectGroup/";
            var projectAssignmentUrl = '/api/courseProjectAssignment/';
            var correlationApiUrl = '/api/cpcorrelation/';
            var studentApiUrl = '/api/courseStudent/';
            var downloadTaskSheetUrl = "/Cp/GetTasksSheetDocument?courseProjectId=";
            var downloadTaskSheetHtmlUrl = "/Cp/GetTasksSheetHtml?courseProjectId=";
            var apiUrlNewses = '/api/CourseProjectNews/';
            var updateSubjectUrl = 'Services/Subjects/SubjectsService.svc/Subjects';

            return {
                getProjects: function (subjectId, params) {
                    return $http({
                        method: 'GET',
                        url: apiUrl,
                        params: params
                    });
                },

                getNewses: function (subjectId) {
                    return $http({
                        method: 'GET',
                        url: apiUrlNewses + subjectId
                    });
                },

               getProject: function (projectId) {
                    return $http({
                        method: 'GET',
                        url: apiUrl + projectId
                    });
                },

               getSubject: function (subjectId) {
                   return $http({
                       method: 'GET',
                       url: apiUrlSubject + subjectId
                   });
               },

               getGroups: function (subjectId) {
                   return $http({
                       method: 'GET',
                       url: apiUrlGroups + subjectId
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


                getGroupCorrelation: function (subjectId) {
                    return $http({
                        method: 'GET',
                        url: correlationApiUrl,
                        params: {
                            entity: 'Group',
                            subjectId: subjectId
                        }
                    });
                },


                getLecturerDiplomGroupCorrelation: function (subjectId) {
                    return $http({
                        method: 'GET',
                        url: correlationApiUrl,
                        params: {
                            entity: 'LecturerCourseGroup',
                            subjectId: subjectId
                        }
                    });
                },


                getCourseProjectCorrelation: function (subjectId) {
                    return $http({
                        method: 'GET',
                        url: correlationApiUrl,
                        params: {
                            entity: 'CourseProject',
                            subjectId: subjectId
                        }
                    });
                },


                getDiplomLecturerCorrelation: function (subjectId) {
                    return $http({
                        method: 'GET',
                        url: correlationApiUrl,
                        params: {
                            entity: 'CourseLecturer',
                            subjectId: subjectId
                        }
                    });
                },


                getDiplomProjectTaskSheetTemplateCorrelation: function (subjectId) {
                    return $http({
                        method: 'GET',
                        url: correlationApiUrl,
                        params: {
                            entity: 'CourseProjectTaskSheetTemplate',
                            subjectId: subjectId
                        }
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
                setCreateBts: function (subjrctId, isNeededCopyToBts) {
                    return $http({
                        method: 'PATCH',
                        url: updateSubjectUrl,
                        data: {
                            subject: {
                                Id: subjrctId,
                                IsNeededCopyToBts: isNeededCopyToBts
                            }
                        }
                    });
                }
            };
        }]);
