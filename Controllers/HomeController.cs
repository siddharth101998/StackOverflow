using Microsoft.AspNetCore.Mvc;
using StackOverflow.Data;
using StackOverflow.Models;
using StackOverflow.Services;
using System.Diagnostics;
using Lib.Net.Http.WebPush;

namespace StackOverflow.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger,ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



    
        public IActionResult Search(string searchterm, int page = 1)
        {
            int pageSize = 10;
            bool newContentFound = false;
            // Calculate skip count for pagination
            int skipCount = (page - 1) * pageSize;

            // Retrieve paged posts directly from the database
            var pagedPosts = _context.Posts
                .Where(post => post.Title.Contains(searchterm)&&(post.PostTypeId==1||post.PostTypeId==2))
                .OrderByDescending(post => post.Id)
                .Skip(skipCount)
                .Take(pageSize)
                .ToList();

            // Retrieve the total count of posts matching the search criteria
            int totalPostsCount = _context.Posts
                .Count(post => post.Title.Contains(searchterm));

            // Calculate total pages
            int totalPages = (int)Math.Ceiling((double)totalPostsCount / pageSize);

            // Convert paged posts to custom post model
            var customPosts = pagedPosts
                .Select(post => new CustomPostModel
                {
                    PostId = post.Id,
                    Title = post.Title,
                    Body = post.Body,
                    UserName = _context.Users.FirstOrDefault(user => user.Id == post.OwnerUserId)?.DisplayName,
                    VoteCount = _context.Votes.Count(vote => vote.PostId == post.Id),
                    AnswerCount = post.AnswerCount,
                    Badge = _context.Badges.FirstOrDefault(badge => badge.UserId == post.OwnerUserId)?.Name,
                    Reputation= _context.Users.FirstOrDefault(user => user.Id == post.OwnerUserId)?.Reputation
                }).ToList();

            // Calculate start and end page for pagination
            int startPage = Math.Max(1, page - 5);
            int endPage = Math.Min(totalPages, startPage + 9);

            // Create the view model
            var viewModel = new SearchViewModel
            {
                CustomPosts = customPosts,
                CurrentPage = page,
                TotalPages = totalPages,
                SearchTerm = searchterm,
                StartPage = startPage,
                EndPage = endPage
            };
            if (newContentFound)
            {
                // Get the subscription information for users who opted-in for push notifications
                var subscriptions = _context.PushNotificationSubscriptions.ToList();

                foreach (var subscription in subscriptions)
                {
                    // Trigger push notifications for each subscription
                    PushNotificationService.SendNotification(subscription, "New relevant content found!");
                }
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_SearchView", viewModel);
            }

            // Otherwise, return the full view
            return View(viewModel);
        }
        //[HttpPost]
        //public IActionResult Subscribe([FromBody] PushNotificationSubscription subscription)
        //{
        //    // Save the subscription information to the database
        //    // Example code assuming PushNotificationSubscription model and ApplicationDbContext are used
        //    var pushSubscription = new PushNotificationSubscription
        //    {
        //        UserId = subscription.UserId,
        //        Endpoint = subscription.endpoint,
        //        Publickey = subscription.keys.p256dh,
        //        AuthToken = subscription.keys.auth,
        //        CreatedAt = DateTime.Now
        //    };

        //    _context.PushNotificationSubscriptions.Add(pushSubscription);
        //    _context.SaveChanges();

        //    return Json(new { success = true });
        //}

    }



}

