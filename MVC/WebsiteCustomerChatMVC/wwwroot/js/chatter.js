var currentChatID;
var conn;

function showChat(x) {
    var con = document.getElementsByClassName("conversation");
    currentChatID = `${x}`;

    for (var c of con) {
        c.style.display = "none";
    }
    
    document.getElementById(`${x}_chat`).style.display = "inherit";
}

function bold(d) {

    document.getElementById(`${d}`).style.fontWeight = 'bold';

}

function AppendChat(id, message) {
    if (message !== null && message !== undefined && message !== "") {
        console.log(message);
        var data = message.split('|');
        var chat = document.getElementById(`${id}_chat`);
        if (data[0] == "ToOperator") {

            if (data[1] = "text") {
                chat.innerHTML += "<div class='text client'>Client: " + data[2] + "</div>";
            } else if (data[1] == "media") {
                chat.innerHTML += "<div class='media client'>Client: " + data[2] + "</div>";
            }
        } else if (data[0] == "ToClient") {

            if (data[1] = "text") {
                chat.innerHTML += "<div class='text operator'>You: " + data[2] + "</div>";
            }
            else if (data[1] == "media") {
                chat.innerHTML += "<div class='media operator'>You: " + data[2] + "</div>";
            }
        }

        chat.scrollTop = chat.scrollHeight;

    }
}
function sendText() {
    var text = document.getElementById("textmessage").value;
    conn.invoke("SendText", currentChatID, text);
    AppendChat(`${currentChatID}`, `ToClient|text|${text}` );
}

function launchChat() {

   var identifyMeAs = document.getElementById("identifyMeAs").value;


    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/ChatterHub")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    

    connection.start().then(() => {
        connection.invoke("ChangeMyName", identifyMeAs);
    }).then(() => {

        var chats = [];

        connection.invoke("GetExistingWorkspace").then((result) => {
            var lines = result.split("^^^^^");
            for (var l of lines) {
                var chat = {};
                
                var cols = l.split("~~");
                chat.id = cols[0];
                chat.chatdata = cols[1];
               
                chats.push(chat);
            }


            var chatlist =document.getElementById("chatUsers");
            var chatbody =document.getElementById("chatBody");

            for (var chat of chats)
            {
                chatlist.innerHTML = `<div onclick='showChat("${chat.id}")' id="${chat.id}_menu" class='chatclient'>client</div>` + chatlist.innerHTML;
                chatbody.innerHTML += `<div id="${chat.id}_chat" class="conversation" style="display:none">Start</div>`;

                if (chat.chatdata !== undefined && chat.chatdata !== null && chat.chatdata !== "") {

                    var messegesArray = chat.chatdata.split('\n');

                    for (var m of messegesArray) {
                        AppendChat(chat.id, m);
                    }

                }

            }

        });

    }).catch(err => console.error(err.toString()));
    conn = connection;

   

    
    connection.on("NewClient", (uid, message, name) => {
        var chatlist = document.getElementById("chatUsers");

        chatlist.innerHTML =
            `<div onclick='showChat("${uid}")' id="${uid}_menu" class='chatclient'>  ${name} ${message}</div>` + chatlist.innerHTML;

        var chatbody = document.getElementById("chatBody");
        chatbody.innerHTML += `<div id="${uid}_chat" class="conversation" style="display:none">Start</div>`;

    });

    connection.on("reconClient", (cookieID, newID) => {

        console.log(cookieID);
        var chatOnList = document.getElementById(`${cookieID}_menu`);
        chatOnList.setAttribute("onClick", `showChat("${cookieID}")`);
        chatOnList.setAttribute("id", `${cookieID}_menu`);


        var chat = document.getElementById(`${cookieID}_chat`);
        chat.setAttribute("id", `${cookieID}_chat`);

        AppendChat(cookieID, "Client Reconnect...");
    });

    connection.on("NewMessage", (uid, Message) => {

        bold(`${uid}_menu`);
        AppendChat(`${uid}`, `${Message}`);



    });






}

$(document).ready(function () {
   
  





});