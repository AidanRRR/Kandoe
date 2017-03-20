import { Component } from '@angular/core';
import {NavController, NavParams, ViewController, ToastController} from 'ionic-angular';
import {SessionService} from "../../services/session.service";
import {TranslateService} from "ng2-translate";
import {ThemeService} from "../../services/theme.service";
import {Card} from "../../domain/card";
import {UserService} from "../../services/user.service";

/*
  Generated class for the SessionDetailsModal page.

  See http://ionicframework.com/docs/v2/components/#navigation for more info on
  Ionic pages and navigation.
*/
@Component({
  selector: 'page-session-details-modal',
  templateUrl: 'session-details-modal.html'
})
export class SessionDetailsModalPage {

  private currentSessionId: string;
  private currentSession: any = {};
  private themeName: string;
  private isOrganizer: boolean = false;
  private cards: Card[];

  constructor(public navCtrl: NavController,
              public navParams: NavParams,
              private sessionService: SessionService,
              public translate: TranslateService,
              public viewCtrl: ViewController,
              private themeService: ThemeService,
              public toastCtrl: ToastController,
              private userService: UserService
  ) {
    translate.setDefaultLang('nl');
  }

  ngOnInit() {
    this.currentSessionId = this.navParams.get("sessionId");
    let username = this.userService.getCurrentUsername();
    this.sessionService.getSessionById(this.currentSessionId).subscribe(
      data => {
        this.currentSession = data;
        console.log("get session succes");
        this.themeService.getTheme(this.currentSession.themeId).subscribe(
          data => {
            console.log("TESTCARDS:" + data.cards);
            this.themeName = data.name;
            this.cards = data.cards;
            if (data.organizers.indexOf(username) > 1 || data.username === username) this.isOrganizer = true;
          }
        )
      },
      error => {
        console.log("get session with id " + this.currentSessionId + " failed");
      });
  }

  dismiss(){
    this.viewCtrl.dismiss();
  }


  startSession() {
    this.sessionService.startSession(this.currentSessionId).subscribe(
      data => {
        if (data.hasErrors) {
          let toast = this.toastCtrl.create({
            message: 'Sessie starten mislukt: ' + data.errorMessages[0],
            duration: 1500,
            position: 'top'

          });
          toast.present();
        }
        else {
          let toast = this.toastCtrl.create({
            message: 'Sessie gestart.',
            duration: 1500,
            position: 'top'

          });
          toast.present();
        }
      },
      err => {
        let toast = this.toastCtrl.create({
          message: 'Sessie starten mislukt',
          duration: 1500,
          position: 'top'

        });
        toast.present();
        console.error(err);
      }
    )
  }

  ionViewDidLoad() {
    console.log('ionViewDidLoad SessionDetailsModalPage');
  }

}
