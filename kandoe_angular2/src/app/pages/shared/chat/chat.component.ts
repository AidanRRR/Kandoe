import {Component, OnInit, Input, EventEmitter, Output, ElementRef, AfterViewChecked, ViewChild} from "@angular/core";

export interface ChatLine {
  user: string;
  message: string;
}

@Component({
  selector: 'chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit, AfterViewChecked {

  constructor() {
  }

  ngOnInit() {
    this.scrollDown();
  }

  ngAfterViewChecked() {
    this.scrollDown();
  }

  @ViewChild('chatBody')
  private chatBody: ElementRef;

  // Scroll naar beneden in de chat elke keer de view update
  // zodat we altijd de nieuwste berichten te zien krijgen
  private scrollDown() {
    try {
      let elem = this.chatBody.nativeElement;
      elem.scrollTop = elem.scrollHeight;
    }
    catch (err) {
      // Negeren
    }
  }

  // Probeer bericht te verzenden als gebruiker op ENTER drukt
  private onKeyUp(e: any) {
    if (e.code == 'Enter') {
      if (this.inputBuffer.length > 0) {
        this.onMessageSend.emit(this.inputBuffer);
        this.inputBuffer = '';
      }
    }
  }

  private inputBuffer: string = '';

  @Input()
  public messages: ChatLine[] = [];

  @Output()
  public onMessageSend: EventEmitter<string> = new EventEmitter<string>();
}
