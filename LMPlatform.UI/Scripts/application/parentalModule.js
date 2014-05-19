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

    $scope.$watch("data", function (newValue, oldValue) {
        $scope.studentsStat = [];
        if (newValue.Students.length) {
            newValue.Students.forEach(
                function (element, index) {
                    $scope.studentsStat[index] = getStudentStat(element);
                });
        }
    });


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

    var getStudentStat = function (student) {
        var studentStat = {
            Name: student.FullName,
            LecHours: 0
        };

        var lecMarks = Enumerable.From($scope.data.LecturesMarkVisiting).First(function (x) { return x.StudentId == student.StudentId; }).Marks;

        studentStat.LecHours = 0;

        lecMarks.forEach(function (mark) {
            var hours = parseInt(mark.Mark);
            if (hours)
                studentStat.LecHours += hours;
        });

        return studentStat;
    };

}]);




