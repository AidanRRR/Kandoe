import {Injectable} from "@angular/core";
import {Http, RequestOptions, Headers, Response, RequestOptionsArgs} from "@angular/http";
import {Observable} from "rxjs/Rx";
import {User} from "../domain/user";
import {AuthService} from "./auth.service";
import {environment} from "../environments/environment";

const API = environment.api;

export interface CreateUserModel {
  username: string;
  password: string;
  email: string;
  firstName: string;
  lastName: string;
  notifications: boolean;
}

@Injectable()
export class UserService {
  private options;

  constructor(private http: Http, private auth: AuthService) {
    let headers = new Headers({'Content-Type': 'application/json'});
    this.options = new RequestOptions({headers: headers});
  }

  getCurrentUsername(){
    let currentUser = JSON.parse(localStorage.getItem('currentUser'));
    if (name != null) return currentUser.username;
  }

  getCurrentUser() {
    return this.http.get(API + '/Users/GetUserById/' + this.getCurrentUsername(), this.options)
      .map(this.handleRequest)
      .catch(this.handleError);
  }

  create(user: CreateUserModel) {
    return this.http.post(API + '/auth/register', {
      "username": user.username,
      "firstName": user.firstName,
      "lastName": user.lastName,
      "password": user.password,
      "email": user.email,
      "notifications": true
    }, this.options)
      .map(this.handleRequest)
      .catch(this.handleError);
  }

  getUser(name: string){
    return this.http.get(API+'/user/'+name, this.options)
      .map(this.handleRequest)
      .catch(this.handleError);
  }

  updateUser(user : User) {
    return this.http.post(API + '/users/updateuser', {
      "userName": user.userName,
      "firstName": user.firstName,
      "lastName": user.lastName,
      "email": user.email,
      "notifications": user.notifications,
      "isEnabled": true
    }, this.options)
      .map(this.handleRequest)
      .catch(this.handleError);
  }

  private handleRequest(res: Response) {
    let body = res.json();
    return body.data || {};
  }


  private handleError(error: Response | any) {
    // In a real world app, we might use a remote logging infrastructure
    let errMsg: string;
    if (error instanceof Response) {
      const body = error.json() || '';
      const err = body.error || JSON.stringify(body);
      errMsg = `${error.status} - ${error.statusText || ''} ${err}`;
    } else {
      errMsg = error.message ? error.message : error.toString();
    }
    console.error(errMsg);
    return Observable.throw(errMsg);
  }
}
