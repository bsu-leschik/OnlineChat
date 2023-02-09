import { Injectable, Output } from '@angular/core';
import { Router, NavigationStart, Event as NavigationEvent, NavigationEnd, NavigationCancel, NavigationError } from '@angular/router';

@Injectable({
  providedIn: 'root'
})

export class StorageService {

  private readonly data: Record<string, any> = {};
  private static isLoggedIn = false;

  constructor() { }

  public set(key: string, value: any) : void {
    this.data[key] = value;
  }
  public get<T>(key: string) {
    return this.data[key] as T;
  }

  public setLoggedIn(value: boolean){
    StorageService.isLoggedIn = value;
  }

  public loggedIn(): boolean{

    return StorageService.isLoggedIn;
  }


}
