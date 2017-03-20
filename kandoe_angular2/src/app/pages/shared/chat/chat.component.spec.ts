/* tslint:disable:no-unused-variable */
import {async, ComponentFixture, TestBed} from "@angular/core/testing";
import {By} from "@angular/platform-browser";
import {FormsModule} from "@angular/forms";
import {ChatComponent} from "./chat.component";

describe('ChatComponent', () => {
  let component: ChatComponent;
  let fixture: ComponentFixture<ChatComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ChatComponent],
      imports: [
        FormsModule
      ]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChatComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should render messages', () => {
    expect(component).toBeTruthy();
    component.messages = [
      {user: 'testuser', message: 'testmessage'}
    ];
    fixture.detectChanges();

    let e = fixture.debugElement.query(By.css('.chat-message-user')).nativeElement;
    expect(e.textContent).toEqual('testuser');
    e = fixture.debugElement.query(By.css('.chat-message-text')).nativeElement;
    expect(e.textContent).toEqual('testmessage');
  });

  it('should append new messages to the end', () => {
    expect(component).toBeTruthy();
    component.messages = [];
    for (let i = 0; i <= 5; i++) {
      component.messages.push({user: 'user' + i, message: 'message' + i});
    }
    fixture.detectChanges();
    fixture.debugElement
      .queryAll(By.css('.chat-message-text'))
      .map(de => de.nativeElement)
      .forEach((e, i) => expect(e.textContent).toEqual('message' + i));
  });
});
