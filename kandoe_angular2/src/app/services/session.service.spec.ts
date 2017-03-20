/* tslint:disable:no-unused-variable */
import {TestBed, inject} from "@angular/core/testing";
import {
  SessionService,
  SessionState,
  ChatMessage,
  ChatMessageEvent,
  TurnStartEvent,
  MoveEvent
} from "./session.service";
import { HttpModule } from '@angular/http';
import { AuthService } from '../services/auth.service';

const BEGIN_STATE: SessionState = {
  turnNr: 1,
  turnStartTime: new Date().toUTCString(),
  cards: [
    {cardId: 'c1',imageUrl: '', text: 'Kaart 1', themeId: 't1'},
    {cardId: 'c2',imageUrl: '', text: 'Kaart 2', themeId: 't1'},
    {cardId: 'c3',imageUrl: '', text: 'Kaart 3', themeId: 't1'},
    {cardId: 'c4',imageUrl: '', text: 'Kaart 4', themeId: 't1'},
    {cardId: 'c5',imageUrl: '', text: 'Kaart 5', themeId: 't1'},
  ],
  chatLogs: [
    {userId: '2', message: 'Hallo', timestamp: new Date().toUTCString()},
  ],
  cardPositions: {c1: 1, c2: 1, c3: 1, c4: 1, c5: 1},
  currentPlayer: {userName: 'Maarten', email: '', notifications: false},
  players: [
    {userName: 'Maarten', email: '', notifications: false},
    {userName: 'Vincent', email: '', notifications: false},
    {userName: 'Aidan', email: '', notifications: false},
  ],
  cardPicks: {},
  gameStarted: true,
  session: {
    sessionId: 'a',
    themeId: 't1',
    name: 'a',
    description: 'a',
    replayKey: 'a',
    circleType: 0,
    cards: [
      {cardId: 'c1', imageUrl: '', text: 'Kaart 1', themeId: 't1'},
      {cardId: 'c2', imageUrl: '', text: 'Kaart 2', themeId: 't1'},
      {cardId: 'c3', imageUrl: '', text: 'Kaart 3', themeId: 't1'},
      {cardId: 'c4', imageUrl: '', text: 'Kaart 4', themeId: 't1'},
      {cardId: 'c5', imageUrl: '',  text: 'Kaart 5', themeId: 't1'},
    ],
    invitedUserEmails: [],
    playerIds: [],
    turnTime: 30,
    pickTime: 30,
    minPicks: 0,
    maxPicks: 3,
    ownerId: '',
    managerIds: [],
    phase: 0
  },
  sessionId: 'a'
};

describe('SessionService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpModule],
      providers: [SessionService, AuthService]
    });
  });

  it('should load', inject([SessionService], (service: SessionService) => {
    expect(service).toBeTruthy();
  }));

  it('should update state with chat events', inject([SessionService], (service: SessionService) => {
    expect(service).toBeTruthy();
    let s0: SessionState = BEGIN_STATE;
    expect(s0.chatLogs.length).toEqual(1);
    let date = new Date().toUTCString();
    let e: ChatMessageEvent = {
      sessionId: 'a',
      timestamp: date,
      message: 'test bericht',
      turnNr: 1,
      type: 'CHAT_MESSAGE',
      userId: '1'
    };
    let s1 = service.updateState(s0, e);

    expect(s1.chatLogs.length).toEqual(2);
    // Input state mag niet gemuteerd worden
    expect(s0.chatLogs.length).toEqual(1);

    // Laatste bericht is nieuwste
    let msg: ChatMessage = s1.chatLogs[1];
    expect(msg.message).toEqual('test bericht');
    expect(msg.userId).toEqual('1');
    expect(msg.timestamp).toEqual(date);
  }));

  it('should update state with move events', inject([SessionService], (service: SessionService) => {
    expect(service).toBeTruthy();
    let s0: SessionState = BEGIN_STATE;
    let date = new Date().toUTCString();

    let updatedCard = 'c1';
    let otherCards = ['c2', 'c3', 'c4', 'c5'];

    expect(s0.cardPositions[updatedCard]).toBe(1);
    otherCards.forEach(c => expect(s0.cardPositions[c]).toBe(1));

    let e: MoveEvent = {
      sessionId: 'a',
      timestamp: date,
      cardId: updatedCard,
      turnNr: 1,
      type: 'MOVE',
      userId: '1'
    };

    let s1 = service.updateState(s0, e);
    // Originele state niet gemuteerd
    expect(s0.cardPositions[updatedCard]).toBe(1);
    otherCards.forEach(c => expect(s0.cardPositions[c]).toBe(1));
    // Verplaatste kaart geupdate in nieuwe state
    expect(s1.cardPositions[updatedCard]).toBe(2);
    // Andere kaarten niet veranderd in nieuwe state
    otherCards.forEach(c => expect(s1.cardPositions[c]).toBe(1));
  }));

  it('should update state with turn start events', inject([SessionService], (service: SessionService) => {
    expect(service).toBeTruthy();

    let s0: SessionState = BEGIN_STATE;
    expect(s0.turnNr).toEqual(1);
    expect(s0.currentPlayer.userName).toEqual('Maarten');

    let e: TurnStartEvent = {
      sessionId: 'a',
      timestamp: new Date().toUTCString(),
      turnNr: 2,
      type: 'TURN_START',
      userId: 'Vincent'
    };

    let s1 = service.updateState(s0, e);
    // Originele state mag niet veranderd zijn
    expect(s0.turnNr).toEqual(1);
    expect(s0.currentPlayer.userName).toEqual('Maarten');
    // Nieuwe state moet speler aan de beurt en beurt nr. updaten
    expect(s1.turnNr).toEqual(e.turnNr);
    expect(s1.currentPlayer.userName).toEqual('Vincent');
  }));


  it('should update state with session start events', inject([SessionService], (service: SessionService) => {
    expect(service).toBeTruthy();

    let s0: SessionState = JSON.parse(JSON.stringify(BEGIN_STATE));
    s0.gameStarted = false;
  }));

  it('should update state with card pick events', inject([SessionService], (service: SessionService) => {
    expect(service).toBeTruthy();
  }));

  it('should update state with game start events', inject([SessionService], (service: SessionService) => {
    expect(service).toBeTruthy();
  }));
});
