/* tslint:disable:no-unused-variable */
import {async, ComponentFixture, TestBed} from "@angular/core/testing";
import {ThemesComponent} from "./themes.component";
import {FormsModule} from "@angular/forms";
import {ThemeCardComponent} from "../shared/theme-card/theme-card.component";
import {ThemeService} from "../../services/theme.service";
import {UserService} from "../../services/user.service";
import {HttpModule} from "@angular/http";
import {AuthService} from "../../services/auth.service";
import {MockTranslatePipe} from '../../pipes/mock-translate.pipe';

describe('ThemesComponent', () => {
  let component: ThemesComponent;
  let fixture: ComponentFixture<ThemesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ThemesComponent, ThemeCardComponent, MockTranslatePipe],
      imports: [FormsModule, HttpModule],
      providers: [ThemeService, UserService, AuthService]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ThemesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
