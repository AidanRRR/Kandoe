import { Component } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import {UserService} from "../../services/user.service";
import {TranslateService} from 'ng2-translate';

/*
  Generated class for the Account page.

  See http://ionicframework.com/docs/v2/components/#navigation for more info on
  Ionic pages and navigation.
*/
@Component({
  selector: 'page-account',
  templateUrl: 'account.html'
})
export class AccountPage {

  private user: any = {};
  private error: string = '';
  name: string;

  constructor(public navCtrl: NavController, public navParams: NavParams, private userService : UserService, public translate: TranslateService) {
    translate.setDefaultLang('nl');
  this.name = this.userService.getCurrentUsername();}

  ionViewDidLoad() {
    console.log('ionViewDidLoad AccountPage');
  }

  ngOnInit(){
    this.userService.getCurrentUser().subscribe(
      res => this.user = res,
      error => console.log(error)
    )
  }

  doSaveAccount(){
    this.userService.updateUser(this.user).subscribe(
      data => {
        console.log("Update user profile success");
      },
      error => this.error = "Update user profile failed");
  }

}
