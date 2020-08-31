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
    public class AskedQuestion : BaseEntity
    {
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public string Title { get; set; }
        [Display(Name = "توضیحات")]
        [DataType(DataType.Html)]
        [AllowHtml]
        [Column(TypeName = "ntext")]
        [UIHint("RichText")]
        public string Body { get; set; }
        [Display(Name = "اولویت")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public int Order { get; set; }
        //[Display(Name = "گروه خدمات")]
        //public Guid ServiceGroupId { get; set; }
        //public ServiceGroup ServiceGroup { get; set; }
        [Display(Name = "صفحه نمایش")]
        public string Param { get; set; }

        //internal class configuration : EntityTypeConfiguration<AskedQuestion>
        //{
        //    public configuration()
        //    {
        //        HasRequired(p => p.ServiceGroup).WithMany(t => t.Questions).HasForeignKey(p => p.ServiceGroupId);
        //    }
        //}
    }
}