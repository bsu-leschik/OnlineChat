import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {ChatroomInfo, ChatType} from "../chatroom";
import {Constants} from "../../constants";

@Injectable({
  providedIn: 'root'
})
export class ChatroomService {

  private readonly _api = Constants.ApiUrl + '/chatrooms';
  constructor(private httpClient: HttpClient) {}

  public getChatrooms() : Observable<ChatroomInfo[]> {
    return this.httpClient.get<ChatroomInfo[]>(this._api + '/get-chatrooms', {
      withCredentials: true
    });
  }

  public createChatroom(chatType: ChatType, users: string[]) : Observable<CreateChatroomResponse> {
    return this.httpClient.post<CreateChatroomResponse>(this._api + '/create',
      {
        'type': chatType,
        'users': users
      }, {
        withCredentials: true
      });
  }
}

export interface CreateChatroomResponse {
  chatId: string;
  created: boolean;
}
