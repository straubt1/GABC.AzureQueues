# Global Azure Bootcamp - Cincinnati, Ohio

![gabc]

## Cleanup Before Starting

* Delete all Blob containers
* Delete all Queues except for `appnotifications`
* Delete Web App

## Hands on Lab
### Create a Storage Account
**Do this first, takes some time**

1. Navigate to https://portal.azure.
2. Click `Add`
![newstorage]
3. Enter a Name and set the following:
	- Deployment Model -> `Resource manager`
	- Performance -> `Standard`
	- Subscription -> You may have several
	- Resources Group -> Create one or use an existing
	- Location -> `East US`

	![newstorage2]
5. Click `Create`

### Create an ASP.NET MVC Web Application
1. Open Visual Studio
2. Click `File -> `New Project`
3. Select `ASP.NET Web Application` && `.NET Framework 4.5.2` 
![newwebapp]
4. Select 
	- `MVC` from the  `ASP.NET 4.5.2 Templates` 
	- `No Authentication`
	- Do not check `Host in the Cloud` (we will do this later)
	![newmvc]
5. Click `OK`

### Add Azure Webjob
1. Right-Click on the Web Application Project -> `Add` -> `New Azure Webjob Project`
2. Set the following:
	- Project Name
	- WebJob Name
	- WebJob run mode -> `Run Continuously`

	![newwebjob]
3. Click 'OK'

#### Project Structure
* Program.cs
* Properties -> webjob-publish-settings.json
* Functions.cs
* App.config
* Web Application -> webjobs-list.json
	- WATCH FOR DUPLICATES!!!

> We are going to use a Azure Storage Queue to trigger our WebJob.
Other options are to `Run on Schedule` or `Run on Demand`.

> Can you think of any tasks you would need to run on a schedule or demand?

### Set the Connection String
Open `App.config`

```xml
<connectionStrings>
	<add name="AzureWebJobsDashboard" connectionString="Insert Connection String Here!"/>
	<add name="AzureWebJobsStorage"   connectionString="Insert Connection String Here!"/>
</connectionStrings>
```

> Two different connections strings, why?

### Write the First Consumer
Write a method to fire when a message is placed onto the queue as a `string`
```cs
private const string QueueName = "appnotifications";

/// <summary>
/// Process a queue message as a raw string
/// </summary>
/// <param name="message"></param>
/// <param name="log"></param>
public static void ProcessNotification(
	[QueueTrigger(QueueName)] string message,
	TextWriter log)
{
	//String Interpolation C# 6, tastes like candy!
	var logMessage = $"Message Recieved as string: {message}";

	log.WriteLine(logMessage);
	Console.WriteLine(logMessage);
}
```
#### Create the Queue
Open `Cloud Explorer` -> goto the Storage Account -> Right-Click `Queues` -> `Create Queue`

![createqueue]

#### Debug the webjob locally
Right-Click on the WebJob project -> `Debug` -> `Start new instance`

![debugwebjob]

#### Push a message onto the queue
Open `Cloud Explorer` -> goto the Storage Account -> double-click on the queue

![addmessage1]:

Click `Add Message`

![addmessage3]:
### Define Queue Message
Create the following POCO
```cs
public class AppNotification
{
	/// <summary>
	/// Unique id for the notification, set by the notifier
	/// </summary>
	public string Id { get; set; }
	/// <summary>
	/// Who the notification is for
	/// </summary>
	public string To { get; set; }
	/// <summary>
	/// Who the notification is from
	/// </summary>
	public string From { get; set; }
	/// <summary>
	/// The subject of the notification
	/// </summary>
	public string Subject { get; set; }
	/// <summary>
	/// The content of the message
	/// </summary>
	public string Content { get; set; }
	/// <summary>
	/// Status of the notification
	/// </summary>
	public string Status { get; set; }
}
```

### Write the Consumer that takes the POCO
Write a method to fire when a message is placed onto the queue, this time it will be deserialized.

> The webjob will use `Newtonsoft.Json` to deserialize from JSON

```cs
/// <summary>
/// Process a queue message as a POCO
/// </summary>
/// <param name="message"></param>
/// <param name="log"></param>
public static void ProcessNotification(
	[QueueTrigger(QueueName)] AppNotification message,
	TextWriter log)
{
	var logMessage = $"Message Recieved as POCO: {JsonConvert.SerializeObject(message)}";

	log.WriteLine(logMessage);
	Console.WriteLine(logMessage);
}
```

#### Push a Message onto the Queue

Sample JSON
```javascript
{
  "Id": "001",
  "To": "Eric.Webb@email.com",
  "From": "Tom.Straub@email.com",
  "Subject": "Check out Azure",
  "Body": "Did you know...."
}
```
Add a new message

![addmessage2]

### Check Out the Standard Information Available

```cs
/// <summary>
/// Process queue message and get default information
/// </summary>
/// <param name="message"></param>
/// <param name="expirationTime"></param>
/// <param name="insertionTime"></param>
/// <param name="nextVisibleTime"></param>
/// <param name="id"></param>
/// <param name="popReceipt"></param>
/// <param name="dequeueCount"></param>
/// <param name="queueTrigger"></param>
/// <param name="cloudStorageAccount"></param>
/// <param name="log"></param>
public static void ProcessNotification(
	[QueueTrigger(QueueName)] AppNotification message,
	DateTimeOffset expirationTime,
	DateTimeOffset insertionTime,
	DateTimeOffset nextVisibleTime,
	string id,
	string popReceipt,
	int dequeueCount,
	string queueTrigger,
	CloudStorageAccount cloudStorageAccount,
	TextWriter log)
{
	var logMessage = $"logMessage={message}\n" +
						$"expirationTime={expirationTime}\n" +
						$"insertionTime={insertionTime}\n" +
						$"nextVisibleTime={nextVisibleTime}\n" +
						$"id={id}\n" +
						$"popReceipt={popReceipt}\n" +
						$"dequeueCount ={dequeueCount}\n" +
						$"queue endpoint={cloudStorageAccount.QueueEndpoint}\n" +
						$"queueTrigger={queueTrigger}";

	log.WriteLine(logMessage);
	Console.WriteLine(logMessage);
}
```

### Write the Consumer to interact with other queues

```cs
/// <summary>
/// Process a queue message and forward it to another queue
/// </summary>
/// <param name="message"></param>
/// <param name="forwardQueue"></param>
/// <param name="eventQueue"></param>
/// <param name="log"></param>
public static void ProcessNotification(
	[QueueTrigger(QueueName)] AppNotification message,
	[Queue(QueueName + "-forward")] out AppNotification forwardQueue,
	[Queue(QueueName + "-event")] out string eventQueue,
	TextWriter log)
{
	var logMessage = $"Message Recieved: {JsonConvert.SerializeObject(message)}";

	log.WriteLine(logMessage);

	//forward to another queue for further processing
	forwardQueue = message;
	forwardQueue.Status = $"Notification was processed at '{DateTime.UtcNow}'";

	//write generic event as string to another queue
	eventQueue = DateTime.Now.ToString();
}
```

### Publish to Azure

![publish1]

![publish2]

![publish3]

![publish4]

![publish5]

### Run this in the Cloud!

### Kudu
![webjobwarning]

Update `Web.Release.Config` in the Web App to this:
```xml
<?xml version="1.0"?>
<configuration xmlns:xdt="http://schemas.microsoft.com/XML-Document-Transform">
  <connectionStrings xdt:Transform="Insert">
    <add name="AzureWebJobsDashboard" connectionString="Insert Connection String Here!" />
    <add name="AzureWebJobsStorage"   connectionString="Insert Connection String Here!" />
  </connectionStrings>
  <system.web>
    <compilation xdt:Transform="RemoveAttributes(debug)" />
  </system.web>
</configuration>
```


# References

https://azure.microsoft.com/en-us/documentation/articles/storage-use-emulator/#start-and-initialize-the-storage-emulator

https://azure.microsoft.com/en-us/documentation/articles/websites-dotnet-webjobs-sdk-storage-queues-how-to

https://github.com/projectkudu/kudu


[gabc]: /Images/GABC.png "Global Azure Bootcamp"
[kudu]: /Images/kudu.png "kudu"
[newmvc]: /Images/NewMvc.jpg "step"
[newstorage]: /Images/NewStorage.jpg "step"
[newstorage2]: /Images/NewStorage2.jpg "step"
[newwebapp]: /Images/NewWebApp.jpg "step"
[newwebjob]: /Images/NewWebJob.jpg "step"
[webjobwarning]: /Images/WebJobWarning.jpg "warning"
[createqueue]: /Images/CreateQueue.jpg "create queue"
[debugwebjob]: /Images/DebugWebjob.jpg "debug webjob"
[addmessage1]: /Images/AddMessage.jpg "Add Messasge"
[addmessage2]: /Images/AddMessage2.jpg "Add Messasge"
[addmessage3]: /Images/AddMessage3.jpg "Add Messasge"
[publish1]: /Images/Publish1.jpg "Publish"
[publish2]: /Images/Publish2.jpg "Publish"
[publish3]: /Images/Publish3.jpg "Publish"
[publish4]: /Images/Publish4.jpg "Publish"
[publish5]: /Images/Publish5.jpg "Publish"
