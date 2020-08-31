﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models
{
    public class SiteType:BaseEntity
    {
        public SiteType()
        {
            ServiceForms = new List<ServiceForm>();
        }
        [Display(Name = "نوع سایت")]
        public string Title { get; set; }
        public virtual ICollection<ServiceForm> ServiceForms { get; set; }
    }
}