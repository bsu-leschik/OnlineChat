import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class StorageService {
  private readonly data: Record<string, any> = {};
  constructor() { }
  public set(key: string, value: any) : void {
    this.data[key] = value;
  }
  public get<T>(key: string) {
    return this.data[key] as T;
  }
}
