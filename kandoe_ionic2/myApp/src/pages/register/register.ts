import { Component } from '@angular/core';
import {NavController, NavParams, ToastController} from 'ionic-angular';
import {UserService} from "../../services/user.service";
import {TabsPage} from "../tabs/tabs";
import {AuthService} from "../../services/auth.service";
import {LoginPage} from "../login/login";
import {TranslateService} from "ng2-translate";

/*
  Generated class for the Register page.

  See http://ionicframework.com/docs/v2/components/#navigation for more info on
  Ionic pages and navigation.
*/
@Component({
  selector: 'page-register',
  templateUrl: 'register.html'
})
export class RegisterPage {
  private model: any = {}


  constructor(public navCtrl: NavController,
              public navParams: NavParams,
              private userService: UserService,
              private authService: AuthService,
              public toastCtrl: ToastController,
              public translate: TranslateService
  ) {
    this.navCtrl = navCtrl;
    translate.setDefaultLang('nl');
  }

  doRegister() {
    this.userService.create(this.model).subscribe(
      data => {
        console.log("Register success");
        this.authService.login(this.model.username,this.model.password).subscribe(
          bool => {
            if(bool){
              this.navCtrl.setRoot(TabsPage);
            }
            else{
              let toast = this.toastCtrl.create({
                message: 'Login not successful',
                duration: 1500,
                position: 'top'

              });
              toast.present();
              this.navCtrl.setRoot(LoginPage);
            }
          }
        )
      },
      error => {
        let toast = this.toastCtrl.create({
          message: "Register not successful",
          duration: 1500,
          position: 'top'

        });
        toast.present();
      })
  }

  changeLanguage() {
    if(this.translate.currentLang == 'nl'){
      this.translate.use('en');
    }else {
      this.translate.use('nl');
    }
  }

}
