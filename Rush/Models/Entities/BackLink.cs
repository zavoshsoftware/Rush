using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Models
{
    public class BackLink : BaseEntity
    {
        public BackLink()
        {
            BackLinkDetails = new List<BackLinkDetail>();
        }
        [Display(Name = "نام وبسایت")]
        public string FullName { get; set; }

        [Display(Name = "آدرس سایت")]
        public string Address { get; set; }

        [Display(Name = "بک لینک یک ماهه")]
        public decimal? OneMonthBackLink { get; set; }

        [Display(Name = "بک لینک سه ماهه")]
        public decimal? ThreeMonthBackLink { get; set; }

        [Display(Name = "اولویت")]
        public int Priority { get; set; }

        public int DomainAuthority { get; set; }

        public virtual ICollection<BackLinkDetail> BackLinkDetails { get; set; }
        //public Guid? ProductId { get; set; }
        //public virtual Product Product { get; set; }

        //internal class configuration : EntityTypeConfiguration<BackLink>
        //{
        //    public configuration()
        //    {
        //        HasOptional(p => p.Product).WithMany(t => t.BackLinks).HasForeignKey(p => p.ProductId);
        //    }
        //}
    }
}