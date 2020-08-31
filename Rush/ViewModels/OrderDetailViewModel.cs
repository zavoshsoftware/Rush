using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class OrderDetailViewModel:BaseViewModel
    {
        public Order Order { get; set; }
        public List<OrderDetailInformation> OrderDetailInformations { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}