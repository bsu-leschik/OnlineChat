const hubConnection = new signalR.HubConnectionBuilder()
                    .withUrl("/chat")
                    .build();

document.getElementById('send-message-button').disabled = true;

hubConnection.start().then(function () {
    document.getElementById('send-message-button').disabled = false;
    console.log("started...");
}).catch(err => console.log("hey!\n"+err)); 

function getNickname() {
    return document.getElementById('nickname-input').value;
}

hubConnection.on("Send", function(name, message) {
    const element = document.createElement("li")
    element.textContent = name + ": " + message;
    console.log(message);
    const firstElement = document.getElementById("discussion").firstChild;
    document.getElementById("discussion").insertBefore(element, firstElement);  
});

document.getElementById("send-message-button")
        .addEventListener("click", () => {
            const message = document.getElementById('message-input').value;
            const nick = getNickname();
            if (nick === undefined || nick === '') {
                alert('You need to input nickname to send messages!');
            } else {
                hubConnection.invoke("Send", nick, message).then(result => console.log(result));
            }
        });