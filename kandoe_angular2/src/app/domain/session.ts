import {Card} from "./card";
import {CircleType} from "./circletype";
import {SessionEvent, SessionSnapshots} from "../services/session.service";

export enum SessionPhase {
  Planned,
  Active,
  Finished,
}

export interface Session {
  sessionId: string;
  themeId: string;
  name: string;
  description: string;
  replayKey: string;
  circleType?: CircleType;
  cards: Card[];
  invitedUserEmails: string[];
  playerIds: string[];
  turnTime: number;
  pickTime: number;
  minPicks: number;
  maxPicks: number;
  ownerId: string;
  managerIds: string[];
  sessionEvents?: SessionEvent[];
  sessionSnapshots?: SessionSnapshots[];
  phase?: SessionPhase;
  scheduledStartTime?: string;
  scheduledEndTime?: string;
}
