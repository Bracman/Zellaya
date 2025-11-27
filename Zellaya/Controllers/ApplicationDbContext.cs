using Microsoft.EntityFrameworkCore;
using Zellaya.Models;

namespace Zellaya.Data
{
    public class ApplicationDbContext : DbContext   
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<Order> Orders => Set<Order>();

        public DbSet<Board> Boards => Set<Board>(); 

        public DbSet<Board_Orders> Board_Orders => Set<Board_Orders>();
        public DbSet<Component> Components=>Set<Component>();

        public DbSet<OrderComponent> OrderComponents => Set<OrderComponent>();
    }
}
