import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Anlegen } from './anlegen';

describe('Anlegen', () => {
  let component: Anlegen;
  let fixture: ComponentFixture<Anlegen>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Anlegen],
    }).compileComponents();

    fixture = TestBed.createComponent(Anlegen);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
