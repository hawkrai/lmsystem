var parentalApp = angular.module("parentalApp", ['parentalApp.controllers', 'ngRoute', 'ui.bootstrap'])
    .config(function ($locationProvider) {
    })
    .config(function ($routeProvider, $locationProvider) {
        $routeProvider
            .when('/Statistics', {
                templateUrl: '/Parental/Statistics',
                controller: 'StatCtrl'
            })
            .when('/Plan/:subjectId*', {
                templateUrl: '/Parental/Plan',
                controller: 'PlanCtrl'
            })
            .when('/', {
                templateUrl: '/Parental/Front',
            });
    });

var controllersApp = angular.module('parentalApp.controllers', ['ui.bootstrap', 'xeditable']);

controllersApp.controller('MainCtrl', function ($scope, $http) {

    $scope.UrlServiceParental = '/Services/Parental/ParentalService.svc/';
    $scope.UrlServiceCore = '/Services/CoreService.svc/';

    $scope.init = function (groupId) {
        $scope.groupId = groupId;
    };

});

controllersApp.controller("PlanCtrl", ['$scope', '$routeParams', '$http', '$modal', function ($scope, $routeParams, $http, $modal) {
    $scope.lectures = [];
    $scope.labs = [];
    $scope.practicals = [];


    $scope.UrlServiceLectures = '/Services/Lectures/LecturesService.svc/';
    $scope.UrlServiceLabs = '/Services/Labs/LabsService.svc/';
    $scope.UrlServicePractical = '/Services/Practicals/PracticalService.svc/';



    $scope.init = function () {

        $scope.subject = { Id: $routeParams.subjectId };

        $http.get($scope.UrlServiceParental + "GetGroupSubjects/" + $scope.groupId)
            .then(function (response) {
                $scope.subject = Enumerable.From(response.data.Subjects).First(function (x) { return x.Id == $scope.subject.Id; });
            });

        $scope.loadLectures();
        $scope.loadLabs();
        $scope.loadPracticals();

    };

    $scope.loadLectures = function () {
        $.ajax({
            type: 'GET',
            url: $scope.UrlServiceLectures + "GetLectures/" + $scope.subject.Id,
            dataType: "json",
            contentType: "application/json",

        }).success(function (data, status) {
            if (data.Code != '200') {
                alertify.error(data.Message);
            } else {
                $scope.$apply(function () {
                    $scope.lectures = data.Lectures;
                });
            }
        });
    };

    $scope.loadLabs = function () {
        $.ajax({
            type: 'GET',
            url: $scope.UrlServiceLabs + "GetLabs/" + $scope.subject.Id,
            dataType: "json",
            contentType: "application/json",

        }).success(function (data, status) {
            if (data.Code != '200') {
                alertify.error(data.Message);
            } else {
                $scope.$apply(function () {
                    $scope.labs = data.Labs;
                });
            }
        });
    };

    $scope.loadPracticals = function () {
        $.ajax({
            type: 'GET',
            url: $scope.UrlServicePractical + "GetPracticals/" + $scope.subject.Id,
            dataType: "json",
            contentType: "application/json",

        }).success(function (data, status) {
            if (data.Code != '200') {
                alertify.error(data.Message);
            } else {
                $scope.$apply(function () {
                    $scope.practicals = data.Practicals;
                });
            }
        });
    };

}]);

controllersApp.controller("StatCtrl", ['$scope', '$http', '$modal', function ($scope, $http, $modal) {

    $scope.Subjects = [];
    $scope.subject = { Name: "Все предметы", Id: -1, ShortName: "" };
    $scope.subjectStat = [];
    $scope.totalStat = [];
    $scope.allSubjectsStat = [];

    $scope.init = function () {
        $scope.statData = [];
        $scope.loadSubjects();
    };

    $scope.$watch("subject", function (newValue, oldValue) {
        showCurrent();
    });

    $scope.$watch("statData", function (newValue, oldValue) {
        if (newValue && newValue.length > 0)
            $scope.initStatData($scope.subject, newValue);
    });

    $scope.initStatData = function (subject, data) {
        var subjectId = subject.Id;
        var subjectName = subject.ShortName != "" ? subject.ShortName : subject.Name;
        var subjectFullName = subject.Name;

        $scope.subjectStat = [];
        var statObj = { subjectName: subjectName, subjectFullName: subjectFullName, subjectId: subjectId, students: [] };

        if (data.length) {
            data.forEach(
                function (element, index) {

                    statObj.students[index] = getStudentStat(subjectId, element);
                });
        }

        $scope.statData.push(statObj);
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
                    $scope.loadData();
                    $scope.Subjects.splice(0, 0, $scope.subject);
                }
            }
        });
    };

    $scope.loadData = function () {
        $http({
            method: 'Get',
            url: $scope.UrlServiceParental + "LoadGroup?groupId=" + $scope.groupId,
            headers: { 'Content-Type': 'application/json' }
        }).success(function (data, status) {
            if (data.Code != '200') {
                alertify.error(data.Message);
            } else {
                alertify.success('Данные загружены');
                $scope.Subjects.forEach(function (subject) {
                    if (subject.Id) {

                        $scope.groupName = data.GroupName;
                        $scope.initStatData(subject, data.Students);
                    }
                });
                $scope.allSubjectsStat = getAllSubjectsStat();
                showCurrent();
            }
        });
    };

    var showCurrent = function () {
        var id = $scope.subject.Id;
        if (id == 0) {
            $scope.subjectStat = $scope.totalStat;
        }
        if (id < 0) {
            $scope.subjectStat = [];
        }
        if (id > 0) {
            var subject = Enumerable.From($scope.statData).First(function (x) { return x.subjectId == id; });
            if (subject && subject.students)
                $scope.subjectStat = subject.students;
        }
    };

    var getStudentStat = function (id, student) {
        var studentStat = {
            Name: student.FIO,
            Id: student.Id,
            LecHours: 0,
            PractHours: 0,
            LabHours: 0,
            TotalHours: 0,
            LabMark: 0,
            PractMark: 0,
            LabsCount: 0,
            PractsCount: 0,
            TestMark: 0,
            PractHoursView: false
        };

        for (var i = 0; i < student.UserLecturePass.length; i++) {
            if (student.UserLecturePass[i].Key == id) {
                studentStat.LecHours = student.UserLecturePass[i].Value;
                studentStat.LabHours = student.UserLabPass[i].Value;
                studentStat.TotalHours = studentStat.LecHours + studentStat.LabHours;
                studentStat.LabMark = Math.round(student.UserAvgLabMarks[i].Value * 100) / 100;
                studentStat.LabsCount = student.UserLabCount[i].Value;
                studentStat.TestMark = Math.round(student.UserAvgTestMarks[i].Value * 100) / 100;
                break;
            }
        }

        return studentStat;
    };

    var getAllSubjectsStat = function () {
        var allSubjectsResult = [];

        if ($scope.statData && $scope.statData.length > 0) {
            var anySubject = Enumerable.From($scope.statData).First();

            anySubject.students.forEach(function (student) {
                var statStudentObj = { studentName: student.Name, studentId: student.Id, subjectsStat: [] };

                $scope.statData.forEach(function (subject) {
                    if (Enumerable.From(subject.students).Where(function (x) { return x.Id == student.Id; }).Select()
                        .ToArray().length >
                        0) {
                        var subStudent = Enumerable.From(subject.students).First(function (x) {
                            return x.Id == student.Id;
                        });
                        var avgMark = (subStudent.TestMark > 0 && subStudent.LabMark > 0)
                            ? ((subStudent.TestMark + subStudent.LabMark) / 2).toFixed(1)
                            : ((subStudent.TestMark == 0 && subStudent.LabMark > 0) ||
                                (subStudent.TestMark > 0 && subStudent.LabMark == 0)
                                ? (subStudent.TestMark + subStudent.LabMark).toFixed(1)
                                : '-');

                        var statSubjectObj = {
                            SubjectName: subject.subjectName,
                            SubjectFullName: subject.subjectFullName,
                            SubjectId: subject.subjectId,
                            TotalHours: subStudent.TotalHours,
                            AvgMark: avgMark
                        };

                        statStudentObj.subjectsStat.push(statSubjectObj);
                    }
                });

                allSubjectsResult.push(statStudentObj);
            });
        }

        return allSubjectsResult;
    };

}]);




