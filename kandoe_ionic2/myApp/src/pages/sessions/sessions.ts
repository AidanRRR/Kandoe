import { Component } from '@angular/core';
import {NavController, NavParams, PopoverController, ModalController} from 'ionic-angular';
import {SessionPhase, Session} from "../../domain/session";
import {SessionService} from "../../services/session.service";
import {TranslateService} from 'ng2-translate';
import {LanguagePopoverPage} from "../language-popover/language-popover";
import {AccountPopoverPage} from "../account-popover/account-popover";
import {KandoePage} from "../kandoe/kandoe";
import {SessionDetailsModalPage} from "../session-details-modal/session-details-modal";

/*
  Generated class for the Sessions page.

  See http://ionicframework.com/docs/v2/components/#navigation for more info on
  Ionic pages and navigation.
*/
@Component({
  selector: 'page-sessions',
  templateUrl: 'sessions.html'
})
export class SessionsPage {
  private sessions:Session[];
  private activeSessions: Session[];
  private plannedSessions: Session[];
  private finishedSessions: Session[];

  constructor(public navCtrl: NavController,
              public navParams: NavParams,
              private sessionService: SessionService,
              public popoverCtrl: PopoverController,
              public translate: TranslateService,
              private modalCtrl: ModalController
  ) {
    translate.setDefaultLang('nl');
  }

  ngOnInit() {
    this.refreshSessions();
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

  refreshSessions() {
    this.sessionService.getSessionsCurrentUser().subscribe(
      data => {
        this.sessions = data;
        this.activeSessions = this.sessions.filter(s => s.phase == SessionPhase.Active);
        this.plannedSessions = this.sessions.filter(s => s.phase == SessionPhase.Planned);
        this.finishedSessions = this.sessions.filter(s => s.phase == SessionPhase.Finished);
      },
      error => {
        console.error("get  all sessions failed");
        console.error(error);
      });
  }

  onPlay(sessionId : any) {
    this.navCtrl.push(KandoePage, {
      sessionId : sessionId
    });
  }

  presentDetailsModal(id: string) {
    let modal = this.modalCtrl.create(SessionDetailsModalPage, {"sessionId": id});
    modal.onDidDismiss(data => {
      this.refreshSessions();
    });
    modal.present();
  }

  doRefresh(refresher) {
    console.log('Begin async operation', refresher);
    this.refreshSessions();
    setTimeout(() => {
      console.log('Async operation has ended');
      refresher.complete();
    }, 2000);
  }

}
