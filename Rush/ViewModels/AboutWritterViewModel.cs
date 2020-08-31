using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModels
{
    public class AboutWritterViewModel:BaseViewModel
    {
        public List<Blog> Blogs { get; set; }
        public User Writter { get; set; }
    }
}