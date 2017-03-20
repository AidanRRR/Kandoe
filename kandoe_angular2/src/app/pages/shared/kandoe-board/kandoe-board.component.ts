import {Component, OnInit, Input, Output, EventEmitter} from "@angular/core";
import {SessionService} from "../../../services/session.service";
import {Coord} from "../../../domain/coord";
import {SessionCard} from "../../../domain/sessioncard";
import {CircleType} from "../../../domain/circletype";

// Witruimte tussen 2 cirkels op het bord
const CIRCLE_SPACING = 25;
// Aantal cirkels
const CIRCLE_COUNT = 5;
// Graden naar radialen factor
const DEG_TO_RAD = 0.0174533;

export interface BoardCard {

}

@Component({
  selector: 'kandoe-board',
  templateUrl: './kandoe-board.component.html',
  styleUrls: ['./kandoe-board.component.css']
})
export class KandoeBoardComponent implements OnInit {

  constructor(private sessionService: SessionService) {
    this.circleRadius = [0, 1, 2, 3, 4].map(i => {
      return (230 - ((CIRCLE_SPACING + this.circleWidth) * i));
    });
  }

  ngOnInit() {
  }


  private cardAngle(card: SessionCard): number {
    let deg = (360 / this.cards.length) * card.nr;
    return deg * DEG_TO_RAD;
  }

  private cardX(card: SessionCard): number {
    if (this.draggingCard && this.hoveredCard.nr == card.nr) {
      // TODO: geef muis positie terug ipv normale positie op cirkel
      return (this.base.x + this.circleRadius[card.level - 1] * Math.cos(this.cardAngle(card)));
    }
    else {
      return (this.base.x + this.circleRadius[card.level - 1] * Math.cos(this.cardAngle(card)));
    }
  }

  private cardY(card: SessionCard): number {
    return (this.base.x + this.circleRadius[card.level - 1] * Math.sin(this.cardAngle(card)));
  }

  private cardRadius(card: SessionCard): number {
    if (this.hoveredCard != null && (card.card.cardId == this.hoveredCard.card.cardId))
      return 29;
    else
      return 26;
  }

  private hoveredCard: SessionCard = null;

  private mouseEnter(c: SessionCard) {
    this.hoveredCard = c;
  }

  private mouseLeave(c: SessionCard) {
    this.hoveredCard = null;
  }

  private draggingCard: boolean = false;

  private mouseDown(c: SessionCard) {
    //console.log('dragging: ' + c.nr);
    //this.draggingCard = true;
  }

  private mouseUp(c: SessionCard) {
    this.onCardClicked.emit(c.card.cardId);
  }

  private mouseMove(e: any) {
  }

  private strokeColor(c: SessionCard): string {
    if (this.hoveredCard != null && this.hoveredCard.nr == c.nr) {
      return this.circleType == CircleType.Chance ? this.problemColor : this.chanceColor;
    }
    return 'black';
  }

  @Output()
  private onCardClicked: EventEmitter<string> = new EventEmitter<string>();

  @Input()
  private cards: SessionCard[] = [];

  private problemColor: string = 'red';
  private chanceColor: string = '#50aee2';//'blue';
  @Input()
  private circleType: CircleType = CircleType.Chance;

  private base: Coord = {x: 250, y: 250};
  private circleWidth: number = 15;
  private circleRadius: number[];
}
