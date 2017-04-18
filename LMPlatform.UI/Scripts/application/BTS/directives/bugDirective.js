angular
    .module('btsApp.directive.bug', [])
    .directive('bugDirective', function () {
        return function (scope, element, attrs) {
            element.find('.deleteButton').tooltip({ title: "Удалить ошибку", placement: 'top' });
        };
    });
