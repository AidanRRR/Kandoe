/* tslint:disable:no-unused-variable */
import {TestBed, inject} from "@angular/core/testing";
import {ThemeService} from "./theme.service";
import {HttpModule} from "@angular/http";

describe('ThemeService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ThemeService],
      imports: [HttpModule]
    });
  });

  it('should ...', inject([ThemeService], (service: ThemeService) => {
    expect(service).toBeTruthy();
  }));
});
