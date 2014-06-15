'use strict';
knowledgeTestingApp.controller('passingCtrl', function($scope, $http) {

    $scope.init = function() {
        testPassing.init();
    };
    //$scope.nextQuestionNumber = 1;

    //$scope.webServiceUrl = '/TestPassing/';
    //$scope.getNextQuestionMethodName = 'GetNextQuestion';
    //$scope.makeUserAnswerMethodName = 'AnswerQuestionAndGetNext';

    //$scope.onNextButtonClicked = function() {
    //    var testId = getHashValue('testId');
    //    $scope.getNextQuestion(testId);
    //};
    //$scope.onProgressBarClicked = function(eventArgs) {
    //    $scope.nextQuestionNumber = new Number(eventArgs.target.textContent);
    //    var testId = getHashValue('testId');
    //    $scope.getNextQuestion(testId);
    //};
    //$scope.onAnswerButtonClicked = function() {
    //    var userAnswers;
    //    if ($scope.currentQuestionType == 'SequenceAnswer') {
    //        var i = 0;
    //        userAnswers = Enumerable.From($('#sortable').sortable("toArray"))
    //            .Select(function(answerId) {
    //                return {
    //                    Id: answerId,
    //                    IsCorrect: i++
    //                };
    //            }).ToArray();

    //    } else {
    //        userAnswers = Enumerable.From($('.answerContainer'))
    //            .Select(function(answerContainer) {
    //                return {
    //                    Id: $('.answerId', answerContainer).val(),
    //                    IsCorrect: new Number($('.correctnessIndicator', answerContainer).is(':checked')),
    //                    Content: $('.answerText', answerContainer).val()
    //                };
    //            }).ToArray();
    //    }

    //    this._makeUserAnswer(userAnswers, getUrlValue('testId'), $scope.nextQuestionNumber);
    //};
    //$scope.onSkipButtonClicked = function() {
    //    $scope.nextQuestionNumber++;
    //    var testId = getHashValue('testId');
    //    $scope.getNextQuestion(testId);
    //};
    //$scope.getNextQuestion = function(testId) {
    //    $http({
    //        method: "GET",
    //        url: kt.actions.tests.getNextQuestion,
    //        dataType: "html",
    //        params: {
    //            testId: 1,
    //            questionNumber: $scope.nextQuestionNumber
    //        }

    //    }).success(function(data) {
    //        //$scope.onTestLoaded();
    //    }).error(function(data) {
    //        //$scope.onTestLoaded();
    //    });
    //};

    //$scope.makeUserAnswer = function(answers, testId, currentQuestionNumber) {
    //    $.ajax({
    //        url: getNextQuestion,
    //        type: "POST",
    //        data: JSON.stringify({
    //            answers: answers,
    //            testId: testId,
    //            questionNumber: currentQuestionNumber
    //        }),
    //        dataType: "json",
    //        contentType: "application/json; charset=utf-8",
    //        success: $.proxy($scope.onUserMadeAnswer, this)
    //    });
    //};

    //$scope.onUserMadeAnswer = function() {
    //    var testId = getHashValue('testId');
    //    $scope.getNextQuestion(testId);
    //};

    //$scope.onTestLoaded = function(content) {
    //    $('#questionContent').html(content);
    //    $(".kkcountdown").kkcountdown();
    //    $('.progress-bar-notPassed').on('click', $.proxy($scope.onProgressBarClicked, this));
    //    $('#answerButton').on('click', $.proxy($scope.onAnswerButtonClicked, this));
    //    $('#skipButton').on('click', $.proxy($scope.onSkipButtonClicked, this));
    //    $('#sortable').sortable({ cursor: "move", containment: "parent" });
    //    $('#buttonsPanel').height($('#answersPanel').height());
    //    $scope.nextQuestionNumber = new Number($('#currentQuestionNumber').val());
    //    $scope.currentQuestionType = $('#currentQuestionType').val();
    //};
});