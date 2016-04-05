# BotFrameworkSample

Bot Framework project I completed that hosts a bot to interface with Visual Studio Team Services.  Uses Logic Apps to complete the tasks, and Azure Directory Authentication

## How the different pieces interact ##

### Bot Host ###

First I created the bot host.  This is an Azure Web Site that accepts incoming requests from the Bot Framework, routes them to luis.ai, and then does some action based on the intent.  For now the intents I detect from luis.ai are "SearchVSTF" to search for an item, and "CreateVSTF" to create an item.  I coded in a few other commands like /topfeatures to execute specific queries.

I also added a method in the controller to handle Active Directory authentication.  If a bot user hasn't authenticated, it directs them to login first.

### Logic Apps ###

If a query or command is ever needed, the Bot Host will simply trigger a Logic App and wait for the response.  The Logic Apps each accept the request from the Bot Host (or any other host that may be using these Logic Apps), and use conditions and a custom API (VSTS API App) to execute against Visual Studio Team Services.

### VSTS Custom API ###

Custom API App I hosted in Azure to connect to Visual Studio Team Services.  It exposes Swagger metadata the Logic App can read to execute the commands as necessary.  If talking to an on-premise VSTS, Azure Hybrid Connectivity can be enabled.