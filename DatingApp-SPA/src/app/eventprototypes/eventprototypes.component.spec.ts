/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { EventprototypesComponent } from './eventprototypes.component';

describe('EventprototypesComponent', () => {
  let component: EventprototypesComponent;
  let fixture: ComponentFixture<EventprototypesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EventprototypesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EventprototypesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
