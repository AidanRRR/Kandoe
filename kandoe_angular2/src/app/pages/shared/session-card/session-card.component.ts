import {Component, OnInit, Input} from "@angular/core";
import {Session} from "../../../domain/session";
import {Router} from "@angular/router";
import {UserService} from "../../../services/user.service";

@Component({
  selector: 'session-card',
  templateUrl: './session-card.component.html',
  styleUrls: ['./session-card.component.css']
})
export class SessionCardComponent implements OnInit {

  private username: string;
  private loggedIn: boolean = false;

  constructor(private router: Router, private userService: UserService) {
    this.username = this.userService.getCurrentUsername();
    this.loggedIn = !!this.username;
  }

  ngOnInit() {
  }

  @Input()
  public session: Session;

  private onEditClick(id: string) {
    this.router.navigate(['/session', id]);
  }

  onPlay() {
    this.router.navigate(['/play', this.session.sessionId]);
  }

  onReplay() {
    this.router.navigate(['/replay', this.session.sessionId]);
  }
}
