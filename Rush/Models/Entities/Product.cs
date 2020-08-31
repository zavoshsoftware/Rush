using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Models
{
    public class Product:BaseEntity
    {
        public Product()
        {
            Reportages=new List<Reportage>();
            ReportageGroups=new List<ReportageGroup>();
            BackLinkDetails = new List<BackLinkDetail>();
        }
        [Display(Name = "عنوان محصول")]

        public string Title { get; set; }

        public Guid ProductTypeId { get; set; }
        public virtual ProductType ProductType { get; set; }

        public virtual ICollection<Reportage> Reportages { get; set; }
        public virtual ICollection<ReportageGroup> ReportageGroups { get; set; }
        public virtual ICollection<BackLinkDetail> BackLinkDetails { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<OrderDetailInformation> OrderDetailInformations { get; set; }

        internal class configuration : EntityTypeConfiguration<Product>
        {
            public configuration()
            {
                HasRequired(p => p.ProductType).WithMany(t => t.Products).HasForeignKey(p => p.ProductTypeId);
            }
        }
    }
}