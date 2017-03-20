/* tslint:disable:no-unused-variable */
import {async, ComponentFixture, TestBed} from "@angular/core/testing";
import {KandoeBoardComponent} from "./kandoe-board.component";
import {CardService} from '../../../services/card.service';
import {UserService} from '../../../services/user.service';
import {AuthService} from '../../../services/auth.service';
import {SessionService} from '../../../services/session.service';
import {HttpModule} from "@angular/http";
import {FormsModule} from "@angular/forms";

describe('KandoeBoardComponent', () => {
  let component: KandoeBoardComponent;
  let fixture: ComponentFixture<KandoeBoardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [KandoeBoardComponent],
      imports: [FormsModule, HttpModule],
      providers: [CardService, UserService, AuthService, SessionService]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(KandoeBoardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
   expect(component).toBeTruthy();
  });
});
