var parentalApp = angular.module("parentalApp", ['ngRoute', 'ui.bootstrap']);

parentalApp.controller("StatCtrl", ['$scope', '$http', '$modal', function ($scope, $http, $modal) {

    $scope.UrlServiceParental = '/Services/Parental/ParentalService.svc/';
    $scope.UrlServiceCore = '/Services/CoreService.svc/';

    $scope.Subjects = [];
    $scope.subject = { Name: "Все предметы", Id: 0, ShortName: "" };
    $scope.subjectStat = [];
    $scope.totalStat = [];

    $scope.init = function (groupId) {
        $scope.statData = [];
        $scope.groupId = groupId;
        $scope.loadSubjects();
    };

    $scope.$watch("subject", function (newValue, oldValue) {
        if (newValue.Id <= 0) {
            $scope.subjectStat = $scope.totalStat;
        } else {
            if ($scope.statData.length > 0 && $scope.statData[newValue.Id])
                $scope.subjectStat = $scope.statData[newValue.Id];
        }
    });

    $scope.$watch("statData", function (newValue, oldValue) {
        if (newValue.length > 0)
            $scope.initStatData($scope.subject.Id, newValue);
    });

    $scope.initStatData = function (subjectId, data) {
        $scope.subjectStat = [];
        $scope.statData[subjectId] = [];

        if (data.SubGroupsOne.Students.length) {
            data.SubGroupsOne.Students.forEach(
                function (element, index) {
                    $scope.statData[subjectId][index] = getStudentStat(element, data);
                });
        }

        if (data.SubGroupsTwo.Students.length) {
            var length = $scope.statData[subjectId].length;
            data.SubGroupsTwo.Students.forEach(
                function (element, index) {
                    $scope.statData[subjectId][length + index] = getStudentStat(element, data);
                });
        }

        getTotalStat();
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

                    data.Subjects.forEach(function (val) {
                        if (val.Id) {
                            $scope.loadData(val.Id);
                        }
                    });

                    

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

                $scope.initStatData(subjectId, queryData);
                $scope.totalStat = getTotalStat();
                showCurrent();

               // alertify.success(data.Message);
            }
        });
    };

    var showCurrent = function () {
        var id = $scope.subject.Id;
        if (id <= 0) {
            $scope.subjectStat = $scope.totalStat;
        } else {
            if ($scope.statData.length > 0 && $scope.statData[id])
                $scope.subjectStat = $scope.statData[id];
        }
    };

    var getStudentStat = function (subGroupStudent, data) {
        var studentStat = {
            Name: subGroupStudent.FullName,
            Id: subGroupStudent.studentId,
            LecHours: 0,
            PractHours: 0,
            LabHours: 0,
            TotalHours: 0,
            LabMark: 0,
            PractMark: 0,
            LabsCount: 0,
            PractsCount: 0
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

        return studentStat;
    };

    var getTotalStat = function () {
        var totalResult = [];
        if ($scope.statData && $scope.statData.length > 0) {

            $scope.statData.forEach(function (studentsStatData) {
                if (studentsStatData.length > 0)
                    studentsStatData.forEach(function (student, index) {
                        totalResult[index] = sumStudentStat(totalResult[index], student);
                    });

            });
        }

        return totalResult;
    };

    var sumStudentStat = function (current, student) {

        if (current) {
            current.LecHours += student.LecHours;
            current.PractHours += student.PractHours;
            current.LabHours += student.LabHours;
            current.TotalHours += student.TotalHours;
            current.LabMark += student.LabMark;
            current.PractMark += student.PractMark;
            current.LabsCount += student.LabsCount;
            current.PractsCount += student.PractsCount;
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
                PractsCount: student.PractsCount
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
            if (result.pos > 0)
                result.avg = result.sum / result.pos;
        }

        return result;
    };

}]);




