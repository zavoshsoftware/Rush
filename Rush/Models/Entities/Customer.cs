﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models
{
    public class Customer:BaseEntity
    {
        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Display(Name = "تصویر")]
        public string ImageUrl { get; set; }

        [Display(Name = "اولویت")]
        public int Priority { get; set; }
    }
}