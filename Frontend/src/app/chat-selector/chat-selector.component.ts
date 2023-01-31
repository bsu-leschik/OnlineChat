import {Component, OnInit} from '@angular/core';
import {StorageService} from "../shared/services/storage.service";
import {Constants} from "../constants";
import {Router} from "@angular/router";
import {ChatroomService} from "../shared/services/chatroom.service";
import {ChatroomInfoBase, ChatType, PrivateChatroomInfo, PublicChatroomInfo} from "../shared/chatroom";

@Component({
  selector: 'app-chat-selector',
  templateUrl: './chat-selector.component.html',
  styleUrls: ['./chat-selector.component.css']
})
export class ChatSelectorComponent implements OnInit {
  public chatrooms: ChatroomInfoBase[] = [];

  constructor(private storage: StorageService,
              private router: Router,
              private chatroomsService: ChatroomService) {
    if (storage.get<string>(Constants.NicknameStorageField) === undefined) {
      router.navigate(['login']);
    }
  }

  ngOnInit(): void {
    this.updateChatrooms();
  }

  private updateChatrooms(): void {
    this.chatroomsService.getChatrooms()
      .subscribe(result => {
        if (result == null || result.chatrooms == null) {
          alert('error');
        }
        this.chatrooms = result!.chatrooms;
      });
  }

  getRoomName(room: ChatroomInfoBase) : string {
    if (room.type == ChatType.Public) {
      return (room as PublicChatroomInfo).name;
    }
    return 'Private chat with ' + this.getSecondUserForPrivateChat(room as PrivateChatroomInfo);
  }

  public joinChatroom(chatId: string) {
    this.storage.set(Constants.ChatIdStorageField, chatId);
    this.router.navigate(['chat']);
  }

  public createChatroom() {
    this.router.navigate(['create-chat']);
  }

  isPublic(room: ChatroomInfoBase) {
    return room.type == ChatType.Public;
  }


  getSecondUserForPrivateChat(room: PrivateChatroomInfo): string {
    const username = this.storage.get<string>(Constants.NicknameStorageField);
    for (let user of room.users) {
      if (user != username) {
        return user;
      }
    }

    return "";
  }
}
