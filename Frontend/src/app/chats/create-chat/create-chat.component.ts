import {Component, OnInit} from '@angular/core';
import {UsersCommunicatorService} from "../shared/services/users-communicator.service";
import {StorageService} from "../../shared/services/storage.service";
import {Constants} from "../../constants";
import {ChatroomService} from "../shared/services/chatroom.service";
import {ChatType} from "../../shared/chatroom";
import {Router} from "@angular/router";

@Component({
  selector: 'app-create-chat',
  templateUrl: './create-chat.component.html',
  styleUrls: ['./create-chat.component.css']
})
export class CreateChatComponent implements OnInit {
  usernames: string[] = [];
  addedUsers: string[] = [];
  text: string = '';
  showErrorMessage: boolean = false;
  chatType: string = 'private';
  chatName: string = '';

  constructor(private storage: StorageService,
              private userService: UsersCommunicatorService,
              private chatService: ChatroomService,
              private router: Router) {
  }

  ngOnInit(): void {
    this.updateUsers();
  }

  addUser(username: string) {
    this.addedUsers.push(username);
  }

  removeUser(username: string) {
    let index = this.addedUsers.indexOf(username);
    if (index > -1) {
      this.addedUsers.splice(index, 1);
    }
  }

  onTextChanged() {
    this.updateUsers(this.text);
  }

  onCreateClicked() {
    let users: string[] = [this.storage.get<string>(Constants.NicknameStorageField)];
    this.addedUsers.forEach(n => users.push(n));

    let type: ChatType = this.addedUsers.length != 1
      ? ChatType.Public
      : this.chatType == 'private'
        ? ChatType.Private
        : ChatType.Public;
    this.chatService.createChatroom(type, users, this.chatName).subscribe(result => {
      if (result.created) {
        this.storage.set(Constants.ChatIdStorageField, result.chatId);
        this.router.navigate(['chat']);
        return;
      }

      this.showErrorMessage = true;
    })
  }

  private updateUsers(username?: string) {
    this.userService.getUsernames(username)
      .subscribe(result => {
        const username = this.storage.get<string>(Constants.NicknameStorageField);
        let index = result.usernames.indexOf(username);
        if (index > -1) {
          result.usernames.splice(index, 1);
        }
        this.usernames = result.usernames;
      });
  }
}
