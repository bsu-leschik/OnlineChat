import {Component, OnInit} from '@angular/core';
import {HubConnection, HubConnectionBuilder} from '@microsoft/signalr';
import {StorageService} from "../storage.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})

export class ChatComponent implements OnInit {
  private readonly nickname: string;
  private readonly chatId: number;
  private hubConnection: HubConnection;
  public messages: Message[] = [];

  constructor(private storage: StorageService, private router: Router) {
    console.log('init');
    this.nickname = this.storage.get<string>('nickname');
    this.chatId = this.storage.get<number>('chatId');
    if (this.nickname == undefined || this.chatId == undefined) {
      this.router.navigate(['login']);
    }
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('https://localhost:7023/chat')
      .build();
    this.hubConnection.on('Receive', message => {
      this.messages.push(message);
    });
  }

  ngOnInit(): void {
    this.hubConnection.start().then(async () => {
      const oldMessages = await this.hubConnection.invoke('Connect', this.nickname, this.chatId);
      for (let message of oldMessages)
        this.messages.push(message);
      let welcomeText: HTMLParagraphElement = document.getElementById('welcome-text') as HTMLParagraphElement;
      welcomeText.textContent += this.nickname + '!';
    }).catch(() => alert('Failed to start connection'));
  }
  onSendClicked() : void {
    const input = document.getElementById('message-input') as HTMLInputElement;
    const message = input.value;
    if (message.length == 0) {
      return;
    }
    input.value = '';
    this.hubConnection.invoke('Send', message)
      .then()
      .catch(() => alert('Failed to send message'));
  }
}

export interface Message {
  sender: String;
  text: String;
}
