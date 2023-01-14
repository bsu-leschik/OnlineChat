import { TestBed } from '@angular/core/testing';

import { UsersCommunicatorService } from './users-communicator.service';

describe('UsersCommunicatorService', () => {
  let service: UsersCommunicatorService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UsersCommunicatorService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
