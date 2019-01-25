'use strict';
knowledgeTestingApp.controller('passingCtrl', function($scope, $http) {

    $scope.init = function() {
        testPassing.getTestDescription($scope.onDescriptionLoaded);
    };

    $scope.nextButtonClicked = function() {
        testPassing.init();
    };
});

studentsTestingApp.controller('passingCtrl', function ($scope, $http) {
    
    $scope.init = function () {
        testPassing.getTestDescription($scope.onDescriptionLoaded);
    };

    $scope.nextButtonClicked = function () {
        testPassing.init();
    };
});

var testPassing = {
    init: function() {
        //$('#nextButton').on('click', $.proxy(this._onNextButtonClicked, this));
        this._nextQuestionNumber = 1;
        this._onNextButtonClicked();
    },

    _webServiceUrl: '/TestPassing/',
    _getNextQuestionMethodName: 'GetNextQuestion',
    _makeUserAnswerMethodName: 'AnswerQuestionAndGetNext',
    _getDescriptionMethodName: 'GetTestDescription',

    getTestDescription: function(callback) {
        var testId = getHashValue('testId');

        $.ajax({
            url: this._webServiceUrl + this._getDescriptionMethodName,
            type: "GET",
            data: {
                testId: testId,
            },
            dataType: "json",
            success: $.proxy(this._onDescriptionLoaded, this)
        });
    },

    _onDescriptionLoaded: function(data) {
    	$('#title').text(data.Title);
    	if (data.Description) {
    		$('#description').text(data.Description);
    	}
        
    },

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

    _onReturnButtonClicked: function()
    {
        var returnUrl = decodeURIComponent(getHashValue('return'));
        window.location = returnUrl;
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
        
        if (answers && ((answers.length == 1 && !answers[0].Content) || (answers.length > 1 && Enumerable.From(answers)
            .Where(function(item) {
                return item.IsCorrect != 0;
            }).Count() == 0))) {
            alertify.error("Не выбран ни один вариант ответа");
        } else {
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
        }
    },

    _onUserMadeAnswer: function () {
        var testId = getHashValue('testId');
        this._getNextQuestion(testId);
    },

    _onTestLoaded: function (content) {
        $('#questionContent').html(content);
        $('.progress-bar-clickable').on('click', $.proxy(this._onProgressBarClicked, this));
        $('#answerButton').on('click', $.proxy(this._onAnswerButtonClicked, this));
        if ($('#sortable').sortable) {
	        $('#sortable').sortable({ cursor: "move", containment: "parent" });
        }
       
        $('#buttonsPanel').height($('#answersPanel').height());
        this._nextQuestionNumber = new Number($('#currentQuestionNumber').val());
        this._currentQuestionType = $('#currentQuestionType').val();
        this._initializeTimer();
        if (disableSkipButton == "True") {
            $('#skipButton').attr("disabled", "disabled");
        } else {
            $('#skipButton').on('click', $.proxy(this._onSkipButtonClicked, this));
        }
        var returnUrl = getHashValue('return');
        if (returnUrl)
            $('#returnButton').on('click', $.proxy(this._onReturnButtonClicked, this));
        else
            $('#returnButtonContainer').attr("style", "display:none");
    },
    
    _initializeTimer: function () {
        // Dirty hack with timer id resolves issue
        // with previous timer callback execution
        this.timerId = (new Date).getTime();
        $(".kkcountdown").kkcountdown({
            displayZeroDays: false,
            hoursText: ':',
            textAfterCount: 'Время истекло.',
            callback: $.proxy(this._timeEnded, this, this.timerId)
        });
    },
    
    _timeEnded: function (timerId) {
        if (this.timerId == timerId) {
            alertify.error('Время истекло!');
            this._makeUserAnswer(null, getHashValue('testId'), this._nextQuestionNumber);
        }
    }
};