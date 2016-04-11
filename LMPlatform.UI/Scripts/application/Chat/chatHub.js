
$(function () {
    $('#divChat1').draggable({

        handle: ".header",
        stop: function () {

        }
    });
    setScreen(false);
    // Declare a proxy to reference the hub. 
    var chatHub = $.connection.chatHub;

    registerClientMethods(chatHub);

    // Start Hub
    $.connection.hub.start().done(function () {
        registerEvents(chatHub);
        $("#btnStartChat").trigger("click");
    });
});

function setScreen(isLogin) {
    if (!isLogin) {
        $("#divChat").hide();
        $("#divLogin").hide();
    }
     else {
        $("#divChat").show();
        $("#divLogin").hide();
    }

}

function registerEvents(chatHub) {

    $("#btnStartChat").click(function (e) {
        e.preventDefault(e);
        var name = $("#txtNickName").val();
        var text = document.getElementById('role');
        var role = text.value;
        if (name.length > 0) {
            //chatHub.server.disconnect();
            chatHub.server.connect(name, role);
        } else {
            alert("Please enter name");
        }
    });

    $('#btnSendMsg').click(function () {

        var msg = $("#txtMessage").val();
        if (msg.length > 0) {

            var userName = $('#hdUserName').val();
            var text = document.getElementById('role');
            var role = text.value;

            chatHub.server.sendMessageToAll(userName, msg, role);
            $("#txtMessage").val('');
        }
    });


    $("#txtNickName").keypress(function (e) {
        if (e.which === 13) {
            $("#btnStartChat").click();
        }
    });

    $("#txtMessage").keypress(function (e) {
        if (e.which == 13) {
            $('#btnSendMsg').click();
        }
    });


}

function registerClientMethods(chatHub) {

    // Calls when user successfully logged in
    var text = document.getElementById('role');
    var userRole = text.value;
    chatHub.client.onConnected = function (id, userName, allUsers, messages, userRole) {
        setScreen(true);
        $('#hdId').val(id);
        $('#hdUserName').val(userName);
        $('#spanUser').html(userName);
        $('#role').html(userRole);
        // Add All Users
        for (i = 0; i < allUsers.length; i++) {

            AddUser(chatHub, allUsers[i].ConnectionId, allUsers[i].UserName, allUsers[i].UserRole);

        }

        // Add Existing Messages
        for (i = 0; i < messages.length; i++) {

            AddMessage(messages[i].UserName, messages[i].Message, messages[i].UserRole);
        }


    };

    // On New User Connected
    chatHub.client.onNewUserConnected = function (id, name, userRole) {

        AddUser(chatHub, id, name, userRole);
    };


    // On User Disconnected
    chatHub.client.onUserDisconnected = function (id, userName) {

        $('#' + id).remove();

        var ctrId = 'private_' + id;
        $('#' + ctrId).remove();


        var disc = $('<div class="disconnect">" Пользователь ' + userName + ' " вышел.</div>');

        $(disc).hide();
        $('#divusers').prepend(disc);
        $(disc).fadeIn(200).delay(2000).fadeOut(200);

    };

    chatHub.client.messageReceived = function (userName, message, userRole) {

        AddMessage(userName, message, userRole);
    };


    chatHub.client.sendPrivateMessage = function (windowId, fromUserName, message) {

        var ctrId = 'private_' + windowId;


        if ($('#' + ctrId).length == 0) {

            createPrivateChatWindow(chatHub, windowId, ctrId, fromUserName);

        }

        $('#' + ctrId).find('#divMessage').append('<div class="message"><span class="userName badge">' + fromUserName + ':</span> ' + message + '</div>');

        // set scrollbar
        var height = $('#' + ctrId).find('#divMessage')[0].scrollHeight;
        $('#' + ctrId).find('#divMessage').scrollTop(height);

    };

    chatHub.client.onError = function (message) {
        alert(message);
    };
}

function AddUser(chatHub, id, name, userRole) {

    var userId = $('#hdId').val();

    var code = "";

    if (userId == id) {
        if (userRole=='admin')
            code = $('<div class="loginUser">' + name + " <i class='fa fa-user-md  fa-1x'></i> </div>");
        else if (userRole=='lector')
            code = $('<div class="loginUser">' + name + " <i class='fa fa-user fa-1x'></div>");
        else
            code = $('<div class="loginUser">' + name + " </div>");
    }
    else {
        if (userRole == 'admin')
            code = $('<a id="' + id + '" class="user" >' + name +  ' <i class="fa fa-user-md  fa-1x"></i><a>');
        else if (userRole == 'lector')
            code = $('<a id="' + id + '" class="user" >' + name +  ' <i class="fa fa-user fa-1x"></i><a>');
        else
            code = $('<div class="loginUser">' + name + " </div>");

        $(code).dblclick(function () {

            var id = $(this).attr('id');

            if (userId !== id)
                OpenPrivateChatWindow(chatHub, id, name);

        });
    }

    $("#divusers").append(code);

}

function AddMessage(userName, message, userRole) {

    if(userRole=="admin") 
        $('#divChatWindow').append('<div class="message"><span class="userAdmin badge">' + userName + ':</span> ' + message + '</div>');
    else if (userRole=="lector")
        $('#divChatWindow').append('<div class="message"><span class="userLector badge">' + userName + ':</span> ' + message + '</div>');
     else {
        $('#divChatWindow').append('<div class="message"><span class="userName badge">' + userName + ':</span> ' + message + '</div>');
    }
   
    var height = $('#divChatWindow')[0].scrollHeight;
    $('#divChatWindow').scrollTop(height);
}

function OpenPrivateChatWindow(chatHub, id, userName) {

    var ctrId = 'private_' + id;

    if ($('#' + ctrId).length > 0) return;

    createPrivateChatWindow(chatHub, id, ctrId, userName);

}

function createPrivateChatWindow(chatHub, userId, ctrId, userName) {

    var div = '<div id="' + ctrId + '" class="ui-widget-content draggable single-chat panel panel-primary cursor" rel="0">' +
               '<div class="header panel-heading">' +
                  '<div class="pull-right">' +
                      '<i id="imgDelete"  class="glyphicon glyphicon-remove close-btn" ></i>' +
                   '</div>' +
                   '<h3 class="panel-title">'+
                   userName +
                   '</h3>'+
               '</div>' +
               '<div id="divMessage" class="messageArea">' +
               '</div>' +
               '<div class="form-group">' +
               '<div class="input-group">' +
                  '<input type="text" id="txtPrivateMessage" class="form-control"/>' +
                  '<span class="input-group-btn">'+
                  '<input id="btnSendMessage" class="btn btn-primary" type="button" value="Отправить"/>' +
                  '</span>'+
               '</div>' +
               '</div>' +
            '</div>';

    var $div = $(div);

    // DELETE BUTTON IMAGE
    $div.find('#imgDelete').click(function () {
        $('#' + ctrId).remove();
    });

    // Send Button event
    $div.find("#btnSendMessage").click(function () {

        window.$textBox = $div.find("#txtPrivateMessage");
        var msg = window.$textBox.val();
        if (msg.length > 0) {

            chatHub.server.sendPrivateMessage(userId, msg);
            window.$textBox.val('');
        }
    });

    // Text Box event
    $div.find("#txtPrivateMessage").keypress(function (e) {
        if (e.which === 13) {
            $div.find("#btnSendMessage").click();
        }
    });

    AddDivToContainer($div);

}

function FindUser() {

    $('#' + id).remove();
}

function AddDivToContainer($div) {
    $('#divContainer').prepend($div);

    $div.draggable({

        handle: ".header",
        stop: function () {

        }
    });
}