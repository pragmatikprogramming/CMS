using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Entities;

namespace CMS.Domain.Abstract
{
    public interface IBlogPostRepository
    {
        void Create(BlogPost m_BlogPost);
        BlogPost RetrieveOne(int id);
        List<BlogPost> RetrieveAll();
        void Update(BlogPost m_BlogPost);
        void Delete(int id);
        int getPageWorkFlowState(int id);
        int getLockedBy(int id);
        void lockBlogPost(int id);
        void unlockBlogPost(int id);
        void publishBlogPost(int id);
        List<Category> getCategories();
        List<BlogPost> RetrieveAllByCategory(int Category);
        List<BlogPostComment> GetComments(int BlogId);
        void CommentPublish(int id);
        void CommentDelete(int id);
        void newsSortOrder(List<int> m_SortOrder);
        List<int> getNewsRotatorBlogIds();
    }
}
