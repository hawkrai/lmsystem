'use strict';
studentsTestingApp.controller('testPasssingNavigationCtrl', function ($scope, $location) {

    $scope.isActive = function (path) {
        return $location.path().substr(0, path.length) == path;
    };

});