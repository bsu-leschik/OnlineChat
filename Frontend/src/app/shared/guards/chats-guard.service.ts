import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, GuardsCheckStart, Router, RouterModule, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthenticationService } from '../services/authentication.service';
import { StorageService } from './../services/storage.service';

@Injectable({
  providedIn: 'root'
})
export class ChatsGuard implements CanActivate{

  constructor(private storage: StorageService, private auth: AuthenticationService, private router: Router) { }
  
  private readonly redirect: UrlTree = this.router.parseUrl('/auth/login');

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    if(this.storage.loggedIn()){
      return true;
    }
    else {
      return new Promise((resolve, reject) => {
        this.auth.tryAutoLogin()
        .then((value: boolean) => value ? resolve(true) : resolve(this.redirect))
      });
    }
  }
}
