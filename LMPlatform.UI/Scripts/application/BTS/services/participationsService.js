angular
    .module('btsApp.service.participations', [])
    .factory('participationsService', [
        '$http',
        function ($http) {

            var lecturersUrl = '/Services/CoreService.svc/GetLecturers/All';

            return {
                getLecturers: function () {
                    return $http({
                        method: 'GET',
                        url: lecturersUrl
                    });
                }
            };
        }]);
