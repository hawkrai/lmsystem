var knowledgeTestingApp = angular.module('knowledgeTestingApp', ['ngRoute', 'ui.bootstrap', 'ui.bootstrap.tooltip']);

knowledgeTestingApp.config(function ($routeProvider) {

    $routeProvider.when("/tests", {
        controller: "testsCtrl",
        templateUrl: "/Content/KnowledgeTesting/tests.html"
    });
    
    $routeProvider.when("/control", {
        controller: "controlCtrl",
        templateUrl: "/Content/KnowledgeTesting/control.html"
    });
    
    $routeProvider.when("/results", {
        controller: "resultsCtrl",
        templateUrl: "/Content/KnowledgeTesting/results.html"
    });
    
    $routeProvider.when("/passing", {
        controller: "passingCtrl",
        templateUrl: "/Content/KnowledgeTesting/passing.html"
    });
    
    $routeProvider.when("/questions", {
        controller: "questionsCtrl",
        templateUrl: "/Content/KnowledgeTesting/questions.html"
    });

    $routeProvider.otherwise({ redirectTo: "/tests" });

});

var studentsTestingApp = angular.module('studentsTestingApp', ['ngRoute', 'ui.bootstrap', 'ui.bootstrap.tooltip']);

studentsTestingApp.config(function ($routeProvider) {

    $routeProvider.when("/studentTests", {
        controller: "studentTestsCtrl",
        templateUrl: "/Content/KnowledgeTesting/studentTests.html"
    });
    
    $routeProvider.when("/passing", {
        controller: "passingCtrl",
        templateUrl: "/Content/KnowledgeTesting/passing.html"
    });
    
    $routeProvider.when("/results", {
        controller: "studentResultsCtrl",
        templateUrl: "/Content/KnowledgeTesting/studentResults.html"
    });


    $routeProvider.otherwise({ redirectTo: "/studentTests" });
});