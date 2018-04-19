import { Component, OnInit } from '@angular/core';
import { HubConnection } from '@aspnet/signalr';
import { OrderService } from './services/order.service';
import { OrderHub } from './hubs/order-hub';
import { IOrderStatusUpdate } from './dtos/order-status-update';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  constructor(private orderService: OrderService, private orderHub: OrderHub) {
  }
  public canPlaceOrder: boolean = false;
  public messages: Array<string> = new Array<string>();

  public placeOrder() {
    this.orderService.create();;
  }

  public ngOnInit(): void {
    this.orderHub.connect().then(() => {
      this.canPlaceOrder = true;
    })
    .catch((e) => console.log(e));

    this.orderHub.statusUpdates.subscribe((su: IOrderStatusUpdate) => {
      this.messages.push('Order ' + su.orderId + ' status was updated to ' + su.status);
    });
  }
}
