import { Component, OnInit } from '@angular/core';
import {StorageService} from "../shared/services/storage.service";
import {Constants} from "../constants";
import {Router} from "@angular/router";
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  public username: string = "";
  constructor(public storage: StorageService, public router: Router, private httpClient: HttpClient) {
  }

  ngOnInit(): void {
    this.username = this.storage.get<string>(Constants.NicknameStorageField);
    this.storage.isLoggedIn = this.username != null;
  }

  logout() {
    this.httpClient.post(Constants.ServerUrl + '/api/authentication/logout', {},{
      withCredentials: true
    }).subscribe(result => {
      this.storage.isLoggedIn = false;
      this.router.navigate(['login']);
    });
  }
}
