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

            queryString = "INSERT INTO CMS_BlogPosts(blogId, title, publishDate, expirationDate, contentGroup, categories, [content], comments, pageWorkFlowState, lockedBy, lastModifiedBy, lastModifiedDate) VALUES(@blogId, @title, @publishDate, @expirationDate, @contentGroup, @categories, @content, @comments, 1, @lockedBy, @lastModifiedBy, @lastModifiedDate)";
            SqlCommand insertBlogPost = new SqlCommand(queryString, conn);
            insertBlogPost.Parameters.AddWithValue("blogId", m_BlogId);
            insertBlogPost.Parameters.AddWithValue("title", m_BlogPost.Title);
            insertBlogPost.Parameters.AddWithValue("publishDate", m_BlogPost.PublishDate.ToString());
            insertBlogPost.Parameters.AddWithValue("expirationDate", m_BlogPost.PublishDate.ToString());
            insertBlogPost.Parameters.AddWithValue("contentGroup", m_BlogPost.ContentGroup);
            insertBlogPost.Parameters.AddWithValue("categories", string.Join(",", m_BlogPost.Categories.ToArray()));  //m_BlogPost.Categories);
            insertBlogPost.Parameters.AddWithValue("content", m_BlogPost.Content);
            insertBlogPost.Parameters.AddWithValue("comments", m_BlogPost.Comments);
            insertBlogPost.Parameters.AddWithValue("lockedBy", HttpContext.Current.Session["uid"]);
            insertBlogPost.Parameters.AddWithValue("lastModifiedBy", HttpContext.Current.Session["uid"]);
            insertBlogPost.Parameters.AddWithValue("lastModifiedDate", DateTime.Now);
            insertBlogPost.ExecuteNonQuery();

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
            string m_Cats = "";
            if(blogPostReader.Read())
            {
                m_BlogPost.Id = blogPostReader.GetInt32(0);
                m_BlogPost.BlogId = blogPostReader.GetInt32(1);
                m_BlogPost.Title = blogPostReader.GetString(2);
                m_BlogPost.PublishDate = blogPostReader.GetDateTime(3);
                m_BlogPost.ContentGroup = blogPostReader.GetInt32(4);
                m_BlogPost.Content = blogPostReader.GetString(6);
                m_BlogPost.PageWorkFlowState = blogPostReader.GetInt32(7);
                m_BlogPost.LockedBy = blogPostReader.GetInt32(8);
                m_BlogPost.LastModifiedBy = blogPostReader.GetInt32(9);
                m_BlogPost.LastModifiedDate = blogPostReader.GetDateTime(10);
                m_BlogPost.Comments = blogPostReader.GetInt32(11);
                m_BlogPost.ExpirationDate = blogPostReader.GetDateTime(12);

                m_BlogPost.LockedByName = DBPage.GetLockedByName(m_BlogPost.LockedBy);
                m_BlogPost.LastModifiedByName = DBPage.GetLockedByName(m_BlogPost.LastModifiedBy);

                //handle category string to int conversion
                m_Cats = blogPostReader.GetString(5);
                string[] catsArray = m_Cats.Split(',');
                List<int> myCats = new List<int>();
                myCats.Add(0);

                foreach (string cat in catsArray)
                {
                    myCats.Add(int.Parse(cat));
                }

                m_BlogPost.Categories = myCats;
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
                //m_BlogPost.Categories = blogPostReader.GetString(5);
                m_BlogPost.Content = blogPostReader.GetString(6);
                m_BlogPost.PageWorkFlowState = blogPostReader.GetInt32(7);
                m_BlogPost.LockedBy = blogPostReader.GetInt32(8);
                m_BlogPost.LastModifiedBy = blogPostReader.GetInt32(9);
                m_BlogPost.LastModifiedDate = blogPostReader.GetDateTime(10);
                m_BlogPost.Comments = blogPostReader.GetInt32(11);
                m_BlogPost.ExpirationDate = blogPostReader.GetDateTime(12);

                m_BlogPost.LockedByName = DBPage.GetLockedByName(m_BlogPost.LockedBy);
                m_BlogPost.LastModifiedByName = DBPage.GetLockedByName(m_BlogPost.LastModifiedBy);

                if (previousPageId != m_BlogPost.BlogId)
                {
                    m_BlogPosts.Add(m_BlogPost);
                }

                previousPageId = m_BlogPost.BlogId;
            }

            conn.Close();
            return m_BlogPosts;
        }

        public static List<BlogPost> RetrieveAllByCategory(int Category)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_BlogPosts WHERE pageWorkFlowState != 4 ORDER BY blogId, publishDate, id DESC";
            SqlCommand getBlogPosts = new SqlCommand(queryString, conn);
            SqlDataReader blogPostReader = getBlogPosts.ExecuteReader();

            List<BlogPost> m_BlogPosts = new List<BlogPost>();
            int previousPageId = 0;

            while (blogPostReader.Read())
            {
                BlogPost m_BlogPost = new BlogPost();

                m_BlogPost.Id = blogPostReader.GetInt32(0);
                m_BlogPost.BlogId = blogPostReader.GetInt32(1);
                m_BlogPost.Title = blogPostReader.GetString(2);
                m_BlogPost.PublishDate = blogPostReader.GetDateTime(3);
                m_BlogPost.ContentGroup = blogPostReader.GetInt32(4);
                //m_BlogPost.Categories = blogPostReader.GetString(5);
                m_BlogPost.Content = blogPostReader.GetString(6);
                m_BlogPost.PageWorkFlowState = blogPostReader.GetInt32(7);
                m_BlogPost.LockedBy = blogPostReader.GetInt32(8);
                m_BlogPost.LastModifiedBy = blogPostReader.GetInt32(9);
                m_BlogPost.LastModifiedDate = blogPostReader.GetDateTime(10);
                m_BlogPost.Comments = blogPostReader.GetInt32(11);
                m_BlogPost.ExpirationDate = blogPostReader.GetDateTime(12);

                m_BlogPost.LockedByName = DBPage.GetLockedByName(m_BlogPost.LockedBy);
                m_BlogPost.LastModifiedByName = DBPage.GetLockedByName(m_BlogPost.LastModifiedBy);

                if (previousPageId != m_BlogPost.BlogId)
                {
                    string m_Cats = blogPostReader.GetString(5);
                    string[] catsArray = m_Cats.Split(',');
                    List<int> myCats = new List<int>();
                    myCats.Add(0);

                    foreach (string cat in catsArray)
                    {
                        if (Category == int.Parse(cat))
                        {
                            myCats.Add(int.Parse(cat));
                        }
                    }

                    m_BlogPost.Categories = myCats;

                    if (myCats.Count > 1)
                    {
                        m_BlogPosts.Add(m_BlogPost);
                    }
                }

                previousPageId = m_BlogPost.BlogId;
            }

            conn.Close();
            return m_BlogPosts;
        }

        public static void Update(BlogPost m_BlogPost)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            if (m_BlogPost.PageWorkFlowState == 1)
            {
                string queryString = "UPDATE CMS_BlogPosts SET title = @title, publishDate = @publishDate, expirationDate = @expirationDate, contentGroup = @contentGroup, categories = @categories, content = @content, comments = @comments, pageWorkFlowState = 1, lockedBy = @lockedBy, lastModifiedBy = @lastModifiedBy, lastModifiedDate = @lastModifiedDate WHERE id = @id";
                SqlCommand updateBlogPost = new SqlCommand(queryString, conn);
                updateBlogPost.Parameters.AddWithValue("title", m_BlogPost.Title);
                updateBlogPost.Parameters.AddWithValue("publishDate", m_BlogPost.PublishDate.ToString());
                updateBlogPost.Parameters.AddWithValue("expirationDate", m_BlogPost.ExpirationDate.ToString());
                updateBlogPost.Parameters.AddWithValue("contentGroup", m_BlogPost.ContentGroup);
                updateBlogPost.Parameters.AddWithValue("categories", string.Join(",", m_BlogPost.Categories.ToArray()));  //m_BlogPost.Categories);
                updateBlogPost.Parameters.AddWithValue("content", m_BlogPost.Content);
                updateBlogPost.Parameters.AddWithValue("comments", m_BlogPost.Comments);
                updateBlogPost.Parameters.AddWithValue("lockedBy", HttpContext.Current.Session["uid"]);
                updateBlogPost.Parameters.AddWithValue("lastModifiedBy", HttpContext.Current.Session["uid"]);
                updateBlogPost.Parameters.AddWithValue("lastModifiedDate", DateTime.Now);
                updateBlogPost.Parameters.AddWithValue("id", m_BlogPost.Id);
                updateBlogPost.ExecuteNonQuery();

            }
            else if (m_BlogPost.PageWorkFlowState == 2 || m_BlogPost.PageWorkFlowState == 3)
            {
                string queryString = "INSERT INTO CMS_BlogPosts(blogId, title, publishDate, expirationDate, contentGroup, categories, [content], comments, pageWorkFlowState, lockedBy, lastModifiedBy, lastModifiedDate) VALUES(@blogId, @title, @publishDate, @expirationDate, @contentGroup, @categories, @content, @comments, 1, @lockedBy, @lastModifiedBy, @lastModifiedDate)";
                SqlCommand insertBlogPost = new SqlCommand(queryString, conn);
                insertBlogPost.Parameters.AddWithValue("blogId", m_BlogPost.BlogId);
                insertBlogPost.Parameters.AddWithValue("title", m_BlogPost.Title);
                insertBlogPost.Parameters.AddWithValue("publishDate", m_BlogPost.PublishDate.ToString());
                insertBlogPost.Parameters.AddWithValue("expirationDate", m_BlogPost.ExpirationDate.ToString());
                insertBlogPost.Parameters.AddWithValue("contentGroup", m_BlogPost.ContentGroup);
                insertBlogPost.Parameters.AddWithValue("categories", string.Join(",", m_BlogPost.Categories.ToArray()));  //m_BlogPost.Categories);
                insertBlogPost.Parameters.AddWithValue("content", m_BlogPost.Content);
                insertBlogPost.Parameters.AddWithValue("comments", m_BlogPost.Comments);
                insertBlogPost.Parameters.AddWithValue("lockedBy", HttpContext.Current.Session["uid"]);
                insertBlogPost.Parameters.AddWithValue("lastModifiedBy", HttpContext.Current.Session["uid"]);
                insertBlogPost.Parameters.AddWithValue("lastModifiedDate", DateTime.Now);
                insertBlogPost.ExecuteNonQuery();
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
    }
}