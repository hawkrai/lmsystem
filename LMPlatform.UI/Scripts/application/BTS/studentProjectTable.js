var studentProjectsList = {
    init: function () {
        $('.groupButton').on('click', $.proxy(this._onGroupClicked, this));
    },

    _webServiceUrl: '/BTS/',
    _getStudentProjectsTableMethodName: 'ProjectParticipation',


    _onGroupClicked: function (eventArgs) {
        var groupId = $(eventArgs.target).children().first().val();
        this._getStudentProjectsTable(groupId);
    },

    _getStudentProjectsTable: function (groupId) {
        $.ajax({
            url: this._webServiceUrl + this._getStudentProjectsTableMethodName,
            type: "POST",
            data: {
                groupId: groupId
            },
            dataType: "html",
            success: $.proxy(this._onResultsTableLoaded, this)
        });
    },

    _onResultsTableLoaded: function (content) {
        $('#studentProjectsTable').html(content);
    }
};

function initStudentProjectsList() {
    studentProjectsList.init();
}

$(document).ready(function () {
    studentProjectsList.init();
});