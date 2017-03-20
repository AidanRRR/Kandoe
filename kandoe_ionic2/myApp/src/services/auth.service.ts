import {Injectable} from "@angular/core";
import {Http, Response, RequestOptions, Headers} from "@angular/http";
import "rxjs/add/operator/map";
import {environment} from "../environments/environment";


const API = environment.api;
// const API = "http://95.85.12.203:5020";


@Injectable()
export class AuthService {

  public token: string;


  constructor(private http: Http) {
    let currentUser = JSON.parse(localStorage.getItem('currentUser'));
    this.token = currentUser && currentUser.token;
  }


  getTokenCurrentUser(){
    let currUser = JSON.parse(localStorage.getItem('currentUser'));
    console.log(currUser.token);
    return currUser.token;
  }

  login(username: string, password: string) {
    let headers = new Headers({'Content-Type': 'application/json'});
    let options = new RequestOptions({headers: headers});
    return this.http.post(API+'/auth/login', {"username":username,"password":password},options)
      .map((response: Response) => {
        // login successful if there's a jwt token in the response
        let user = response.json();
        console.log("loginresponse: "+response.json());

        if (user && user.token) {
          this.token = user.token;
          // store user details and jwt token in local storage to keep user logged in between page refreshes
          localStorage.setItem('currentUser', JSON.stringify(user));
          this.verify();
          return true;
        } else return false;
      });
  }

  logout() {
    this.token = null;
    localStorage.removeItem('currentUser')
  }

  verify(){
    let headers = new Headers({ 'x-acces-token': this.token });
    let options = new RequestOptions({ headers: headers });

    return this.http.post(API+'/auth/verify',{},options).map((response: Response) => {
      if(response.json().success){
        localStorage.setItem('username',JSON.stringify(response.json()));
      }
      return response.json().success == true;
    });
  }
}
