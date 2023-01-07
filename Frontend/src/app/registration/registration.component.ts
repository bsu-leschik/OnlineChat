import { Component, OnInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Router} from "@angular/router";
import {Constants} from "../constants";

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {

  constructor(private httpClient: HttpClient, private router: Router) { }

  ngOnInit(): void {}
  onRegClicked() {
    const usernameInput = document.getElementById('nickname-input') as HTMLInputElement;
    const passwordInput = document.getElementById('password-input') as HTMLInputElement;
    const username = usernameInput.value;
    const password = passwordInput.value;
    this.httpClient.post<RegistrationResponse>(Constants.ApiUrl + '/register', {
      'username': username,
      'password': password
    }).subscribe(result => {
      if (result.reason != 'Success') {
        this.showErrorMessage(result.reason);
        return;
      }
      this.router.navigate(['chat-selector']);
    })
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

interface RegistrationResponse {
  reason: string;
}
