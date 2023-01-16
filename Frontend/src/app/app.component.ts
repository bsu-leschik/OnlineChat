import {Component} from '@angular/core';
import {AuthenticationService, TokenLoginResponseCode} from "./shared/services/authentication.service";
import {StorageService} from "./shared/services/storage.service";
import {Constants} from "./constants";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})


export class AppComponent {
  title = 'Frontend';

  constructor(private authService: AuthenticationService,
              private storage: StorageService) {
  }

  ngOnInit() {
    this.authService.tryAutoLogin().subscribe(
      result => {
        if (result.responseCode != TokenLoginResponseCode.Success) {
          return;
        }

        this.storage.isLoggedIn = true;
        this.storage.set(Constants.NicknameStorageField, result.username);
      }
    );
  }
}
