import {Component} from '@angular/core';
import {AuthenticationService, TokenLoginResponseCode} from "./shared/services/authentication.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})


export class AppComponent {
  title = 'Frontend';

  constructor(private authService: AuthenticationService) {
      
  }

  ngOnInit() { }

}
