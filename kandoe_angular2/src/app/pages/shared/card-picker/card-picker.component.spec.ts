/* tslint:disable:no-unused-variable */
import {async, ComponentFixture, TestBed} from "@angular/core/testing";
import {CardPickerComponent} from "./card-picker.component";
import {MockTranslatePipe} from '../../../pipes/mock-translate.pipe';
import {KandoeCardComponent} from '../kandoe-card/kandoe-card.component';
import {FormsModule} from "@angular/forms";

describe('CardPickerComponent', () => {
  let component: CardPickerComponent;
  let fixture: ComponentFixture<CardPickerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [KandoeCardComponent, CardPickerComponent, MockTranslatePipe],
      imports: [FormsModule]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CardPickerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
   expect(component).toBeTruthy();
  });
});
