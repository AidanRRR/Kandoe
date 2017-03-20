import { Component } from '@angular/core';
import {ViewController, NavParams, NavController} from "ionic-angular";
import {TranslateService} from "ng2-translate";



@Component({
  selector: 'page-language-popover',
  templateUrl: 'language-popover.html'
})
export class LanguagePopoverPage {
  test: string;

  constructor(public viewCtrl: ViewController,
              public navCtrl: NavController,
              public navParams: NavParams,
              public translate: TranslateService
  ) {
    translate.setDefaultLang('nl');
  }

  changeLanguage(language: string) {
    this.translate.use(language);
    this.viewCtrl.dismiss();
  }



}
