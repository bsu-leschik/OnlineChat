import { Component, OnInit } from '@angular/core';
import {StorageService} from "../storage.service";
import {Router} from "@angular/router";
import {Constants} from "../constants";
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(private storage: StorageService, private router: Router, private httpClient: HttpClient) {

  }

  ngOnInit(): void {}

  async onConnectClicked(): Promise<void> {
    const usernameInput: HTMLInputElement = document.getElementById('nickname-input') as HTMLInputElement;
    const passwordInput: HTMLInputElement = document.getElementById('password-input') as HTMLInputElement;
    const username = usernameInput.value;
    const password = passwordInput.value;
    if (username == null || password == null) {
      this.showErrorMessage('Password or nickname is null.');
      return;
    }
    if (!this.checkNickname(username)) {
      this.showErrorMessage('Invalid nickname');
      return;
    }
    this.httpClient.post<LoginResponse>(Constants.ServerUrl + '/login', {
      'username': username,
      'password': password,
    }, {
      withCredentials: true
    }).subscribe(response => {
      if (response == LoginResponse.Success) {
        this.storage.set(Constants.NicknameStorageField, username);
        this.router.navigate(['select-chat']);
        return;
      }
      if (response == LoginResponse.WrongPassword) {
        this.showErrorMessage('Wrong password');
        return;
      }
      this.showErrorMessage('Wrong username');
    });
  }

  checkNickname(nickname: string): boolean {
    return nickname !== '' && nickname !== 'admin';
  }

  showErrorMessage(message: string) : void {
    let element: HTMLParagraphElement = document.getElementById('error-message') as HTMLParagraphElement;
    if (element == null) {
      return;
    }

    element.textContent = message;
    element.hidden = false;
  }
}

enum LoginResponse {
  Success = 0,
  WrongPassword,
  WrongUsername
}
