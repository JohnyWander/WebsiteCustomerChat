$(document).ready(function() {
  const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:7004/chatHub",
	{
		accessTokenFactory: ()=>{
			
			var cookieValue = getCookie("chatCookie");
			return cookieValue;
		}
	})
    .configureLogging(signalR.LogLevel.Information)
    .build();

  connection.start().catch(err => console.error(err.toString()));
  var connectionId = connection.connectionId;
   
   if(getCookie("chatCookie")==""){	  
console.log("No cookie");   
  createCookie("chatCookie",connectionId,30);
   }
  connection.on("SendedResponse",function(stat){
	  
	
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


  connection.on("ReceiveMessage", function (message,name) {
    appendMessage(message,name,'incoming');
  });

  $("#chatIcon").click(function() {
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
      connection.invoke("SendMessage",messageText)
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
});