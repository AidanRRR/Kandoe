/* tslint:disable:no-unused-variable */
import {TestBed, inject} from "@angular/core/testing";
import {AuthGuardService} from "./auth-guard.service";
import {RouterModule, Router} from "@angular/router";
import {HttpModule} from "@angular/http";
import {AuthService} from "./auth.service";

describe('AuthGuardService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AuthGuardService, {provide: Router, useClass: RouterModule}, AuthService],
      imports: [HttpModule, RouterModule]
    });
  });

  it('should ...', inject([AuthGuardService], (service: AuthGuardService) => {
    expect(service).toBeTruthy();
  }));
});
