using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Commodity_Prices_Watcher.DAL
{
    public class CommodityPricesWatcherContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int, IdentityUserClaim<int>, IdentityUserRole<int>, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public CommodityPricesWatcherContext(DbContextOptions<CommodityPricesWatcherContext> options) : base(options) 
        {
            
        }

        public virtual DbSet<StaticContent> StaticContent { get; set; }
        public virtual DbSet<CommodityCategory> CommodityCategory { get; set; }
        public virtual DbSet<AttachmentsLookUp> AttachmentsLookUp { get; set; }
        public virtual DbSet<SharedPrice> SharedPrice { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Name=ConnectionStrings:CommodityPricesWatcherTestServer",
                x=> x.UseNetTopologySuite());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // builder.UseCollation("Arabic_CI_AS");

            modelBuilder.Entity<StaticContent>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_StaticContent");
                entity.ToTable("StaticContent");

                entity.Property(e => e.Id).HasColumnName("Id");
                entity.Property(e => e.CreateDate).HasColumnType("datetime");
                entity.Property(e => e.DescriptionA).HasColumnName("DescriptionA");
                entity.Property(e => e.DescriptionE).HasColumnName("DescriptionE");
                entity.Property(e => e.Icon).HasMaxLength(255);
                entity.Property(e => e.PageName).HasMaxLength(255);
                entity.Property(e => e.RouterLink).HasMaxLength(255);
                entity.Property(e => e.TitleA)
                    .HasMaxLength(255)
                    .HasColumnName("TitleA");
                entity.Property(e => e.TitleE)
                    .HasMaxLength(255)
                    .HasColumnName("TitleE");
            });


            base.OnModelCreating(modelBuilder);
        }
    }
}
