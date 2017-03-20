import {Component} from "@angular/core";
import {Router} from "@angular/router";
import {UserService} from "../../services/user.service";
import {TranslateService} from "ng2-translate";

declare const toastr: any;

@Component({
  selector: 'register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  private model: any = {};
  private error: string = '';

  constructor(private router: Router,
              private userService: UserService,
              public translate: TranslateService) {
  }

  public register(): void {
    this.userService.create(this.model).subscribe(
      data => {
        toastr.success("Registreren gelukt");
        this.router.navigate(['/login']);
      },
      error => this.error = "Connectie met server mislukt!");
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
}
