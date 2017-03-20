import {Component} from "@angular/core";
import {NavController, NavParams, ViewController} from "ionic-angular";
import {ThemeService} from "../../services/theme.service";
import {CardService} from "../../services/card.service";
import {TranslateService} from "ng2-translate";
import {Card} from "../../domain/card";
import {Theme} from "../../domain/theme";
import {UserService} from "../../services/user.service";
import { Events } from 'ionic-angular';


@Component({
  selector: 'page-edit-theme-modal',
  templateUrl: 'edit-theme-modal.html'
})
export class EditThemeModalPage {

  private currentThemeId: string;
  private currentTheme: any = {};
  private cards: Card[];
  name: string;
  newTag: string;

  constructor(public navCtrl: NavController,
              public navParams: NavParams,
              public viewCtrl: ViewController,
              private themeService: ThemeService,
              private cardService: CardService,
              private userService: UserService,
              public translate: TranslateService,
  public events: Events) {
    translate.setDefaultLang('nl');
    this.name = this.userService.getCurrentUsername();
  }


  ngOnInit() {
    this.currentThemeId = this.navParams.get("themeId");
    console.log(this.currentThemeId);

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

  saveTheme(){
    this.currentTheme.username = this.name;
    this.themeService.updateTheme(this.currentTheme).subscribe(
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
          //todo: tags werken nog niet
          tags: data.tags,
          username: data.username
        };
        console.log("Edit theme succes");
      },
      error => {
        console.log("Edit theme failed");
      });
    this.events.publish('reloadThemesPage');
    this.navCtrl.pop();


  }

  addNewTag() {
    if (this.currentTheme.tags == null) this.currentTheme.tags = [];
    this.currentTheme.tags.push(this.newTag);
  }

  removeTag(tag: string) {
    this.currentTheme.tags = this.currentTheme.tags.filter(item => item !== tag);
  }

  ionViewDidLoad() {
    console.log('ionViewDidLoad EditThemeModalPage');
  }

  dismiss(){
    this.viewCtrl.dismiss();
  }
}
