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
            .then(function(response) {
                $scope.subject = Enumerable.From(response.data.Subjects).First(function(x) { return x.Id == $scope.subject.Id; });
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

        if (data.SubGroupsOne.Students.length) {
            data.SubGroupsOne.Students.forEach(
                function (element, index) {
                    statObj.students[index] = getStudentStat(element, data);
                });
        }

        if (data.SubGroupsTwo.Students.length) {
            var length = statObj.students.length;
            data.SubGroupsTwo.Students.forEach(
                function (element, index) {
                    statObj.students[length + index] = getStudentStat(element, data);
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

                    data.Subjects.forEach(function (val) {
                        if (val.Id) {
                            $scope.loadData(val);
                        }
                    });

                    $scope.Subjects = data.Subjects;

					$scope.Subjects.splice(0, 0, $scope.subject);
					$scope.Subjects.splice($scope.Subjects.length, 0, { Name: "Суммарная статистика по всем предметам", Id: 0, ShortName: "" });
                    
					
                }
            }
        });
    };

    $scope.loadData = function (subject) {
        $http({
            method: 'Get',
            url: $scope.UrlServiceCore + "GetGroups?subjectId=" + subject.Id + "&groupId=" + $scope.groupId,
            headers: { 'Content-Type': 'application/json' }
        }).success(function (data, status) {
            if (data.Code != '200') {
                alertify.error(data.Message);
            } else {

                var queryData = Enumerable.From(data.Groups).First(function (x) { return x.GroupId == $scope.groupId; });

                $scope.groupName = queryData.GroupName;
                $scope.initStatData(subject, queryData);

                $scope.totalStat = getTotalStat();
                $scope.allSubjectsStat = getAllSubjectsStat();

                showCurrent();

                // alertify.success(data.Message);
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

    var getStudentStat = function (subGroupStudent, data) {
        var studentStat = {
            Name: subGroupStudent.FullName,
            Id: subGroupStudent.StudentId,
            LecHours: 0,
            PractHours: 0,
            LabHours: 0,
            TotalHours: 0,
            LabMark: 0,
            PractMark: 0,
            LabsCount: 0,
            PractsCount: 0,
            TestMark: 0,
			PractHoursView: true
        };

        var studentObj = Enumerable.From(data.Students).First(function (x) { return x.StudentId == subGroupStudent.StudentId; });

        var lecVisit = Enumerable.From(data.LecturesMarkVisiting).First(function (x) { return x.StudentId == subGroupStudent.StudentId; }).Marks;
        var lecVisitArray = Enumerable.From(lecVisit).Select(function (x) { return x.Mark; }).ToArray();

        var pracVisitArray = Enumerable.From(studentObj.PracticalVisitingMark).Select(function (x) { return x.Mark; }).ToArray();
        var pracMarkArray = Enumerable.From(studentObj.StudentPracticalMarks).Select(function (x) { return x.Mark; }).ToArray();

        var labVisitArray = Enumerable.From(subGroupStudent.LabVisitingMark).Select(function (x) { return x.Mark; }).ToArray();
        var labMarkArray = Enumerable.From(subGroupStudent.StudentLabMarks).Select(function (x) { return x.Mark; }).ToArray();

        var lecVisitResult = markArrayProc(lecVisitArray);
        var practVisitResult = markArrayProc(pracVisitArray);
        var labVisitResult = markArrayProc(labVisitArray);

		studentStat.LecHours = lecVisitResult.sum; //<= 0 ? "-" : lecVisitResult.sum;
        studentStat.PractHours = practVisitResult.sum; //<= 0 ? "-" : practVisitResult.sum;
        studentStat.LabHours = labVisitResult.sum; //<= 0 ? "-" : labVisitResult.sum;
        studentStat.TotalHours = lecVisitResult.sum + practVisitResult.sum + labVisitResult.sum;

        var labMarkResult = markArrayProc(labMarkArray);
        var practMarkResult = markArrayProc(pracMarkArray);

        studentStat.LabMark = labMarkResult.avg;
        studentStat.PractMark = practMarkResult.avg;
        studentStat.LabsCount = labMarkResult.pos;
        studentStat.PractsCount = practMarkResult.pos;
        studentStat.TestMark = parseFloat(studentObj.TestMark || 0);

		studentStat.PractHoursView = pracVisitArray.length > 0;

        return studentStat;
    };

    var getTotalStat = function () {
        var totalResult = [];
        if ($scope.statData && $scope.statData.length > 0) {

            $scope.statData.forEach(function (studentsStatData) {
                if (studentsStatData.students.length > 0)
                    studentsStatData.students.forEach(function (student, index) {
                        totalResult[index] = sumStudentStat(totalResult[index], student);
                    });

            });
        }

        return totalResult;
    };

    var getAllSubjectsStat = function () {
        var allSubjectsResult = [];

        if ($scope.statData && $scope.statData.length > 0) {
            var anySubject = Enumerable.From($scope.statData).First();

            anySubject.students.forEach(function (student) {
                var statStudentObj = { studentName: student.Name, studentId: student.Id, subjectsStat: [] };

                $scope.statData.forEach(function (subject) {
                    var subStudent = Enumerable.From(subject.students).First(function (x) { return x.Id == student.Id; });
                    var avgMark = (student.TestMark > 0 && student.LabMark > 0) ? (student.TestMark + student.LabMark) / 2 :
                        ((student.TestMark == 0 && student.LabMark > 0) || (student.TestMark > 0 && student.LabMark == 0) ?
                         (student.TestMark + student.LabMark) : '-');

                    var statSubjectObj = {
                        SubjectName: subject.subjectName,
                        SubjectFullName: subject.subjectFullName,
                        SubjectId: subject.subjectId,
                        TotalHours: subStudent.TotalHours,
                        AvgMark: avgMark
                    };

                    statStudentObj.subjectsStat.push(statSubjectObj);
                });

                allSubjectsResult.push(statStudentObj);
            });
        }

        return allSubjectsResult;
    };

    var sumStudentStat = function (current, student) {

        if (current) {
            current.LecHours += student.LecHours;
            current.PractHours += student.PractHours;
            current.LabHours += student.LabHours;
            current.TotalHours += student.TotalHours;


            var labsCount = current.LabsCount + student.LabsCount;
            var practsCount = current.PractsCount + student.PractsCount;

            current.LabMark = labsCount != 0 ? Number(((current.LabMark * current.LabsCount + student.LabMark * student.LabsCount) / labsCount).toFixed(1)) : 0;
            current.PractMark = practsCount != 0 ? Number(((current.PractMark * current.PractsCount + student.PractMark * student.PractsCount) / practsCount).toFixed(1)) : 0;

            current.LabsCount = labsCount;
            current.PractsCount = practsCount;
        } else {
            return {
                Name: student.Name,
                Id: student.Id,
                LecHours: student.LecHours,
                PractHours: student.PractHours,
                LabHours: student.LabHours,
                TotalHours: student.TotalHours,
                LabMark: student.LabMark,
                PractMark: student.PractMark,
                LabsCount: student.LabsCount,
                PractsCount: student.PractsCount,
				TestMark: student.TestMark
            };
        }

        return current;
    };

    var markArrayProc = function (arr) {
        var result = { sum: 0, avg: 0, pos: 0, neg: 0 };

        if (arr && arr.length > 0) {
            arr.forEach(function (val) {
                var value = parseInt(val);

                if (value && value > 0) {
                    result.sum += value;
                } else {
                    result.neg += 1;
                }
            });

            result.pos = arr.length - result.neg;
            if (result.pos > 0) {
                result.avg = Math.round(result.sum / result.pos * 100) / 100;
            }
        }

        return result;
    };

}]);




