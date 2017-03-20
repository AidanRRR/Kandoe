/* tslint:disable:no-unused-variable */
import {TestBed, inject} from "@angular/core/testing";
import {UserService} from "./user.service";
import {HttpModule} from "@angular/http";
import {AuthService} from "./auth.service";

describe('UserService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [UserService, AuthService],
      imports: [HttpModule]
    });
  });

  it('should ...', inject([UserService], (service: UserService) => {
    expect(service).toBeTruthy();
  }));
});
