import {Injectable, EventEmitter} from "@angular/core";
import {environment} from "../../environments/environment";
import {Card} from "../domain/card";
import {User} from "../domain/user";
import {Session, SessionPhase} from "../domain/session";
import {Observable} from "rxjs";
import {Http, Headers, RequestOptions, Response} from "@angular/http";
import {AuthService} from "./auth.service";

export interface SessionEvent {
  sessionId: string;
  type: string;
  userId: string;
  timestamp: string;
  turnNr: number;
}

export interface SessionSnapshots {
  turnNr: number;
  description: string;
  userCreated: boolean;
}

export interface ChatMessageEvent extends SessionEvent {
  message: string;
}
export interface MoveEvent extends SessionEvent {
  cardId: string;
}
export interface TurnStartEvent extends SessionEvent {
}
export interface TurnExpiredEvent extends SessionEvent {
}
export interface SessionStartEvent extends SessionEvent {
  session: Session;
  players: User[];
}
export interface SessionEndEvent extends SessionEvent {
}
export interface CardPickEvent extends SessionEvent {
  cards: string[];
}
export interface GameStartEvent extends SessionEvent {
  cards: string[];
}

export interface ConnectEvent extends SessionEvent {
  state: SessionState;
}
export interface ErrorEvent extends SessionEvent {
  message: string;
  errorType: string;
}

export interface CardPositions {
  [CardId: string]: number;
}

export interface CardPicks {
  [UserId: string]: string[];
}

export interface ChatMessage {
  timestamp: string;
  userId: string;
  message: string;
}

export interface Snapshot {
  turnNr: number;
  description: string;
  userCreated: boolean;
}

export interface Replay {
  session: Session;
  snapshots: Snapshot[];
  events: SessionEvent[];
}

export interface SessionState {
  session: Session;
  sessionId: string;
  cards: Card[];
  players: User[];
  cardPositions: CardPositions;
  cardPicks: CardPicks;
  gameStarted: boolean;
  turnStartTime: string;
  turnNr: number;
  currentPlayer: User;
  chatLogs: ChatMessage[];
}

export interface SessionStateUpdate {
  event: any;
  oldState: SessionState;
  newState: SessionState;
}

interface SessionConnection {
  sessionId: string;
  emitter: EventEmitter<SessionStateUpdate>;
  state?: SessionState;
}

interface SessionConnectionMap {
  [sessionId: string]: SessionConnection;
}

declare const jQuery: any;
const API = environment.api;
//const API = environment.sessionAPI;
//let API = environment.production ? environment.api : environment.sessionAPI;

@Injectable()
export class SessionService {
  private token: string = null;
  private options;

  constructor(private http: Http, private authService: AuthService) {
    let headers = new Headers({'Content-Type': 'application/json'});
    this.options = new RequestOptions({headers: headers});
  }

  private getToken(): string {
    if (this.token == null) {
      let user = JSON.parse(localStorage.getItem('currentUser'));
      if (user == null || user.token == null) {
        console.error(`Niet ingelogd`);
      }
      this.token = user.token;
    }
    return this.token;
  }

  /**
   * Achterliggende signalR connectie
   */
  private hub: any = null;
  private initialised: Promise<boolean>;

  public initSignalR() {
    // Al geladen
    if (this.hub != null) {
      return;
    }

    if (jQuery == null) {
      console.error(`jQuery niet geladen, signalR kan niet worden geladen...`);
      return;
    }

    let signalR = jQuery.connection.hub;

    signalR.url = environment.signalUrl;
    signalR.logging = false;

    // Binden van client callbacks
    let client = jQuery.connection.SessionHub.client;
    client.onSessionEvent = this.onSessionEvent.bind(this);
    client.onConnect = this.onConnect.bind(this);

    this.hub = jQuery.connection.SessionHub;

    this.initialised = new Promise<boolean>((resolve, reject) => {
      signalR.start({jsonp: true}).done(() => {
        resolve(true);
      });
    });
  }

  /**
   * Event management
   */
  private sessions: SessionConnectionMap = {};

  public subscribe(sessionId: string, eventType: string, callback: (any) => void): any {
    if (this.hub == null) {
      console.error('Geen signalR geladen, SessionService.subscribe wordt genegeerd');
      return;
    }

    let ssn: SessionConnection = this.sessions[sessionId];
    if (ssn == null) {
      console.log('connection aanmaken');
      ssn = {
        sessionId: sessionId,
        emitter: new EventEmitter<SessionStateUpdate>()
      };
      this.sessions[sessionId] = ssn;
      this.initialised.then(() => {
        console.log('Connecting to session: ' + sessionId);
        this.hub.server.connect(this.getToken(), sessionId);
      });
    }

    let sub = ssn.emitter.subscribe((update: SessionStateUpdate) => {
      if (eventType == null || update.event.type == eventType) {
        callback(update);
      }
    });

    return sub;
  }

  public getSessionState(sessionId: string): SessionState {
    let ssn: SessionConnection = this.sessions[sessionId];
    if (ssn == null) {
      return null;
    }
    return ssn.state;
  }

  /**
   * Hulpfuncties
   */
  public getUser(state: SessionState, id: string): User {
    return state.players.filter(p => p.userName == id)[0];
  }

  /**
   * Acties om naar SignalR hub te sturen
   */
  public sendMessage(sessionId: string, message: string) {
    this.hub.server.sendMessage(sessionId, message);
  }

  public moveCard(sessionId: string, cardId: string) {
    this.hub.server.moveCard(sessionId, cardId);
  }

  public pickCards(sessionId: string, cardIds: string[]) {
    this.hub.server.pickCards(sessionId, cardIds);
  }

  public takeSnapshot(sessionId: string) {
    this.hub.server.takeSnapshot(sessionId);
  }

  public endSession(sessionId: string) {
    this.hub.server.endSession(sessionId);
  }

  /**
   * Callbacks van SignalR hub
   */
  private onSessionEvent(e: any) {
    let ssn = this.sessions[e.sessionId];
    if (ssn == null) {
      console.error(`Session niet gevonden: ${JSON.stringify(e)}`);
    }
    else {
      let oldState = ssn.state;
      let newState = this.updateState(oldState, e);
      ssn.state = newState;
      ssn.emitter.emit({
        event: e,
        oldState: oldState,
        newState: newState
      });
    }
  }

  private onConnect(syncedState: any) {
    console.log('on connect');
    console.log(syncedState);
  }

  /**
   * Replays
   */
  private handleError(error: Response | any) {
    // In a real world app, we might use a remote logging infrastructure
    let errMsg: string;
    if (error instanceof Response) {
      const body = error.json() || '';
      const err = body.error || JSON.stringify(body);
      errMsg = `${error.status} - ${error.statusText || ''} ${err}`;
    } else {
      errMsg = error.message ? error.message : error.toString();
    }
    console.error(errMsg);
    return Observable.throw(errMsg);
  }

  public getReplay(sessionId: string, replayKey: string): any {
    let opts = new RequestOptions({headers: new Headers({'Content-Type': 'application/json'})});
    console.log(`${API}/Session/GetReplay/${sessionId}`);
    return this.http.get(`${API}/Session/GetReplay/${sessionId}/${replayKey}`, this.authHeaders())
      .map(resp => resp.json())
      .catch(this.handleError);
  }

  /**
   * state updates
   */
  public updateState(state: SessionState, e: any): SessionState {
    // Werken met een kopie ipv state zelf te updaten
    let updated: SessionState;
    if (state != null) {
      updated = JSON.parse(JSON.stringify(state));
    }

    switch (e.type) {
      case 'CONNECT':
        updated = e.state;
        break;
      case 'CHAT_MESSAGE':
        let cme: ChatMessageEvent = e;
        let msg: ChatMessage = {
          message: cme.message,
          timestamp: cme.timestamp,
          userId: cme.userId
        };
        updated.chatLogs.push(msg);
        break;
      case 'MOVE':
        let me: MoveEvent = e;
        if (updated.cardPositions[me.cardId] == null) {
          console.error(`MoveEvent ongeldige card ID: ${JSON.stringify(me)}`);
        }
        else {
          let pos = updated.cardPositions[me.cardId];
          updated.cardPositions[me.cardId] = pos + 1;
        }
        break;
      case 'TURN_START':
        let tse: TurnStartEvent = e;
        let player = this.getUser(updated, tse.userId);
        if (player == null) {
          console.error('TurnStartEvent ongeldige speler: ${tse}');
        }
        else {
          updated.turnNr = tse.turnNr;
          updated.currentPlayer = player;
          updated.turnStartTime = tse.timestamp;
        }
        break;
      case 'SESSION_START':
        let sse: SessionStartEvent = e;
        updated.session = sse.session;
        updated.players = sse.players;
        break;
      case 'CARD_PICK':
        let cpe: CardPickEvent = e;
        updated.cardPicks[cpe.userId] = cpe.cards;
        break;
      case 'GAME_START':
        let gse: GameStartEvent = e;
        updated.cards = gse.cards.map(cid => updated.session.cards.filter(c => c.cardId == cid)[0]);
        updated.cards.forEach(c => updated.cardPositions[c.cardId] = 1);
        updated.gameStarted = true;
        updated.turnNr = 1;
        updated.turnStartTime = gse.timestamp;
        updated.currentPlayer = updated.players[1];
        break;
      case 'SESSION_END':
        let see: SessionEndEvent = e;
        updated.session.phase = SessionPhase.Finished;
        break;
      case 'ERROR':
        break;
      default:
        console.error(`Onbekend event type: ${e.type}`);
        break;
    }

    return updated;
  }

  /**
   * CRUD acties
   */

  authHeaders(): RequestOptions {
    let token = this.getToken();
    let headers = new Headers({'X-Access-Token': token, 'Content-Type': 'application/json'});
    let options = new RequestOptions({headers: headers});
    return options;
  }

  create(session: Session) {
    return this.http.post(API + '/session/addsession', JSON.stringify(session), this.authHeaders())
      .map(res => res.json())
      .catch(this.handleError);
  }

  getSessionsByTheme(id: string) {
    return this.http.get(API + '/Session/GetSessionsByTheme/' + id, this.options)
      .map(this.extractData)
      .catch(this.handleError);
  }

  getSessionById(id: string) {
    return this.http.get(API + '/Session/GetSessionById/' + id, this.options)
      .map(this.extractData)
      .catch(this.handleError);
  }

  getSessionsCurrentUser(){
    return this.http.get(API + '/Session/getparticipatingsessions', this.authHeaders())
      .map(this.extractData)
      .catch(this.handleError);
  }

  getPendingInvites() {
    return this.http.get(API + '/Session/GetInvitedSessions', this.authHeaders())
      .map(res => res.json())
      .catch(this.handleError);
  }

  acceptInvite(sessionId: string) {
    return this.http.get(API + '/Session/AcceptSessionInvite/' + sessionId, this.authHeaders())
      .map(res => res.json())
      .catch(this.handleError);
  }

  sendInvite(sessionId: string, email: string) {
    let body = {
      email: email,
      sessionId: sessionId
    };
    return this.http.post(API + '/Session/InviteUsersSession', JSON.stringify(body), this.authHeaders())
      .map(res => res.json())
      .catch(this.handleError);
  }

  startSession(sessionId: string) {
    return this.http.get(API + '/Session/StartSession/' + sessionId, this.authHeaders())
      .map(res => res.json())
      .catch(this.handleError);
  }

  private extractData(res: Response) {
    let body = res.json();
    return body.data || {};
  }
}
