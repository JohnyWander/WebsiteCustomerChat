var connectionId;

$(document).ready(function () {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("https://localhost:7004/chatHub",
            {
                accessTokenFactory: () => {

                    var cookieValue = getCookie("chatCookie");
                    return cookieValue;
                }
            })
        .configureLogging(signalR.LogLevel.Information)
        .build();

    connection.start().then(() => {
        console.log("Connection established");
        connection.invoke("getOperatorName").then((result) => {
            document.getElementById("OperatorDescription").innerHTML = "&nbsp" + result;
        });
        // Access the connectionId after the connection is established
        var connectionId = connection.connectionId;
        console.log("ConnectionId: " + connectionId);
        if (getCookie("chatCookie") == "") {
            createCookie("chatCookie", connectionId, 30);
            //document.cookie = `"chatCookie=${connectionId}; expires=Thu, 01 Jan 2025 00:00:00 UTC; path=/;"`;
        }

    }).catch((err) => {
        
        document.getElementById("sendButton").style.display = "none";
        document.getElementById("messageInput").style.display= "none";
        document.getElementById("chatBox").innerHTML = "Chat is offline";
    });

   

    connection.on("SendedResponse", function (stat) {


    });

    connection.on("LoadHistory", function (hist, newid) {
        var lines = hist.split('\n');

        var connectionId = connection.connectionId;

        createCookie("chatCookie", newid, 30);


        for (var l of lines) {

            var cols = l.split('|');
            var target = cols[0];
            var type = cols[1];
            var data = cols[2];

            if (target == "ToOperator") {
                appendMessage("You", data, 'outgoing');
            } else {
                appendMessage("Operator", data, 'incoming');
            }

        }


    });

    function getCookie(name) {
        var cookieName = name + "=";
        var decodedCookie = decodeURIComponent(document.cookie);
        var cookieArray = decodedCookie.split(';');
        for (var i = 0; i < cookieArray.length; i++) {
            var cookie = cookieArray[i].trim();
            if (cookie.indexOf(cookieName) === 0) {
                return cookie.substring(cookieName.length, cookie.length);
            }
        }
        return "";
    }




    function createCookie(name, value, days) {
        var expires = "";
        if (days) {
            var date = new Date();
            date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
            expires = "; expires=" + date.toUTCString();
        }
        document.cookie = name + "=" + value + expires + "; path=/";
    }


    connection.on("ReceiveMessage", function (message, name) {
        appendMessage(message, name, 'incoming');
    });

    $("#chatIcon").click(function () {
        $("#chatContainer").toggle();
    });

    $("#sendButton").click(function () {
        sendMessage();
    });

    $("#messageInput").keypress(function (event) {
        if (event.which === 13) {
            event.preventDefault();
            sendMessage();
        }
    });

    function sendMessage() {
        var messageInput = $("#messageInput");
        var messageText = messageInput.val().trim();

        if (messageText !== '') {
            connection.invoke("SendMessage", messageText)
                .catch(err => console.error(err.toString()));

            appendMessage("You", messageText, 'outgoing');
            messageInput.val('');
        }
    }

    function appendMessage(user, message, type) {
        var messageElement = $("<div>").addClass("message").addClass(type);
        messageElement.html(`<p><strong>${user}: </strong>${message}</p>`);
        $("#chatBox").append(messageElement);

        // Scroll to bottom
        $("#chatBox").scrollTop($("#chatBox")[0].scrollHeight);
    }

    function fetchHistory(history) {

    }

});