# Global Azure Bootcamp - Cincinnati, Ohio

![gabc]

## Cleanup Before Starting

* Delete all Blob containers
* Delete all Queues except for `appnotifications`
* Delete Web App

## Hands on Lab
### Create a Storage Account
**do this first, takes some time**

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
Can you think of any tasks you would need to run on a schedule or demand?

### Write the First Consumer
Write a method to fire when a message is placed onto the queue as a `string`

Debug the webjob locally

Push a message onto the queue

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

```cs
using Newtonsoft.Json;
```

#### Push a Message onto the Queue

```javascript
{
  "Id": "001",
  "To": "Eric.Webb@email.com",
  "From": "Tom.Straub@email.com",
  "Subject": "Check out Azure",
  "Body": "Did you know...."
}
```

### Write the Consumer to interact with other queues

### Check Out the Standard Information Available

### Publish to Azure

### Run this in the Cloud!

### Kudu
![webjobwarning]


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
