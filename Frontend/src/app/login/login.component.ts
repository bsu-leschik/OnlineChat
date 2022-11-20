import { Component, OnInit } from '@angular/core';
import {StorageService} from "../storage.service";
import {Navigation, Router} from "@angular/router";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(private storage: StorageService, private router: Router) { }

  ngOnInit(): void {

  }

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
    this.storage.set('nickname', nickname);
    this.storage.set('chatId', 0);
    await this.router.navigate(['chat']);
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
