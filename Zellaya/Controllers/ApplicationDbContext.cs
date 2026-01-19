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

        public DbSet<Documents> Documents => Set<Documents>();

        public DbSet<Board_Orders> Board_Orders => Set<Board_Orders>();
        public DbSet<Component> Components=>Set<Component>();

        public DbSet<OrderComponent> OrderComponents => Set<OrderComponent>();

        public DbSet<OrderItemDetails> OrderItemDetails { get; set; }

        public DbSet<StockMovement> StockMovements { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderItemDetails>()
                .HasNoKey()        
                .ToView(null);
            modelBuilder.Entity<StockMovement>()
                .ToTable("stock_movements")
                .HasKey(x => x.id_movement);
            modelBuilder.Entity<StockMovement>()
                  .HasOne(m => m.Component)
                  .WithMany(c => c.StockMovements)
                  .HasForeignKey(m => m.id_component);
        }
    }

}
