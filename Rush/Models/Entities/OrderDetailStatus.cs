using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models
{
    public class OrderDetailStatus : BaseEntity
    {
        public OrderDetailStatus()
        {
            OrderDetails = new List<OrderDetail>();
            OrderDetailInformations=new List<OrderDetailInformation>();
        }

        [Display(Name = "وضعیت")]
        [StringLength(30)]
        [Required]
        public string Title { get; set; }

        [Display(Name = "کد")]
        [Required]
        public int Code { get; set; }

        public virtual ICollection<OrderDetailInformation> OrderDetailInformations { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}