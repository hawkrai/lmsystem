'use strict';
knowledgeTestingApp.controller('testsNavigationCtrl', function ($scope, $location) {

    $scope.isActive = function (path) {
        return $location.path().substr(0, path.length) == path;
    };

});