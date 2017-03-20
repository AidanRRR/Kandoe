import { Component } from '@angular/core';

import { ThemesPage } from '../themes/themes';
import {NavParams, NavController} from "ionic-angular";
import {PushService} from "../../services/push.service";
import {SessionsPage} from "../sessions/sessions";
import {InvitePage} from "../invite/invite";

@Component({
  templateUrl: 'tabs.html'
})
/**
 * test
 */
export class TabsPage {
  // this tells the tabs component which Pages
  // should be each tab's root Page
  tab1Root: any = ThemesPage;
  tab2Root: any = SessionsPage;
  tab3Root: any = InvitePage;

  constructor(public navCtrl: NavController,
              public navParams: NavParams,
              private pushService: PushService

  ) {
    this.navCtrl = navCtrl;
  }

  ngOnInit(){
    this.pushService.initPushNotification();
  }
}
