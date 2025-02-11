namespace StackOverflow.Models
{
    public class PushNotificationSubscription
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Endpoint { get; set; }
        public string AuthToken { get; set; }
        public String Publickey { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
