'use strict';
knowledgeTestingApp.controller('passingCtrl', function($scope, $http) {

    $scope.init = function() {
        testPassing.init();
    };
});

var testPassing = {
    init: function () {
        //$('#nextButton').on('click', $.proxy(this._onNextButtonClicked, this));
        this._nextQuestionNumber = 1;
        this._onNextButtonClicked();
    },

    _webServiceUrl: '/TestPassing/',
    _getNextQuestionMethodName: 'GetNextQuestion',
    _makeUserAnswerMethodName: 'AnswerQuestionAndGetNext',

    _onNextButtonClicked: function () {
        var testId = getHashValue('testId');
        this._getNextQuestion(testId);
    },

    _onProgressBarClicked: function (eventArgs) {
        this._nextQuestionNumber = new Number(eventArgs.target.textContent);
        var testId = getHashValue('testId');
        this._getNextQuestion(testId);
    },

    _onAnswerButtonClicked: function () {
        var userAnswers;
        if (this._currentQuestionType == 'SequenceAnswer') {
            var i = 0;
            userAnswers = Enumerable.From($('#sortable').sortable("toArray"))
                            .Select(function (answerId) {
                                return {
                                    Id: answerId,
                                    IsCorrect: i++
                                };
                            }).ToArray();

        } else {
            userAnswers = Enumerable.From($('.answerContainer'))
                .Select(function (answerContainer) {
                    return {
                        Id: $('.answerId', answerContainer).val(),
                        IsCorrect: new Number($('.correctnessIndicator', answerContainer).is(':checked')),
                        Content: $('.answerText', answerContainer).val()
                    };
                }).ToArray();
        }

        this._makeUserAnswer(userAnswers, getHashValue('testId'), this._nextQuestionNumber);
    },

    _onSkipButtonClicked: function () {
        this._nextQuestionNumber++;
        var testId = getHashValue('testId');
        this._getNextQuestion(testId);
    },

    _getNextQuestion: function (testId) {
        $.ajax({
            url: this._webServiceUrl + this._getNextQuestionMethodName,
            type: "GET",
            data: {
                testId: testId,
                questionNumber: this._nextQuestionNumber
            },
            dataType: "html",
            success: $.proxy(this._onTestLoaded, this)
        });
    },

    _makeUserAnswer: function (answers, testId, currentQuestionNumber) {
        $.ajax({
            url: this._webServiceUrl + this._makeUserAnswerMethodName,
            type: "POST",
            data: JSON.stringify({
                answers: answers,
                testId: testId,
                questionNumber: currentQuestionNumber
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: $.proxy(this._onUserMadeAnswer, this)
        });
    },

    _onUserMadeAnswer: function () {
        var testId = getHashValue('testId');
        this._getNextQuestion(testId);
    },

    _onTestLoaded: function (content) {
        $('#questionContent').html(content);
        $('.progress-bar-notPassed').on('click', $.proxy(this._onProgressBarClicked, this));
        $('#answerButton').on('click', $.proxy(this._onAnswerButtonClicked, this));
        $('#skipButton').on('click', $.proxy(this._onSkipButtonClicked, this));
        $('#sortable').sortable({ cursor: "move", containment: "parent" });
        $('#buttonsPanel').height($('#answersPanel').height());
        this._nextQuestionNumber = new Number($('#currentQuestionNumber').val());
        this._currentQuestionType = $('#currentQuestionType').val();
    },
};