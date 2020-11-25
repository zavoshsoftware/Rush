using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Models
{
    public class DatabaseContext : DbContext
    {
        static DatabaseContext()
        {
            System.Data.Entity.Database.SetInitializer(new MigrateDatabaseToLatestVersion<DatabaseContext, Migrations.Configuration>());
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<ServiceGroup> ServiceGroups { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogGroup> BlogGroups { get; set; }
        public DbSet<TextType> TextTypes { get; set; }
        public DbSet<Text> Texts { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ContactForm> ContactForms { get; set; }
        public DbSet<NewsLetter> NewsLetters { get; set; }
        public DbSet<ServiceForm> ServiceForms { get; set; }
        public DbSet<BlogComment> BlogComments { get; set; }
        public DbSet<SiteType> SiteTypes { get; set; }
        public DbSet<ServiceType> ServiceTypes { get; set; }
        public DbSet<Reportage> Reportages { get; set; }

        public System.Data.Entity.DbSet<Models.ReportageGroup> ReportageGroups { get; set; }
        public DbSet<BackLink> BackLinks { get; set; }
        public DbSet<EmailAddress> EmailAddress { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<AskedQuestion> AskedQuestions { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<DiscountCode> DiscountCodes { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<ZarinpallAuthority> ZarinpallAuthorities { get; set; }
        public DbSet<StepDiscount> StepDiscounts { get; set; }
        public DbSet<StepDiscountDetail> StepDiscountDetails { get; set; }
        public DbSet<OrderDetailStatus> OrderDetailStatuses { get; set; }
        public DbSet<OrderDetailInformation> OrderDetailInformations { get; set; }
        public DbSet<BackLinkDetail> BackLinkDetails { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }

    }
}
