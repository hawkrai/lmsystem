var testsUnlocks = {
    init: function () {
        $('#unlockAllStudents').bind('click', $.proxy(this._onUnlockAllStudentsClicked, this));
        $('#lockAllStudents').bind('click', $.proxy(this._onLockAllStudentsClicked, this));
        $('#unlockCancelButton').bind('click', $.proxy(this._onCloseClicked, this));
        $('#testNamesDropdown').on('change', $.proxy(this._reloadTestUnlocks, this));
        $('#selectorSearchString').on('keypress', $.proxy(this._reloadTestUnlocks, this));
    },
    
    initResults: function() {
        $('.personaLockButton').bind('click', $.proxy(this._onPersonalLockButtonClicked, this));
    },

    _webServiceUrl: '/Tests/',
    _changeLockMethodName: 'ChangeLockForUserForStudent',
    _unlockTestsMethodName: 'UnlockTests',

    _onCloseClicked: function() {
        datatable.fnDraw();
        $('#testUnlockDetails').modal('hide');
    },

    _onUnlockAllStudentsClicked: function() {
        this._saveUnlocks(true);
    },
    
    _onLockAllStudentsClicked: function () {
        this._saveUnlocks(false);
    },
    
    _saveUnlocks: function(unlock) {
        var studentIds = Enumerable.From($('.studentUnlockItem'))
            .Select(function(hiddenField) {
                return new Number(hiddenField.value);
            }).ToArray();

        var model = {
            studentIds: studentIds,
            testId: this._testId,
            unlock: unlock
        };

        this._sendUnlocks(model);
    },
    
    _sendUnlocks: function (model) {
        $.ajax({
            url: this._webServiceUrl + this._unlockTestsMethodName,
            type: "POST",
            data: JSON.stringify(model),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: $.proxy(this._onPesonalUnlockSaved, this)
        });
    },
    
    _onPersonalLockButtonClicked: function(eventArgs) {
        var studenId = new Number(eventArgs.target.dataset.studentId);
        var unlocked = eventArgs.target.dataset.unlocked == "True";
        var model = {
            testId: this._testId,
            studentId: studenId,
            unlocked: !unlocked
        };

        this._changeLocks(model);
    },
    
    _changeLocks: function (model) {
        $.ajax({
            url: this._webServiceUrl + this._changeLockMethodName,
            type: "POST",
            data: JSON.stringify(model),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: $.proxy(this._onPesonalUnlockSaved, this)
        });
    },
    
    _onPesonalUnlockSaved: function() {
        this._reloadTestUnlocks();
    },

    showDialog: function (id) {
        this._testId = new Number(id);
        $('#testIdOnUnlockForm').val(id);
        this._reloadTestUnlocks();
    },

    _reloadTestUnlocks: function() {
        $('#selectorFilterForm').submit();
    }
};

function questionUnlocksLoaded() {
    testsUnlocks.initResults();
    $('#testUnlockDetails').modal();
}

$(document).ready(function () {
    testsUnlocks.init();
});