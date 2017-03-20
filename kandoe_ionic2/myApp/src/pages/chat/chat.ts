import { Component, OnInit, Input, EventEmitter, Output, ElementRef, AfterViewChecked, ViewChild  } from '@angular/core';

import {PopoverController} from 'ionic-angular';
import {AccountPopoverPage} from "../account-popover/account-popover";
import {LanguagePopoverPage} from "../language-popover/language-popover";

export interface ChatLine {
  user: string;
  message: string;
}

@Component({
  selector: 'page-chat',
  templateUrl: 'chat.html'
})

export class  ChatPage implements OnInit, AfterViewChecked{

  constructor(public popoverCtrl: PopoverController) { }

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
}
