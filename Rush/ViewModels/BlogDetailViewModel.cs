using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class BlogDetailViewModel:BaseViewModel
    {
        public List<BlogGroupCount> BlogGroups { get; set; }
        public Blog Blog { get; set; }
        public List<Blog> RecentBlogs { get; set; }

        public List<CommentList> Comments { get; set; }
        public string Writter { get; set; }
        public string WritterId { get; set; }
    }
    public class BlogGroupCount
    {
        public BlogGroup BlogGroup { get; set; }
         public int Count { get; set; }
    }

    public class CommentList
    {
        public BlogComment Comment { get; set; }
        public List<BlogComment> Responses { get; set; }
    }
}