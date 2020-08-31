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
    public class ServiceForm : BaseEntity
    {
        [Display(Name = "عنوان")]
        public string Title { get; set; }

        public Guid? ServiceGroupId { get; set; }
        public ServiceGroup ServiceGroup { get; set; }

        public Guid? ServiceId { get; set; }
        public Service Service { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid SiteTypeId { get; set; }
        public SiteType SiteType { get; set; }

        public Guid ServiceTypeId { get; set; }
        public ServiceType ServiceType { get; set; }

        [Display(Name = "آدرس سایت")]
        public string SiteAddress { get; set; }

        [Display(Name = "شماره تماس")]
        public string Phone { get; set; }

        [Display(Name = "ایمیل")]
        [StringLength(256, ErrorMessage = "طول {0} نباید بیشتر از {1} باشد")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "ایمیل وارد شده صحیح نیست")]

        public string Email { get; set; }

        [Display(Name = "کلمات کلیدی")]
        public string MainWords { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.Html)]
        [AllowHtml]
        [Column(TypeName = "ntext")]
        [UIHint("RichText")]
        public string FormDescription { get; set; }

        internal class configuration : EntityTypeConfiguration<ServiceForm>
        {
            public configuration()
            {
                HasRequired(p => p.ServiceGroup).WithMany(t => t.ServiceForms).HasForeignKey(p => p.ServiceGroupId);

                HasRequired(p => p.Service).WithMany(t => t.ServiceForms).HasForeignKey(p => p.ServiceId);

                HasRequired(p => p.User).WithMany(t => t.ServiceForms).HasForeignKey(p => p.UserId);

                HasRequired(p => p.ServiceType).WithMany(t => t.ServiceForms).HasForeignKey(p => p.ServiceTypeId);

                HasRequired(p => p.SiteType).WithMany(t => t.ServiceForms).HasForeignKey(p => p.SiteTypeId);
            }
        }
    }
}