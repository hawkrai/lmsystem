
angular
    .module('complexMaterialsApp.ctrl.home', ['ngResource'])
    .controller('homeCtrl', [
        '$scope',
        '$location',
        '$resource',
        function ($scope, $location, $resource) {

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

            $scope.getHeaderValue = function () {
                return "Модуль электронных учебно-методических комплексов";
            }

            $scope.navigationManager = navigationManagerFactory();

            $scope.isActive = function (href) {
                return href == $location.path();
            };

            var users = $resource('api/User');
            $scope.user = {};
            users.get(function (data) {
                $scope.user = data;
            }, $scope.handleError);

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
