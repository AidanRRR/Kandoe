import { Component } from '@angular/core';
import {NavController, NavParams, ViewController, Events} from 'ionic-angular';
import {ThemeService} from "../../services/theme.service";
import {TranslateService} from "ng2-translate";
import {Theme} from "../../domain/theme";
import {UserService} from "../../services/user.service";

/*
  Generated class for the ThemeModal page.

  See http://ionicframework.com/docs/v2/components/#navigation for more info on
  Ionic pages and navigation.
*/
@Component({
  selector: 'page-theme-modal',
  templateUrl: 'theme-modal.html'
})
export class ThemeModalPage {

  newTheme: any = {};
  themes: Theme[];
  name: string;
  tag: string;
  newTag: string;

  constructor(public navCtrl: NavController,
              public navParams: NavParams,
              public viewCtrl: ViewController,
              private themeService: ThemeService,
              private userService: UserService,
              public translate: TranslateService,
              public events: Events

  ) {
    translate.setDefaultLang('nl');
    this.name = this.userService.getCurrentUsername();
  }

  ionViewDidLoad() {
    console.log('ionViewDidLoad ThemeModalPage');
  }

  dismiss(){
    this.viewCtrl.dismiss();
  }

  saveTheme(){
   this.newTheme.userName = this.name;
    this.themeService.create(this.newTheme).subscribe(
      data => {
        let add: Theme = {
          themeId: data.themeId,
          name: data.name,
          description: data.description,
          cards: [],
          organizers: data.organizers,
          updatedOn: data.updatedOn,
          createdOn: data.createdOn,
          isEnabled: data.isEnabled,
          isPublic: data.public,
          //todo: tags werken ook nog niet
          tags: data.tags,
          username: data.userName
        };
        console.log("Add theme succes");
      },
      error => {
        console.log("Add theme failed");
      });
    this.events.publish('reloadThemesPage');
    this.navCtrl.pop();
  }

  addNewTag() {
    if (this.newTheme.tags == null) this.newTheme.tags = [];
    this.newTheme.tags.push(this.newTag);
  }

  removeTag(tag: string) {
    this.newTheme.tags = this.newTheme.tags.filter(item => item !== tag);
  }
}
