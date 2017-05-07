angular
    .module('btsApp.directive.projectUser', [])
    .directive('projectUserDirective', function () {
        return function (scope, element, attrs) {
            element.find('.deleteButton').tooltip({ title: "Удалить участника проекта", placement: 'right' });
        };
    });
