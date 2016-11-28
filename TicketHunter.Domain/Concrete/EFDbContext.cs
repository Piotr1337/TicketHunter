namespace TicketHunter.Domain.Entities
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class EFDbContext : DbContext
    {
        public EFDbContext()
            : base("name=EFDbContext")
        {

        }

        public virtual DbSet<Artists> Artists { get; set; }
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<Events> Events { get; set; }
        public virtual DbSet<SubCategories> SubCategories { get; set; }
        public virtual DbSet<Ticket> Ticket { get; set; }
        public virtual DbSet<UserProfileDetails> UserProfileDetails { get; set; }
        public virtual DbSet<UserAddress> UserAddress { get; set; }
        public virtual DbSet<Countries> Countries { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Artists>()
                .Property(e => e.ImageMimeType)
                .IsUnicode(false);

            modelBuilder.Entity<Events>()
                .Property(e => e.ImageMimeType)
                .IsUnicode(false);

            modelBuilder.Entity<Events>()
                .HasMany(e => e.Ticket)
                .WithRequired(e => e.Events)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Ticket>()
                .Property(e => e.Price)
                .HasPrecision(18, 0);
        }
    }
}
