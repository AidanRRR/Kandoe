/* tslint:disable:no-unused-variable */

import {async, ComponentFixture, TestBed} from "@angular/core/testing";
import {By} from "@angular/platform-browser";
import {KandoeCardComponent} from "./kandoe-card.component";
import {FormsModule} from "@angular/forms";
import {CardService} from '../../../services/card.service';
import {UserService} from '../../../services/user.service';
import {AuthService} from '../../../services/auth.service';
import {HttpModule} from "@angular/http";

describe('KandoeCardComponent', () => {
  let component: KandoeCardComponent;
  let fixture: ComponentFixture<KandoeCardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [KandoeCardComponent],
      imports: [FormsModule, HttpModule],
      providers: [CardService, UserService, AuthService]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(KandoeCardComponent);
    component = fixture.componentInstance;
    component.card = {
      cardId: 'a',
      imageUrl: "www.kandoe.be/images/afbeelding.jpg",
      text: 'kaart tekst',
      themeId: '1'
    };
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
  
  it('should show the card text', () => {
    expect(component).toBeTruthy();
    let e = fixture.debugElement.query(By.css('.kandoe-card-text')).nativeElement;
    expect(e.textContent).toEqual('kaart tekst');
  });


  it('should show the card image', () => {
    expect(component).toBeTruthy();
    let e = fixture.debugElement.query(By.css('img')).nativeElement;
    expect(e.src).toContain('afbeelding.jpg');
  });

  it('should show the card show/hide comments button', () => {
    expect(component).toBeTruthy();
    let e = fixture.debugElement.query(By.css('.comments-button'));
    expect(e).toBeTruthy();
    component.commentable = false;
    fixture.detectChanges();
    e = fixture.debugElement.query(By.css('.comments-button'));
    expect(e).toBeFalsy();
  });

  it('should show the card show/hide edit button', () => {
    expect(component).toBeTruthy();
    let e = fixture.debugElement.query(By.css('.edit-button'));
    expect(e).toBeFalsy();
    component.editable = true;
    fixture.detectChanges();
    e = fixture.debugElement.query(By.css('.edit-button'));
    expect(e).toBeTruthy();
  });
});
