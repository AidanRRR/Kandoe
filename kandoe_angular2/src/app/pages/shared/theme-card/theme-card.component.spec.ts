/* tslint:disable:no-unused-variable */
import {async, ComponentFixture, TestBed} from "@angular/core/testing";
import {ThemeCardComponent} from "./theme-card.component";
import {RouterModule, Router} from "@angular/router";

describe('ThemeCardComponent', () => {
  let component: ThemeCardComponent;
  let fixture: ComponentFixture<ThemeCardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ThemeCardComponent],
      providers: [{provide: Router, useClass: RouterModule}]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ThemeCardComponent);
    component = fixture.componentInstance;
    component.theme = {
      themeId: 'a',
      name: 'test thema',
      description: 'mock thema voor het testen',
      tags: ['a', 'b'],
      isPublic: true,
      updatedOn: new Date().toUTCString(),
      createdOn: new Date().toUTCString(),
      username: '',
      users: []
    };
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
