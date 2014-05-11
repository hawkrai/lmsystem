var testsResultsList = {
    init: function () {
        $('.groupButton').on('click', $.proxy(this._onGroupClicked, this));
    },
    
    _webServiceUrl: '/TestPassing/',
    _getResultsForGroupMethodName: 'GetTestResultsForGroup',
    

    _onGroupClicked: function(eventArgs) {
        var groupId = $(eventArgs.target).children().first().val();
        this._getResultsForGroup(groupId);
    },
    
    _getResultsForGroup: function (groupId) {
        $.ajax({
            url: this._webServiceUrl + this._getResultsForGroupMethodName,
            type: "GET",
            data: {
                groupId: groupId
            },
            dataType: "html",
            success: $.proxy(this._onResultsTableLoaded, this)
        });
    },
    
    _onResultsTableLoaded: function(content) {
        $('#testReultsTable').html(content);
    }
};

function initTestResultsList() {
}

$(document).ready(function () {
    testsResultsList.init();
});
