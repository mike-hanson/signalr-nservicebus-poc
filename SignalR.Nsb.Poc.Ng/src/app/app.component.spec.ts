import { TestBed, async, inject } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

import { AppComponent } from './app.component';
import { OrderService } from './services/order.service';
import { OrderHub } from './hubs/order-hub';

describe('AppComponent', () => {
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        AppComponent
      ],
      imports: [
          FormsModule,
          HttpClientModule
      ],
      providers: [OrderService, OrderHub, HttpClient]
    }).compileComponents();
  }));

  it('Should be defined', async(() => {
    const fixture = TestBed.createComponent(AppComponent);
    const component = fixture.debugElement.componentInstance;
    expect(component).toBeTruthy();
  }));

  it('Should use service to create order', inject([OrderService, OrderHub], (service: OrderService, hub: OrderHub) => {
    const fixture = TestBed.createComponent(AppComponent);
    const component = fixture.debugElement.componentInstance;
    const userId = 'mike.hanson';
    
    const promise = new Promise<void>(() => { });
    const observable = new Observable<object>();
    spyOn(hub, 'connect').and.returnValue(promise);
    spyOn(service, 'create').and.returnValue(observable);

    component.placeOrder();

    expect(service.create).toHaveBeenCalled();    
  }));
});