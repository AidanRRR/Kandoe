/* tslint:disable:no-unused-variable */
import {async, ComponentFixture, TestBed} from "@angular/core/testing";
import {LoginComponent} from "./login.component";
import {Router, RouterModule, ActivatedRoute} from "@angular/router";
import {LocationStrategy} from "@angular/common";
import {UserService} from "../../services/user.service";
import {FormsModule} from "@angular/forms";
import {HttpModule} from "@angular/http";
import {AuthService} from "../../services/auth.service";

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [LoginComponent],
      providers: [{provide: Router, useClass: RouterModule}, {
        provide: ActivatedRoute,
        useClass: RouterModule
      }, LocationStrategy, UserService, AuthService],
      imports: [FormsModule, RouterModule, HttpModule]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  /*it('should create', () => {
   expect(component).toBeTruthy();
   });*/
});
