import {Component, OnInit} from '@angular/core';
import {StorageService} from "../shared/services/storage.service";
import {Constants} from "../constants";
import {Router} from "@angular/router";
import {ChatroomInfo, ChatType} from "../shared/chatroom";
import {ChatroomService} from "../shared/services/chatroom.service";

@Component({
  selector: 'app-chat-selector',
  templateUrl: './chat-selector.component.html',
  styleUrls: ['./chat-selector.component.css']
})
export class ChatSelectorComponent implements OnInit {
  public chatrooms: ChatroomInfo[] = [];
  private readonly intervalId;

  constructor(private storage: StorageService,
              private router: Router,
              private chatroomsService: ChatroomService) {
    if (storage.get<string>(Constants.NicknameStorageField) === undefined) {
      router.navigate(['login']);
    }
    this.intervalId = setInterval(() => this.updateChatrooms(), 1000)
  }

  ngOnInit(): void {
    this.updateChatrooms();
  }

  private updateChatrooms(): void {
    this.chatroomsService.getChatrooms()
      .subscribe(chatrooms => {
        if (chatrooms != null) {
          const username = this.storage.get<string>(Constants.NicknameStorageField);
          this.filter(chatrooms, username);
          this.chatrooms = chatrooms;
        }
      });
  }

  public joinChatroom(chatId: string) {
    this.storage.set(Constants.ChatIdStorageField, chatId);
    this.router.navigate(['chat']);
  }

  public createChatroom() {
    this.router.navigate(['create-chat']);
  }

  public ngOnDestroy() {
    clearInterval(this.intervalId);
  }

  isPublic(room: ChatroomInfo) {
    return room.chatType == ChatType.Public;
  }

  filter(chatrooms: ChatroomInfo[], username: string) {
    chatrooms.forEach(c => {
      let index = c.users.indexOf(username, 0);
      if (index > -1) {
        c.users.splice(index, 1);
      }
    });
  }
}
