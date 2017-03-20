/* tslint:disable:no-unused-variable */
import {TestBed, inject} from "@angular/core/testing";
import {ImageService} from "./image.service";
import { HttpModule } from '@angular/http';
import { AuthService } from '../services/auth.service';

describe('ImageService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ImageService],
      imports: [HttpModule]
    });
  });

  it('should ...', inject([ImageService], (service: ImageService) => {
    expect(service).toBeTruthy();
  }));
});