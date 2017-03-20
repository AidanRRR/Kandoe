import {Component, OnInit} from "@angular/core";
import {SessionService} from "../../services/session.service";
import {Session, SessionPhase} from "../../domain/session";

@Component({
  selector: 'app-sessions',
  templateUrl: './sessions.component.html',
  styleUrls: ['./sessions.component.css']
})
export class SessionsComponent implements OnInit {
  private sessions:Session[];
  private activeSessions: Session[];
  private plannedSessions: Session[];
  private finishedSessions: Session[];

  constructor(private sessionService: SessionService) {
  }

  ngOnInit() {
    this.refreshSessions();
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
}
