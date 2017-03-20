import {Card} from "./card";

export interface SessionCard {
  nr: number;
  color: string;
  text?: string;
  level: number;
  card: Card;
}