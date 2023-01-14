import { Component, OnInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Router} from "@angular/router";
import {Constants} from "../constants";
import {AuthenticationService} from "../shared/services/authentication.service";

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {

  constructor(private httpClient: HttpClient,
              private router: Router,
              private authService: AuthenticationService) { }

  ngOnInit(): void {}
  onRegClicked() {
    const usernameInput = document.getElementById('nickname-input') as HTMLInputElement;
    const passwordInput = document.getElementById('password-input') as HTMLInputElement;
    const username = usernameInput.value;
    const password = passwordInput.value;
    this.httpClient.post<RegistrationResponse>(Constants.ApiUrl + '/registration', {
      'username': username,
      'password': password
    }).subscribe(result => {
      console.log(result);
      if (result.reason != 'Success') {
        this.showErrorMessage(result.reason);
        return;
      }
      this.authService.login(username, password).subscribe(result => {
        console.log('log');
        this.router.navigate(['chat-selector']);
      });
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
