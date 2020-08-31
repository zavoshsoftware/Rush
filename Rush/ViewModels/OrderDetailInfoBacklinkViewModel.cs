using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class OrderDetailInfoBacklinkViewModel : BaseViewModel
    {
        public Guid Id { get; set; }
        [Display(Name = "کلمه کلیدی*")]
        [Required(ErrorMessage = "فیلد {0} را تکمیل نمایید")]
        public string BacklinkKeyword { get; set; }

        [Display(Name = "آدرس لینک*")]
        [Required(ErrorMessage = "فیلد {0} را تکمیل نمایید")]
        public string BacklinkUrl { get; set; }

        public int StatusCode { get; set; }
    }
}