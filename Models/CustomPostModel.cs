using System.Net;

namespace StackOverflow.Models
{
    public class CustomPostModel
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string UserName { get; set; }
 
        public int VoteCount {  get; set; }
        public int AnswerCount { get; set; }

        public int? Reputation {  get; set; }

        public string? Badge { get; set; }
    }
}
