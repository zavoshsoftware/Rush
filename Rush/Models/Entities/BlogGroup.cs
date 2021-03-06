﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Models
{
    public class BlogGroup : BaseEntity
    {

        public BlogGroup()
        {
            Blogs = new List<Blog>();
        }
        [Display(Name = "عنوان گروه")]
        public string Title { get; set; }
        [Display(Name = "خلاصه")]
        public string Summery { get; set; }

        [Display(Name = "تصویر")]
        public string ImageUrl { get; set; }
        public List<Blog> Blogs  { get; set; }

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

        [Display(Name = "توضیحات")]
        [DataType(DataType.Html)]
        [AllowHtml]
        [Column(TypeName = "ntext")]
        [UIHint("RichText")]
        public string Body { get; set; }
    }
}