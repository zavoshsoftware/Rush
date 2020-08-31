using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models
{
    public class Rate:BaseEntity
    {
        [Display(Name = "نوع")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public int Type { get; set; }

        [Display(Name = "موجودیت")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public Guid EntityId { get; set; }

        [Display(Name = "رتبه")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public decimal StarRate { get; set; }

        [Display(Name = "IP")]
        [MaxLength(20, ErrorMessage = "تعداد کاراکتر {0} نباید بیشتر از {1} باشد.")]
        public string IP { get; set; }
    }
}