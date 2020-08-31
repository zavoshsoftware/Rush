using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class BlogGroupListViewModel:BaseViewModel
    {
        //public List<Blog> Blogs { get; set; }
        public List<BlogGroup> BlogGroups { get; set; }
    }
}