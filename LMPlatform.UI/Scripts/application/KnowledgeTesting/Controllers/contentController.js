'use strict';
knowledgeTestingApp.controller('contentCtrl', function ($scope, $http, $modalInstance) {

    $scope.closeDialog = function () {
        $modalInstance.close();
    };

    $scope.contents = [];

    $scope.uploadContent = function () {
        $('.upload-test-content .fileupload').trigger('click');
    };

    $scope.loadFilesLabUser = function () {
        $.ajax({
            url: '/Tests/GetFiles',
            type: "GET",
            dataType: "json",
            success: function (data) {
                $scope.$apply(function() {
                    $scope.contents = data;
                });
            }
        });
    };

    $scope.fileNameChanged = function () {
            var data = $.find('input[type=file][name=uploadContent]')[0].files[0];
            var formData = new FormData();
            formData.append("file", data);
            $.ajax({
                url: '/Tests/UploadFile',
                type: 'POST',
                data: formData,
                processData: false,
                contentType: false,
                success: function () {
                    $($.find('input[type=file][name=uploadContent]')[0]).val('');
                    $scope.loadFilesLabUser();
                },
                complete: function () {
                }
        })
        .error(function (data, status, headers, config) {
            alertify.error('Во время сохранения произошла ошибка');
        });
    };

    $scope.loadFilesLabUser();
});