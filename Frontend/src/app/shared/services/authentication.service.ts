import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Constants} from "../../constants";
import {Observable, Subscription} from "rxjs";
import { StorageService } from './storage.service';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  private readonly _api = Constants.ApiUrl + '/authentication';
  constructor(private httpClient: HttpClient, private storage : StorageService) {}

  public login(username: string, password: string) : Observable<LoginResponse> {
    return this.httpClient.post<LoginResponse>(this._api + '/login', {
      'username': username,
      'password': password
    },
      {
      withCredentials: true
    });
  }

  public logout() {
    this.httpClient.post(Constants.ServerUrl + '/api/authentication/logout', {},{
      withCredentials: true
    }).subscribe(result => {
      this.storage.isLoggedIn = false;
    });
  }

  public sendAutoLogin() : Observable<TokenLoginResponse> {
    return this.httpClient.get<TokenLoginResponse>(this._api + '/auto-login', {
      withCredentials: true
    }) ;
  }
  public tryAutoLogin(): Promise<boolean>{
    return new Promise((resolve, reject) => {
      this.sendAutoLogin()
      .subscribe(result => {
        if (result.responseCode == TokenLoginResponseCode.Success) {
          this.storage.isLoggedIn = true;
          this.storage.set(Constants.NicknameStorageField, result.username);
          resolve(true)
        }
        else{
          console.log(result)
          resolve(false);
        }
      })
    }) 
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
