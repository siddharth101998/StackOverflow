using Microsoft.EntityFrameworkCore;
using StackOverflow.Models;
using System.Diagnostics;
using System.Net;
namespace StackOverflow.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<PushNotificationSubscription> PushNotificationSubscriptions { get; set; }
        public DbSet<Badges> Badges { get; set; }

        public DbSet<Posts> Posts { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Votes> Votes { get; set; }
  

  

    }
}
