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
    public class Service : BaseEntity
    {
        public Service()
        {
            ServiceForms = new List<ServiceForm>();
        }
  
        [Display(Name = "اولویت")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public int Order { get; set; }

        [Display(Name = "عنوان خدمت")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        [StringLength(256, ErrorMessage = "طول {0} نباید بیشتر از {1} باشد")]
        public string Title { get; set; }

        [Display(Name = "پارامتر URL")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        [StringLength(500, ErrorMessage = "طول {0} نباید بیشتر از {1} باشد")]
        public string UrlParam { get; set; }

        [Display(Name = "عنوان متا")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        [StringLength(500, ErrorMessage = "طول {0} نباید بیشتر از {1} باشد")]
        public string PageTitle { get; set; }

        [Display(Name = "توضیحات متا")]
        [StringLength(1000, ErrorMessage = "طول {0} نباید بیشتر از {1} باشد")]
        [DataType(DataType.Html)]
        [AllowHtml]
        [Column(TypeName = "ntext")]
        [UIHint("RichText")]
        public string PageDescription { get; set; }

        [Display(Name = "تصویر")]
        [StringLength(500, ErrorMessage = "طول {0} نباید بیشتر از {1} باشد")]
        public string ImageUrl { get; set; }

        [Display(Name = "خلاصه")]
        [DataType(DataType.MultilineText)]
        public string Summery { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.Html)]
        [AllowHtml]
        [Column(TypeName = "ntext")]
        [UIHint("RichText")]
        public string Body { get; set; }

        [Display(Name = "نمایش فرم")]
        public bool IsFormActive { get; set; }

        [Display(Name = "امتیاز")]
        public decimal? AverageRate { get; set; }

        public Guid ServiceGroupId { get; set; }
        public virtual ServiceGroup ServiceGroup { get; set; }

        public virtual ICollection<ServiceForm> ServiceForms { get; set; }


        internal class configuration : EntityTypeConfiguration<Service>
        {
            public configuration()
            {
                HasRequired(p => p.ServiceGroup).WithMany(t => t.Services).HasForeignKey(p => p.ServiceGroupId);
            }
        }
    }
}