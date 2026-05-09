import { TestBed } from '@angular/core/testing';

import { Auftraege } from './auftraege';

describe('Auftraege', () => {
  let service: Auftraege;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Auftraege);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
