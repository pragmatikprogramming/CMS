﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Domain.Abstract;
using CMS.Domain.Entities;
using CMS.Domain.DataAccess;


namespace CMS.Domain.Models
{
    public class BlogPostRepository : IBlogPostRepository
    {
        public void Create(BlogPost m_BlogPost)
        {
            DBBlogPost.Create(m_BlogPost);
        }

        public BlogPost RetrieveOne(int id)
        {
            BlogPost m_BlogPost = DBBlogPost.RetrieveOne(id);
            return m_BlogPost;
        }

        public List<BlogPost> RetrieveAll()
        {
            List<BlogPost> m_BlogPosts = DBBlogPost.RetrieveAll();
            return m_BlogPosts;
        }

        public void Update(BlogPost m_BlogPost)
        {
            DBBlogPost.Update(m_BlogPost);
        }

        public void Delete(int id)
        {
            DBBlogPost.Delete(id);
        }

        public int getPageWorkFlowState(int id)
        {
            int mVal = DBBlogPost.getPageWorkFlowState(id);
            return mVal;
        }

        public int getLockedBy(int id)
        {
            int mVal = DBBlogPost.getLockedBy(id);
            return mVal;
        }

        public void lockBlogPost(int id)
        {
            DBBlogPost.lockBlogPost(id);
        }

        public void unlockBlogPost(int id)
        {
            DBBlogPost.unlockBlogPost(id);
        }

        public void publishBlogPost(int id)
        {
            DBBlogPost.publishBlogPost(id);
        }

        public List<Category> getCategories()
        {
            List<Category> m_Categories = DBBlogPost.getCategories();
            return m_Categories;
        }
    }
}