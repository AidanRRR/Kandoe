import {Component, OnInit, Input, Output, EventEmitter} from "@angular/core";
import {Card} from "../../../domain/card";

@Component({
  selector: 'card-picker',
  templateUrl: './card-picker.component.html',
  styleUrls: ['./card-picker.component.css']
})
export class CardPickerComponent implements OnInit {

  constructor() {
  }

  ngOnInit() {
  }

  @Input()
  public cards: Card[] = [];

  @Input()
  public minPicks: number = 1;

  @Input()
  public maxPicks: number = 3;

  private picks: string[] = [];

  private onCardClick(card: Card) {
    if (card.selected || this.picks.length < this.maxPicks) {
      card.selected = !card.selected;
      this.picks = this.cards.filter(c => c.selected == true).map(c => c.cardId);
    }
  }

  private confirmSelection() {
    if (this.picks.length >= this.minPicks) {
      this.selectionConfirmed.emit(this.picks);
    }
  }

  @Output()
  public selectionConfirmed: EventEmitter<string[]> = new EventEmitter<string[]>();
}
