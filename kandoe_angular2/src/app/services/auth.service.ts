import {Injectable} from "@angular/core";
import {Http, Response, RequestOptions, Headers} from "@angular/http";
import "rxjs/add/operator/map";
import {Observable} from "rxjs";
import {environment} from "../../environments/environment";

const API = environment.api;


@Injectable()
export class AuthService {

   constructor(private http: Http) {
    let currentUser = JSON.parse(localStorage.getItem('currentUser'));
  }

  getTokenCurrentUser(){
    let currUser = JSON.parse(localStorage.getItem('currentUser'));
    return currUser.token;
  }


  login(username: string, password: string) {
    let headers = new Headers({'Content-Type': 'application/json'});
    let options = new RequestOptions({headers: headers});
    return this.http.post(API + '/auth/login', {"username": username, "password": password}, options)
      .map(this.handleLogin)
      .catch(this.handleError);
  }

  private handleLogin(res: Response) {
    // login successful if there's a jwt token in the response
    let user = res.json();
    if (user.success && user.token) {
      // store user details and jwt token in local storage to keep user logged in between page refreshes
      localStorage.setItem('currentUser', JSON.stringify(user));
      localStorage.setItem('username', JSON.stringify(user.username));
      return true;
    } else return false;
  }

  private handleVerify(res: Response) {
    return res.json().success == true;
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

  logout() {
    localStorage.removeItem('currentUser');
    localStorage.removeItem('username');
  }

  verify() {
    let token = this.getTokenCurrentUser();
    let headers = new Headers({'x-acces-token': token});
    let options = new RequestOptions({headers: headers});

    return this.http.post(API + '/auth/verify', {}, options)
      .map(this.handleVerify)
      .catch(this.handleError);
  }
}
