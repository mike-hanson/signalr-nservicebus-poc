import { HubConnection } from '@aspnet/signalr';

export abstract class HubBase {
    private _hubConnection: HubConnection;

    protected start(hubUrl: string): Promise<void>
    {
        this._hubConnection = new HubConnection(hubUrl);

        // this._hubConnection.on('noop', this.noOp);

        return this._hubConnection.start().catch(err => console.log(err));
    }

    protected registerPushHandler(method: string, handler: (...args: any[]) => void): void {
        this._hubConnection.on(method, handler);
    }

    protected invokeServerMethod(method: string, ...args: any[]): void {
        this._hubConnection.invoke(method, args)
    }

    private noOp() {
        // dummy method to ensure on connect event is triggered on server hub
    }    
}