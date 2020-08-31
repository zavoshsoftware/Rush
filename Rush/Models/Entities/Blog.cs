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
    public class Blog : BaseEntity
    {
        public Blog()
        {
            BlogComments = new List<BlogComment>();
        }
        [Display(Name = "عنوان")]
        public string Title { get; set; }
        [Display(Name = "خلاصه")]
        public string Summery { get; set; }
        [Display(Name = "تصویر")]
        public string ImageUrl { get; set; }
        [Display(Name = "Pdf")]
        public string PdfUrl { get; set; }
        [Display(Name = "تعداد بازدید")]
        public int Visit { get; set; }
        [Display(Name = "توضیحات")]
        [DataType(DataType.Html)]
        [AllowHtml]
        [Column(TypeName = "ntext")]
        [UIHint("RichText")]
        public string Body { get; set; }

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
        public Guid  BlogGroupId { get; set; }
        public BlogGroup BlogGroup { get; set; }

        public Guid? WritterId { get; set; }
        public User Writter { get; set; }

        [Display(Name = "امتیاز")]
        public decimal? AverageRate { get; set; }

        [Display(Name = "فایل صوتی")]
        public string FileUrl { get; set; }

        public virtual ICollection<BlogComment> BlogComments { get; set; }

        internal class configuration : EntityTypeConfiguration<Blog>
        {
            public configuration()
            {
                HasRequired(p => p.BlogGroup).WithMany(t => t.Blogs).HasForeignKey(p => p.BlogGroupId);

                HasRequired(p => p.Writter).WithMany(t => t.Blogs).HasForeignKey(p => p.WritterId);
            }
        }
    }
}