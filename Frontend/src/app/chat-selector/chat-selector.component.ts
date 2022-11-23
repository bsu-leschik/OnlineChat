import {Component, OnInit} from '@angular/core';
import {StorageService} from "../storage.service";
import {Constants} from "../constants";
import {Router} from "@angular/router";
import {HttpClient, HttpHeaders} from "@angular/common/http";

@Component({
  selector: 'app-chat-selector',
  templateUrl: './chat-selector.component.html',
  styleUrls: ['./chat-selector.component.css']
})
export class ChatSelectorComponent implements OnInit {
  public chatrooms: ChatroomInfo[] = [];
  private intervalId;

  constructor(private storage: StorageService, private httpClient: HttpClient, private router: Router) {
    if (storage.get<string>(Constants.NicknameStorageField) === undefined) {
      router.navigate(['login']);
    }
    this.intervalId = setInterval(() => this.updateChatrooms(), 1000)
  }

  ngOnInit(): void {
    this.updateChatrooms();
  }

  private updateChatrooms(): void {
    this.httpClient.get<ChatroomInfo[]>(Constants.ChatroomsControllerUrl).subscribe(chatrooms => {
      if (chatrooms != null)
        this.chatrooms = chatrooms;
    });
  }

  public joinChatroom(chatId: number) {
    this.storage.set(Constants.ChatIdStorageField, chatId);
    this.router.navigate(['chat']);
  }

  public createChatroom() {
    const httpOptions = {
      headers: new HttpHeaders({'Content-Type': 'application/json', 'accept': 'text/plain'})
    }
    let username = this.storage.get<string>(Constants.NicknameStorageField);
    this.httpClient.post(Constants.CreateChatroomUrl, null).subscribe(result => {
      this.chatrooms.push(new class implements ChatroomInfo {
        id: number = result as number;
        usersCount: number = 0;
      });
    });
  }
  public ngOnDestroy() {
    clearInterval(this.intervalId);
  }
}

interface ChatroomInfo {
  id: number;
  usersCount: number;
}
