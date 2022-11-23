import { Component, OnInit } from '@angular/core';
import {StorageService} from "../storage.service";
import {Navigation, Router} from "@angular/router";
import {Constants} from "../constants";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(private storage: StorageService, private router: Router) {
    if (storage.get<string>(Constants.NicknameStorageField) != null) {
      this.router.navigate(['select-chat']);
    }
  }

  ngOnInit(): void {}

  async onConnectClicked(): Promise<void> {
    let inputElement: HTMLInputElement = document.getElementById('nickname-input') as HTMLInputElement;
    const nickname = inputElement.value;
    if (nickname == null) {
      this.showErrorMessage('Nickname is null.');
      return;
    }
    if (!this.checkNickname(nickname)) {
      this.showErrorMessage('Invalid nickname');
      return;
    }
    this.storage.set(Constants.NicknameStorageField, nickname);
    this.storage.set(Constants.ChatIdStorageField, 0);
    await this.router.navigate(['select-chat']);
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
