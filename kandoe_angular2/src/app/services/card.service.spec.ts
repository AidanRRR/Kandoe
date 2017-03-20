/* tslint:disable:no-unused-variable */


import {TestBed, inject} from "@angular/core/testing";
import {CardService} from "./card.service";
import {HttpModule} from "@angular/http";
import { AuthService } from '../services/auth.service';

describe('CardService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [CardService],
      imports: [HttpModule]
    });
  });

  it('should ...', inject([CardService], (service: CardService) => {
    expect(service).toBeTruthy();
  }));
});
