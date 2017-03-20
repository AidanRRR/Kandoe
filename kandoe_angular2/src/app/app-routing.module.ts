import {NgModule} from "@angular/core";
import {RouterModule, Routes} from "@angular/router";
import {AboutComponent} from "./pages/about/about.component";
import {LoginComponent} from "./pages/login/login.component";
import {RegisterComponent} from "./pages/register/register.component";
import {ProfileComponent} from "./pages/profile/profile.component";
import {AuthGuardService} from "./services/auth-guard.service";
import {PageNotFoundComponent} from "./pages/not-found/not-found.component";
import {ThemesComponent} from "./pages/themes/themes.component";
import {ThemeDetailComponent} from "./pages/theme-detail/theme-detail.component";
import {PublicThemesComponent} from "./pages/public-themes/public-themes.component";
import {ReplayComponent} from "./pages/replay/replay.component";
import {SessionsComponent} from "./pages/sessions/sessions.component";
import {SessionDetailComponent} from "./pages/session-detail/session-detail.component";
import {InvitesComponent} from "./pages/invites/invites.component";
import {SessionComponent} from "./pages/session/session.component";

const routes: Routes = [
  {path: '', redirectTo: '/about', pathMatch: 'full'},
  {path: 'about', component: AboutComponent},
  {path: 'login', component: LoginComponent},
  {path: 'register', component: RegisterComponent},
  {path: 'sessions', component: SessionsComponent, canActivate: [AuthGuardService]},
  {path: 'session/:id', component: SessionDetailComponent},
  {path: 'play/:id', component: SessionComponent, canActivate: [AuthGuardService]},
  {path: 'replay/:id', component: ReplayComponent, canActivate: [AuthGuardService]},
  {path: 'themes', component: ThemesComponent, canActivate: [AuthGuardService]},
  {path: 'publicThemes/:filter', component: PublicThemesComponent},
  {path: 'invites', component: InvitesComponent, canActivate: [AuthGuardService]},
  {path: 'theme/:id', component: ThemeDetailComponent},
  {path: 'profile', component: ProfileComponent, canActivate: [AuthGuardService]},
  {path: '**', component: PageNotFoundComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
