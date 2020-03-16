angular.module('cpApp.ctrl.upfile', ['ui.bootstrap', 'xeditable', 'textAngular', 'angularSpinner'])
    .controller('uploadFileCtrl',
    function ($scope, $http, $filter, usSpinnerService) {

            $scope.setTitle('Защита курсового проекта (работы)');

            $scope.labs = [];

            $scope.IsRet = false;
            $scope.file = null;
            $scope.fileId = 0;
            $scope.studentId = 0;


            $scope.groupWorkingData = {
                selectedSubGroup: null,
                selectedGroup: null,
                selectedGroupId: 0,
                selectedSubGroupId: 0
            };
            $scope.UrlServiceLabs = '/Services/Labs/LabsService.svc/';

            $scope.startSpin = function() {
                $(".loading").toggleClass('ng-hide', false);
                usSpinnerService.spin('spinner-1');
            };

            $scope.stopSpin = function() {
                $(".loading").toggleClass('ng-hide', true);
                usSpinnerService.stop('spinner-1');
            };
            $scope.saveLabFiles = function(id) {

                if ($scope.user.IsStudent &&
                ($scope.editFileSend.Comments == null ||
                    $scope.editFileSend.Comments.length == 0 ||
                    JSON.parse($scope.getLecturesFileAttachments()).length == 0)) {
                    bootbox.alert("Необходимо заполнить поля и прикрепить файлы.");
                    return false;
                }
                if ($scope.user.IsLecturer && JSON.parse($scope.getLecturesFileAttachments()).length == 0) {
                    bootbox.alert("Необходимо прикрепить файлы.");
                    return false;
                }

                $http({
                    method: 'POST',
                    url: $scope.UrlServiceLabs + "SendFile",
                    data: {
                        subjectId: $scope.subjectId,
                        userId: id,
                        id: $scope.editFileSend.Id,
                        comments: $scope.editFileSend.Comments,
                        pathFile: $scope.editFileSend.PathFile,
                        attachments: $scope.getLecturesFileAttachments(),
                        isCp: true,
                        isRet: $scope.IsRet
                    },
                    headers: { 'Content-Type': 'application/json' }
                }).success(function(data, status) {
                    if (data.Code != '200') {
                        alertify.error(data.Message);
                        $scope.IsRet = false;
                    } else {
                        $scope.loadFilesLabUser();
                        alertify.success(data.Message);
                    }
                    $("#dialogAddFiles").modal('hide');
                });
            };
            $scope.deleteUserFile = function(file) {
                bootbox.dialog({
                    message: "Вы действительно хотите удалить работу?",
                    title: "Удаление работы",
                    buttons: {
                        danger: {
                            label: "Отмена",
                            className: "btn-default btn-sm",
                            callback: function() {

                            }
                        },
                        success: {
                            label: "Удалить",
                            className: "btn-primary btn-sm",
                            callback: function() {
                                $http({
                                    method: 'POST',
                                    url: $scope.UrlServiceLabs + "DeleteUserFile",
                                    data: { id: file.Id },
                                    headers: { 'Content-Type': 'application/json' }
                                }).success(function(data, status) {
                                    if (data.Code != '200') {
                                        alertify.error(data.Message);
                                    } else {
                                        alertify.success(data.Message);
                                        $scope.labFilesUser.splice($scope.labFilesUser.indexOf(file), 1);
                                    }
                                });
                            }
                        }
                    }
                });
            };

            //$scope.selectedGroupChange = function(groupId, subGroupId) {
            //    if (groupId != null) {
            //        $scope.groupWorkingData.selectedGroupId = groupId;
            //        $scope.groupWorkingData.selectedSubGroupId = 0;
            //    }
            //    if (subGroupId != null) {
            //        $scope.groupWorkingData.selectedSubGroupId = subGroupId;
            //    }

            //    $scope.groupWorkingData.selectedGroup = null;
            //    $.each($scope.groups,
            //        function(index, value) {
            //            if (value.GroupId == $scope.groupWorkingData.selectedGroupId) {
            //                $scope.groupWorkingData.selectedGroup = value;
            //                return;
            //            }
            //        });

            //    if ($scope.groupWorkingData.selectedSubGroupId == 0) {
            //        if ($scope.groupWorkingData.selectedGroup.SubGroupsOne != null) {
            //            $scope.groupWorkingData.selectedSubGroupId =
            //                $scope.groupWorkingData.selectedGroup.SubGroupsOne.SubGroupId;
            //        } else if ($scope.groupWorkingData.selectedGroup.SubGroupsTwo != null) {
            //            $scope.groupWorkingData.selectedSubGroupId =
            //                $scope.groupWorkingData.selectedGroup.SubGroupsTwo.SubGroupId;
            //        }
            //    }

            //    if ($scope.groupWorkingData.selectedGroup.SubGroupsOne != null &&
            //        $scope.groupWorkingData.selectedGroup.SubGroupsTwo != null) {
            //        $scope.subGroups = [
            //            $scope.groupWorkingData.selectedGroup.SubGroupsOne,
            //            $scope.groupWorkingData.selectedGroup.SubGroupsTwo
            //        ];
            //    } else {
            //        $scope.subGroups = [];
            //    }

            //    $scope.groupWorkingData.selectedSubGroup = null;
            //    if ($scope.groupWorkingData.selectedSubGroupId != 0)
            //        if ($scope.subGroups[0] != null &&
            //            $scope.groupWorkingData.selectedSubGroupId == $scope.subGroups[0].SubGroupId) {
            //            $scope.groupWorkingData.selectedSubGroup = $scope.subGroups[0];
            //        } else if ($scope.subGroups[1] != null &&
            //            $scope.groupWorkingData.selectedSubGroupId == $scope.subGroups[1].SubGroupId) {
            //            $scope.groupWorkingData.selectedSubGroup = $scope.subGroups[1];
            //        }
            //};

            $scope.loadGroups = function() {
                $.ajax({
                    type: 'GET',
                    url: "/Services/CoreService.svc/GetGroupsV2/" + $scope.subjectId,
                    dataType: "json",
                    contentType: "application/json",

                }).success(function (data, status) {
                    if (data.Code != '200') {
                        alertify.error(data.Message);
                    } else {
                        $scope.$apply(function () {
                            $scope.groups = data.Groups;

                            if ($scope.groupWorkingData.selectedGroupId == 0) {
                                $scope.loadFilesV2($scope.groups[0]);
                            }

                        });
                    }
                });
            };

            $scope.editFileSend = {
                Comments: "",
                PathFile: "",
                Id: 0
            };

            $scope.labFilesUser = [];


            $scope.groups = [];
            $scope.subGroups = [];

            function getParameterByName(name) {
                name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
                var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
                    results = regex.exec(location.search);
                return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
            }

            $scope.isReturned = function(element) {
                return !element.IsReturned;
            }

            var subjectId = getParameterByName("subjectId");

            $scope.userRole = 0;
            $scope.userId = 0;
            $scope.subjectId = 0;
            $scope.init = function(userRole, userId) {
                $scope.subjectId = subjectId;
                $scope.loadGroups();
                $scope.userRole = userRole;
                $scope.userId = userId;
                
                bootbox.setDefaults({
                    locale: "ru"
                });
                //$scope.labs = [];
                //$scope.loadLabs();
                if ($scope.userRole == "1") {
                    $scope.loadFilesLabUser();
                };

                //if ($scope.groups.length > 0) {
                //    $scope.reload();
                //}
            };

            $scope.resultPlagiatium = [];

            $scope.getLecturesFileAttachments = function() {
                var itemAttachmentsTable = $('#fileupload').find('table').find('tbody tr');
                var data = $.map(itemAttachmentsTable,
                    function(e) {
                        var newObject = null;
                        if (e.className === "template-download fade in") {
                            if (e.id === "-1") {
                                newObject = {
                                    Id: 0,
                                    Title: "",
                                    Name: $(e).find('td a').text(),
                                    AttachmentType: $(e).find('td.type').text(),
                                    FileName: $(e).find('td.guid').text()
                                };
                            } else {
                                newObject = {
                                    Id: e.id,
                                    Title: "",
                                    Name: $(e).find('td a').text(),
                                    AttachmentType: $(e).find('td.type').text(),
                                    FileName: $(e).find('td.guid').text()
                                };
                            }
                        }
                        return newObject;
                    });
                var dataAsString = JSON.stringify(data);
                return dataAsString;
            };

            $scope.checkPlagiarism = function(id, files, userName) {
                $scope.userFileIdCheck = id;
                $scope.plagiat = {
                    FileName: files.Attachments[0].Name,
                    UserName: userName
                };

                $('#dialogPlagiarism').modal();
                $("#table_plagiarism_singleDoc").toggleClass('ng-hide', true);
                $("#exportButton").toggleClass('ng-hide', true);

                $(".loadingP").toggleClass('ng-hide', false);
                usSpinnerService.spin('spinner-1');
                $http({
                    method: 'POST',
                    url: $scope.UrlServiceLabs + "CheckPlagiarism",
                    data: {
                        userFileId: id,
                        subjectId: $scope.subjectId,
                        isCp: true,
                    },
                    headers: { 'Content-Type': 'application/json' }
                }).success(function(data, status) {
                    if (data.Code != '200') {
                        alertify.error(data.Message);
                    } else {
                        alertify.success(data.Message);

                        $scope.resultPlagiatium = data.DataD;

                        $("#table_plagiarism_singleDoc").toggleClass('ng-hide', false);

                    }
                    usSpinnerService.stop('spinner-1');
                    $(".loadingP").toggleClass('ng-hide', true);
                    $("#exportButton").toggleClass('ng-hide', false);

                });
            };

            $scope.checkPlagiarismSubject = function() {
                $scope.resultPlagiatiumSybject = [];
                $('#dialogPlagiarismSubject').modal();
                $("#exportButton").toggleClass('ng-hide', true);
            };

            $scope.loadPlagiarismSubject = function () {
                $('.number-spinner').find('button').prop("disabled", true);
                $(".loadingPSubject").toggleClass('ng-hide', false);
                usSpinnerService.spin('spinner-1');
                $scope.resultPlagiatiumSybject = [];
                $http({
                    method: 'POST',
                    url: $scope.UrlServiceLabs + "CheckPlagiarismSubjects",
                    data: {
                        subjectId: $scope.subjectId,
                        type: $("input[name=typePlagiarism]:checked").val(),
                        threshold: $("#threshold").val(),
                        isCp: true
                    },
                    headers: { 'Content-Type': 'application/json' }
                }).success(function(data, status) {
                    if (data.Code != '200') {
                        alertify.error(data.Message);
                    } else {
                        alertify.success(data.Message);

                        $scope.resultPlagiatiumSybject = data.DataD;

                    }
                    usSpinnerService.stop('spinner-1');
                    $(".loadingPSubject").toggleClass('ng-hide', true);
                    $('.number-spinner').find('button').prop("disabled", false);
                    $("#exportButton").toggleClass('ng-hide', false);
                });
            };

            $scope.exportPlagiarism = function() {
                    window.location.href = "/Statistic/ExportPlagiarism?subjectId=" +
                        $scope.subjectId +
                        "&isCp=true";
                },
            $scope.exportPlagiarismStudent = function() {
                    window.location.href = "/Statistic/ExportPlagiarismStudent?userFileId=" +
                        $scope.userFileIdCheck +
                        "&subjectId=" +
                        $scope.subjectId + 
                        "&isCp=true";
                },
            $scope.receivedLabFile = function(id, files) {
                    $http({
                        method: 'POST',
                        url: $scope.UrlServiceLabs + "ReceivedLabFile",
                        data: {
                            userFileId: id
                        },
                        headers: { 'Content-Type': 'application/json' }
                    }).success(function(data, status) {
                        if (data.Code != '200') {
                            alertify.error(data.Message);
                        } else {
                            //$scope.$apply(function () {
                            //$scope.reloadFiles();
                            files.IsReceived = true;
                            files.IsReturned = false;
                            //});
                            alertify.success(data.Message);
                        }
                    });
                };

            $scope.cancelReceivedLabFile = function(id, files) {
                $http({
                    method: 'POST',
                    url: $scope.UrlServiceLabs + "CancelReceivedLabFile",
                    data: {
                        userFileId: id
                    },
                    headers: { 'Content-Type': 'application/json' }
                }).success(function(data, status) {
                    if (data.Code != '200') {
                        alertify.error(data.Message);
                    } else {
                        //$scope.$apply(function () {
                        //$scope.reloadFiles();
                        files.IsReceived = false;

                        //});
                        alertify.success(data.Message);
                    }
                });
            };

            $scope.reload = function() {
                $scope.startSpin();
                // performance issue
                $scope.loadLabsV2();
                $scope.loadFilesV2();
            };

            $scope.$on('groupLoaded',
                function(event, arg) {
                    $scope.reload();
                });

            $scope.loadFilesLabUser = function() {
                $http({
                    method: 'POST',
                    url: $scope.UrlServiceLabs + "GetFilesLab",
                    data: {
                        userId: $scope.userId,
                        subjectId: $scope.subjectId,
                        isCoursPrj: true
                    },
                    headers: { 'Content-Type': 'application/json' }
                }).success(function(data, status) {
                    if (data.Code != '200') {
                        alertify.error(data.Message);
                    } else {
                        $scope.labFilesUser = data.UserLabFiles;
                        alertify.success(data.Message);
                    }
                });
            };

            $scope.replaceLabFiles = function(userId) {
                $http({
                    method: 'POST',
                    url: $scope.UrlServiceLabs + "DeleteUserFile",
                    data: { id: $scope.fileId },
                    headers: { 'Content-Type': 'application/json' }
                }).success(function(data, status) {
                    if (data.Code != '200') {
                        alertify.error(data.Message);
                    } else {
                        alertify.success("Работа возвращена");
                        $scope.labFilesUser.splice($scope.labFilesUser.indexOf($scope.file), 1);
                        $scope.IsRet = true;
                        $scope.file.IsReturned = true;
                        $scope.saveLabFiles(userId);

                    }
                });
            };

            $scope.returnFile = function(id, file, studentId) {
                $scope.fileId = id;
                $scope.file = file;
                $scope.studentId = studentId;
                $scope.addLabFiles();
            };

            $scope.addLabFiles = function() {
                window.maxNumberOfFiles = 1;
                $scope.editFileSend.Comments = "";
                $scope.editFileSend.PathFile = "";
                $scope.editFileSend.Id = "0";


                $("#labsFile").empty();

                $.ajax({
                    type: 'GET',
                    url: "/Subject/GetUserFilesLab?id=0",
                    contentType: "application/json",

                }).success(function(data, status) {
                    $scope.$apply(function() {
                        $("#labsFile").append(data);
                    });
                });

                $('#dialogAddFiles').modal();


            };

            $scope.editLabFiles = function(file) {
                $scope.editFileSend.Comments = file.Comments;
                $scope.editFileSend.PathFile = file.PathFile;
                $scope.editFileSend.Id = file.Id;

                $("#labsFile").empty();

                $.ajax({
                    type: 'GET',
                    url: "/Subject/GetUserFilesLab?id=" + file.Id,
                    contentType: "application/json",

                }).success(function(data, status) {
                    $scope.$apply(function() {
                        $("#labsFile").append(data);
                    });
                });
                $('#dialogAddFiles').modal();
            };


            $scope.loadLabs = function() {
                $.ajax({
                    type: 'GET',
                    url: $scope.UrlServiceLabs + "GetLabs/" + $scope.subjectId,
                    dataType: "json",
                    contentType: "application/json",

                }).success(function(data, status) {
                    if (data.Code != '200') {
                        alertify.error(data.Message);
                    } else {
                        $scope.$apply(function() {
                            $scope.labs = data.Labs;
                        });
                    }
                });
            };


            $scope.changeGroups = function (selectedGroup) {

                $scope.groupWorkingData.selectedGroupId = selectedGroup.GroupId;
                $scope.startSpin();

                $scope.groupWorkingData.selectedGroup = null;
                $.each($scope.groups,
                    function (index, value) {
                        if (value.GroupId == $scope.groupWorkingData.selectedGroupId) {
                            $scope.groupWorkingData.selectedGroup = value;
                            return;
                        }
                    });

             };

            $scope.reloadFiles = function(selectedGroup) {
                $scope.startSpin();
                $scope.loadFilesV2(selectedGroup);
            };

            $scope.loadFilesV2 = function (groupId) {

                $.ajax({
                    type: 'GET',
                    url: $scope.UrlServiceLabs +
                        "GetFilesV2?subjectId=" +
                        $scope.subjectId +
                        "&groupId=" +
                        groupId.GroupId +
                        "&isCp=true",
                    dataType: "json",
                    contentType: "application/json",

                }).success(function (data, status) {
                    if (data.Code != '200') {
                        alertify.error(data.Message);
                    } else {
                        $scope.$apply(function () {
                            $scope.groupWorkingData.selectedGroup.StudentsFiles = data.Students;
                        });

                    }
                    $scope.stopSpin();
                });
                $scope.changeGroups(groupId);


            };

            $scope.loadLabsV2 = function() {

                $.ajax({
                    type: 'GET',
                    url: $scope.UrlServiceLabs +
                        "GetLabsV2?subjectId=" +
                        $scope.subjectId +
                        "&groupId=" +
                        $scope.groupWorkingData.selectedGroupId,
                    dataType: "json",
                    contentType: "application/json",

                }).success(function(data, status) {
                    if (data.Code != '200') {
                        alertify.error(data.Message);
                    } else {
                        $scope.$apply(function() {
                            $scope.groupWorkingData.selectedGroup.SubGroupsOne.LabsV2 = $filter("filter")(data.Labs,
                                function(r) {
                                    return r.SubGroup === 1;
                                });
                            $scope.groupWorkingData.selectedGroup.SubGroupsTwo.LabsV2 = $filter("filter")(data.Labs,
                                function(r) {
                                    return r.SubGroup === 2;
                                });

                            $scope.groupWorkingData.selectedGroup.SubGroupsThird.LabsV2 = $filter("filter")(data.Labs,
                                function(r) {
                                    return r.SubGroup === 3;
                                });

                            $scope.groupWorkingData.selectedGroup.SubGroupsOne.ScheduleProtectionLabsV2 =
                                $filter("filter")(data.ScheduleProtectionLabs,
                                    function(r) {
                                        return r.SubGroup === 1;
                                    });
                            $scope.groupWorkingData.selectedGroup.SubGroupsTwo.ScheduleProtectionLabsV2 =
                                $filter("filter")(data.ScheduleProtectionLabs,
                                    function(r) {
                                        return r.SubGroup === 2;
                                    });

                            $scope.groupWorkingData.selectedGroup.SubGroupsThird.ScheduleProtectionLabsV2 =
                                $filter("filter")(data.ScheduleProtectionLabs,
                                    function(r) {
                                        return r.SubGroup === 3;
                                    });
                        });
                    }
                });
            };

            $scope.commentImage = function(comment, studentIndex, markIndex, name) {
                var id = name + studentIndex + markIndex;
                var elem = document.getElementById(id);

                if (elem !== null) {
                    if (comment != "" & comment != null) {
                        elem.style.display = 'block';
                        elem.title = comment;
                    } else {
                        elem.style.display = 'none';
                        elem.title = null;
                    }
                }
            };

            $scope.NumberControl = function(object, errorName) {
                var error = document.getElementById(errorName);
                if (object.value === "") {
                    object.value = "";
                    error.style.display = 'none';
                } else {
                    if (parseInt(object.value) < object.min) {
                        object.value = object.min;
                        error.style.display = 'block';
                    } else {
                        if (parseInt(object.value) > object.max) {
                            object.value = object.max;
                            error.style.display = 'block';
                        } else {
                            error.style.display = 'none';
                        }
                    }
                }
            };

            $scope.getCurentDate = function() {
                var dt = new Date();
                var month = dt.getMonth() + 1;
                if (month < 10) month = '0' + month;
                var day = dt.getDate();
                if (day < 10) day = '0' + day;
                var year = dt.getFullYear();
                return day + '.' + month + '.' + year;
            };

            $scope.StrToDate = function(Dat) {
                var year = Number(Dat.split(".")[2])
                var month = Number(Dat.split(".")[1])
                var day = Number(Dat.split(".")[0])
                var dat = new Date(year, month, day)
                return dat
            };

            $scope.saveZip = function() {
                document.location.href = "/Subject/GetZipLabs?id=" +
                    $scope.groupWorkingData.selectedGroup.GroupId +
                    "&subjectId=" +
                    $scope.subjectId;
                //$.ajax({
                //	type: 'GET',
                //	url: "/Subject/GetZipLabs?id=" + subGroupId + "&subjectId=" + $scope.subjectId,
                //	contentType: "application/zip",
                //}).success(function (data, status) {
                //});
            };

            $scope.getZip = function(userId) {
                var subGroupId = $scope.groupWorkingData.selectedSubGroup.SubGroupId;
                document.location.href = "/Subject/GetStudentZipLabs?id=" +
                    subGroupId +
                    "&subjectId=" +
                    $scope.subjectId +
                    "&userId=" +
                    userId;
                //$.ajax({
                //	type: 'GET',
                //	url: "/Subject/GetZipLabs?id=" + subGroupId + "&subjectId=" + $scope.subjectId,
                //	contentType: "application/zip",
                //}).success(function (data, status) {
                //});
            };

        });
