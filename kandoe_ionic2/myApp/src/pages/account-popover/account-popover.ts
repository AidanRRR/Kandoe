import { Component } from '@angular/core';
import {ViewController, NavParams, NavController} from "ionic-angular";
import {AccountPage} from "../account/account";
import {LoginPage} from "../login/login";
import {AuthService} from "../../services/auth.service";
import {TranslateService} from "ng2-translate";

/*
  Generated class for the AccountPopover page.

  See http://ionicframework.com/docs/v2/components/#navigation for more info on
  Ionic pages and navigation.
*/
@Component({
  selector: 'page-account-popover',
  templateUrl: 'account-popover.html'
})
export class AccountPopoverPage {

  constructor(public viewCtrl: ViewController,
              public navCtrl: NavController,
              public navParams: NavParams,
              private authService: AuthService,
              public translate: TranslateService
  ) {translate.setDefaultLang('nl');}

  close(){
    this.viewCtrl.dismiss();
  }

  goToAccount(){
    this.navCtrl.push(AccountPage);
    this.viewCtrl.dismiss();

  }

  goToLogout(){
    this.authService.logout();
    this.navCtrl.setRoot(LoginPage);
    this.navCtrl.push(LoginPage);
    this.viewCtrl.dismiss();

  }

}
