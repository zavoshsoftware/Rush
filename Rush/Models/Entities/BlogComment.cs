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
    public class BlogComment:BaseEntity
    {
        public BlogComment()
        {
            BlogComments = new List<BlogComment>();
        }
        [Display(Name = "نام و نام خانوادگی")]
        public string FullName { get; set; }

        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.Html)]
        [AllowHtml]
        [Column(TypeName = "ntext")]
        [UIHint("RichText")]
        public string Body { get; set; }

        public Guid BlogId { get; set; }
        public Blog Blog { get; set; }

        public Guid? ParentId { get; set; }
        public BlogComment Parent { get; set; }
        public virtual ICollection<BlogComment> BlogComments { get; set; }

        internal class configuration : EntityTypeConfiguration<BlogComment>
        {
            public configuration()
            {
                HasRequired(p => p.Blog).WithMany(t => t.BlogComments).HasForeignKey(p => p.BlogId);
                HasRequired(p => p.Parent).WithMany(t => t.BlogComments).HasForeignKey(p => p.ParentId);
            }
        }
    }
}