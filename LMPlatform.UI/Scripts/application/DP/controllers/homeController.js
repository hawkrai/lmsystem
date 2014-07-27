
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

            $scope.formatDate = function (date) {
                if (date == null) return null;
                return $.datepicker.formatDate("dd/mm/yy", date);
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
