const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/chat")
    .build();

hubConnection.on("Receive", function(messageInfo) {
    appendMessage(messageInfo.sender, messageInfo.text);
})

document.getElementById('messenger-wrap').hidden = true;

var username = '';

function connect() {
    console.log('connecting...');
    hubConnection.start().then(async r => {
        document.getElementById('login-wrap').hidden = true;
        document.getElementById('messenger-wrap').hidden = false;
        const messageList = await hubConnection.invoke("Connect", username, 0);
        for (let message of messageList) {
            appendMessage(message.sender, message.text);
        }
    });
}

document.getElementById('connect-button').addEventListener("click", e => {
    username = document.getElementById('nickname-input').value;
    connect();
});

function appendMessage(sender, message) {
    const element = document.createElement("li");
    element.textContent = sender + ": " + message;
    const firstElement = document.getElementById("discussion").firstChild;
    document.getElementById("discussion").insertBefore(element, firstElement);
}

document.getElementById('send-message-button')
    .addEventListener('click', async e => {
        const text = document.getElementById('message-input').value;
        await hubConnection.invoke("Send", text);
    })