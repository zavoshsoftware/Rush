using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModels;
using Helper;
using Models;
using System.Text.RegularExpressions;

namespace Rush.Controllers
{
    public class HomeController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        MenuHelper menuHelper = new MenuHelper();
        // GET: Home
        [Route("")]
        public ActionResult Index()
        {
            HomeViewModel home = new HomeViewModel(); 
            home.Menu = menuHelper.ReturnMenu();
            home.Blogs= db.Blogs.Where(current => current.IsDeleted == false && current.IsActive == true).OrderByDescending(current => current.CreationDate).Take(3).ToList();
            home.SliderBoldText = db.Texts.Where(current => current.Id == new Guid("6c4e2e24-87cb-4089-ada2-34b95cab3c06")).FirstOrDefault().Body;
            home.SliderText = db.Texts.Where(current => current.Id == new Guid("4f9f91a7-8283-47b9-9f4e-f47c256e1839")).FirstOrDefault().Body;
            home.UnderSliderMainText = db.Texts.Where(current => current.Id == new Guid("e13696c4-da8b-4469-8fb4-03f5f79193b9")).FirstOrDefault();
            home.UnderSliderText = db.Texts.Where(current => current.TextTypeId == new Guid("855e085c-3dbd-4436-8cf6-afb085b1bed9") && current.Id!=new Guid("e13696c4-da8b-4469-8fb4-03f5f79193b9")).ToList();
            home.MiddleText= db.TextTypes.Where(current => current.Id == new Guid("37c0b32c-c57b-4583-80fa-8af2f23e3be7")).FirstOrDefault().Description;
            home.Chooseus = db.Texts.Where(current => current.TextTypeId == new Guid("a79d60fa-f7d3-4c57-bc20-bf5e6e398356") && current.IsDeleted==false && current.IsActive==true).ToList();
            home.WhyRush = db.TextTypes.Where(current => current.Id == new Guid("a79d60fa-f7d3-4c57-bc20-bf5e6e398356") ).FirstOrDefault();
            home.FooterLink = menuHelper.GetFooterLink();
            home.Username = menuHelper.ReturnUsername();
            home.Customers = db.Customers.Where(current => current.IsDeleted == false).OrderBy(current => current.Priority).ToList();
            home.ContactInfo = ReturnContactInfo();
            ViewBag.Title = "آژانس دیجیتال مارکتینگ راش وب";
            ViewBag.Canonical = "https://www.rushweb.ir";
            ViewBag.Description = @"راش وب، از طراحی سایت تا سئو، خرید رپورتاژ آگهی و تبلیغات گوگل تا تولید محتوا و هرآنچه برای درخشش در گوگل احتیاج دارید را در اختیارتان می گذارد.";
            //home.TeamMembers = db.Teams.Where(current => current.IsDeleted == false).OrderBy(current => current.Priority).ToList();

            return View(home);
        }
       public Contact_Index ReturnContactInfo()
        {
            Contact_Index contact = new Contact_Index();
            contact.MainText = db.TextTypes.Where(current => current.Id == new Guid("5eab3b9e-5b32-47ef-9017-00f3b46a00ac")).FirstOrDefault();
            contact.Address = db.Texts.Where(current => current.Id == new Guid("d598a66e-26b5-4e46-8b23-dd057a1e37b3")).FirstOrDefault();
            contact.Phone = db.Texts.Where(current => current.Id == new Guid("a927030d-aa26-47bf-b301-fb7fb9dcd5ab")).FirstOrDefault();
            contact.Email = db.Texts.Where(current => current.Id == new Guid("05c5aaed-5d83-47df-b303-31bc59dac324")).FirstOrDefault();
            ViewBag.Canonical = "https://www.rushweb.ir/home/contact";
            //contact.MainText = db.TextTypes.Where(current => current.Id == new Guid("581502d5-6b50-401c-8f3d-4db9c1d64266")).FirstOrDefault();
            //contact.Address = db.Texts.Where(current => current.Id == new Guid("8526adb9-3392-4797-a4ee-309615a11005")).FirstOrDefault();
            //contact.Phone = db.Texts.Where(current => current.Id == new Guid("3e30248c-36b9-40f1-bbf6-9f133d918b98")).FirstOrDefault();
            //contact.Email = db.Texts.Where(current => current.Id == new Guid("0646108e-f254-492d-aa9b-8f84242baee3")).FirstOrDefault();
            return contact;
        }
        public ActionResult Contact()
        {
            ContactViewModel contact = new ContactViewModel();
            contact.Menu = menuHelper.ReturnMenu();
            contact.FooterLink = menuHelper.GetFooterLink();
            contact.Username = menuHelper.ReturnUsername();
            ViewBag.Canonical = "https://www.rushweb.ir/home/contact";
            return View(contact);
        }
        public ActionResult About()
        {
            AboutViewModel about = new AboutViewModel();
            about.Menu = menuHelper.ReturnMenu();
            about.AbouText = db.Texts.Where(current => current.Id == new Guid("06b5e44f-45f8-43ed-8e7a-55852fcfe967")).FirstOrDefault();
            about.FooterLink = menuHelper.GetFooterLink();
            about.Username = menuHelper.ReturnUsername();
            ViewBag.Canonical = "https://www.rushweb.ir/home/about";
            return View(about);
        }

        public ActionResult SendMessage(string name,string cellNum,string message)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    ContactForm contact = new ContactForm();
                    contact.Id = Guid.NewGuid();
                    contact.Name = name;
                    contact.CellNum = cellNum;
                    contact.Body = message;
                    contact.IsActive = true;
                    contact.IsDeleted = false;
                    contact.CreationDate = DateTime.Now;

                    db.ContactForms.Add(contact);
                    db.SaveChanges();

                    return Json("true", JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json("false", JsonRequestBehavior.AllowGet);
                }
            }
            else
                return Json("false", JsonRequestBehavior.AllowGet);
        }
        public ActionResult JoinNewsLetter(string email)
        {
            if (ModelState.IsValid)
            { bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);

                if (!isEmail)
                    return Json("InvalidEmail", JsonRequestBehavior.AllowGet);
                else
                {
                    try
                    {
                        NewsLetter newsLetter = new NewsLetter();
                        newsLetter.Id = Guid.NewGuid();
                        newsLetter.Email = email;
                        newsLetter.IsActive = true;
                        newsLetter.IsDeleted = false;
                        newsLetter.CreationDate = DateTime.Now;

                        db.NewsLetters.Add(newsLetter);
                        db.SaveChanges();

                        return Json("true", JsonRequestBehavior.AllowGet);
                    }
                    catch
                    {
                        return Json("false", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            else
                return Json("false", JsonRequestBehavior.AllowGet);
        }
    }
}