import {Component} from '@angular/core';
import {Platform} from 'ionic-angular';
import { StatusBar, Splashscreen } from 'ionic-native';
import {LoginPage} from "../pages/login/login";
import {AuthService} from "../services/auth.service";
import {TabsPage} from "../pages/tabs/tabs";


@Component({
  templateUrl: 'app.html'
})
export class MyApp {

  rootPage;

  constructor(platform: Platform, private auth:AuthService) {



    platform.ready().then(() => {
      // Okay, so the platform is ready and our plugins are available.
      // Here you can do any higher level native things you might need.
      StatusBar.backgroundColorByHexString('#469BC9');
      if(localStorage.getItem('currentUser')){
        this.rootPage = TabsPage;
      }else{
        this.rootPage = LoginPage;
      }
      Splashscreen.hide();

      // auth.verify().subscribe(x => {
      //   if(x){
      //     this.rootPage = TabsPage;
      //     Splashscreen.hide();
      //
      //   }else{
      //     this.rootPage = LoginPage;
      //     Splashscreen.hide();
      //
      //   }
      // });
    });
  }






}
