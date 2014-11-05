
angular
    .module('dpApp.ctrl.home', [])
    .controller('homeCtrl', [
        '$scope',
        '$location',
        function ($scope, $location) {

            $scope.Title = "Дипломное проектирование";

            $scope.setTitle = function(title) {
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

            $scope.handleError = function(respData) {
                var data = respData.data || respData;
                var message = '';

                if (data.ExceptionMessage) {
                    alertify.error(data.ExceptionMessage);
                    return;
                }

                for (var prop in data.ModelState) {
                    if (data.ModelState.hasOwnProperty(prop)) {
                        $.each(data.ModelState[prop], function() {
                            message += this + '<br/>';
                        });
                    }
                }
                alertify.error(message);
            };

            $scope.navigationManager = navigationManagerFactory();

            $scope.isActive = function(href) {
                return href == $location.path();
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
