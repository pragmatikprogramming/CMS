using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Abstract;
using CMS.Domain.Entities;

namespace CMS.Domain.Abstract
{
    public interface IHomeRepository
    {
        List<Page> MainMenu();
        WidgetContainer getContainer(int id);
        List<BlogPost> NewsAnnouncements();
        List<Event> FeaturedEvents();
        List<MenuItem> NonSystemMenu(int id);
        List<BlogPost> GetBlog(int id);
        List<BlogPost> GetNews();
        BlogPost SwapNews(int id);
        void SubmitComment(BlogPostComment m_Comment);
        
    }
}
