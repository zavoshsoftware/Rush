using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models
{
    public class StepDiscount : BaseEntity
    {
        public StepDiscount()
        {
            StepDiscountDetails = new List<StepDiscountDetail>();
        }
        [Display(Name="عنوان")]
        public string Title { get; set; }

        public virtual ICollection<StepDiscountDetail> StepDiscountDetails { get; set; }
    }
}