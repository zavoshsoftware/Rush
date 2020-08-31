namespace Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Globalization;
    using System.Linq;

    public class User : BaseEntity
    {
        public User()
        {

            ServiceForms = new List<ServiceForm>();
            Blogs = new List<Blog>();
            Orders = new List<Order>();

        }

        [Display(Name = "کلمه عبور")]
        [StringLength(150, ErrorMessage = "طول {0} نباید بیشتر از {1} باشد")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "تلفن همراه")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        [StringLength(20, ErrorMessage = "طول {0} نباید بیشتر از {1} باشد")]
        [RegularExpression(@"(^(09|9)[0123456789][0123456789]\d{7}$)|(^(09|9)[0123456789][0123456789]\d{7}$)", ErrorMessage = "Invalid")]
        public string CellNum { get; set; }

        [Display(Name = "نام و نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        [StringLength(250, ErrorMessage = "طول {0} نباید بیشتر از {1} باشد")]
        public string FullName { get; set; }

        [Display(Name = "کد")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public int Code { get; set; }

       
        
        [Display(Name = "آدرس")]
        [StringLength(500, ErrorMessage = "طول {0} نباید بیشتر از {1} باشد")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Display(Name = "کد پستی")]
        [StringLength(11, ErrorMessage = "طول {0} نباید بیشتر از {1} باشد")]
        public string PostalCode { get; set; }

        public Guid RoleId { get; set; }
     
        public virtual Role Role { get; set; }

        public virtual ICollection<ServiceForm> ServiceForms { get; set; }

        public virtual ICollection<Blog> Blogs { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

    }
}

