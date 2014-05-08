
angular
    .module('dpApp.ctrl.home', [])
    .controller('homeCtrl', [
        '$scope',
        '$location',
        function ($scope, $location) {

            $scope.Title = "Дипломное проектирование";

            $scope.navigationManager = navigationManagerFactory();

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
