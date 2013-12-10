using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Domain.Abstract;
using CMS.Domain.Entities;
using CMS.Domain.DataAccess;

namespace CMS.Domain.Models
{
    public class NewsRepository : INewsRepository
    {
        public List<BlogPost> GetNews()
        {
            List<BlogPost> m_BlogPosts = DBBlogPost.RetrieveAllByCategory(2);
            return m_BlogPosts;
        }

        public WidgetContainer GetNewsContainer()
        {
            WidgetContainer m_Container = DBWidgetContainer.RetrieveOneByName("News");
            return m_Container;
        }

        public BlogPost GetArticle(int id)
        {
            BlogPost m_BlogPost = DBBlogPost.RetrieveOne(id);
            return m_BlogPost;
        }
    }
}