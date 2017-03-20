import {Injectable} from "@angular/core";
import {Router, CanActivate} from "@angular/router";
import {AuthService} from "./auth.service";
import {environment} from "../../environments/environment";

@Injectable()
export class AuthGuardService implements CanActivate {

  constructor(private router: Router, private auth: AuthService) {
  }

  canActivate() {


    if (environment.ignoreAuth) {
      console.log('Ignoring auth guard');
      return true;
    }


    if (JSON.parse(localStorage.getItem('currentUser')) != null) {
      this.auth.verify().subscribe(x => {
        if (!x) {
          this.router.navigate(['/login']);
        }
      }, error => console.log(error));
      return true;
    } else {
      this.router.navigate(['/login']);
      return false;
    }

  }

}
