﻿using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModels
{
    public class BlogListViewModel : BaseViewModel
    {
        public List<Blog> Blogs { get; set; }
        public BlogGroup BlogGroup { get; set; }
    }
}