import {Component, ChangeDetectorRef, Input} from '@angular/core';
import {NavController, NavParams, PopoverController} from 'ionic-angular';
import {AccountPopoverPage} from "../account-popover/account-popover";
import {LanguagePopoverPage} from "../language-popover/language-popover";
import {
  SessionService, SessionStateUpdate, MoveEvent, TurnStartEvent,
  ChatMessageEvent, SessionState
} from "../../services/session.service";
import {SessionPhase} from "../../domain/session";
import {CircleType} from "../../domain/circletype";
import {Card} from "../../domain/card";
import {ChatLine} from "../chat/chat";
import {SessionCard} from "../../domain/sessioncard";
import {UserService} from "../../services/user.service";


/*
  Generated class for the Kandoe page.

  See http://ionicframework.com/docs/v2/components/#navigation for more info on
  Ionic pages and navigation.
*/
@Component({
  selector: 'page-kandoe',
  templateUrl: 'kandoe.html'
})
export class KandoePage {
  private sessionId: string;
  private sessionCards =  [];
  private selectionCards =  [];
  segment: string = "game";
  newMessage:string;
  private currentUser: string;
  messageStyle: string;


  constructor(public navCtrl: NavController,
              private sessionService: SessionService,
              private cdr: ChangeDetectorRef,
              public navParams: NavParams,
              public popoverCtrl: PopoverController,
              private userService: UserService) {}

  ionViewDidLoad() {
    console.log('ionViewDidLoad KandoePage');
  }

  presentAccountPopover(myEvent) {
    let popover = this.popoverCtrl.create(AccountPopoverPage);
    popover.present({
      ev: myEvent
    });
  }

  presentLanguagePopover(myEvent) {
    let popover = this.popoverCtrl.create(LanguagePopoverPage);
    popover.present({
      ev: myEvent
    });
  }



  // oneUp(cardId:any){
  //   this.cardPool.find(x => x.cardId == cardId).level++;
  //   console.log(this.cardPool[cardNr]);
  // }

  ngOnInit() {
    console.log('init session');
    this.currentUser = this.userService.getCurrentUsername();
    this.sessionService.initSignalR();
    setInterval(this.updateTimer.bind(this), 1000);

    this.sessionId = this.navParams.get('sessionId');
    // this.sessionId = '5c1dcc02-6a45-4fb4-8407-d25783e630d5';

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
    console.log("STATE" + state);
    this.circleType = state.session.circleType || CircleType.Chance;
    this.picking = !state.gameStarted;

    this.cardPool = state.session.cards;
    this.sessionCards = this.cardPool;
    console.log(this.cardPool);
    this.phase = state.session.phase;

    this.chatLog = state.chatLogs
      .map(m => {
        let user = this.sessionService.getUser(state, m.userId);
        if (user == null)
          return null;
        else
          return {user: user.userName, message: m.message};
      })
      .filter(m => m != null);

    // TODO: update card model
    this.cards = state.cards.map((c, i) => {
      let sc: SessionCard = {
        card: {cardId: c.cardId, text: c.text, imageUrl: c.imageUrl, themeId: c.themeId},
        color: this.colors[(i % this.colors.length)],
        level: state.cardPositions[c.cardId] || 1,
        nr: i + 1
      };
      console.log(sc);
      return sc;
    });

    this.currentPlayer = state.currentPlayer.userName;
    this.currentTurnStart = new Date(state.turnStartTime).getTime();
    // TODO: session afhankelijk
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
    console.log("ONMOVE" + update);
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

    this.chatLog.push({
      user: event.userId,
      message: event.message
    });

    if (this.currentUser == event.userId){
      this.messageStyle = "personalMessage";
    }else { this.messageStyle = "otherMessage";}

    // Change Detection forceren
    this.cdr.detectChanges();
  }

  private onCardPickSelectionConfirmed(cardIds: string[]) {
    console.log(`Kaarten gekozen: ${JSON.stringify(cardIds)}`);
    this.sessionService.pickCards(this.sessionId, cardIds);
  }

  // Gebruiker typt bericht en drukt op ENTER
  private onChatMessageSend(msg: string) {
    this.newMessage="";
    this.sessionService.sendMessage(this.sessionId, msg);
  }

  private cardPool: Card[] = [];

  private picking: boolean = false;
  private connected: boolean = false;
  private connectError: string = null;
  private phase: SessionPhase = SessionPhase.Active;

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


  @Input()
  public minPicks: number = 1;

  @Input()
  public maxPicks: number = 3;

  private picks: string[] = [];

  private onCardSelectionClick(card: Card) {
    if (card.selected || this.picks.length < this.maxPicks) {
      card.selected = !card.selected;
      this.picks = this.cardPool.filter(c => c.selected == true).map(c => c.cardId);
      console.log("card picked : "+ card.cardId + card.selected);
    }
    console.log(this.cardPool)
  }


  private confirmSelection() {
    if (this.picks.length >= this.minPicks) {
      this.onCardPickSelectionConfirmed(this.picks);
    }
  }

}
