using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViewModels;
using Models;

namespace Helper
{
    public class MenuHelper
    {
        private DatabaseContext db = new DatabaseContext();
        public List<Menu> ReturnMenu()
        {
            List<Menu> menu = new List<Menu>();
            

            List<ServiceGroup> serviceGroupList = db.ServiceGroups.Where(current => current.IsDeleted == false && current.IsActive == true).OrderBy(current=>current.Order).ToList();
            foreach(ServiceGroup serviceGroup in serviceGroupList)
            {
                bool hasChild = false;
                List<Service> services = db.Services.Where(current => current.IsDeleted == false && current.IsActive == true && current.ServiceGroupId == serviceGroup.Id).ToList();
                if (services.Count() > 0)
                     hasChild = true;


                menu.Add(new Menu
                {
                    ServiceGroup = serviceGroup,
                    Services = services,
                    HasChild = hasChild
                });
            }

            return menu;
        }
        public string GetFooterLink()
        {
            string url = HttpContext.Current.Request.Url.PathAndQuery;
            if (url == "/")
                return
                    "<a target='_blank' href='http://zavoshsoftware.com/'>زاوش</a>";
           
            else
                return "زاوش";
        }
        public string ReturnUsername()
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                User user = db.Users.Where(current => current.CellNum == HttpContext.Current.User.Identity.Name && current.IsDeleted==false && current.IsActive==true).FirstOrDefault();
                if (user != null)
                    return user.FullName;
                else
                    return null;
            }
            else
                return null;
        }
    }
}