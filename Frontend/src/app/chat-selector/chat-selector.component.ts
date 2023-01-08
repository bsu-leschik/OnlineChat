import {Component, OnInit} from '@angular/core';
import {StorageService} from "../storage.service";
import {Constants} from "../constants";
import {Router} from "@angular/router";
import {HttpClient} from "@angular/common/http";
import {ChatroomInfo, ChatType} from "../shared/chatroom";

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
    this.httpClient.get<ChatroomInfo[]>(Constants.ApiUrl + '/chatrooms/get-chatrooms', {
      withCredentials: true
    }).subscribe(chatrooms => {
        console.log(chatrooms);
        if (chatrooms != null) {
          const username = this.storage.get<string>(Constants.NicknameStorageField);
          this.filter(chatrooms, username);
          console.log(chatrooms);
          this.chatrooms = chatrooms;
        }
      });
  }

  public joinChatroom(chatId: string) {
    this.storage.set(Constants.ChatIdStorageField, chatId);
    this.router.navigate(['chat']);
  }

  public createChatroom() {
    alert('not implemented');
    return;
    // const httpOptions = {
    //   headers: new HttpHeaders({'Content-Type': 'application/json', 'accept': 'text/plain'})
    // }
    // let username = this.storage.get<string>(Constants.NicknameStorageField);
    // this.httpClient.post(Constants.CreateChatroomUrl, null).subscribe(result => {
    //   this.chatrooms.push(new class implements ChatroomInfo {
    //     id: number = result as number;
    //     usersCount: number = 0;
    //   });
    // });
  }

  public ngOnDestroy() {
    clearInterval(this.intervalId);
  }

  isPublic(room: ChatroomInfo) {
    return room.chatType == ChatType.Public;
  }

  filter(chatrooms: ChatroomInfo[], username: string) {
    chatrooms.forEach(c => {
      let index = c.users.indexOf(username);
      if (index > -1) {
        c.users.splice(index, 1);
      }
    });
  }
}
