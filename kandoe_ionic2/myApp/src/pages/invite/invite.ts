import { Component } from '@angular/core';
import {NavController, NavParams, ToastController, PopoverController} from 'ionic-angular';
import {SessionService} from "../../services/session.service";
import {Session} from "../../domain/session";
import {Calendar} from 'ionic-native';
import {AccountPopoverPage} from "../account-popover/account-popover";
import {LanguagePopoverPage} from "../language-popover/language-popover";


/*
  Generated class for the Invite page.

  See http://ionicframework.com/docs/v2/components/#navigation for more info on
  Ionic pages and navigation.
*/
@Component({
  selector: 'page-invite',
  templateUrl: 'invite.html'
})
export class InvitePage {

  private invitedSessions: Session[];
  private isEmpty : boolean = true;

  constructor(public navCtrl: NavController,
              public navParams: NavParams,
              private sessionService: SessionService,
              public toastCtrl: ToastController,
              public calendar: Calendar,
              public popoverCtrl: PopoverController
  ) {}

  ngOnInit() {
    this.refreshInvites();
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

  acceptInvite(sessionId: string) {
    console.log(sessionId);
    this.sessionService.acceptInvite(sessionId).subscribe(
      data => {
        if (data.hasErrors) {
          let toast = this.toastCtrl.create({
            message: 'Uitnodiging accepteren mislukt: ' + data.errorMessages[0],
            duration: 1500,
            position: 'top'

          });
          toast.present();
        }
        else {
          this.refreshInvites();
          let toast = this.toastCtrl.create({
            message: 'Uitnodiging geaccepteerd.',
            duration: 1500,
            position: 'top'

          });
          toast.present();
        }
      },
      err => {
        let toast = this.toastCtrl.create({
          message: 'Accepteren uitnodiging mislukt.',
          duration: 1500,
          position: 'top'

        });
        toast.present();
        console.error(err);
      }
    )
  }

  addInviteToCalender(ssn: Session) {
    if (!Calendar.hasWritePermission) {
      Calendar.requestReadWritePermission();
    }

    let startDate = new Date(ssn.scheduledStartTime); // beware: month 0 = january, 11 = december
    let endDate = new Date(ssn.scheduledStartTime);

    Calendar.createEvent(ssn.name, 'Kandoe', 'Session play time', startDate,  new Date(endDate.setHours(endDate.getHours()+1)));
    let toast = this.toastCtrl.create({
      message: 'Event toegevoegd aan kalender',
      duration: 1500,
      position: 'top'

    });
    toast.present();
  }

  refreshInvites() {
    this.sessionService.getPendingInvites().subscribe(
      data => {
        if(!data.hasErrors) {
          this.invitedSessions = data.data;
        }if(data.data.length != 0){
          this.isEmpty=false;
        }else {
          console.error(data.errorMessages);
        }
      },
      err => {
        console.error(err);
      }
    );
  }

  doRefresh(refresher) {
    console.log('Begin async operation', refresher);
    this.refreshInvites();
    setTimeout(() => {
      console.log('Async operation has ended');
      refresher.complete();
    }, 2000);
  }

}
