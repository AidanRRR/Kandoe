import {Component, OnInit} from "@angular/core";
import {SessionService} from '../../services/session.service';
import {Session} from '../../domain/session';

declare const toastr: any;

@Component({
  selector: 'app-invites',
  templateUrl: './invites.component.html',
  styleUrls: ['./invites.component.css']
})
export class InvitesComponent implements OnInit {

  private invitedSessions: Session[];

  constructor(private sessionService: SessionService) {
  }

  ngOnInit() {
    this.refreshInvites();
  }

  acceptInvite(sessionId: string) {
    console.log(sessionId);
    this.sessionService.acceptInvite(sessionId).subscribe(
      data => {
        if (data.hasErrors) {
          toastr.error('Uitnodiging accepteren mislukt');
          console.log(data.errorMessages[0]);
        }
        else {
          this.refreshInvites();
          toastr.success('Uitnodiging geaccepteerd.');
        }
      },
      err => {
        toastr.error('Accepteren uitnodiging mislukt.');
        console.error(err);
      }
    )
  }

  refreshInvites() {
    this.sessionService.getPendingInvites().subscribe(
      data => {
        if(!data.hasErrors) {
          this.invitedSessions = data.data;
        }
        else {
          toastr.error('Fout bij vernieuwen');
          console.error(data.errorMessages);
        }
      },
      err => {
        toastr.error('Fout bij vernieuwen');
        console.error(err);
      }
    );
  }
}
