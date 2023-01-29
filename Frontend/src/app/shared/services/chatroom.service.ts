import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {ChatType, StandardChatroomInfo} from "../chatroom";
import {Constants} from "../../constants";
import { nextTick } from 'process';

@Injectable({
  providedIn: 'root'
})
export class ChatroomService {

  private readonly _api = Constants.ApiUrl + '/chatrooms';
  constructor(private httpClient: HttpClient) {}

  public getChatrooms() : Observable<any[]> {
    return this.httpClient.get<any[]>(this._api + '/get-chatrooms', {
      withCredentials: true
    });
  }

  public createChatroom(chatType: ChatType, users: string[]) : Observable<CreateChatroomResponse> {
    return this.httpClient.post<CreateChatroomResponse>(this._api + '/create',
      {
        'type': chatType,
        'usernames': users
      }, {
        withCredentials: true
      });
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

export interface CreateChatroomResponse {
  chatId: string;
  created: boolean;
}
