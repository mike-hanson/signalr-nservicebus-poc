
import { EventEmitter } from "@angular/core";

import { HubBase } from "./hub-base";
import { IOrderStatusUpdate } from "../dtos/order-status-update";

export class OrderHub extends HubBase {
    
    public statusUpdates: EventEmitter<IOrderStatusUpdate> = new EventEmitter<IOrderStatusUpdate>();

    public connect(): Promise<void> {
        return this.start("/hubs/order").then(() => {
            this.registerPushHandler("StatusUpdated", (su: IOrderStatusUpdate) => {
                this.statusUpdates.emit(su);
            })    
        });
    }
}