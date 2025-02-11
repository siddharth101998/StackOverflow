namespace StackOverflow.Models
{
    public class SearchViewModel
    {
        public List<CustomPostModel> CustomPosts { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string SearchTerm { get; set; } // Add this property
        public int StartPage { get; set; }
        public int EndPage { get; set; }
    }
}
