import { TestBed } from '@angular/core/testing';

import { UserControllerServiceService } from '../Services/user-controller-service.service';

describe('UserControllerServiceService', () => {
  let service: UserControllerServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UserControllerServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
