const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/chat")
    .build();

const connectButton = document.getElementById('connect-button');
const sendMessageButton = document.getElementById('send-message-button');
const messageInputField = document.getElementById('message-input');
const discussion = document.getElementById("discussion");

hubConnection.on("Receive", function(messageInfo) {
    appendMessage(messageInfo.sender, messageInfo.text);
})

document.getElementById('messenger-wrap').hidden = true;

var username = '';

function connect() {
    hubConnection.start().then(async r => {
        document.getElementById('login-wrap').hidden = true;
        document.getElementById('messenger-wrap').hidden = false;
        document.getElementById('welcome-text').textContent += username + '!';
        const messageList = await hubConnection.invoke("Connect", username, 0);
        for (let message of messageList) {
            appendMessage(message.sender, message.text);
        }
    });
}

function validateNickname(nickname) {
    return nickname !== '' && nickname !== 'null' && nickname !== undefined;
}

connectButton.addEventListener("click", e =>    {
    username = document.getElementById('nickname-input').value;
    if (validateNickname(username))
        connect();
    else 
        document.getElementById('login-error-message').hidden = false;
});

function appendMessage(sender, message) {
    const element = document.createElement('li');
    element.id = 'message';
    element.textContent = sender + ': ' + message;
    const firstElement = discussion.firstChild;
    discussion.insertBefore(element, firstElement);
}

sendMessageButton.addEventListener('click', async e => {
        const text = messageInputField.value;
        messageInputField.value = '';
        await hubConnection.invoke("Send", text);
    });