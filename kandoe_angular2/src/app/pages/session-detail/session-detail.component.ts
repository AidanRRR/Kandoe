import {Component, OnInit} from "@angular/core";
import {ActivatedRoute, Router} from "@angular/router";
import {UserService} from "../../services/user.service";
import {SessionService} from "../../services/session.service";
import {ThemeService} from "../../services/theme.service";
import {Card} from "../../domain/card";
import {Session} from "../../domain/session";

declare const toastr: any;

@Component({
  selector: 'session-detail',
  templateUrl: './session-detail.component.html',
  styleUrls: ['./session-detail.component.css']
})
export class SessionDetailComponent implements OnInit {

  private currentSessionId: string;
  private currentSession: any = {};

  private themanaam: string = '';

  private cloneSessionName: string = '';

  private cards: Card[];

  private isOrganizer: boolean = false;
  private isPublic: boolean = false;


  constructor(private route: ActivatedRoute, private router: Router, private themeService: ThemeService, private userService: UserService, private sessionService: SessionService) {
  }

  ngOnInit() {
    if (this.route.params != null) {
      this.route.params.subscribe(params => this.currentSessionId = params['id']);
    }

    let username = this.userService.getCurrentUsername();

    this.sessionService.getSessionById(this.currentSessionId).subscribe(
      data => {
        this.currentSession = data;
        this.cards = data.cards;
        console.log("get session succes");
        this.themeService.getTheme(this.currentSession.themeId).subscribe(
          data => {
            this.themanaam = data.name;
            if (data.organizers.indexOf(username) > 1 || data.username === username) this.isOrganizer = true;
            this.isPublic = data.isPublic;
            console.log("get organizer bool succes");
          },
          error => {
            console.log("get organizer bool failed");
          });
      },
      error => {
        console.log("get session failed");
      });
  }

  navigateToTheme() {
    this.router.navigate(['/theme', this.currentSession.themeId]);
  }

  private inviteEmail: string = '';
  sendInvite() {
    console.log('invite: ' + this.inviteEmail);
    if (this.inviteEmail.length == 0)
      return;

    this.sessionService.sendInvite(this.currentSessionId, this.inviteEmail).subscribe(
      data => {
        if (data.hasErrors) {
          toastr.error("Uitnodigen mislukt: " + data.errorMessages[0]);
        }
        else {
          toastr.success("Uitnodiging verzonden.");
        }
        this.inviteEmail = '';
      },
      error => {
        toastr.error('Uitnodigen mislukt.');
        console.error(error);
      }
    )
  }

  startSession() {
    this.sessionService.startSession(this.currentSessionId).subscribe(
      data => {
        if (data.hasErrors) {
          toastr.error('Sessie starten mislukt: ' + data.errorMessages[0]);
        }
        else {
          toastr.success('Sessie gestart.');
        }
      },
      err => {
        toastr.error('Sessie starten mislukt');
        console.error(err);
      }
    )
  }

  cloneSession(){
    let newSession: Session = this.currentSession;
    newSession.name = this.cloneSessionName;
    this.sessionService.create(newSession).subscribe(
      data => {
        console.log("clone session succes");
        toastr.success("Sessie toevoegen gelukt!");
      },
      error => {
        console.log("clone session failed");
        toastr.error("Sessie toevoegen mislukt!");
      });
  }

  replaySession(){
    this.router.navigate(['/replay', this.currentSession.replayKey]);
  }
}
