import { Component } from '@angular/core';
import {NavController, NavParams, ViewController} from 'ionic-angular';
import {ThemeService} from "../../services/theme.service";
import {CardService} from "../../services/card.service";
import {Card} from "../../domain/card";
import {TranslateService} from "ng2-translate";



@Component({
  selector: 'page-theme-details-modal',
  templateUrl: 'theme-details-modal.html'
})
export class ThemeDetailsModalPage {

  private currentThemeId: string;
  private currentTheme: any = {};
  private cards: Card[];

  constructor(public navCtrl: NavController,
              public navParams: NavParams,
              public viewCtrl: ViewController,
              private themeService: ThemeService,
              private cardService: CardService,
              public translate: TranslateService

  ) {translate.setDefaultLang('nl');

  }

  ngOnInit() {
    this.currentThemeId = this.navParams.get("themeId");

    this.themeService.getTheme(this.currentThemeId).subscribe(
      data => {
        this.currentTheme = data;
        this.cards = this.currentTheme.cards;
        console.log("get theme succes");
      },
      error => {
        console.log("get theme with id " + this.currentThemeId + " failed");
      });
  }

  ionViewDidLoad() {
    console.log('ionViewDidLoad ThemeDetailsModalPage');
  }

  dismiss(){
    this.viewCtrl.dismiss();
  }

}
