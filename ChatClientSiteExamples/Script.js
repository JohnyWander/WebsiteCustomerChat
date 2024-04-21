$(document).ready(function() {
  const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

  connection.start().catch(err => console.error(err.toString()));

  connection.on("ReceiveMessage", function (user, message) {
    appendMessage(user, message, 'incoming');
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
      connection.invoke("SendMessage", "User", messageText)
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