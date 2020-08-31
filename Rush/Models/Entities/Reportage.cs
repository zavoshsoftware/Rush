using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Models
{
    public class Reportage:BaseEntity
    {
        [Display(Name = "نام وبسایت")]
        public string FullName { get; set; }

        [Display(Name = "آدرس سایت")] 
        public string Address { get; set; }

        [Display(Name = "DomainAuthority")]
        public int DomainAuthority { get; set; }

        [Display(Name = "قیمت")]
        public decimal Price { get; set; }
        [Display(Name = "اولویت")]
        public int? Priority { get; set; }

        [Display(Name = "نمایش قیمت ارزی")]
        public bool IsDollar { get; set; }

        public Guid? ReportageGroupId { get; set; }
        public ReportageGroup ReportageGroup { get; set; }

        [Display(Name = "قیمت خرید")]
        public decimal? BuyAmount { get; set; }

        [Display(Name = "دارای تخفیف است؟")]
        public bool IsInPromotion { get; set; }

        [Display(Name = "قیمت بعد از تخفیف")]
        public decimal? DiscountAmount { get; set; }

        public Guid? ProductId { get; set; }
        public virtual Product Product { get; set; }

        internal class configuration : EntityTypeConfiguration<Reportage>
        {
            public configuration()
            {
                HasRequired(p => p.ReportageGroup).WithMany(t => t.Reportages).HasForeignKey(p => p.ReportageGroupId);
                HasOptional(p => p.Product).WithMany(t => t.Reportages).HasForeignKey(p => p.ProductId);
            }
        }
        [Display(Name="لوگو")]
        public string ImageUrl { get; set; }

        [Display(Name="شرایط کار")]
        [DataType(DataType.MultilineText)]
        public string Terms { get; set; }
    }
}