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
              private router: Router) {
  }

  ngOnInit() {
   }

}
