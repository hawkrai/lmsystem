angular
    .module('cpApp.ctrl.visitStats', ['ngTable', 'ngResource'])
    .controller('visitStatsCtrl', [
        '$scope',
        '$modal',
        'ngTableParams',
        '$resource',
        '$location',
        'projectService',
        function ($scope, $modal, ngTableParams, $resource, $location, projectService) {

            $scope.setTitle("Статистика посещения консультаций");

            $scope.forms = {};
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


            

            var dpConsultations = $resource('api/CourseProjectConsultation');
            var dpConsultationDates = $resource('api/CourseProjectConsultationDate');

            $scope.consultations = [];

            $scope.getConsultationMark = function (student, consultationDateId) {

                var results = student.CourseProjectConsultationMarks;

                for (var i = 0; i < results.length; i++) {
                    if (results[i].ConsultationDateId == consultationDateId) {
                        return results[i];
                    }
                }
                return {
                    ConsultationDateId: consultationDateId,
                    StudentId: student.Id
                };
            };


            $scope.saveConsultationMark = function (consultationMark, newValue) {

                if (newValue && newValue.trim() != '+' && newValue.trim() != '-') return "Введите + или -";

                consultationMark.Mark = newValue;

                dpConsultations.save(consultationMark)
                    .$promise.then(function (data, status, headers, config) {
                        $scope.tableParams.reload();
                        alertify.success('Оценка успешно сохранена');
                    }, $scope.handleError);
                return false;
            };


            $scope.deleteConsultationDate = function (id) {
                bootbox.confirm({
                    title: "Удаление даты консультации",
                    message: "Вы действительно хотите удалить дату консультации?",
                    callback: function (isConfirmed) {
                        if (isConfirmed) {
                            dpConsultationDates.delete({ id: id }).$promise.then(function () {
                                $scope.tableParams.reload();
                                alertify.success("Дата успешно удалена");
                            }, $scope.handleError);
                        }
                    },
                    buttons: {
                        'cancel': {
                            label: 'Отмена',
                            className: 'btn btn-sm'
                        },
                        'confirm': {
                            label: 'Удалить',
                            className: 'btn btn-primary btn-sm',
                        }
                    },
                });
            };

            $scope.addConsultationDate = function () {
                var modalInstance = $modal.open({
                    templateUrl: '/Cp/ConsultationDate',
                    controller: editConsultationDateController,
                    keyboard: false,
                    scope: $scope
                });

                modalInstance.result.then(function (result) {
                    $scope.tableParams.reload();
                });
            };


            var editConsultationDateController = function ($modalInstance) {

                $scope.submitted = false;
                $scope.consultation = {
                    date: $scope.todayIso()
                };
                var subjectId = getParameterByName("subjectId");
                $scope.saveConsultationDate = function () {
                    $scope.submitted = true;
                    if ($scope.forms.consultationDate.date.$error.required) return;
                    var subjectId = getParameterByName("subjectId");
                    var consultationDate = {
                        Day: new Date($scope.consultation.date),
                        SubjectId: subjectId
                    }

                    dpConsultationDates.save(consultationDate)
                    .$promise.then(function () {
                        $scope.tableParams.reload();
                        alertify.success('Дата консультации успешно добавлена');
                    }, $scope.handleError);

                    $modalInstance.close();
                };

                $scope.form = {};
                $scope.datePickerOpen = function ($event) {
                    $event.preventDefault();
                    $event.stopPropagation();

                    $scope.form.datePickerOpened = true;
                };

                $scope.closeDialog = function () {
                    $modalInstance.close();
                };
            };


            $scope.lecturers = [];
            $scope.lecturer = { Id: null };
            $scope.selectLecturer = function (lecturer) {
                if (!lecturer) return;
                $scope.selectedLecturerId = lecturer.Id;
                $scope.tableParams.reload();
            };

            projectService.getDiplomLecturerCorrelation(subjectId)
                .success(function (data) {
                    $scope.lecturers = data;
                    if (data.length == 1) {
                        $scope.selectLecturer(data[0].Id);
                        $scope.lecturer = $scope.lecturers[0];
                    }
                });

            $scope.selectGroups = function (id) {
                if (id) {
                    $scope.selectedGroupId = id;
                    $scope.tableParams.reload();
                }
              
            };

            
            $scope.tableParams = new ngTableParams(
                angular.extend({
                    page: 1,
                    count: 1000
                }, $location.search()), {
                    total: 0,
                    getData: function ($defer, params) {
                        $location.search(params.url());
                        dpConsultations.get(angular.extend(params.url(), {
                            filter:
                            {
                                groupId: $scope.selectedGroupId,
                                subjectId: subjectId,
                                lecturerId: $scope.selectedLecturerId
                            }}),
                            function (data) {
                                $defer.resolve(data.Students.Items);
                                params.total(data.Students.Total);
                                $scope.consultations = data.CourseProjectConsultationDates;
                            });
                    }
                });
        }]);