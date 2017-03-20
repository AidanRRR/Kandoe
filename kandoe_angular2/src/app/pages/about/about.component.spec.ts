/* tslint:disable:no-unused-variable */


import {async, ComponentFixture, TestBed} from "@angular/core/testing";
import {AboutComponent} from "./about.component";
import {MockTranslatePipe} from '../../pipes/mock-translate.pipe';

describe('AboutComponent', () => {
  let component: AboutComponent;
  let fixture: ComponentFixture<AboutComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [AboutComponent, MockTranslatePipe]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AboutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
