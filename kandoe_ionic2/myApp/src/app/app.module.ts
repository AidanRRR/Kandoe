import { NgModule, ErrorHandler } from '@angular/core';
import { IonicApp, IonicModule, IonicErrorHandler } from 'ionic-angular';
import { MyApp } from './app.component';
import { ThemesPage } from '../pages/themes/themes';
import { TabsPage } from '../pages/tabs/tabs';
import { ChatPage } from '../pages/chat/chat';
import { MomentModule } from 'angular2-moment';
import {LoginPage} from "../pages/login/login";
import {KandoePage} from "../pages/kandoe/kandoe";
import {AuthService} from "../services/auth.service";
import {RegisterPage} from "../pages/register/register";
import {UserService} from "../services/user.service";
import {ThemeService} from "../services/theme.service";
import {CardService} from "../services/card.service";
import {ThemeModalPage} from "../pages/theme-modal/theme-modal";
import {ThemeDetailsModalPage} from "../pages/theme-details-modal/theme-details-modal";
import {EditThemeModalPage} from "../pages/edit-theme-modal/edit-theme-modal";
import {AccountPopoverPage} from "../pages/account-popover/account-popover";
import {LanguagePopoverPage} from "../pages/language-popover/language-popover";
import {AccountPage} from "../pages/account/account";
import {PushService} from "../services/push.service";
import {TranslateModule} from 'ng2-translate/ng2-translate';
import {Http} from "@angular/http";
import {TranslateLoader, TranslateStaticLoader} from "ng2-translate";
import {SessionService} from "../services/session.service";
import {OrderBy} from "../pipes/orderBy";
import {SessionsPage} from "../pages/sessions/sessions";
import {InvitePage} from "../pages/invite/invite";
import {SessionDetailsModalPage} from "../pages/session-details-modal/session-details-modal";
import {Calendar} from "ionic-native";



export function createTranslateLoader(http: Http) {
  return new TranslateStaticLoader(http, 'assets/lang', '.json');
}

@NgModule({
  declarations: [
    MyApp,
    ChatPage,
    ThemesPage,
    LoginPage,
    KandoePage,
    TabsPage,
    OrderBy,
    InvitePage,
    RegisterPage,
    ThemeModalPage,
    ThemeDetailsModalPage,
    EditThemeModalPage,
    AccountPopoverPage,
    LanguagePopoverPage,
    AccountPage,
    SessionsPage,
    SessionDetailsModalPage
  ],
  imports: [
    IonicModule.forRoot(MyApp),
    MomentModule,
    TranslateModule.forRoot({
      provide: TranslateLoader,
      useFactory: (createTranslateLoader),
      deps: [Http]
    })
  ],
  bootstrap: [IonicApp],
  entryComponents: [
    MyApp,
    KandoePage,
    RegisterPage,
    ChatPage,
    ThemesPage,
    LoginPage,
    TabsPage,
    InvitePage,
    ThemeModalPage,
    ThemeDetailsModalPage,
    EditThemeModalPage,
    AccountPopoverPage,
    LanguagePopoverPage,
    AccountPage,
    SessionsPage,
    SessionDetailsModalPage
  ],
  providers: [{provide: ErrorHandler, useClass: IonicErrorHandler},
    AuthService,
    UserService,
    ThemeService,
    CardService,
    PushService,
    SessionService,
    AccountPopoverPage,
    LanguagePopoverPage,
    Calendar
  ]
})
export class AppModule {}
