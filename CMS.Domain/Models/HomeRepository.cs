using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            //id is the page id at this point.  We need template id to retrieve the container
            if (id == 0)
            {
                id = 39;
            }
            Page m_Page = DBPage.RetrieveOne(id);
            WidgetContainer m_Container = DBWidgetContainer.RetrieveOneByTemplateId(m_Page.TemplateId);
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

        public List<BlogPost> GetTeenBlog()
        {
            List<BlogPost> m_BlogPosts = DBBlogPost.RetrieveAllByCategory(3);
            return m_BlogPosts;
        }

        public List<BlogPost> GetNews()
        {
            List<BlogPost> m_BlogPosts = DBBlogPost.RetrieveAllByCategory(2);
            return m_BlogPosts;
        }

        public BlogPost SwapNews(int id)
        {
            BlogPost m_BlogPost = DBBlogPost.RetrieveOne(id);
            return m_BlogPost;
        }
    }
}