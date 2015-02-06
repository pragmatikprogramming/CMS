using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Domain.Entities;
using CMS.Domain.HelperClasses;
using System.Data.SqlClient;

namespace CMS.Domain.DataAccess
{
    public class DBBlogPost
    {
        public static void Create(BlogPost m_BlogPost)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT IDENT_CURRENT('CMS_BlogPosts')";
            SqlCommand getBlogId = new SqlCommand(queryString, conn);
            int m_BlogId = (int)(decimal)getBlogId.ExecuteScalar();
            conn.Close();

            if (m_BlogId == 1)
            {
                conn.Open();
                queryString = "SELECT COUNT(*) FROM CMS_BlogPosts";
                SqlCommand getPageCount = new SqlCommand(queryString, conn);
                int pageCount = (int)getPageCount.ExecuteScalar();

                if (m_BlogId == pageCount)
                {
                    m_BlogId = pageCount + 1;
                }
                conn.Close();
            }
            else
            {
                m_BlogId++;
            }


            conn.Open();

            queryString = "INSERT INTO CMS_BlogPosts(blogId, title, publishDate, expirationDate, contentGroup, [content], comments, pageWorkFlowState, lockedBy, lastModifiedBy, lastModifiedDate, newsImageId, newsImageName, author, introText, redirectUrl) VALUES(@blogId, @title, @publishDate, @expirationDate, @contentGroup, @content, @comments, 1, @lockedBy, @lastModifiedBy, @lastModifiedDate, @newsImageId, @newsImageName, @author, @introText, @redirectUrl)";
            SqlCommand insertBlogPost = new SqlCommand(queryString, conn);
            insertBlogPost.Parameters.AddWithValue("blogId", m_BlogId);
            insertBlogPost.Parameters.AddWithValue("title", m_BlogPost.Title);
            insertBlogPost.Parameters.AddWithValue("publishDate", m_BlogPost.PublishDate.ToString());
            insertBlogPost.Parameters.AddWithValue("expirationDate", m_BlogPost.PublishDate.ToString());
            insertBlogPost.Parameters.AddWithValue("contentGroup", m_BlogPost.ContentGroup);
            insertBlogPost.Parameters.AddWithValue("content", m_BlogPost.Content);
            insertBlogPost.Parameters.AddWithValue("comments", m_BlogPost.Comments);
            insertBlogPost.Parameters.AddWithValue("lockedBy", HttpContext.Current.Session["uid"]);
            insertBlogPost.Parameters.AddWithValue("lastModifiedBy", HttpContext.Current.Session["uid"]);
            insertBlogPost.Parameters.AddWithValue("lastModifiedDate", DateTime.Now);
            insertBlogPost.Parameters.AddWithValue("newsImageId", m_BlogPost.NewsImageId);
            insertBlogPost.Parameters.AddWithValue("newsImageName", m_BlogPost.NewsImageName ?? "");
            insertBlogPost.Parameters.AddWithValue("author", m_BlogPost.Author ?? "");
            insertBlogPost.Parameters.AddWithValue("introText", m_BlogPost.IntroText);
            insertBlogPost.Parameters.AddWithValue("redirectUrl", m_BlogPost.RedirectUrl ?? "");
            insertBlogPost.ExecuteNonQuery();

            foreach (int catId in m_BlogPost.Categories)
            {
                queryString = "INSERT INTO CMS_BlogPostsToCategories(blogPostId, categoryId) VALUES(@blogId, @catId)";
                SqlCommand insertCat = new SqlCommand(queryString, conn);
                insertCat.Parameters.AddWithValue("blogId", m_BlogId);
                insertCat.Parameters.AddWithValue("catId", catId);
                insertCat.ExecuteNonQuery();
            }
            

            conn.Close();
        }

        public static BlogPost RetrieveOne(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString;
            string action = HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString();
            if (action == "BlogPreview")
            {
                queryString = "SELECT * FROM CMS_BlogPosts WHERE id = @id";
            }
            else
            {
                queryString = "SELECT * FROM CMS_BlogPosts WHERE id = @id AND pageWorkFlowState != 4";
            }

            SqlCommand getBlogPost = new SqlCommand(queryString, conn);
            getBlogPost.Parameters.AddWithValue("id", id);
            SqlDataReader blogPostReader = getBlogPost.ExecuteReader();

            BlogPost m_BlogPost = new BlogPost();

            if(blogPostReader.Read())
            {
                m_BlogPost.Id = blogPostReader.GetInt32(0);
                m_BlogPost.BlogId = blogPostReader.GetInt32(1);
                m_BlogPost.Title = blogPostReader.GetString(2);
                m_BlogPost.PublishDate = blogPostReader.GetDateTime(3);
                m_BlogPost.ContentGroup = blogPostReader.GetInt32(4);
                m_BlogPost.Content = blogPostReader.GetString(5);
                m_BlogPost.PageWorkFlowState = blogPostReader.GetInt32(6);
                m_BlogPost.LockedBy = blogPostReader.GetInt32(7);
                m_BlogPost.LastModifiedBy = blogPostReader.GetInt32(8);
                m_BlogPost.LastModifiedDate = blogPostReader.GetDateTime(9);
                m_BlogPost.Comments = blogPostReader.GetInt32(10);
                m_BlogPost.ExpirationDate = blogPostReader.GetDateTime(11);
                m_BlogPost.NewsImageId = blogPostReader.GetInt32(12);
                m_BlogPost.Author = blogPostReader.GetString(13);
                m_BlogPost.IntroText = blogPostReader.GetString(14);
                m_BlogPost.NewsImageName = blogPostReader.GetString(15);
                m_BlogPost.RedirectUrl = blogPostReader.GetString(16);

                m_BlogPost.LockedByName = DBPage.GetLockedByName(m_BlogPost.LockedBy);
                m_BlogPost.LastModifiedByName = DBPage.GetLockedByName(m_BlogPost.LastModifiedBy);

                SqlConnection conn2 = DB.DbConnect();
                conn2.Open();

                queryString = "SELECT * FROM CMS_BlogPostsToCategories WHERE blogPostId = @blogId";
                SqlCommand getCats = new SqlCommand(queryString, conn2);
                getCats.Parameters.AddWithValue("blogId", m_BlogPost.BlogId);
                SqlDataReader catsReader = getCats.ExecuteReader();

                List<int> m_Cats = new List<int>();

                while (catsReader.Read())
                {
                    m_Cats.Add(catsReader.GetInt32(2));
                }

                m_BlogPost.Categories = m_Cats;

                conn2.Close();
            }
            

            conn.Close();
            return m_BlogPost;
        }

        public static List<BlogPost> RetrieveAll()
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_BlogPosts WHERE pageWorkFlowState != 4 ORDER BY blogId, publishDate, id DESC";
            SqlCommand getBlogPosts = new SqlCommand(queryString, conn);
            SqlDataReader blogPostReader = getBlogPosts.ExecuteReader();

            List<BlogPost> m_BlogPosts = new List<BlogPost>();
            int previousPageId = 0;

            while(blogPostReader.Read())
            {
                BlogPost m_BlogPost = new BlogPost();

                m_BlogPost.Id = blogPostReader.GetInt32(0);
                m_BlogPost.BlogId = blogPostReader.GetInt32(1);
                m_BlogPost.Title = blogPostReader.GetString(2);
                m_BlogPost.PublishDate = blogPostReader.GetDateTime(3);
                m_BlogPost.ContentGroup = blogPostReader.GetInt32(4);
                m_BlogPost.Content = blogPostReader.GetString(5);
                m_BlogPost.PageWorkFlowState = blogPostReader.GetInt32(6);
                m_BlogPost.LockedBy = blogPostReader.GetInt32(7);
                m_BlogPost.LastModifiedBy = blogPostReader.GetInt32(8);
                m_BlogPost.LastModifiedDate = blogPostReader.GetDateTime(9);
                m_BlogPost.Comments = blogPostReader.GetInt32(10);
                m_BlogPost.ExpirationDate = blogPostReader.GetDateTime(11);
                m_BlogPost.NewsImageId = blogPostReader.GetInt32(12);
                m_BlogPost.Author = blogPostReader.GetString(13);
                m_BlogPost.IntroText = blogPostReader.GetString(14);
                m_BlogPost.NewsImageName = blogPostReader.GetString(15);
                m_BlogPost.RedirectUrl = blogPostReader.GetString(16);
                m_BlogPost.LockedByName = DBPage.GetLockedByName(m_BlogPost.LockedBy);
                m_BlogPost.LastModifiedByName = DBPage.GetLockedByName(m_BlogPost.LastModifiedBy);

                if (previousPageId != m_BlogPost.BlogId)
                {
                    m_BlogPosts.Add(m_BlogPost);
                }

                previousPageId = m_BlogPost.BlogId;

                SqlConnection conn2 = DB.DbConnect();
                conn2.Open();

                queryString = "SELECT * FROM CMS_BlogPostsToCategories WHERE blogPostId = @blogId";
                SqlCommand getCats = new SqlCommand(queryString, conn2);
                getCats.Parameters.AddWithValue("blogId", m_BlogPost.BlogId);
                SqlDataReader catsReader = getCats.ExecuteReader();

                List<int> m_Cats = new List<int>();

                while (catsReader.Read())
                {
                    m_Cats.Add(catsReader.GetInt32(2));
                }

                m_BlogPost.Categories = m_Cats;

                conn2.Close();
            }

            conn.Close();
            return m_BlogPosts;
        }

        public static List<BlogPost> RetrieveAllByCategory(int Category)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_BlogPostsToCategories WHERE categoryId = @catId";
            SqlCommand getCats = new SqlCommand(queryString, conn);
            getCats.Parameters.AddWithValue("catId", Category);
            

            SqlDataReader catsReader = getCats.ExecuteReader();

            List<int> m_BlogIds = new List<int>();

            while (catsReader.Read())
            {
                m_BlogIds.Add(catsReader.GetInt32(1));
            }

            conn.Close();
            conn.Open();

            string BlogIds = string.Join(",", m_BlogIds.ToArray());

            queryString = "SELECT * FROM CMS_BlogPosts WHERE pageWorkFlowState = 2 AND publishDate >= @m_Date AND BlogId IN (" + BlogIds + ") ORDER BY blogId, id desc";
            SqlCommand getBlogPosts = new SqlCommand(queryString, conn);
            getBlogPosts.Parameters.AddWithValue("m_Date", DateTime.Now.AddYears(-1));
            SqlDataReader blogReader = getBlogPosts.ExecuteReader();

            int previousPageId = 0;

            List<BlogPost> m_BlogPosts = new List<BlogPost>();

            while (blogReader.Read())
            {
                BlogPost m_BlogPost = new BlogPost();

                m_BlogPost.Id = blogReader.GetInt32(0);
                m_BlogPost.BlogId = blogReader.GetInt32(1);
                m_BlogPost.Title = blogReader.GetString(2);
                m_BlogPost.PublishDate = blogReader.GetDateTime(3);
                m_BlogPost.ContentGroup = blogReader.GetInt32(4);
                m_BlogPost.Content = blogReader.GetString(5);
                m_BlogPost.PageWorkFlowState = blogReader.GetInt32(6);
                m_BlogPost.LockedBy = blogReader.GetInt32(7);
                m_BlogPost.LastModifiedBy = blogReader.GetInt32(8);
                m_BlogPost.LastModifiedDate = blogReader.GetDateTime(9);
                m_BlogPost.Comments = blogReader.GetInt32(10);
                m_BlogPost.ExpirationDate = blogReader.GetDateTime(11);
                m_BlogPost.NewsImageId = blogReader.GetInt32(12);
                m_BlogPost.Author = blogReader.GetString(13);
                m_BlogPost.IntroText = blogReader.GetString(14);
                m_BlogPost.NewsImageName = blogReader.GetString(15);
                m_BlogPost.RedirectUrl = blogReader.GetString(16);

                m_BlogPost.LockedByName = DBPage.GetLockedByName(m_BlogPost.LockedBy);
                m_BlogPost.LastModifiedByName = DBPage.GetLockedByName(m_BlogPost.LastModifiedBy);

                if (previousPageId != m_BlogPost.BlogId)
                {
                    m_BlogPosts.Add(m_BlogPost);
                }

                previousPageId = m_BlogPost.BlogId;
            }

            conn.Close();
            m_BlogPosts.Reverse();
            return m_BlogPosts;
        }

        public static void Update(BlogPost m_BlogPost)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            if (m_BlogPost.PageWorkFlowState == 1)
            {
                string queryString = "UPDATE CMS_BlogPosts SET title = @title, publishDate = @publishDate, expirationDate = @expirationDate, contentGroup = @contentGroup, content = @content, comments = @comments, pageWorkFlowState = 1, lockedBy = @lockedBy, lastModifiedBy = @lastModifiedBy, lastModifiedDate = @lastModifiedDate, newsImageId = @newsImageId, newsImageName = @newsImageName, author = @author, introText = @introText, redirectUrl = @redirectUrl WHERE id = @id";
                SqlCommand updateBlogPost = new SqlCommand(queryString, conn);
                updateBlogPost.Parameters.AddWithValue("title", m_BlogPost.Title);
                updateBlogPost.Parameters.AddWithValue("publishDate", m_BlogPost.PublishDate.ToString());
                updateBlogPost.Parameters.AddWithValue("expirationDate", m_BlogPost.ExpirationDate.ToString());
                updateBlogPost.Parameters.AddWithValue("contentGroup", m_BlogPost.ContentGroup);
                updateBlogPost.Parameters.AddWithValue("content", m_BlogPost.Content);
                updateBlogPost.Parameters.AddWithValue("comments", m_BlogPost.Comments);
                updateBlogPost.Parameters.AddWithValue("lockedBy", HttpContext.Current.Session["uid"]);
                updateBlogPost.Parameters.AddWithValue("lastModifiedBy", HttpContext.Current.Session["uid"]);
                updateBlogPost.Parameters.AddWithValue("lastModifiedDate", DateTime.Now);
                updateBlogPost.Parameters.AddWithValue("id", m_BlogPost.Id);
                updateBlogPost.Parameters.AddWithValue("newsImageId", m_BlogPost.NewsImageId);
                updateBlogPost.Parameters.AddWithValue("newsImageName", m_BlogPost.NewsImageName);
                updateBlogPost.Parameters.AddWithValue("author", m_BlogPost.Author);
                updateBlogPost.Parameters.AddWithValue("introText", m_BlogPost.IntroText);
                updateBlogPost.Parameters.AddWithValue("redirectUrl", m_BlogPost.RedirectUrl ?? "");
                updateBlogPost.ExecuteNonQuery();

                queryString = "DELETE FROM CMS_BlogPostsToCategories WHERE blogPostId = @blogId";
                SqlCommand delCats = new SqlCommand(queryString, conn);
                delCats.Parameters.AddWithValue("blogId", m_BlogPost.BlogId);
                delCats.ExecuteNonQuery();

                foreach (int catId in m_BlogPost.Categories)
                {
                    queryString = "INSERT INTO CMS_BlogPostsToCategories(blogPostId, categoryId) VALUES(@blogId, @catId)";
                    SqlCommand insertCat = new SqlCommand(queryString, conn);
                    insertCat.Parameters.AddWithValue("blogId", m_BlogPost.BlogId);
                    insertCat.Parameters.AddWithValue("catId", catId);
                    insertCat.ExecuteNonQuery();
                }

            }
            else if (m_BlogPost.PageWorkFlowState == 2 || m_BlogPost.PageWorkFlowState == 3)
            {
                string queryString = "INSERT INTO CMS_BlogPosts(blogId, title, publishDate, expirationDate, contentGroup, [content], comments, pageWorkFlowState, lockedBy, lastModifiedBy, lastModifiedDate, newsImageId, newsImageName, author, introText, redirectUrl) VALUES(@blogId, @title, @publishDate, @expirationDate, @contentGroup, @content, @comments, 1, @lockedBy, @lastModifiedBy, @lastModifiedDate, @newsImageId, @newsImageName, @author, @introText, @redirectUrl)";
                SqlCommand insertBlogPost = new SqlCommand(queryString, conn);
                insertBlogPost.Parameters.AddWithValue("blogId", m_BlogPost.BlogId);
                insertBlogPost.Parameters.AddWithValue("title", m_BlogPost.Title);
                insertBlogPost.Parameters.AddWithValue("publishDate", m_BlogPost.PublishDate.ToString());
                insertBlogPost.Parameters.AddWithValue("expirationDate", m_BlogPost.ExpirationDate.ToString());
                insertBlogPost.Parameters.AddWithValue("contentGroup", m_BlogPost.ContentGroup);
                insertBlogPost.Parameters.AddWithValue("content", m_BlogPost.Content);
                insertBlogPost.Parameters.AddWithValue("comments", m_BlogPost.Comments);
                insertBlogPost.Parameters.AddWithValue("lockedBy", HttpContext.Current.Session["uid"]);
                insertBlogPost.Parameters.AddWithValue("lastModifiedBy", HttpContext.Current.Session["uid"]);
                insertBlogPost.Parameters.AddWithValue("lastModifiedDate", DateTime.Now);
                insertBlogPost.Parameters.AddWithValue("newsImageId", m_BlogPost.NewsImageId);
                insertBlogPost.Parameters.AddWithValue("newsImageName", m_BlogPost.NewsImageName);
                insertBlogPost.Parameters.AddWithValue("author", m_BlogPost.Author);
                insertBlogPost.Parameters.AddWithValue("introText", m_BlogPost.IntroText);
                insertBlogPost.Parameters.AddWithValue("redirectUrl", m_BlogPost.RedirectUrl ?? "");
                insertBlogPost.ExecuteNonQuery();

                queryString = "DELETE FROM CMS_BlogPostsToCategories WHERE blogPostId = @blogId";
                SqlCommand delCats = new SqlCommand(queryString, conn);
                delCats.Parameters.AddWithValue("blogId", m_BlogPost.BlogId);
                delCats.ExecuteNonQuery();

                foreach (int catId in m_BlogPost.Categories)
                {
                    queryString = "INSERT INTO CMS_BlogPostsToCategories(blogPostId, categoryId) VALUES(@blogId, @catId)";
                    SqlCommand insertCat = new SqlCommand(queryString, conn);
                    insertCat.Parameters.AddWithValue("blogId", m_BlogPost.BlogId);
                    insertCat.Parameters.AddWithValue("catId", catId);
                    insertCat.ExecuteNonQuery();
                }
            }
            else
            {

            }

            conn.Close();
        }

        public static void Delete(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            BlogPost m_BlogPost = DBBlogPost.RetrieveOne(id);

            string queryString = "UPDATE CMS_BlogPosts SET pageWorkFlowState = 4 WHERE blogId = @id";
            SqlCommand updateBlogPosts = new SqlCommand(queryString, conn);
            updateBlogPosts.Parameters.AddWithValue("id", m_BlogPost.BlogId);
            updateBlogPosts.ExecuteNonQuery();

            queryString = "INSERT INTO CMS_Trash(objectId, objectTable, objectName, deleteDate, deletedBy, objectColumn, objectType) VALUES(@objectId, 'CMS_BlogPosts', @objectName, @deleteDate, @deletedBy, 'blogId', 'Blog Post')";
            SqlCommand insertTrash = new SqlCommand(queryString, conn);
            insertTrash.Parameters.AddWithValue("objectId", m_BlogPost.BlogId);
            insertTrash.Parameters.AddWithValue("objectName", m_BlogPost.Title);
            insertTrash.Parameters.AddWithValue("deleteDate", DateTime.Now);
            insertTrash.Parameters.AddWithValue("deletedBy", HttpContext.Current.Session["uid"]);
            insertTrash.ExecuteNonQuery();


            conn.Close();
        }

        public static int getPageWorkFlowState(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT pageWorkFlowState from CMS_BlogPosts WHERE id = @id";
            SqlCommand getInfo = new SqlCommand(queryString, conn);
            getInfo.Parameters.AddWithValue("id", id);
            int mVal = (int)getInfo.ExecuteScalar();

            conn.Close();
            return mVal;
        }

        public static int getLockedBy(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT lockedBy from CMS_BlogPosts WHERE id = @id";
            SqlCommand getInfo = new SqlCommand(queryString, conn);
            getInfo.Parameters.AddWithValue("id", id);
            int mVal = (int)getInfo.ExecuteScalar();

            conn.Close();
            return mVal;
        }

        public static void lockBlogPost(int blogId)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "UPDATE CMS_BlogPosts SET lockedBy = @lockedBy WHERE blogId = @blogId";
            SqlCommand updateBlogPost = new SqlCommand(queryString, conn);
            updateBlogPost.Parameters.AddWithValue("lockedBy", HttpContext.Current.Session["uid"]);
            updateBlogPost.Parameters.AddWithValue("blogId", blogId);
            updateBlogPost.ExecuteNonQuery();

            conn.Close();
        }

        public static void unlockBlogPost(int blogId)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "UPDATE CMS_BlogPosts SET lockedBy = 0 WHERE blogId = @blogId";
            SqlCommand updateBlogPost = new SqlCommand(queryString, conn);
            updateBlogPost.Parameters.AddWithValue("blogId", blogId);
            updateBlogPost.ExecuteNonQuery();

            conn.Close();
        }

        public static void publishBlogPost(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "UPDATE CMS_BlogPosts SET pageWorkFlowState = 2 WHERE id = @id";
            SqlCommand updateBlogPost = new SqlCommand(queryString, conn);
            updateBlogPost.Parameters.AddWithValue("id", id);
            updateBlogPost.ExecuteNonQuery();

            conn.Close();
        }

        public static List<Category> getCategories()
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_Categories";
            SqlCommand getCategories = new SqlCommand(queryString, conn);
            SqlDataReader categoryReader = getCategories.ExecuteReader();

            List<Category> m_Categories = new List<Category>();

            while (categoryReader.Read())
            {
                Category tempCat = new Category();
                tempCat.Id = categoryReader.GetInt32(0);
                tempCat.CategoryTitle = categoryReader.GetString(1);

                m_Categories.Add(tempCat);
            }

            conn.Close();
            return m_Categories;
        }

        public static List<BlogPostComment> GetComments(int BlogId)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_BlogPostComments WHERE blogId = @blogId AND pageWorkFlowState != 4 ORDER BY id DESC";
            SqlCommand getComments = new SqlCommand(queryString, conn);
            getComments.Parameters.AddWithValue("blogId", BlogId);
            SqlDataReader commentReader = getComments.ExecuteReader();

            List<BlogPostComment> m_Comments = new List<BlogPostComment>();

            while (commentReader.Read())
            {
                BlogPostComment m_Comment = new BlogPostComment();
                m_Comment.Id = commentReader.GetInt32(0);
                m_Comment.BlogId = commentReader.GetInt32(1);
                m_Comment.Comment = commentReader.GetString(2);
                m_Comment.Name = commentReader.GetString(3);
                m_Comment.PageWorkFlowState = commentReader.GetInt32(4);

                m_Comments.Add(m_Comment);
            }

            return m_Comments;
        }

        public static void CommentPublish(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "UPDATE CMS_BlogPostComments SET pageWorkFlowState = 2 WHERE id = @id";
            SqlCommand updateComment = new SqlCommand(queryString, conn);
            updateComment.Parameters.AddWithValue("id", id);
            updateComment.ExecuteNonQuery();

            conn.Close();
        }

        public static void CommentDelete(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "UPDATE CMS_BlogPostComments SET pageWorkFlowState = 4 WHERE id = @id";
            SqlCommand updateBlogPosts = new SqlCommand(queryString, conn);
            updateBlogPosts.Parameters.AddWithValue("id", id);
            updateBlogPosts.ExecuteNonQuery();

            queryString = "INSERT INTO CMS_Trash(objectId, objectTable, objectName, deleteDate, deletedBy, objectColumn, objectType) VALUES(@objectId, 'CMS_BlogPostComments', @objectName, @deleteDate, @deletedBy, 'id', 'Blog Post Comment')";
            SqlCommand insertTrash = new SqlCommand(queryString, conn);
            insertTrash.Parameters.AddWithValue("objectId", id);
            insertTrash.Parameters.AddWithValue("objectName", String.Empty);
            insertTrash.Parameters.AddWithValue("deleteDate", DateTime.Now);
            insertTrash.Parameters.AddWithValue("deletedBy", HttpContext.Current.Session["uid"]);
            insertTrash.ExecuteNonQuery();

            conn.Close();
        }

        public static void NewsRotatorSortOrder(List<int> m_SortOrder)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "DELETE FROM CMS_WidgetNewsRotator";
            SqlCommand delSortOrder = new SqlCommand(queryString, conn);
            delSortOrder.ExecuteNonQuery();

            queryString = "INSERT INTO CMS_WidgetNewsRotator(pageId, sortOrder) VALUES(@pageId, @sortOrder)";

            int count = 1;

            foreach(int m_sort in m_SortOrder)
            {
                SqlCommand insSortOrder = new SqlCommand(queryString, conn);
                insSortOrder.Parameters.AddWithValue("pageId", m_sort);
                insSortOrder.Parameters.AddWithValue("sortOrder", count);
                insSortOrder.ExecuteNonQuery();

                count++;
            }

            conn.Close();
        }

        public static List<BlogPost> getNewsRotator()
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_WidgetNewsRotator ORDER by sortOrder";
            SqlCommand getFeaturedNews = new SqlCommand(queryString, conn);
            SqlDataReader m_News = getFeaturedNews.ExecuteReader();

            List<BlogPost> m_FeaturedNews = new List<BlogPost>();

            while (m_News.Read())
            {
                SqlConnection conn2 = DB.DbConnect();
                conn2.Open();

                queryString = "SELECT TOP 1 * FROM CMS_BlogPosts WHERE blogId = @blogId AND pageWorkFlowState = 2 ORDER BY id DESC";
                SqlCommand getNews = new SqlCommand(queryString, conn2);
                getNews.Parameters.AddWithValue("blogId", m_News.GetInt32(1));
                SqlDataReader m_Blogs = getNews.ExecuteReader();

                if (m_Blogs.Read())
                {
                    BlogPost m_Blog = new BlogPost();

                    m_Blog.Id = m_Blogs.GetInt32(0);
                    m_Blog.BlogId = m_Blogs.GetInt32(1);
                    m_Blog.Title = m_Blogs.GetString(2);
                    m_Blog.PublishDate = m_Blogs.GetDateTime(3);
                    m_Blog.ContentGroup = m_Blogs.GetInt32(4);
                    m_Blog.Content = m_Blogs.GetString(5);
                    m_Blog.PageWorkFlowState = m_Blogs.GetInt32(6);
                    m_Blog.LockedBy = m_Blogs.GetInt32(7);
                    m_Blog.LastModifiedBy = m_Blogs.GetInt32(8);
                    m_Blog.LastModifiedDate = m_Blogs.GetDateTime(9);
                    m_Blog.Comments = m_Blogs.GetInt32(10);
                    m_Blog.ExpirationDate = m_Blogs.GetDateTime(11);
                    m_Blog.NewsImageId = m_Blogs.GetInt32(12);
                    m_Blog.Author = m_Blogs.GetString(13);
                    m_Blog.IntroText = m_Blogs.GetString(14);
                    m_Blog.NewsImageName = m_Blogs.GetString(15);
                    m_Blog.RedirectUrl = m_Blogs.GetString(16);

                    m_Blog.LockedByName = DBPage.GetLockedByName(m_Blog.LockedBy);
                    m_Blog.LastModifiedByName = DBPage.GetLockedByName(m_Blog.LastModifiedBy);

                    m_FeaturedNews.Add(m_Blog);
                }

                conn2.Close();
            }

            conn.Close();
            return m_FeaturedNews;
        }

        public static List<int> getNewsRotatorBlogIds()
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_WidgetNewsRotator ORDER BY sortOrder";
            SqlCommand getIds = new SqlCommand(queryString, conn);
            SqlDataReader m_Ids = getIds.ExecuteReader();

            List<int> Ids = new List<int>();

            while (m_Ids.Read())
            {
                Ids.Add(m_Ids.GetInt32(1));
            }

            return Ids;
        }

        public static BlogPost getTopByBlogId(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT TOP 1 * FROM CMS_BlogPosts WHERE blogId = @blogId ORDER BY id DESC";
            SqlCommand getBlog = new SqlCommand(queryString, conn);
            getBlog.Parameters.AddWithValue("blogId", id);
            SqlDataReader blogPostReader = getBlog.ExecuteReader();

            BlogPost m_BlogPost = new BlogPost();

            if (blogPostReader.Read())
            {
                m_BlogPost.Id = blogPostReader.GetInt32(0);
                m_BlogPost.BlogId = blogPostReader.GetInt32(1);
                m_BlogPost.Title = blogPostReader.GetString(2);
                m_BlogPost.PublishDate = blogPostReader.GetDateTime(3);
                m_BlogPost.ContentGroup = blogPostReader.GetInt32(4);
                m_BlogPost.Content = blogPostReader.GetString(5);
                m_BlogPost.PageWorkFlowState = blogPostReader.GetInt32(6);
                m_BlogPost.LockedBy = blogPostReader.GetInt32(7);
                m_BlogPost.LastModifiedBy = blogPostReader.GetInt32(8);
                m_BlogPost.LastModifiedDate = blogPostReader.GetDateTime(9);
                m_BlogPost.Comments = blogPostReader.GetInt32(10);
                m_BlogPost.ExpirationDate = blogPostReader.GetDateTime(11);
                m_BlogPost.NewsImageId = blogPostReader.GetInt32(12);
                m_BlogPost.Author = blogPostReader.GetString(13);
                m_BlogPost.IntroText = blogPostReader.GetString(14);
                m_BlogPost.NewsImageName = blogPostReader.GetString(15);

                m_BlogPost.LockedByName = DBPage.GetLockedByName(m_BlogPost.LockedBy);
                m_BlogPost.LastModifiedByName = DBPage.GetLockedByName(m_BlogPost.LastModifiedBy);

                SqlConnection conn2 = DB.DbConnect();
                conn2.Open();

                queryString = "SELECT * FROM CMS_BlogPostsToCategories WHERE blogPostId = @blogId";
                SqlCommand getCats = new SqlCommand(queryString, conn2);
                getCats.Parameters.AddWithValue("blogId", m_BlogPost.BlogId);
                SqlDataReader catsReader = getCats.ExecuteReader();

                List<int> m_Cats = new List<int>();

                while (catsReader.Read())
                {
                    m_Cats.Add(catsReader.GetInt32(2));
                }

                m_BlogPost.Categories = m_Cats;

                conn2.Close();
            }

            conn.Close();

            return m_BlogPost;
        }
    }
}