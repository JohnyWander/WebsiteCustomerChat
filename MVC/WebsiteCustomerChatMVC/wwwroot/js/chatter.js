var currentChatID;
var conn;
function showChat(x) {
    var con = document.getElementsByClassName("conversation");
    currentChatID = x;

    for (var c of con) {
        c.style.display = "none";
    }

    document.getElementById(x).style.display = "inherit";
}

function bold(d) {
    document.querySelectorAll(`[data-id=${d}]`).forEach(function (element) {
        element.style.fontWeight = 'bold';

    });
}

function AppendChat(id, message) {
    document.querySelectorAll(`[chat-id=${id}]`).forEach(function (element) {
        element.innerHTML += "<div class='texmsg'>" + message + "</div>";

    });
    
}
function sendText() {
    var text = document.getElementById("textmessage").value;
    conn.invoke("SendText", currentChatID, text);

}


$(document).ready(function () {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/ChatterHub")
        .configureLogging(signalR.LogLevel.Information)
        .build();



    connection.start().catch(err => console.error(err.toString()));
    conn = connection;

    connection.on("NewClient", (uid, message, name) => {
        var chatlist = document.getElementById("chatUsers");
        chatlist.innerHTML =
            `<div onclick='showChat("${uid}")' data-id=${uid} class='chatclient'>  ${name} ${message}</div>` + chatlist.innerHTML;

        var chatbody = document.getElementById("chatBody");
        chatbody.innerHTML += `<div chat-id="${uid}" id="${uid}" class="conversation" style="display:none">Start</div>`;
    });

    connection.on("NewMessage", (uid, Message) => {

        bold(`"${uid}"`);
        AppendChat(`"${uid}"`, `"${Message}"`);
        


    });

  





});