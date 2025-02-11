namespace StackOverflow.Models
{
    public class Posts
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public int ViewCount { get; set; }

        public int Score { get; set; }
        public int OwnerUserId { get; set; }

        public int AnswerCount { get; set; }

        public string Body { get; set; }

        public int PostTypeId { get; set; }



    }
}
