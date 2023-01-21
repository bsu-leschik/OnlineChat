import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Constants} from "../../constants";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  private readonly _api = Constants.ApiUrl + '/authentication';
  constructor(private httpClient: HttpClient) {}

  public login(username: string, password: string) : Observable<LoginResponse> {
    return this.httpClient.post<LoginResponse>(this._api + '/login', {
      'username': username,
      'password': password
    },
      {
      withCredentials: true
    });
  }

  public logout() : Observable<any> {
    return this.httpClient.post(this._api + '/logout', {},{
      withCredentials: true
    });
  }

  public tryAutoLogin() : Observable<TokenLoginResponse> {
    return this.httpClient.get<TokenLoginResponse>(this._api + '/auto-login', {
      withCredentials: true
    });
  }
}

export enum LoginResponse {
  Success = 0,
  WrongPassword,
  WrongUsername
}

export interface TokenLoginResponse {
  username: string;
  responseCode: TokenLoginResponseCode;
}

export enum TokenLoginResponseCode {
  Success = 0,
  TokenExpired,
  NotLoggedIn,
  BadRequest
}
