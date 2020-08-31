using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class OrderDetailInfoViewModel : BaseViewModel
    {
        public OrderDetail OrderDetail { get; set; }
        public List<OrderDetailInformation> orderDetailInformations { get; set; }
    }
}