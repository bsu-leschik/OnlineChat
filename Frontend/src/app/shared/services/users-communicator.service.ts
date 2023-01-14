import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Constants} from "../../constants";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class UsersCommunicatorService {

  private readonly _api = Constants.ApiUrl + "/users";

  constructor(private httpClient: HttpClient) {
  }

  public getUsernames(startingWith?: string): Observable<GetUsernamesResponse> {
    if (startingWith === undefined) {
      return this.httpClient.get<GetUsernamesResponse>(this._api, {
          withCredentials: true,
        }
      );
    }
    return this.httpClient.get<GetUsernamesResponse>(this._api + '/' + startingWith, {
      withCredentials: true
    });
  }
}

export interface GetUsernamesResponse {
  usernames: string[];
}
