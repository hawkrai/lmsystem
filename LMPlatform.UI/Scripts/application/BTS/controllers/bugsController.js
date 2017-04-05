angular
    .module('btsApp.ctrl.bugs', ['ngTable'])
    .controller('bugsCtrl', [
        '$scope',
        'bugsService',
        'PAGE_SIZE',
        'NgTableParams',
        function ($scope, bugsService, PAGE_SIZE, NgTableParams) {

            $scope.inputedSearchString = '';
            var searchString = '';
            $scope.isProjectBugsPage = false;

            $scope.onAddBug = function (projectId) {
                $.savingDialog("Документирование ошибки", "/BTS/AddBug", null, "primary", function (data) {
                    $scope.tableParams.reload();
                    alertify.success("Добавлена новая ошибка");
                });
            };

            function deleteBug(id) {
                bugsService.deleteBug(id).then(function () {
                    $scope.tableParams.reload();
                    alertify.success("Ошибка удалена");
                });
            };

            $scope.onDeleteBug = function (id) {
                bootbox.confirm({
                    title: 'Удаление ошибки',
                    message: 'Вы дествительно хотите удалить ошибку?',
                    buttons: {
                        'cancel': {
                            label: 'Отмена',
                            className: 'btn btn-default btn-sm'
                        },
                        'confirm': {
                            label: 'Удалить',
                            className: 'btn btn-primary btn-sm',
                        }
                    },
                    callback: function (result) {
                        if (result)
                            deleteBug(id);
                    }
                });
            };

            $scope.tableParams = new NgTableParams({
                sorting: { ModifyingDate: "desc" },
                count: PAGE_SIZE
            }, {
                getData: function (params) {
                    return bugsService.getBugs(params.page(), params.count(), searchString, params.orderBy()).then(function (response) {
                        params.total(response.data.TotalCount);
                        bugsService.addNumbering(response.data.Bugs, (params.page() - 1) * params.count());
                        return response.data.Bugs;
                    });
                }
            });
        }]);
