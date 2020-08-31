using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models
{
    public class EmailAddress:BaseEntity
    {
        [Display(Name ="ایمیل")]
        public string Email { get; set; }
    }
}