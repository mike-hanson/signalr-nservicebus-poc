## SignalR NServiceBus POC

This POC demonstrates how to get SignalR with an NServiceBus Saga to get near real time feedback to a client.

Attempts have been made to get everything running in containers but getting connectivity to RabbitMQ in a container working has been challenging.

To run the entire system without containers you will need to install RabbitMQ locally and setup multiple startup projects through the solution properties.  The following projects need to be started:

* SignalR.Nsb.Poc.Sales
* SignalR.Nsb.Poc.Billing
* SignalR.Nsb.Poc.Shipping
* SignalR.Nsb.Poc.Web

Once containers are working change the startup project to docker-compose


