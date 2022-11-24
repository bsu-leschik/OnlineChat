import {Component, HostListener, OnInit} from '@angular/core';
import {HubConnection, HubConnectionBuilder} from '@microsoft/signalr';
import {StorageService} from "../storage.service";
import {Router} from "@angular/router";
import {Constants} from "../constants";

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
      .withUrl(Constants.ChathubUrl)
      .build();
    this.hubConnection.on('Receive', message => {
      this.messages.push(message);
    });
  }

  ngOnInit(): void {
    this.hubConnection.start().then(async () => {
      const response = await this.hubConnection.invoke('Connect', this.nickname, this.chatId) as ConnectionResponse;
      if (response.response != ConnectionResponseCode.SuccessfullyConnected) {
        ChatComponent.switchResponseCode(response.response);
        return;
      }
      for (let message of response.messages)
        this.messages.push(message);
      let welcomeText: HTMLParagraphElement = document.getElementById('welcome-text') as HTMLParagraphElement;
      welcomeText.textContent += this.nickname + '!';
    }).catch(() => alert('Failed to start connection'));
  }

  async onSendClicked(): Promise<void> {
    const input = document.getElementById('message-input') as HTMLInputElement;
    const message = input.value;
    if (message.length == 0) {
      return;
    }
    input.value = '';
    let value: number = await this.hubConnection.invoke('Send', message)
      .then()
      .catch(() => alert('Failed to send message'));

    console.log(value);
    if (value == 0) {
      ChatComponent.showErrorMessage('Failed to send the message');
    } else {
      ChatComponent.hideErrorMessage();
    }
  }

  @HostListener('window:popstate', ['$event'])
  onPopState() {
    this.hubConnection.stop();
  }

  async onLeaveClicked() {
    await this.hubConnection.stop();
    this.router.navigate(['select-chat']);
  }
  private static showErrorMessage(message: string) {
    const text = document.getElementById('error-text') as HTMLParagraphElement;
    text.textContent = message;
    text.hidden = false;
  }
  private static hideErrorMessage() {
    const text = document.getElementById('error-text') as HTMLParagraphElement;
    text.hidden = true;
  }
  private static switchResponseCode(code: ConnectionResponseCode) {
    switch (code) {
      case ConnectionResponseCode.AccessDenied: alert('Access denied'); break;
      case ConnectionResponseCode.DuplicateNickname: alert('Duplicate nickname'); break;
      case ConnectionResponseCode.BannedNickname: alert('The nickname is banned'); break;
      case ConnectionResponseCode.RoomIsFull: alert('Room is full'); break;
      case ConnectionResponseCode.WrongNickname: alert('Incorrect nickname'); break;
      case ConnectionResponseCode.RoomDoesntExist: alert("Room doesn't exist"); break;
    }
  }
}

export interface Message {
  sender: String;
  text: String;
}

enum ConnectionResponseCode {
  SuccessfullyConnected = 0,
  AccessDenied,
  DuplicateNickname,
  BannedNickname,
  RoomIsFull,
  WrongNickname,
  RoomDoesntExist
}

interface ConnectionResponse {
  messages: Message[];
  response: ConnectionResponseCode;
}
