using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ViewModels
{
    public class RegisterUserViewModel:BaseViewModel
    {
        [Display(Name="نام و نام خانوادگی")]
        public string FullName { get; set; }

        [Display(Name="شماره موبایل")]
        public string CellNumber { get; set; }

        [Display(Name="کلمه عبور")]
        public string Password { get; set; }


    }
}

 