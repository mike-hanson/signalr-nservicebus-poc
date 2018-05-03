## SignalR NServiceBus POC

This POC demonstrates how to use SignalR with an NServiceBus Saga to get near real time feedback to a client.

By default the solution is configured to run in containers so you don't have to install RabbitMQ locally.

### Running The Solution

#### In Containers
Before running in containers you will need to do the following:

* Make sure you have Docker for Windows installed
* Open the Docker Settings from the system tray
* Switch to the Proxies tab
* Enable the Manual configuration option
* Modify the text box labelled Web Server (HTTP) using the following format http://<yourusername>:<yourpassword>@inetproxy:8080

With "docker-compose" as your startup project in Visual Studio, hit F5 then watch the Output Window eventually all services will be started and access the web app. 

To access the web app point your browser at http://webapp
To access the RabbitMQ admin app point your browser to http://rabbitmq:15672 and log in with guest:guest

#### No Containers
To run the entire system without containers you will need to install RabbitMQ locally and setup multiple startup projects through the solution properties.
The following projects need to be started:

* SignalR.Nsb.Poc.Sales
* SignalR.Nsb.Poc.Billing
* SignalR.Nsb.Poc.Shipping
* SignalR.Nsb.Poc.Web

You will also need to make 2 code changes in EndpoingInstanceBuilder.cs and OrderEndpoint.cs.  You need to look for the commented section and swap the commenting
of the statements as indicated.

### Info
The POC is based on the standard NServicBus getting started tutorial and comprises:

* An Angular application that provides a button to initiate the placing of an order via a RESTful API controller
* An API Controller that sends a PlaceOrder command to NServiceBus
* An NServiceBus Saga that handles the PlaceOrder command and listens to OrderPlaced, OrderBilled and OrderShipped events
* A SignalR Hub that provides the link between the Saga and the Angular application for pusing status updates to the Angular app.
* Sales, Billing and Shipping Console applications representing micro services that process an Order through the various lifecycle steps to completion.

NB:  The Angular app was created in VS Code using Angular Cli so does not have a project file, therefore not currently included in the VS 2017 solution.
To view/edit the Angular app open the .\SignalR.Nsb.Poc.Ng folder in Visual Studio Code.

## Known Issues

### Startup Timing Issues

The POC currently uses the 2.3 format of Docker Compose YAML (docker-compose.yml). This supports the use of depends_on and conditions to make sure
RabbitMQ is up and running before the services that depend on it are started.  However this has been made obsolete in version 3.*
(which has caused some consternation in the community) so the whole thing would stop working if the version was upgraded.  The 2.3 version will be supported
for the foreseeable future so it should be fine to leave the POC as it is, but for "real" services we need to make the services themselves fault tolerant,
including detecting that RabbitMQ is not yet ready.  NServiceBus does have support for retries etc, but the defaults have proven not to be sufficient when running
in containers.  The solution may be as simple as using custom retry policies, but that can be resolved once we know more about deployment.



