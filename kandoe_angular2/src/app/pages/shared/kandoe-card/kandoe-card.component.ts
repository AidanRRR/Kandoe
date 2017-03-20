import {Component, OnInit, Input, EventEmitter, Output, ViewChild} from "@angular/core";
import {Card, CardReaction} from "../../../domain/card";
import {CardService} from "../../../services/card.service";
import {UserService} from "../../../services/user.service";

declare var $: any;
declare const toastr: any;

@Component({
  selector: 'kandoe-card',
  templateUrl: './kandoe-card.component.html',
  styleUrls: ['./kandoe-card.component.css']
})
export class KandoeCardComponent implements OnInit {

  @Input()
  public card: Card;
  @Input()
  public commentable: boolean = true;
  @Input()
  public editable: boolean = false;

  @Output()
  private cardClick: EventEmitter<Card>;

  private reactions: CardReaction[] = [];
  private newReaction: string = '';
  private name: string;

  constructor(private cardService: CardService, private userService: UserService) {
    this.cardClick = new EventEmitter();
    this.name = this.userService.getCurrentUsername();
  }

  ngOnInit() {
  }

  private onCardClick() {
    this.cardClick.emit(this.card);
  }

  private onEditClick(e: any) {
    console.log('edit');
    e.stopPropagation();
  }

  private onCommentsClick(e: any) {
    this.cardService.getCard(this.card.cardId).subscribe(
      data => {
        if (data.hasErrors) {
          toastr.error('Fout bij ophalen review\'s');
          console.log(data.errorMessages[0]);
        }
        else {
          console.log(data.reactions);
          this.reactions = data.reactions;
        }
      },
      error => {
        console.log(error);
        toastr.error('Fout bij ophalen review\'s');
      });
    $("#reactionModal").modal('show');
    e.stopPropagation();
  }

  addReaction(){
    let reaction: CardReaction = {cardId: this.card.cardId, username: this.name, message: this.newReaction};
    this.cardService.addReaction(reaction).subscribe(
      data => {
        if (data.hasErrors) {
          toastr.error('Fout bij ophalen review\'s');
          console.log(data.errorMessages[0]);
        }
        else {
          this.reactions = data.reactions;
        }
      },
      error => {
        console.log(error);
        toastr.error('Fout bij ophalen review\'s');
      });
  }

  close(){
    $('#reactionModal').modal('toggle');
    this.reactions = [];
  }
}
