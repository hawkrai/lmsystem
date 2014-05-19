var parentalApp = angular.module("parentalApp", ['ngRoute', 'ui.bootstrap']);

parentalApp.controller("StatCtrl", ['$scope', '$http', '$modal', function ($scope, $http, $modal) {

    $scope.UrlServiceParental = '/Services/Parental/ParentalService.svc/';
    $scope.UrlServiceCore = '/Services/CoreService.svc/';

    $scope.Subjects = [];
    $scope.subject = { Name: "Все предметы", Id: -1, ShortName: "" };
    $scope.studentsStat = [];

    $scope.init = function (groupId) {
        $scope.groupId = groupId;
        $scope.loadSubjects();
    };

    $scope.$watch("subject", function (newValue, oldValue) {
        if (newValue.Id < 0) {
            $scope.data = {};
            $scope.data.Students = {};
        } else {
            $scope.loadData($scope.subject.Id);
        }
    });

    $scope.getStudentStat = function (student) {
        var lecHours = Enumerable.From($scope.data.LecturesMarkVisiting)
            .First(function (x) { return x.StudentId == student.StudentId; }
                .Select(function (x) {
                    if (x.Mark === parseInt(x.Mark)) {
                        return x.Mark;
                    } else {
                        return 0;
                    }
                })
                .ToArray());
        
        var hours = 0;

        lecHours.forEach(function (h) {
            hours += h;
        });

        return hours;
    };

    $scope.loadSubjects = function () {
        $http({
            method: 'Get',
            url: $scope.UrlServiceParental + "GetGroupSubjects/" + $scope.groupId,
            headers: { 'Content-Type': 'application/json' }
        }).success(function (data, status) {
            if (data.Code != '200') {
                alertify.error(data.Message);
            } else {

                if (data.Subjects.length > 0) {
                    $scope.Subjects = data.Subjects;
                    $scope.Subjects.splice(0, 0, $scope.subject);
                }

                alertify.success(data.Message);
            }
        });
    };

    $scope.loadData = function (subjectId) {
        $http({
            method: 'Get',
            url: $scope.UrlServiceCore + "GetGroups/" + subjectId,
            headers: { 'Content-Type': 'application/json' }
        }).success(function (data, status) {
            if (data.Code != '200') {
                alertify.error(data.Message);
            } else {

                var queryData = Enumerable.From(data.Groups).First(function (x) { return x.GroupId == $scope.groupId; });

                $scope.data = queryData;


                alertify.success(data.Message);
            }
        });
    };

}]);




