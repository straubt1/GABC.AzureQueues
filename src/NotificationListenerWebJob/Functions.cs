using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;
using NotificationListenerWebJob.Models;

namespace NotificationListenerWebJob
{
    public class Functions
    {
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

        ///// <summary>
        ///// Process a queue message as a POCO
        ///// </summary>
        ///// <param name="message"></param>
        ///// <param name="log"></param>
        //public static void ProcessNotification(
        //    [QueueTrigger(QueueName)] AppNotification message,
        //    TextWriter log)
        //{
        //    var logMessage = $"Message Recieved as POCO: {JsonConvert.SerializeObject(message)}";

        //    log.WriteLine(logMessage);
        //    Console.WriteLine(logMessage);
        //}

        ///// <summary>
        ///// Process a queue message and forward it to another queue
        ///// </summary>
        ///// <param name="message"></param>
        ///// <param name="forwardQueue"></param>
        ///// <param name="eventQueue"></param>
        ///// <param name="log"></param>
        //public static void ProcessNotification(
        //    [QueueTrigger(QueueName)] AppNotification message,
        //    [Queue(QueueName + "-forward")] out AppNotification forwardQueue,
        //    [Queue(QueueName + "-event")] out string eventQueue,
        //    TextWriter log)
        //{
        //    var logMessage = $"Message Recieved: {JsonConvert.SerializeObject(message)}";

        //    log.WriteLine(logMessage);

        //    //forward to another queue for further processing
        //    forwardQueue = message;
        //    forwardQueue.Status = $"Notification was processed at '{DateTime.UtcNow}'";

        //    //write generic event as string to another queue
        //    eventQueue = DateTime.Now.ToString();
        //}

        //public static void ProcessNotification(
        //    [QueueTrigger(QueueName)] AppNotification message,
        //    DateTimeOffset expirationTime,
        //    DateTimeOffset insertionTime,
        //    DateTimeOffset nextVisibleTime,
        //    string id,
        //    string popReceipt,
        //    int dequeueCount,
        //    string queueTrigger,
        //    CloudStorageAccount cloudStorageAccount,
        //    TextWriter log)
        //{
        //    var logMessage = $"logMessage={message}\n" +
        //                     $"expirationTime={expirationTime}\n" +
        //                     $"insertionTime={insertionTime}\n" +
        //                     $"nextVisibleTime={nextVisibleTime}\n" +
        //                     $"id={id}\n" +
        //                     $"popReceipt={popReceipt}\n" +
        //                     $"dequeueCount ={dequeueCount}\n" +
        //                     $"queue endpoint={cloudStorageAccount.QueueEndpoint}\n" +
        //                     $"queueTrigger={queueTrigger}";

        //    log.WriteLine(logMessage);
        //    Console.WriteLine(logMessage);
        //}
    }
}
