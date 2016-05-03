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
            $scope.group = { Id: null };

            function getParameterByName(name) {
                name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
                var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
                    results = regex.exec(location.search);
                return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
            }



            var subjectId = getParameterByName("subjectId");

            projectService
               .getGroups(subjectId)
               .success(function (data) {
                   $scope.groups = data;
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

            $scope.selectedGroups = false;

            $scope.selectGroups = function (id) {
                if (id) {
                    $scope.selectedGroupId = id;
                    $scope.selectedGroups = true;
                    $scope.tableParams.reload();
                }

            };

            $scope.searchString = "";
            $scope.search = function () {
                $scope.tableParams.filter.searchString = $scope.searchString;
                $scope.tableParams.reload();
            }


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
                                groupId: $scope.selectedGroupId,
                                subjectId: subjectId,
                                secretaryId: $scope.selectedGroupId,
                                searchString: $scope.searchString
                            }}),
                            function (data) {
                                $defer.resolve(data.Students.Items);
                                params.total(data.Students.Total);
                                $scope.percentages = data.PercentageGraphs;
                            });
                    }
                });
        }]);