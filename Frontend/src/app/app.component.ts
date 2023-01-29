import {Component} from '@angular/core';
import {AuthenticationService, TokenLoginResponseCode} from "./shared/services/authentication.service";
import {StorageService} from "./shared/services/storage.service";
import {Constants} from "./constants";
import {Router} from "@angular/router";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})


export class AppComponent {
  title = 'Frontend';

  constructor(private authService: AuthenticationService,
              private storage: StorageService,
              private router: Router) {
  }

  ngOnInit() {
    this.authService.tryAutoLogin().subscribe(
      result => {
        if (result.responseCode != TokenLoginResponseCode.Success) {
          this.router.navigate(['login']);
          return;
        }

        this.storage.isLoggedIn = true;
        this.storage.set(Constants.NicknameStorageField, result.username);
      }
    );
  }
}
