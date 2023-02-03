import { TestBed } from '@angular/core/testing';

import { ChatsGuard } from './chats-guard.service';

describe('AuthGuardService', () => {
  let service: ChatsGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ChatsGuard);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
