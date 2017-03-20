import { Component } from '@angular/core';
import {NavController, NavParams, ToastController} from 'ionic-angular';
import {TabsPage} from "../tabs/tabs";
import {AuthService} from "../../services/auth.service";
import {RegisterPage} from "../register/register";
import {TranslateService} from 'ng2-translate';


@Component({
  selector: 'page-login',
  templateUrl: 'login.html'
})
export class LoginPage {
  private model: any = {}


  constructor(public navCtrl: NavController,
              public navParams: NavParams,
              private auth:AuthService,
              public toastCtrl: ToastController,
              public translate: TranslateService
  ) {
    this.navCtrl = navCtrl;
    translate.setDefaultLang('nl');
  }

  doLogin() {
    this.auth.login(this.model.userName,this.model.password).subscribe(
      bool => {
        console.log(bool);
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
      }
      }
    )
  }

  changeLanguage() {
    if(this.translate.currentLang == 'nl'){
      this.translate.use('en');
    }else {
      this.translate.use('nl');
    }
  }

  goToRegisterPage(){
    this.navCtrl.push(RegisterPage);
  }

}
