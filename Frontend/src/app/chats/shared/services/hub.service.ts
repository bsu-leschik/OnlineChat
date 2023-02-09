import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Constants } from 'src/app/constants';

@Injectable({
  providedIn: 'root'
})
export class HubService {

  public hubConnection: HubConnection;

  private startResponse: Promise<void>;

  private connectionEstablished: boolean = false;

  private connectionError: string | null = null;

  constructor() { this.hubConnection = new HubConnectionBuilder()
    .withUrl(Constants.ChathubUrl)
    .build(); 
    this.startResponse = this.hubConnection.start();
    this.startResponse.catch((reason: any) => {
      this.connectionEstablished = false; 
      this.connectionError = reason
    });
    this.startResponse.then(() => this.connectionEstablished = true);
  }

  public addOnHandler(methodName: string, newMethod: (...args: any[]) => any){
    this.hubConnection.on(methodName, newMethod);
  }

  public async invokeBackendMethod(methodName: string, ...methodArgs: any[]): Promise<String>{
    let args = this.parseArgs(methodArgs);
    
    if(this.connectionEstablished){
      return new Promise((resolve, reject) => 
        this.hubConnection.invoke<ConnectionResponseCode>(methodName, args)
          .then((code: ConnectionResponseCode) => {
            if(code == ConnectionResponseCode.SuccessfullyConnected){
              resolve("Successfully connected");
            }
            else {
              reject(this.getConnectionResponseName(code));
            }
          }
        )
      )
    }
    else {
      return new Promise((resolve, reject) => {
        if(this.connectionError == null){
          this.startResponse.then(async () => {
            resolve(this.getConnectionResponseName(await this.hubConnection.invoke<ConnectionResponseCode>(methodName, args) as ConnectionResponseCode))
          })
        }
        else {
          reject('Failed to start connection');
        }
      })
      
    }
  }

  public breakConnection(frontendHandlerName: string): void{
    this.hubConnection.off(frontendHandlerName);
  }

  private parseArgs(args: any[]) : any[] | any{
    if(args.length == 0){
      return args;
    }
    else if(args.length == 1){
      return args[0];
    }
    return args;
  }

  private getConnectionResponseName(code: ConnectionResponseCode){
    switch(code){
      case 0: return 'Successfully Connected!';
      case 1: return 'Access Denied';
      case 2: return 'Duplicate Nickname';
      case 3: return 'Banned!';
      case 4: return 'Room Is Full!';
      case 5: return 'Wrong Nickname';
      case 6: return 'Room Doesn\'t Exist';
      default: return 'Unknown Error';
    }
  }

}

enum ConnectionResponseCode {
  SuccessfullyConnected = 0,
  AccessDenied,
  DuplicateNickname,
  BannedNickname,
  RoomIsFull,
  WrongNickname,
  RoomDoesntExist
}