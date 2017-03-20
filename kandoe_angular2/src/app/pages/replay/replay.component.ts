import {Component, OnInit} from "@angular/core";
import {SessionService, Replay, SessionState} from "../../services/session.service";
import {ActivatedRoute} from "@angular/router";
import {SessionCard} from "../../domain/sessioncard";
import {ChatLine} from "../shared/chat/chat.component";
import {CircleType} from "../../domain/circletype";

@Component({
  selector: 'app-replay',
  templateUrl: './replay.component.html',
  styleUrls: ['./replay.component.css']
})
export class ReplayComponent implements OnInit {

  private sessionId: string;

  constructor(private sessionService: SessionService, private route: ActivatedRoute) {
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.sessionId = params['id'];
      this.sessionService.getReplay(this.sessionId, "d77773c2-8e6f-46eb-84b7-7f418253a008").subscribe(
        data => {
          console.log(data);
          if (data.hasErrors) {
            this.error = data.errorMessages[0];
            return;
          }

          this.replay = data.data;
          this.total = this.replay.snapshots.length;
          if (this.total == 0) {
            this.error = 'Deze sessie heeft geen snapshots';
          }
          else {
            this.mapSnapshot();
          }
        },
        error => {
          console.log("get replay failed");
          console.error(error);
          this.error = 'Ophalen van replay mislukt';
        });
    });
  }


  public chatLog: ChatLine[] = [];
  private cards: SessionCard[] = [];

  private mapSnapshot() {
    let snapshot = this.replay.snapshots[this.position - 1];
    console.log('tot turn: ' + snapshot.turnNr);
    let events = this.replay.events.filter(e => e.turnNr <= snapshot.turnNr);
    console.log(events.length);
    this.circleType = this.replay.session.circleType || CircleType.Chance;
    let state: SessionState = {
      session: this.replay.session,
      sessionId: this.replay.session.sessionId,
      cards: [],
      players: [],
      cardPositions: {},
      cardPicks: {},
      gameStarted: false,
      turnStartTime: null,
      turnNr: 0,
      currentPlayer: null,
      chatLogs: []
    };

    events.forEach(e => {
      state = this.sessionService.updateState(state, e);
    });

    this.chatLog = state.chatLogs
      .map(m => {
        let user = this.sessionService.getUser(state, m.userId);
        if (user == null)
          return null;
        else
          return {user: user.userName, message: m.message};
      })
      .filter(m => m != null);
    this.cards = state.cards.map((c, i) => {
      let sc: SessionCard = {
        card: {cardId: c.cardId, text: c.text, imageUrl: c.imageUrl, themeId: c.themeId},
        color: this.colors[(i % this.colors.length)],
        level: state.cardPositions[c.cardId] || 1,
        text: c.text,
        nr: i + 1
      };
      return sc;
    });
    console.log(state);
  }

  private error: string = null;
  private replay: Replay = null;
  private position: number = 1;
  private total: number = 20;
  private circleType: CircleType = CircleType.Chance;
  private colors: string[] = [
    'lightblue', 'green', 'purple', 'orange', 'blueviolet', 'coral', 'greenyellow',
    'khaki', 'maroon', 'navy', 'orchid', 'seagreen',
  ];

  private onNextClick() {
    if (this.position < this.total) {
      this.position++;
      this.mapSnapshot();
    }
  }

  private onPrevClick() {
    if (this.position > 1) {
      this.position--;
      this.mapSnapshot();
    }
  }
}
