angular.module('cpApp.ctrl.getRepo', ['ui.bootstrap', 'xeditable', 'textAngular', 'angularSpinner'])
    .controller('getRepoCtrl',
        function ($scope, $http, $filter) {

            $scope.startSpin = function () {
                $(".loading").toggleClass('ng-hide', false);
               
            };

            $scope.stopSpin = function () {
                $(".loading").toggleClass('ng-hide', true);
                
            };
            $scope.setTitle('Репозиторий курсовых работ');

            $scope.UrlServiceLabs = '/Services/Labs/LabsService.svc/';

            $scope.groupWorkingData = {
                selectedSubGroup: null,
                selectedGroup: null,
                selectedGroupId: 0,
                selectedSubGroupId: 0
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

            var subjectId = getParameterByName("subjectId");

            $scope.userRole = 0;
            $scope.userId = 0;
            $scope.subjectId = 0;
            $scope.init = function (userRole, userId) {
                $scope.subjectId = subjectId;
                $scope.loadExGroups();
                $scope.userRole = userRole;
                $scope.userId = userId;

                bootbox.setDefaults({
                    locale: "ru"
                });

            };

            $scope.resultPlagiatium = [];

            $scope.getLecturesFileAttachments = function () {
                var itemAttachmentsTable = $('#fileupload').find('table').find('tbody tr');
                var data = $.map(itemAttachmentsTable,
                    function (e) {
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


            $scope.loadFilesLabUser = function () {
                $http({
                    method: 'POST',
                    url: $scope.UrlServiceLabs + "GetFilesLab",
                    data: {
                        userId: '1',
                        subjectId: $scope.subjectId,
                        isCoursPrj: true
                    },
                    headers: { 'Content-Type': 'application/json' }
                }).success(function (data, status) {
                    if (data.Code != '200') {
                        alertify.error(data.Message);
                    } else {
                        $scope.labFilesUser = data.UserLabFiles;
                        alertify.success(data.Message);
                    }
                });
            };

            $scope.loadExGroups = function () {
                $.ajax({
                    type: 'GET',
                    url: "/Services/CoreService.svc/GetGroupsV3/" + $scope.subjectId,
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


                //$scope.startSpin();
                //$scope.loadFilesV2();

                //$scope.reloadFiles();


            };

            $scope.reloadFiles = function () {
                $scope.startSpin();
                $scope.loadFilesV2();
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

            $scope.loadLabsV2 = function () {

                $.ajax({
                    type: 'GET',
                    url: $scope.UrlServiceLabs +
                        "GetLabsV2?subjectId=" +
                        $scope.subjectId +
                        "&groupId=" +
                        $scope.groupWorkingData.selectedGroupId,
                    dataType: "json",
                    contentType: "application/json",

                }).success(function (data, status) {
                    if (data.Code != '200') {
                        alertify.error(data.Message);
                    } else {
                        $scope.$apply(function () {
                            $scope.groupWorkingData.selectedGroup.SubGroupsOne.LabsV2 = $filter("filter")(data.Labs,
                                function (r) {
                                    return r.SubGroup === 1;
                                });
                            $scope.groupWorkingData.selectedGroup.SubGroupsTwo.LabsV2 = $filter("filter")(data.Labs,
                                function (r) {
                                    return r.SubGroup === 2;
                                });

                            $scope.groupWorkingData.selectedGroup.SubGroupsThird.LabsV2 = $filter("filter")(data.Labs,
                                function (r) {
                                    return r.SubGroup === 3;
                                });

                            $scope.groupWorkingData.selectedGroup.SubGroupsOne.ScheduleProtectionLabsV2 =
                                $filter("filter")(data.ScheduleProtectionLabs,
                                    function (r) {
                                        return r.SubGroup === 1;
                                    });
                            $scope.groupWorkingData.selectedGroup.SubGroupsTwo.ScheduleProtectionLabsV2 =
                                $filter("filter")(data.ScheduleProtectionLabs,
                                    function (r) {
                                        return r.SubGroup === 2;
                                    });

                            $scope.groupWorkingData.selectedGroup.SubGroupsThird.ScheduleProtectionLabsV2 =
                                $filter("filter")(data.ScheduleProtectionLabs,
                                    function (r) {
                                        return r.SubGroup === 3;
                                    });
                        });
                    }
                });
            };


            $scope.commentImage = function (comment, studentIndex, markIndex, name) {
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

            $scope.NumberControl = function (object, errorName) {
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

            $scope.getCurentDate = function () {
                var dt = new Date();
                var month = dt.getMonth() + 1;
                if (month < 10) month = '0' + month;
                var day = dt.getDate();
                if (day < 10) day = '0' + day;
                var year = dt.getFullYear();
                return day + '.' + month + '.' + year;
            };

            $scope.StrToDate = function (Dat) {
                var year = Number(Dat.split(".")[2])
                var month = Number(Dat.split(".")[1])
                var day = Number(Dat.split(".")[0])
                var dat = new Date(year, month, day)
                return dat
            };

            $scope.saveZip = function () {
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

            $scope.getZip = function (userId) {
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
