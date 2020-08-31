using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Models
{
    public class BackLinkDetail : BaseEntity
    {
        [Display(Name = "نام وبسایت")]
        public string Title { get; set; }

        [Display(Name = "مدت زمان")]
        public int Duration { get; set; }

        [Display(Name = "قیمت")]
        public decimal Amount { get; set; }

        public Guid BackLinkId { get; set; }
        public virtual BackLink BackLink { get; set; }

        public Guid ProductId { get; set; }

        public virtual Product Product { get; set; }

        internal class configuration : EntityTypeConfiguration<BackLinkDetail>
        {
            public configuration()
            {
                HasRequired(p => p.Product).WithMany(t => t.BackLinkDetails).HasForeignKey(p => p.ProductId);
            }
        }
    }
}