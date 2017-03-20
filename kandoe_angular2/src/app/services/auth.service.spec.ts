/* tslint:disable:no-unused-variable */
import {TestBed, inject} from "@angular/core/testing";
import {AuthService} from "./auth.service";
import {HttpModule} from "@angular/http";

describe('AuthService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AuthService],
      imports: [HttpModule]
    });
  });

  it('should ...', inject([AuthService], (service: AuthService) => {
    expect(service).toBeTruthy();
  }));
});
