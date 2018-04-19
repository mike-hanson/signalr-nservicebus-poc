import { TestBed, inject } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { HttpEvent, HttpEventType } from '@angular/common/http';

import { OrderService } from './order.service';
import { OrderHub } from '../hubs/order-hub';

describe('OrderService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [OrderService, OrderHub]
    });
  });

  it('Should be defined', inject([OrderService], (service: OrderService) => {
    expect(service).toBeTruthy();
  }));

  it('Should implement a method to create an order', inject([OrderService], (service: OrderService) => {
    expect(typeof service.create).toBe('function');
    expect(service.create.length).toBe(0);
  }));
  
  it('Should make valid request to api service', inject([OrderService, HttpTestingController], (service: OrderService, httpMock: HttpTestingController) => {
    service.create();

    const mockReq = httpMock.expectOne(service.url);
    expect(mockReq.cancelled).toBeFalsy();
    expect(mockReq.request.method).toBe('POST');
    expect(mockReq.request.responseType).toBe('json');
    mockReq.flush({});
    httpMock.verify();
  }));

});
