import {Component, OnInit, ChangeDetectorRef} from "@angular/core";
import {ActivatedRoute} from "@angular/router";
import {SessionCard} from "../../domain/sessioncard";
import {ChatLine} from "../shared/chat/chat.component";
import {
  SessionService,
  ChatMessageEvent,
  SessionStateUpdate,
  SessionState,
  TurnStartEvent,
  MoveEvent
} from "../../services/session.service";
import {Card} from "../../domain/card";
import {SessionPhase} from "../../domain/session";
import {CircleType} from "../../domain/circletype";

@Component({
  selector: 'app-session',
  templateUrl: './session.component.html',
  styleUrls: ['./session.component.css']
})
export class SessionComponent implements OnInit {
  private sessionId: string = 'a';

  constructor(private sessionService: SessionService, private cdr: ChangeDetectorRef, private route: ActivatedRoute) {
  }

  ngOnInit() {
    this.sessionService.initSignalR();
    setInterval(this.updateTimer.bind(this), 1000);

    this.route.params.subscribe(params => {
      this.sessionId = params['id'];

      this.sessionService.subscribe(this.sessionId, 'CHAT_MESSAGE', this.onChatEvent.bind(this));
      this.sessionService.subscribe(this.sessionId, 'TURN_START', this.onTurnStart.bind(this));
      this.sessionService.subscribe(this.sessionId, 'MOVE', this.onMove.bind(this));
      this.sessionService.subscribe(this.sessionId, 'SESSION_START', this.onSessionStart.bind(this))
      this.sessionService.subscribe(this.sessionId, 'CARD_PICK', this.onCardPick.bind(this))
      this.sessionService.subscribe(this.sessionId, 'GAME_START', this.onGameStart.bind(this))
      this.sessionService.subscribe(this.sessionId, 'SESSION_END', this.onSessionEnd.bind(this))
      this.sessionService.subscribe(this.sessionId, 'ERROR', this.onError.bind(this));

      let state = this.sessionService.getSessionState(this.sessionId);
      if (state != null) {
        this.mapState(state);
        this.connected = true;
      }
      this.sessionService.subscribe(this.sessionId, 'CONNECT', this.onConnect.bind(this));
    });
  }

  private onSessionStart(update: SessionStateUpdate) {
    //this.mapState(update.newState);
    console.log('session start...?');
  }

  private onCardPick(update: SessionStateUpdate) {
    console.log('nieuwe picks: ');
    console.log(update.event);
  }

  private onGameStart(update: SessionStateUpdate) {
    console.log('game start');
    this.mapState(update.newState);
  }

  private onSessionEnd(update: SessionStateUpdate) {
    console.log('session end event ontvangen...');
    this.phase = SessionPhase.Finished;
  }

  private onError(update: SessionStateUpdate) {
    let e: any = update.event;
    console.log('onerror');
    if (e.errorType == "CONNECT") {
      console.log('connect err');
      this.connectError = e.message;
    }
    else {
      console.error(e);
    }
  }

  // TODO: ngOnDestroy

  // Volledige state laden
  private mapState(state: SessionState) {
    this.circleType = state.session.circleType || CircleType.Chance;
    this.picking = !state.gameStarted;

    this.cardPool = state.session.cards;
    this.phase = state.session.phase;

    this.maxPicks = state.session.maxPicks;
    this.minPicks = state.session['minPicks'];

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

    this.currentPlayer = state.currentPlayer.userName;
    this.currentTurnStart = new Date(state.turnStartTime).getTime();
    this.currentTurnEnd = this.currentTurnStart + (state.session.turnTime * 1000);

    // Change detector forceren
    this.cdr.detectChanges();
  }

  private onConnect(update: SessionStateUpdate) {
    console.log('connected, syncing state');
    console.log(update.newState);
    this.mapState(update.newState);
    this.connected = true;
  }

  /**
   *
   */
  private audio = new Audio('assets/sounds/pop.mp3');

  private onMove(update: SessionStateUpdate) {
    let e: MoveEvent = update.event;
    let card = this.cards.filter(c => c.card.cardId == e.cardId)[0];
    card.level++;

    // Change detector forceren
    this.cdr.detectChanges();
    this.audio.play();
  }

  private onCardClick(cardId: string) {
    console.log('click: ' + cardId);
    this.sessionService.moveCard(this.sessionId, cardId);
  }

  /**
   *
   */
  private updateTimer() {
    this.timeLeft = this.currentTurnEnd - Date.now();
  }

  private onTurnStart(update: SessionStateUpdate) {
    let e: TurnStartEvent = update.event;

    console.log('TURN START');
    console.log(e);

    let user = this.sessionService.getUser(update.newState, e.userId);
    if (user != null) {
      this.currentPlayer = user.userName;
      this.currentTurnStart = new Date(e.timestamp).getTime();
      this.currentTurnEnd = this.currentTurnStart + (update.newState.session.turnTime * 1000);
      this.updateTimer();
      this.cdr.detectChanges();
    }
    else {
      this.currentPlayer = '?????';
    }
  }

  private onSnapshotClick() {
    this.sessionService.takeSnapshot(this.sessionId);
  }

  private onSessionEndClick() {
    console.log('sessie beeindigen...');
    this.sessionService.endSession(this.sessionId);
  }

  /**
   * Chat
   */
  private onChatEvent(update: SessionStateUpdate) {
    let event: ChatMessageEvent = update.event;
    let user = this.sessionService.getUser(update.newState, event.userId);

    if (user == null) {
      console.error(`Ongeldige UserId ${event.userId}`)
      console.error(update.newState);
      return;
    }
    this.chatLog.push({
      user: user.userName,
      message: event.message
    });

    // Change Detection forceren
    this.cdr.detectChanges();
  }

  private onCardPickSelectionConfirmed(cardIds: string[]) {
    console.log(`Kaarten gekozen: ${JSON.stringify(cardIds)}`);
    this.sessionService.pickCards(this.sessionId, cardIds);
  }

  // Gebruiker typt bericht en drukt op ENTER
  private onChatMessageSend(msg: string) {
    this.sessionService.sendMessage(this.sessionId, msg);
  }

  private cardPool: Card[] = [];

  private picking: boolean = false;
  private connected: boolean = false;
  private connectError: string = null;
  private phase: SessionPhase = SessionPhase.Active;
  private minPicks: number = 0;
  private maxPicks: number = 2;

  private currentTurnStart: number;
  private currentTurnEnd: number;
  private timeLeft: number = 0;
  private currentPlayer: string = null;
  public chatLog: ChatLine[] = [];
  private cards: SessionCard[] = [];
  private circleType: CircleType = CircleType.Chance;

  private colors: string[] = [
    'lightblue', 'green', 'purple', 'orange', 'blueviolet', 'coral', 'greenyellow',
    'khaki', 'maroon', 'navy', 'orchid', 'seagreen',
  ];
}
