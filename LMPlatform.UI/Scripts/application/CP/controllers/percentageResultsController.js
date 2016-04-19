angular
    .module('cpApp.ctrl.percentageResults', ['ngTable', 'ngResource'])
    .controller('percentageResultsCtrl', [
        '$scope',
        '$modal',
        'ngTableParams',
        '$resource',
        '$location',
        'projectService',
        function ($scope, $modal, ngTableParams, $resource, $location, projectService) {

            $scope.setTitle("Результаты процентовки");

            var percentageResults = $resource('api/CpPercentageResult');
            var studentMarks = $resource('api/CourseStudentMark');

            $scope.percentages = [];

            $scope.groups = [];
            $scope.selectGroup = function (group) {
                $scope.selectedGroupId = group.Id;
                $scope.setLecturerSelectedSecretaryId(group.Id);
                $scope.tableParams.reload();
            };

            projectService.getLecturerDiplomGroupCorrelation(subjectId)
                .success(function (data) {
                    $scope.groups = data;
                    var selectedSecretaries = data.filter(function (elt) {
                        return $scope.getLecturerSelectedSecretaryId() == elt.Id ? elt : null;
                    });
                    if (selectedSecretaries.length == 1) {
                        $scope.group = selectedSecretaries[0];
                        $scope.selectGroup($scope.group);
                    } else {
                        $scope.group = { Id: null, Name: "Выберите секретаря" };
                    }
                });


            $scope.getPercentageResult = function (student, percentageGraphId) {

                var results = student.PercentageResults;

                for (var i = 0; i < results.length; i++) {
                    if (results[i].PercentageGraphId == percentageGraphId) {
                        return results[i];
                    }
                }
                return {
                    StudentId: student.Id,
                    PercentageGraphId: percentageGraphId
                };
            };


            $scope.savePercentageResult = function (percentageResult, newValue) {

                if (newValue && (isNaN(newValue, 10) || newValue < 0 || newValue > 100)) return "Введите число от 0 до 100!";

                percentageResult.Mark = newValue;

                percentageResults.save(percentageResult)
                    .$promise.then(function (data, status, headers, config) {
                        $scope.tableParams.reload();
                        alertify.success('Процент успешно сохранен');
                    }, $scope.handleError);
                return false;
            };


            $scope.saveStudentMark = function (assignedDiplomProjectId, newValue) {

                if (newValue && (isNaN(newValue, 10) || newValue < 1 || newValue > 10)) return "Введите число от 1 до 10!";


                studentMarks.save([assignedDiplomProjectId, newValue])
                    .$promise.then(function (data, status, headers, config) {
                        $scope.tableParams.reload();
                        alertify.success('Оценка успешно сохранена');
                    }, $scope.handleError);
                return false;
            };

            function getParameterByName(name) {
                name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
                var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
                    results = regex.exec(location.search);
                return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
            }



            var subjectId = getParameterByName("subjectId");

            $scope.tableParams = new ngTableParams(
                angular.extend({
                    page: 1,
                    count: 1000
                }, $location.search()), {
                    total: 0,
                    getData: function ($defer, params) {
                        $location.search(params.url());
                        percentageResults.get(angular.extend(params.url(), {
                            filter:
                            {
                                subjectId: subjectId,
                                secretaryId: $scope.selectedGroupId
                            }}),
                            function (data) {
                                $defer.resolve(data.Students.Items);
                                params.total(data.Students.Total);
                                $scope.percentages = data.PercentageGraphs;
                            });
                    }
                });
        }]);