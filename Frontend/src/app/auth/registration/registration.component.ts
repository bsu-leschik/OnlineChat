import { Component, OnInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Router} from "@angular/router";
import {Constants} from "../../constants";
import {AuthenticationService} from "../../shared/services/authentication.service";

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {

  public username: string = "";
  public password: string = "";
  public errorMessage: string = "";

  constructor(private httpClient: HttpClient,
              private router: Router,
              private authService: AuthenticationService) { }

  ngOnInit(): void {}

  onRegClicked() {
    this.httpClient.post<RegistrationResponse>(Constants.ApiUrl + '/registration', {
      'username': this.username,
      'password': this.password
    }).subscribe(result => {
      console.log(result);
      if (result != RegistrationResponse.Success) {
        this.showErrorMessage("Failed to registrate");
        return;
      }
      this.authService.login(this.username, this.password).subscribe(result => {
        this.router.navigate(['chats']);
      });
    })
  }

  showErrorMessage(message: string) : void {
    this.errorMessage = message;
  }
}

enum RegistrationResponse
{
  Success = 0,
  DuplicateUsername,
  Error
}
