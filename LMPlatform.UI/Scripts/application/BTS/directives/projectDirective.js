angular
    .module('btsApp.directive.project', [])
    .directive('projectDirective', function () {
        return function (scope, element, attrs) {
            element.find('.editProject').tooltip({ title: "Редактировать проект", placement: 'left' });
            element.find('.deleteButton').tooltip({ title: "Удалить проект", placement: 'top' });
        };
    });
