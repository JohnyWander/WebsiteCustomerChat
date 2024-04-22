
function showChat(x) {
    var con = document.getElementsByClassName("conversation");

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

function AppendChat(c) {
    document.getElementById(`"${c}"`).innerHTML += "<div class='texmsg'>" + Message + "</div>";
}

$(document).ready(function () {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/ChatterHub")
        .configureLogging(signalR.LogLevel.Information)
        .build();



    connection.start().catch(err => console.error(err.toString()));


    connection.on("NewClient", (uid, message, name) => {
        var chatlist = document.getElementById("chatUsers");
        chatlist.innerHTML =
            `<div onclick='showChat("${uid}")' data-id=${uid} class='chatclient'>  ${name} ${message}</div>` + chatlist.innerHTML;

        var chatbody = document.getElementById("chatBody");
        chatbody.innerHTML += `<div id="${uid}" class="conversation" style="display:none">Start</div>`;
    });

    connection.on("NewMessage", (uid, Message) => {

        bold(`"${uid}"`);
        AppendChat(`"${uid}"`);



    });







});