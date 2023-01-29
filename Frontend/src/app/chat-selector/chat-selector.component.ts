import {Component, OnInit} from '@angular/core';
import {StorageService} from "../shared/services/storage.service";
import {Constants} from "../constants";
import {Router} from "@angular/router";
import {ChatType, MinimumChatroomInfo, StandardChatroomInfo} from "../shared/chatroom";
import {ChatroomService} from "../shared/services/chatroom.service";

@Component({
  selector: 'app-chat-selector',
  templateUrl: './chat-selector.component.html',
  styleUrls: ['./chat-selector.component.css']
})
export class ChatSelectorComponent implements OnInit {
  public chatrooms: StandardChatroomInfo[] = [];
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
      .subscribe(rawChatrooms => {
        if (rawChatrooms != null) {
          const username = this.storage.get<string>(Constants.NicknameStorageField);
          let chatrooms = this.processRawChatrooms(rawChatrooms);
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

  isPublic(room: StandardChatroomInfo) {
    return room.chatType == ChatType.Public;
  }

  processRawChatrooms(rawChatrooms : any[]) : StandardChatroomInfo[]{
    let processedChatrooms : StandardChatroomInfo[] = [];
    rawChatrooms.forEach(
      chat => {
        let temp = chat as StandardChatroomInfo;
        //if (temp.chatType == ChatType.Private){
          processedChatrooms.push(temp);
        //}
      }
    );
    return processedChatrooms;
  }

  filter(chatrooms: StandardChatroomInfo[], username: string) {
    chatrooms.forEach(c => {
      let index = c.users.indexOf(username, 0);
      if (index > -1) {
        c.users.splice(index, 1);
      }
    });
  }
}
