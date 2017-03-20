/* tslint:disable:no-unused-variable */
import {async, ComponentFixture, TestBed} from "@angular/core/testing";
import {RegisterComponent} from "./register.component";
import {FormsModule} from "@angular/forms";
import {RouterModule, Router, ActivatedRoute} from "@angular/router";
import {UserService} from "../../services/user.service";
import {HttpModule} from "@angular/http";
import {AuthService} from "../../services/auth.service";
import {LocationStrategy} from "@angular/common";

describe('RegisterComponent', () => {
  let component: RegisterComponent;
  let fixture: ComponentFixture<RegisterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [RegisterComponent],
      providers: [{provide: Router, useClass: RouterModule}, {
        provide: ActivatedRoute,
        useClass: RouterModule
      }, LocationStrategy, UserService, AuthService],
      imports: [FormsModule, RouterModule, HttpModule]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegisterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  /*it('should create', () => {
   expect(component).toBeTruthy();
   });*/
});
