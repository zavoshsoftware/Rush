using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class OrderListViewModel:BaseViewModel
    {
        public List<Order> Orders { get; set; }
    }
}