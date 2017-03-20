import {Component, OnInit} from "@angular/core";
import {Router} from "@angular/router";
import {AuthService} from "../../services/auth.service";
import {TranslateService} from "ng2-translate";

declare const FB: any;
declare const toastr: any;

@Component({
  selector: 'login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  private model: any = {};
  private error: string = '';

  constructor(private router: Router,
              private auth: AuthService,
              public translate: TranslateService
  ) {
    this.auth.logout();

    FB.init({
      appId: '693328257476875',
      cookie: false,  // enable cookies to allow the server to access
      // the session
      xfbml: true,  // parse social plugins on this page
      version: 'v2.6' // use graph api version 2.6
    });

  }

  ngOnInit() {
    FB.getLoginStatus(response => {
      this.statusChangeCallback(response);
    });
  }

  changeLanguage() {
    if(this.translate.currentLang == 'nl'){
      this.translate.use('en');
      localStorage.setItem("language",'en');
    }else {
      this.translate.use('nl');
      localStorage.setItem("language",'nl');
    }
  }

  public login() {
    this.auth.login(this.model.username, this.model.password).subscribe(x => {
        if (!x) {
          toastr.error("gebruikersnaam of wachtwoord is incorrect");
        } else{
          this.router.navigate(['/']);
        }
      },
      error => toastr.error("gebruikersnaam of wachtwoord is incorrect"));
  }

  onFacebookLoginClick() {
    FB.login();
  }

  statusChangeCallback(resp) {
    if (resp.status === 'connected') {
      // connect here with your server for facebook login by passing access token given by facebook
      console.log(resp);
      console.log("Facebook token: " + resp.authResponse.accessToken);
    } else if (resp.status === 'not_authorized') {

    } else {

    }
  };
}
