import { Component, OnInit } from '@angular/core';
import {Theme} from "../../domain/theme";
import {ThemeService} from "../../services/theme.service";
import {UserService} from "../../services/user.service";
import {ModalController, PopoverController, Events, NavController} from 'ionic-angular';
import {ThemeModalPage} from "../theme-modal/theme-modal";
import {AccountPopoverPage} from "../account-popover/account-popover";
import {ThemeDetailsModalPage} from "../theme-details-modal/theme-details-modal";
import {EditThemeModalPage} from "../edit-theme-modal/edit-theme-modal";
import {LanguagePopoverPage} from "../language-popover/language-popover";
import {TranslateService} from "ng2-translate";


@Component({
  selector: 'themes',
  templateUrl: './themes.html'
})
export class ThemesPage implements OnInit {

  private newTheme:any = {};
  private publicThemes:Theme[] =[];
  private filterPublicThemes:Theme[] = [];
  private myThemes:Theme[] =[];
  private filterMyThemes:Theme[] = [];
  private name:string;

  searchQuery: string ='';
  items: string[];

  constructor(private themeService: ThemeService,public navCtrl: NavController, private userService: UserService, private modalCtrl: ModalController, private popoverCtrl: PopoverController, public translate: TranslateService, public events: Events) {
    translate.setDefaultLang('nl');
    this.name = this.userService.getCurrentUsername();
    this.events.subscribe('reloadThemesPage',() => {
      this.loadMyThemes();
      this.loadPublicThemes();
    })
  }

  ngOnInit() {
    this.loadMyThemes();
    this.loadPublicThemes();
    this.events.subscribe('reloadThemesPage',() => {
      this.loadMyThemes();
      this.loadPublicThemes();
    })
  }

  loadMyThemes(){
    this.themeService.getThemesByUser(this.name).subscribe(
      data => {
        this.myThemes = data;
        this.filterMyThemes = data;
        console.log("get my themes succes");
      },
      error => {
        console.log("get my themes failed");
      });
  }

  loadPublicThemes(){
    this.themeService.getPublicThemes().subscribe(
      data => {
        this.publicThemes = data;
        this.filterPublicThemes = data;
        console.log("get all public themes succes");
      },
      error => {
        console.log("get  all public themes failed");
      });
  }

  deleteTheme(themeId: string){
    this.themeService.removeTheme(themeId).subscribe(
      data => {
        console.log("Delete theme succes");
        this.loadMyThemes();
      },
      error => {
        console.log("Delete theme failed");
      });
  }


  addNewTag(){
    if (this.newTheme.tags == null) this.newTheme.tags = [];
    this.newTheme.tags.push(this.newTheme.newTag);
  }

  removeTag(tag: string){
    this.newTheme.tags = this.newTheme.tags.filter(item => item !== tag);
  }

  getThemes(searchValue: any) {

    // set val to the value of the searchbar
    let val = searchValue.target.value;

    // if the value is an empty string don't filter the items
    if (val && val.trim() != '') {
      this.filterPublicThemes = this.publicThemes.filter((item) => {
        if (item.name.toLowerCase().indexOf(val.toLowerCase()) > -1) return true;
        else if (item.description.toLowerCase().indexOf(val.toLowerCase()) > -1) return true;
        else if (item.tags.toString().toLowerCase().indexOf(val.toLowerCase()) > -1) return true;
      });
    }else{
      this.filterPublicThemes = this.publicThemes;
    }

    // if the value is an empty string don't filter the items
    if (val && val.trim() != '') {
      this.filterMyThemes = this.myThemes.filter((item) => {
        if (item.name.toLowerCase().indexOf(val.toLowerCase()) > -1) return true;
        else if (item.description.toLowerCase().indexOf(val.toLowerCase()) > -1) return true;
        else if (item.tags.toString().toLowerCase().indexOf(val.toLowerCase()) > -1) return true;
      });
    }else{
      this.filterMyThemes = this.myThemes;
    }
  }

  presentModal() {
    let modal = this.modalCtrl.create(ThemeModalPage);
    modal.present();
  }

  presentDetailsModal(id: string) {
    let modal = this.modalCtrl.create(ThemeDetailsModalPage, {"themeId": id});
    modal.onDidDismiss(data => {
      this.loadMyThemes();
    });
    modal.present();
  }

  presentAccountPopover(myEvent) {
    let popover = this.popoverCtrl.create(AccountPopoverPage);
    popover.present({
      ev: myEvent
    });
  }

  presentLanguagePopover(myEvent) {
    let popover = this.popoverCtrl.create(LanguagePopoverPage);
    popover.present({
      ev: myEvent
    });
  }

  presenEditThemeModal(id: string) {
    let modal = this.modalCtrl.create(EditThemeModalPage, {"themeId": id});
    modal.present();
  }

  doRefresh(refresher) {
    console.log('Begin async operation', refresher);
    this.ngOnInit();
    setTimeout(() => {
      console.log('Async operation has ended');
      refresher.complete();
    }, 2000);
  }

}
