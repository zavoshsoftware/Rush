using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Models
{
    public class ReportageGroup:BaseEntity
    {
        public ReportageGroup()
        {
            Reportages = new List<Reportage>();
        }
        [Display(Name = "عنوان گروه رپورتاژ")]
        public string Title { get; set; }

        [Display(Name = "پارامتر URL")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        [StringLength(500, ErrorMessage = "طول {0} نباید بیشتر از {1} باشد")]
        public string UrlParam { get; set; }
        [Display(Name = "اولویت")]
        public int? Priority { get; set; }

        [Display(Name = "ارزش واقعی")]
        public decimal? Value { get; set; }

        [Display(Name = "قیمت پکیج")]
        public decimal? Price { get; set; }

        [Display(Name = "پکیج رپورتاژ؟")]
        public bool IsPackage { get; set; }


        [Display(Name = "DA شروع")]
        public int StartDa { get; set; }

        [Display(Name = "DA پایان")]
        public int FinishDa { get; set; }

        [Display(Name = "تعداد سایت")]
        public int SiteNumber { get; set; }
        public ICollection<Reportage> Reportages { get; set; }


        public Guid? ProductId { get; set; }
        public virtual Product Product { get; set; }


        internal class configuration : EntityTypeConfiguration<ReportageGroup>
        {
            public configuration()
            {
                HasOptional(p => p.Product).WithMany(t => t.ReportageGroups).HasForeignKey(p => p.ProductId);
            }
        }
    }
}