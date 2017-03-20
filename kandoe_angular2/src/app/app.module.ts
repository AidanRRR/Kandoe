import {BrowserModule} from "@angular/platform-browser";
import {NgModule} from "@angular/core";
import {FormsModule} from "@angular/forms";
import {HttpModule} from "@angular/http";
import {AppRoutingModule} from "./app-routing.module";
import {AppComponent} from "./app.component";
import {MyDatePickerModule} from "mydatepicker";
// Paginas
import {AboutComponent} from "./pages/about/about.component";
import {LoginComponent} from "./pages/login/login.component";
import {RegisterComponent} from "./pages/register/register.component";
import {SessionComponent} from "./pages/session/session.component";
import {ProfileComponent} from "./pages/profile/profile.component";
import {ThemesComponent} from "./pages/themes/themes.component";
import {ThemeDetailComponent} from "./pages/theme-detail/theme-detail.component";
// Services
import {UserService} from "./services/user.service";
import {AuthService} from "./services/auth.service";
import {SessionService} from "./services/session.service";
import {AuthGuardService} from "./services/auth-guard.service";
// Gedeelde componenten
import {ModalDialogComponent} from "./pages/shared/modal-dialog/modal-dialog.component";
import {KandoeBoardComponent} from "./pages/shared/kandoe-board/kandoe-board.component";
import {KandoeCardComponent} from "./pages/shared/kandoe-card/kandoe-card.component";
import {PageNotFoundComponent} from "./pages/not-found/not-found.component";
import {HeaderComponent} from "./pages/shared/header/header.component";
import {ThemeCardComponent} from "./pages/shared/theme-card/theme-card.component";
import {ChatComponent} from "./pages/shared/chat/chat.component";
import {ThemeService} from "./services/theme.service";
import {CardService} from "./services/card.service";
import {TimespanPipe} from "./pipes/timespan.pipe";
import {CardPickerComponent} from "./pages/shared/card-picker/card-picker.component";
import {CsvService} from "./services/csv.service";
import {SessionCardComponent} from "./pages/shared/session-card/session-card.component";
import {PublicThemesComponent} from "./pages/public-themes/public-themes.component";
import {SessionsComponent} from "./pages/sessions/sessions.component";
import {ReplayComponent} from "./pages/replay/replay.component";
import {ImageService} from "./services/image.service";
// Vertaling module
import { TranslateModule } from 'ng2-translate/ng2-translate';
import {Http} from "@angular/http";
import {TranslateLoader, TranslateStaticLoader} from "ng2-translate";


export function createTranslateLoader(http: Http) {
  return new TranslateStaticLoader(http, 'assets/lang', '.json');
}
import {SessionDetailComponent} from "./pages/session-detail/session-detail.component";
import {InvitesComponent} from "./pages/invites/invites.component";


@NgModule({
  declarations: [
    AppComponent,
    AboutComponent,
    LoginComponent,
    RegisterComponent,
    ModalDialogComponent,
    KandoeBoardComponent,
    SessionComponent,
    KandoeCardComponent,
    ProfileComponent,
    PageNotFoundComponent,
    HeaderComponent,
    LoginComponent,
    RegisterComponent,
    AboutComponent,
    ThemesComponent,
    ThemeCardComponent,
    ThemeDetailComponent,
    ChatComponent,
    TimespanPipe,
    CardPickerComponent,
    SessionCardComponent,
    ReplayComponent,
    PublicThemesComponent,
    SessionsComponent,
    SessionDetailComponent,
    InvitesComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    AppRoutingModule,
    MyDatePickerModule,
    TranslateModule.forRoot({
      provide: TranslateLoader,
      useFactory: (createTranslateLoader),
      deps: [Http]
    })
  ],
  providers: [UserService, AuthService, SessionService, AuthGuardService, ThemeService, CardService, CsvService, ImageService],
  bootstrap: [AppComponent]
})
export class AppModule {
}
