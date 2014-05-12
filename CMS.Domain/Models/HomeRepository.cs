using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using CMS.Domain.Abstract;
using CMS.Domain.Entities;
using CMS.Domain.DataAccess;

namespace CMS.Domain.Models
{
    public class HomeRepository : IHomeRepository
    {
        public List<Page> MainMenu()
        {
            List<Page> m_Pages = DBHome.MainMenu();
            return m_Pages;            
        }

        public WidgetContainer getContainer(int id)
        {
            WidgetContainer m_Container = DBWidgetContainer.RetrieveOne(id);
            return m_Container;
        }

        public List<BlogPost> NewsAnnouncements()
        {
            List<BlogPost> m_Posts = new List<BlogPost>();
            return m_Posts;
        }

        public List<Event> FeaturedEvents()
        {
            List<Event> m_Events = DBEvent.getFeaturedEvents();
            return m_Events;
        }

        public List<MenuItem> NonSystemMenu(int id)
        {
            List<MenuItem> m_MenuItems = DBMenuItem.RetrieveAll(id);
            return m_MenuItems;
        }

        public List<BlogPost> GetBlog(int id)
        {
            List<BlogPost> m_BlogPosts = DBBlogPost.RetrieveAllByCategory(id);
            return m_BlogPosts;
        }

        public List<BlogPost> GetNews()
        {
            List<BlogPost> m_BlogPosts = DBBlogPost.getNewsRotator();
            return m_BlogPosts;
        }

        public BlogPost SwapNews(int id)
        {
            BlogPost m_BlogPost = DBBlogPost.RetrieveOne(id);
            return m_BlogPost;
        }

        public void SubmitComment(BlogPostComment m_Comment)
        {
            DBHome.SubmitComment(m_Comment);

            /*MailMessage m_Message = new MailMessage("website@solanolibrary.com", "jwiggint@gmail.com");
            m_Message.Subject = "New Blog Post for Approval";
            m_Message.Body = @"Go approve the comment";
            SmtpClient client = new SmtpClient("localhost");
            client.UseDefaultCredentials = true;

            client.Send(m_Message);*/
        }

    }
}