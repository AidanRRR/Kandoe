/* tslint:disable:no-unused-variable */

import {async, ComponentFixture, TestBed} from "@angular/core/testing";
import {PageNotFoundComponent} from "./not-found.component";
import {MockTranslatePipe} from '../../pipes/mock-translate.pipe';


describe('NotFoundComponent', () => {
  let component: PageNotFoundComponent;
  let fixture: ComponentFixture<PageNotFoundComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [PageNotFoundComponent, MockTranslatePipe]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PageNotFoundComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});