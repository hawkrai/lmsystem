
angular
    .module('cpApp.ctrl.home', ['ngResource'])
    .controller('homeCtrl', [
        '$scope',
        '$location',
        '$resource',
        function ($scope, $location, $resource) {

            $scope.Title = "Курсовые проекты (работы)";

            $scope.setTitle = function (title) {
                $scope.Title = title;
            };

            $scope.dateFormat = "dd/MM/yyyy";

            $scope.parseDate = function (str) {
                if (str == null) return null;
                return new Date(Date.parse(str));
            };

            $scope.formatDate = function (date, format) {
                if (date == null) return null;
                return $.datepicker.formatDate(format || "dd/mm/yy", date);
            };

            $scope.todayIso = function () {
                return $scope.getUtcDate(new Date()).toISOString();
            };

            $scope.getUtcDate = function (date) {
                if (!date) return null;

                if (typeof date == 'string') {
                    date = new Date(Date.parse(date));
                }

                return new Date(Date.UTC(date.getFullYear(), date.getMonth(), date.getDate()));
            };

            $scope.dateOptions = {
                startingDay: 1,
                showWeeks: false,
            };

            $scope.handleError = function (respData) {
                var data = respData.data || respData;
                var message = '';

                if (data.ExceptionMessage) {
                    alertify.error(data.ExceptionMessage);
                    return;
                }

                for (var prop in data.ModelState) {
                    if (data.ModelState.hasOwnProperty(prop)) {
                        $.each(data.ModelState[prop], function () {
                            message += this + '<br/>';
                        });
                    }
                }
                alertify.error(message);
            };

            $scope.navigationManager = navigationManagerFactory();

            $scope.isActive = function (href) {
                return href == $location.path();
            };

            var users = $resource('api/CourseUser');
            $scope.user = {};
            users.get(function (data) {
                $scope.user = data;
            }, $scope.handleError);
            
            var lecturerSelectedSecretaryId = null;
            $scope.setLecturerSelectedSecretaryId = function (id) {
                lecturerSelectedSecretaryId = id;
            };
            $scope.getLecturerSelectedSecretaryId = function (id) {
                return lecturerSelectedSecretaryId;
            };

            function navigationManagerFactory() {
                var listPath = '/';
                var urlParams = '';
                return {
                    setListPage: function () {
                        listPath = $location.path();
                        urlParams = $location.search();
                    },
                    goToListPage: function () {
                        $location.path(listPath).search(urlParams);
                    }
                };
            }

        }]);
