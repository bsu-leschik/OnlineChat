import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthenticationService } from '../services/authentication.service';
import { StorageService } from '../services/storage.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate{

  constructor(private storage: StorageService, private auth: AuthenticationService, private router: Router) { }
  
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    if(this.storage.isLoggedIn){
      return !true;
    }
    else {
     return new Promise((resolve, reject) => {
      this.auth.tryAutoLogin()
      .then(value => {
        if (value){
          resolve(this.router.parseUrl("/chats"))
        }
        else{
          resolve(true);
        }
      });
     });
    }
  }
}
