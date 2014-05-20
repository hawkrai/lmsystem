var parentalApp = angular.module("parentalApp", ['ngRoute', 'ui.bootstrap']);

parentalApp.controller("StatCtrl", ['$scope', '$http', '$modal', function ($scope, $http, $modal) {

    $scope.data = [];

    $scope.UrlServiceParental = '/Services/Parental/ParentalService.svc/';
    $scope.UrlServiceCore = '/Services/CoreService.svc/';

    $scope.Subjects = [];
    $scope.subject = { Name: "Все предметы", Id: 0, ShortName: "" };
    $scope.studentsStat = [];

    $scope.init = function (groupId) {
        $scope.data = [];
        $scope.data.Students = {};
        $scope.groupId = groupId;
        $scope.loadSubjects();
    };

    $scope.$watch("subject", function (newValue, oldValue) {
        if (newValue.Id <= 0) {

        } else {
            if ($scope.data.length > 0)
                $scope.initStudentsStat(newValue.Id);
        }
    });

    $scope.$watch("data", function (newValue, oldValue) {
        $scope.initStudentsStat();
    });

    $scope.initStudentsStat = function (subjectId) {
        $scope.studentsStat = [];
        var data = $scope.data[subjectId];

        if (data.SubGroupsOne.Students.length) {

            data.SubGroupsOne.Students.forEach(
                function (element, index) {
                    $scope.studentsStat[index] = getStudentStat(element, data);
                });
        }

        if (data.SubGroupsTwo.Students.length) {
            var length = $scope.studentsStat.length;
            data.SubGroupsTwo.Students.forEach(
                function (element, index) {
                    $scope.studentsStat[length + index] = getStudentStat(element, data);
                });
        }
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
                $scope.data[subjectId] = queryData;

                alertify.success(data.Message);
            }
        });
    };

    var getStudentStat = function (subGroupStudent, data) {
        var studentStat = {
            Name: subGroupStudent.FullName,
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

        studentStat.LecHours = lecVisitResult.sum <= 0 ? "-" : lecVisitResult.sum;
        studentStat.PractHours = practVisitResult.sum <= 0 ? "-" : practVisitResult.sum;
        studentStat.LabHours = labVisitResult.sum <= 0 ? "-" : labVisitResult.sum;
        studentStat.TotalHours = lecVisitResult.sum + practVisitResult.sum + labVisitResult.sum;

        var labMarkResult = markArrayProc(labMarkArray);
        var practMarkResult = markArrayProc(pracMarkArray);

        studentStat.LabMark = labMarkResult.avg;
        studentStat.PractMark = practMarkResult.avg;
        studentStat.LabsCount = labMarkResult.pos;
        studentStat.PractsCount = practMarkResult.pos;

        return studentStat;
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




