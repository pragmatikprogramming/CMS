using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMS.Domain.Entities;
using CMS.Domain.Abstract;
using CMS.Domain.HelperClasses;
using Recaptcha;



namespace CMS.WebUI.Controllers
{
    public class HomeController : Controller
    {
        IPageRepository PageRepository;
        IHomeRepository HomeRepository;
        IImageRepository ImageRepository;
        IFormRepository FormRepository;
        IFAQRepository FAQRepository;
        IBlogPostRepository BlogPostRepository;
        IEventRepository EventRepository;

        public HomeController()
        {

        }

        public HomeController(IPageRepository PageRepo, IHomeRepository HomeRepo, IImageRepository ImageRepo, IFormRepository FormRepo, IFAQRepository FAQRepo, IBlogPostRepository BlogPostRepo, IEventRepository EventRepo)
        {
            PageRepository = PageRepo;
            HomeRepository = HomeRepo;
            ImageRepository = ImageRepo;
            FormRepository = FormRepo;
            FAQRepository = FAQRepo;
            BlogPostRepository = BlogPostRepo;
            EventRepository = EventRepo;
        }

        public ActionResult Index(string friendlyURL, int id = 0)
        {
            ViewBag.CurrentYear = DateTime.Now.Year;
            string[] ip_address = Request.UserHostAddress.Split('.');
            if (ip_address.Length == 4 && ip_address[0] != "127")
            {
                string m_Network = ip_address[0] + "." + ip_address[1] + "." + ip_address[2];
                ViewBag.Network = m_Network;
                ViewBag.LastIpOctet = Int32.Parse(ip_address[3]);
            }

            if (id == 0 && friendlyURL == "Home")
            {
                Page m_Page = PageRepository.RetrieveOne(39);
                ViewBag.TemplateId = 4;
                ViewBag.PageId = 39;
                return View("Home", m_Page);
            }
            else
            {
                Page m_Page = new Page();

                if (friendlyURL == null)
                {
                    m_Page = PageRepository.RetrieveOne(id);
                }
                else
                {
                    m_Page = PageRepository.RetrieveOneByFriendlyURL(friendlyURL);
                }

                if (m_Page == null || m_Page.TemplateName == null || m_Page.TemplateName == "")
                {
                    return View("404");
                }
                else
                {
                    if (m_Page.RedirectURL == null || m_Page.RedirectURL == string.Empty)
                    {
                        ViewBag.PageType = m_Page.PageType;
                        ViewBag.id = m_Page.PageTypeId;
                        ViewBag.PageId = m_Page.PageID;
                        ViewBag.TemplateId = m_Page.TemplateId;
                        
                        return View(m_Page.TemplateName, m_Page);
                    }
                    else
                    {
                        return Redirect(m_Page.RedirectURL);
                    }
                }
            }
        }

        public ActionResult MainMenu()
        {
            List<Page> m_Pages = HomeRepository.MainMenu();
            return View("MainMenu", m_Pages);
        }

        public ActionResult Container(int id)
        {
            WidgetContainer m_Container = HomeRepository.getContainer(id);
            return View("Container", m_Container);
        }

        public ActionResult NonSystemMenu(int id, string viewName)
        {
            List<MenuItem> m_MenuItems = HomeRepository.NonSystemMenu(id);
            ViewBag.MenuName = Utility.getMenuName(id);
            return View(viewName, m_MenuItems);
        }

        public ActionResult SystemSubMenu(int id)
        {
            List<Page> m_Pages = PageRepository.RetrieveAll(id);
            Page m_Page = PageRepository.RetrieveOne(id);
            ViewBag.ParentName = m_Page.NavigationName;
            ViewBag.MyCountOdd = 1;
            ViewBag.MyCountEven = 1;
            return View("SystemSubMenu", m_Pages);
        }

        public RedirectResult Search(string q, int searchType)
        {
            if(searchType == 1)
            {
               return Redirect("http://ls2pac.snap.lib.ca.us/?config=SOLANO#section=search&term=" + HttpUtility.UrlEncode(q));
            }
            else if(searchType == 2)
            {
                return Redirect("/search-results?q=" + Url.Encode(q));
            }
            else if(searchType == 3)
            {
                return Redirect("http://www.google.com?q=" + Url.Encode(q));
            }
            else
            {
                return Redirect("/Home");
            }
        }

        public ActionResult FeaturedEvents()
        {
            List<Event> m_Events = HomeRepository.FeaturedEvents();
            return View("FeaturedEvents", m_Events);
        }

        public ActionResult Event(int id)
        {
            Event m_Event = EventRepository.RetrieveOne(id);
            ViewBag.PageType = 6;

            return View("HomeFullWidth", m_Event);
        }

        public ActionResult WirelessPrint(string id)
        {
            ViewBag.Network = id;
            return View("WirelessPrint");
        }

        public ActionResult getBlog(int parentId, int id)
        {
            ViewBag.Count = 0;
            ViewBag.TemplateId = parentId;
            List<BlogPost> m_BlogPosts = HomeRepository.GetBlog(id);
            return View("getBlog", m_BlogPosts);
        }

        public ActionResult getNews()
        {
            ViewBag.Count = 1;
            List<BlogPost> m_BlogPosts = HomeRepository.GetNews();
            ViewBag.NewsId = m_BlogPosts[0].Id;
            return View("getNews", m_BlogPosts);
        }

        public ActionResult getForm(int parentId, int id)
        {
            ViewBag.ParentId = parentId;
            ViewBag.Count = 0;
            Form m_Form = FormRepository.RetrieveOne(id);
            return View("getForm", m_Form);
        }

        public ActionResult getFAQ(int id)
        {
            List<FAQQuestions> m_Questions = FAQRepository.RetrieveAllFAQQuestions(id);
            return View("getFAQ", m_Questions);
        }

        public ActionResult SwapNews(int id)
        {
            ViewBag.BlogPost = HomeRepository.SwapNews(id);
            ViewBag.Id = id;
            ViewBag.Count = 1;
            List<BlogPost> m_BlogPosts = HomeRepository.GetNews();

            return View("SwapNews", m_BlogPosts);
        }

        public ActionResult BlogPost(int id, int parentId = 5)
        {
            BlogPost m_BlogPost = BlogPostRepository.RetrieveOne(id);
            ViewBag.PageType = 5;
            ViewBag.PageId = null;
            ViewBag.TemplateId = parentId;
            ViewBag.Comment = new BlogPostComment();
            string m_Template = Utility.GetTemplateById(parentId);

            return View(m_Template, m_BlogPost);
        }


        [HttpPost]
        public ActionResult SubmitComment(BlogPostComment m_Comment, string recaptcha_challenge_field, string recaptcha_response_field, int TemplateId, int Id)
        {
            RecaptchaValidator m_Validator = new RecaptchaValidator();
            m_Validator.Challenge = recaptcha_challenge_field;
            m_Validator.Response = recaptcha_response_field;
            m_Validator.PrivateKey = "6Ldz_fcSAAAAAJwhvY4Ns3YP9GWDehrct05bUYSj";
            m_Validator.RemoteIP = Request.ServerVariables["REMOTE_ADDR"];

            RecaptchaResponse m_Response = m_Validator.Validate();

            if (!m_Response.IsValid)
            {
                ModelState.AddModelError("captcha", "Incorrect Captcha Response");
            }

            if (ModelState.IsValid)
            {
                HomeRepository.SubmitComment(m_Comment);
                return RedirectToAction("BlogPost", "Home", new { id = m_Comment.Id, parentId = TemplateId });
            }
            else
            {
                BlogPost m_BlogPost = BlogPostRepository.RetrieveOne(Id);
                ViewBag.PageType = 5;
                ViewBag.PageId = null;
                ViewBag.TemplateId = TemplateId;
                ViewBag.Comment = m_Comment;
                string m_Template = Utility.GetTemplateById(TemplateId);

                return View(m_Template, m_BlogPost);
            }
        }

        public ActionResult GetComments(int id)
        {
            List<BlogPostComment> m_Comments = BlogPostRepository.GetComments(id);
            return View("GetComments", m_Comments);
        }

        [HttpPost]
        public ActionResult ProcessForm(int parentId, int id, string recaptcha_challenge_field, string recaptcha_response_field)
        {
            /** RECAPTCHA VERIFICATION **/


            RecaptchaValidator m_Validator = new RecaptchaValidator();
            m_Validator.Challenge = recaptcha_challenge_field;
            m_Validator.Response = recaptcha_response_field;
            m_Validator.PrivateKey = "6Ldz_fcSAAAAAJwhvY4Ns3YP9GWDehrct05bUYSj";
            m_Validator.RemoteIP = Request.ServerVariables["REMOTE_ADDR"];

            RecaptchaResponse m_Response = m_Validator.Validate();

            if (!m_Response.IsValid)
            {
                ModelState.AddModelError("captcha", "Incorrect Captcha Response");
            }

            string formData = "";
            string emailBody = "";
            Form m_Form = FormRepository.RetrieveOne(id);

            Dictionary<string, int> m_Ffs = new Dictionary<string,int>();

            foreach (FormField ff in m_Form.FormFields)
            {
                if (ff.IsRequired == 1)
                {
                    m_Ffs.Add(ff.Label, 1);
                }
                else
                {
                    m_Ffs.Add(ff.Label, 0);
                }
            }

            m_Ffs.Add("recaptcha_challenge_field", 0);
            m_Ffs.Add("recaptcha_response_field", 0);

            emailBody += "<table><tr><td>Field:</td><td>Value:</td></tr>";

            foreach (string key in Request.Form.Keys)
            {
                if (string.IsNullOrEmpty(Request.Form[key]) && m_Ffs[key] == 1)
                {
                    ModelState.AddModelError(key, "Please enter a value for " + key);
                }
                if (key != "recaptcha_challenge_field" && key != "recaptcha_response_field")
                {
                    formData += FormRepository.RemoveLineEndings(key) + "::" + FormRepository.RemoveLineEndings(Request.Form[key]) + "^^";
                    emailBody += "<tr><td>" + key + "</td><td>" + Request.Form[key] + "</td></tr>";
                }
            }

            emailBody += "</table>";

            string m_Label = FormRepository.SpecialExistsOnForm(id);

            if (m_Label != null && m_Label.Length > 0)
            {
                string m_Email = Request.Form[m_Label];

                if (m_Email.Length > 0)
                {
                    FormRepository.SendFormData(m_Email, m_Form.FromEmail, emailBody, m_Form.FormName + " - Submission");
                }
            }

            if (ModelState.IsValid)
            {
                FormRepository.InsertFormData(formData, id);

                Page m_Page = PageRepository.RetrieveOne(parentId);

                string[] m_Emails = m_Form.SubmissionEmail.Split(',');

                foreach (string email in m_Emails)
                {
                    FormRepository.SendFormData(email, m_Form.FromEmail, emailBody, m_Form.FormName + " - Submission");
                }

                ViewBag.PageType = m_Page.PageType;
                ViewBag.id = m_Page.PageTypeId;
                ViewBag.isPostBack = 1;
                ViewBag.Success = m_Form.Success;
                ViewBag.PageId = m_Page.TemplateId;
                ViewBag.TemplateId = m_Page.TemplateId;
                return View(m_Page.TemplateName, m_Page);
            }
            else
            {
                Page m_Page = PageRepository.RetrieveOne(parentId);
                ViewBag.PageType = m_Page.PageType;
                ViewBag.id = m_Page.PageTypeId;
                ViewBag.PageId = m_Page.TemplateId;
                ViewBag.TemplateId = m_Page.TemplateId;
                ViewBag.isPostBack = 0;
                return View(m_Page.TemplateName, m_Page);
            }
        }

        /*public FileContentResult ExportCSV()
        {
            string csv = "1,2,3,4,5,6,7,8,9";

            return File(new System.Text.UTF8Encoding().GetBytes(csv), "text/csv", "Report123.csv");
        }*/
    }
}
