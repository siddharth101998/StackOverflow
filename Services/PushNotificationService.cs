using StackOverflow.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace StackOverflow.Services
{
    public static class PushNotificationService
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public static async Task<bool> SendNotification(PushNotificationSubscription subscription, string message)
        {
            try
            {
                // Construct the payload
                var payload = new
                {
                    notification = new
                    {
                        title = "New Content",
                        body = message
                    }
                };

                // Serialize the payload to JSON
                string payloadJson = Newtonsoft.Json.JsonConvert.SerializeObject(payload);

                // Create an HTTP request to send the push notification
                var request = new HttpRequestMessage(HttpMethod.Post, subscription.Endpoint);
                request.Headers.Add("Authorization", $"Bearer {subscription.AuthToken}");
                request.Headers.Add("Content-Type", "application/json");
                request.Headers.Add("TTL", "60"); // Time-to-live for the notification (in seconds)
                request.Content = new StringContent(payloadJson, Encoding.UTF8, "application/json");

                // Send the HTTP request
                var response = await _httpClient.SendAsync(request);

                // Check if the request was successful
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
                else
                {
                    // Log or handle the failure
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                return false;
            }
        }
    }
}
