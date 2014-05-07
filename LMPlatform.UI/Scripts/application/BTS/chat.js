$(document).on('click', '.panel-heading span.clickable', function (e) {
    var $this = $(this);
    if (!$this.hasClass('panel-collapsed')) {
        $this.parents('.panel').find('.panel-body').slideUp();
        $this.addClass('panel-collapsed');
        $this.find('i').removeClass('glyphicon-minus').addClass('glyphicon-plus');
        $(".panel-footer").slideUp();
    } else {
        $this.parents('.panel').find('.panel-body').slideDown();
        $this.removeClass('panel-collapsed');
        $this.find('i').removeClass('glyphicon-plus').addClass('glyphicon-minus');
        $(".panel-footer").slideDown();
    }
});
$(document).on('click', '.panel div.clickable', function (e) {
    var $this = $(this);
    if (!$this.hasClass('panel-collapsed')) {
        $this.parents('.panel').find('.panel-body').slideUp();
        $this.addClass('panel-collapsed');
        $this.find('i').removeClass('glyphicon-minus').addClass('glyphicon-plus');
        $(".panel-footer").slideUp();
    } else {
        $this.parents('.panel').find('.panel-body').slideDown();
        $this.removeClass('panel-collapsed');
        $this.find('i').removeClass('glyphicon-plus').addClass('glyphicon-minus');
        $(".panel-footer").slideDown();
    }
});

var chatFunction = {

    _webServiceUrl: '/BTS/',
    _getMethodName: 'ProjectManagement',

    init: function () {
        var that = this;
        that.initButtonAction();
    },

    
    initButtonAction: function () {
        $('#chat-btn').on('click', $.proxy(this._onMessageClicked, this));
    },

    _onMessageClicked: function (eventArgs) {
        var commentText = $('#CommentText').val();
        $("#CommentText").val('');
        this._sendMessage(commentText);
    },

    _sendMessage: function (commentText) {
        $.ajax({
            url: this._webServiceUrl + this._getMethodName,
            type: "POST",
            data: {
                comment: commentText
            },
            dataType: "html",
            success: $.proxy(this._onMessageLoaded, this)
        });
    },

    _onMessageLoaded: function (content) {
        var chat = $('#chat');
        chat.empty();
        chat.append(content);
        location.reload();
    }
};

$(document).ready(function () {
    //$('.panel-heading span.clickable').click();
    //$('.panel div.clickable').click();
    chatFunction.init();
});