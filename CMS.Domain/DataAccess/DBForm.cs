using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Domain.Entities;
using System.Data.SqlClient;
using CMS.Domain.HelperClasses;

namespace CMS.Domain.DataAccess
{
    public class DBForm
    {
        public static int Create(Form m_Form)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "INSERT INTO CMS_Forms(formName, submissionEmail, submission, fromEmail) VALUES(@formName, @submissionEmail, @success, @fromEmail)";
            SqlCommand insertForm = new SqlCommand(queryString, conn);
            insertForm.Parameters.AddWithValue("formName", m_Form.FormName);
            insertForm.Parameters.AddWithValue("submissionEmail", m_Form.SubmissionEmail);
            insertForm.Parameters.AddWithValue("success", m_Form.Success);
            insertForm.Parameters.AddWithValue("fromEmail", m_Form.FromEmail);
            insertForm.ExecuteNonQuery();

            queryString = "SELECT IDENT_CURRENT('CMS_Forms')";
            SqlCommand getPageId = new SqlCommand(queryString, conn);
            int m_FormId = (int)(decimal)getPageId.ExecuteScalar();

            conn.Close();

            foreach (int FormField in m_Form.MyFormFields)
            {
                int sortOrder = getSortOrder(m_FormId);

                conn.Open();

                queryString = "INSERT INTO CMS_FormToFormFields(formId, formFieldId, sortOrder) VALUES(@formId, @formFieldId, @sortOrder)";
                SqlCommand insertInfo = new SqlCommand(queryString, conn);
                insertInfo.Parameters.AddWithValue("formId", m_FormId);
                insertInfo.Parameters.AddWithValue("formFieldId", FormField);
                insertInfo.Parameters.AddWithValue("sortOrder", sortOrder);

                insertInfo.ExecuteNonQuery();

                conn.Close();
            }

            return m_FormId;
        }

        public static Form RetrieveOne(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

           

            string queryString = "SELECT * FROM CMS_Forms WHERE id = @id AND pageWorkFlowState != 4";
            SqlCommand getForm = new SqlCommand(queryString, conn);
            getForm.Parameters.AddWithValue("id", id);
            SqlDataReader formReader = getForm.ExecuteReader();

            Form m_Form = new Form();

            if (formReader.Read())
            {
                SqlConnection conn2 = DB.DbConnect();
                conn2.Open();

                m_Form.Id = formReader.GetInt32(0);
                m_Form.FormName = formReader.GetString(1);
                m_Form.SubmissionEmail = formReader.GetString(2);
                m_Form.MyFormFields = new List<int>();
                m_Form.FromEmail = formReader.GetString(5);
                if (!formReader.IsDBNull(4))
                {
                    m_Form.Success = formReader.GetString(4);
                }
                else
                {
                    m_Form.Success = "";
                }

                queryString = "SELECT * FROM CMS_FormFields as ff, CMS_FormToFormFields as fff WHERE ff.id = fff.formFieldId AND fff.formId = @formId ORDER BY sortOrder ASC";
                SqlCommand getFormFields = new SqlCommand(queryString, conn2);
                getFormFields.Parameters.AddWithValue("formId", id);
                SqlDataReader formFieldsReader = getFormFields.ExecuteReader();

                while (formFieldsReader.Read())
                {
                    FormField temp = new FormField();
                    temp.Id = formFieldsReader.GetInt32(0);
                    temp.Label = formFieldsReader.GetString(1);
                    temp.FieldType = formFieldsReader.GetInt32(2);
                    temp.ParentId = formFieldsReader.GetInt32(3);
                    temp.IsRequired = formFieldsReader.GetInt32(11);
                    temp.Children = DBFormField.RetrieveChildren(temp.Id);
                    if (!DBNull.Value.Equals(formFieldsReader[6]))
                    {
                        temp.LabelText = formFieldsReader.GetString(6);
                    }

                    m_Form.MyFormFields.Add(temp.Id);
                    m_Form.FormFields.Add(temp);
                    
                }

                conn2.Close();
            }

            conn.Close();

            return m_Form;
        }

        public static List<Form> RetrieveAll()
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_Forms WHERE pageWorkFlowState != 4";
            SqlCommand getForm = new SqlCommand(queryString, conn);
            SqlDataReader formReader = getForm.ExecuteReader();

            List<Form> m_Forms = new List<Form>();

            while (formReader.Read())
            {
                SqlConnection conn2 = DB.DbConnect();
                conn2.Open();

                Form tempForm = new Form();
                tempForm.Id = formReader.GetInt32(0);
                tempForm.FormName = formReader.GetString(1);
                tempForm.SubmissionEmail = formReader.GetString(2);

                queryString = "SELECT * FROM CMS_FormFields as ff, CMS_FormToFormFields as fff WHERE ff.id = fff.formFieldId AND fff.formId = @formId";
                SqlCommand getFormFields = new SqlCommand(queryString, conn2);
                getFormFields.Parameters.AddWithValue("formId", tempForm.Id);
                SqlDataReader formFieldsReader = getFormFields.ExecuteReader();

                while (formFieldsReader.Read())
                {
                    FormField temp = new FormField();
                    temp.Id = formFieldsReader.GetInt32(0);
                    temp.Label = formFieldsReader.GetString(1);
                    temp.FieldType = formFieldsReader.GetInt32(2);
                    temp.ParentId = formFieldsReader.GetInt32(3);
                    temp.IsRequired = formFieldsReader.GetInt32(11);

                    tempForm.FormFields.Add(temp);
                }

                m_Forms.Add(tempForm);
                conn2.Close();
            }

            conn.Close();

            return m_Forms;
        }

        public static void Update(Form m_Form)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "UPDATE CMS_Forms SET formName = @formName, submissionEmail = @submissionEmail, submission = @success, fromEmail = @fromEmail WHERE id = @id";
            SqlCommand updateForm = new SqlCommand(queryString, conn);
            updateForm.Parameters.AddWithValue("formName", m_Form.FormName);
            updateForm.Parameters.AddWithValue("submissionEmail", m_Form.SubmissionEmail);
            updateForm.Parameters.AddWithValue("id", m_Form.Id);
            updateForm.Parameters.AddWithValue("success", m_Form.Success);
            updateForm.Parameters.AddWithValue("fromEmail", m_Form.FromEmail);
            updateForm.ExecuteNonQuery();

            queryString = "DELETE FROM CMS_FormToFormFields WHERE formId = @formId";
            SqlCommand deleteBridge = new SqlCommand(queryString, conn);
            deleteBridge.Parameters.AddWithValue("formId", m_Form.Id);
            deleteBridge.ExecuteNonQuery();

            conn.Close();

            int sortOrder = 1;

            foreach (FormField FormField in m_Form.FormFields)
            {
                conn.Open();

                queryString = "INSERT INTO CMS_FormToFormFields(formId, formFieldId, sortOrder, isRequired) VALUES(@formId, @formFieldId, @sortOrder, @isRequired)";
                SqlCommand insertInfo = new SqlCommand(queryString, conn);
                insertInfo.Parameters.AddWithValue("formId", m_Form.Id);
                insertInfo.Parameters.AddWithValue("formFieldId", FormField.Id);
                insertInfo.Parameters.AddWithValue("isRequired", FormField.IsRequired);
                insertInfo.Parameters.AddWithValue("sortOrder", sortOrder);

                insertInfo.ExecuteNonQuery();

                conn.Close();
                sortOrder++;
            }
        }

        public static void Delete(int id)
        {
            Form m_Form = DBForm.RetrieveOne(id);

            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "INSERT INTO CMS_Trash(objectId, objectTable, objectName, deleteDate, deletedBy, objectColumn, objectType) VALUES(@objectId, 'CMS_Forms', @objectName, @deleteDate, @deletedBy, 'id', 'Form')";
            SqlCommand insertTrash = new SqlCommand(queryString, conn);
            insertTrash.Parameters.AddWithValue("objectId", m_Form.Id);
            insertTrash.Parameters.AddWithValue("objectName", m_Form.FormName);
            insertTrash.Parameters.AddWithValue("deleteDate", DateTime.Now);
            insertTrash.Parameters.AddWithValue("deletedBy", HttpContext.Current.Session["uid"]);
            insertTrash.ExecuteNonQuery();

            queryString = "UPDATE CMS_Forms SET pageWorkFlowState = 4 WHERE id = @id";
            SqlCommand deleteForm = new SqlCommand(queryString, conn);
            deleteForm.Parameters.AddWithValue("id", id);
            deleteForm.ExecuteNonQuery();

            conn.Close();
        }

        public static List<FormField> getFormFields(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_FormFields as ff, CMS_FormToFormFields as fff WHERE ff.id = fff.formFieldId and fff.formId = @id ORDER BY sortOrder ASC";
            SqlCommand getFields = new SqlCommand(queryString, conn);
            getFields.Parameters.AddWithValue("id", id);
            SqlDataReader formFieldsReader = getFields.ExecuteReader();

            List<FormField> m_FormFields = new List<FormField>();

            while (formFieldsReader.Read())
            {
                FormField temp = new FormField();

                temp.Id = formFieldsReader.GetInt32(0);
                temp.Label = formFieldsReader.GetString(1);
                temp.FieldType = formFieldsReader.GetInt32(2);
                temp.ParentId = formFieldsReader.GetInt32(3);
                temp.IsRequired = formFieldsReader.GetInt32(11);

                m_FormFields.Add(temp);
            }

            conn.Close();

            return m_FormFields;
        }

        public static void SortUp(int parentId, int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT TOP 1 sortOrder FROM CMS_FormToFormFields WHERE formId = @parentId AND formFieldId = @id";
            SqlCommand getSortOrder = new SqlCommand(queryString, conn);
            getSortOrder.Parameters.AddWithValue("parentId", parentId);
            getSortOrder.Parameters.AddWithValue("id", id);

            int oldSortOrder = (int)getSortOrder.ExecuteScalar();
            int newSortOrder = oldSortOrder - 1;

            if (newSortOrder > 0)
            {
                queryString = "UPDATE CMS_FormToFormFields SET sortOrder = @sortOrder WHERE formId = @parentId AND sortOrder = @newSortOrder";
                SqlCommand updateSort = new SqlCommand(queryString, conn);
                updateSort.Parameters.AddWithValue("sortOrder", oldSortOrder);
                updateSort.Parameters.AddWithValue("parentId", parentId);
                updateSort.Parameters.AddWithValue("newSortOrder", newSortOrder);
                updateSort.ExecuteNonQuery();

                queryString = "UPDATE CMS_FormToFormFields SET sortOrder = @sortOrder WHERE formId = @parentId AND formFieldId = @id";
                SqlCommand updateSort2 = new SqlCommand(queryString, conn);
                updateSort2.Parameters.AddWithValue("sortOrder", newSortOrder);
                updateSort2.Parameters.AddWithValue("parentId", parentId);
                updateSort2.Parameters.AddWithValue("id", id);
                updateSort2.ExecuteNonQuery();
            }
        }

        public static void SortDown(int parentId, int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT TOP 1 sortOrder FROM CMS_FormToFormFields WHERE formId = @parentId AND formFieldId = @id";
            SqlCommand getSortOrder = new SqlCommand(queryString, conn);
            getSortOrder.Parameters.AddWithValue("parentId", parentId);
            getSortOrder.Parameters.AddWithValue("id", id);

            int oldSortOrder = (int)getSortOrder.ExecuteScalar();
            int newSortOrder = oldSortOrder + 1;


            // check boundaries and sort order integrity
            queryString = "SELECT COUNT(*) FROM CMS_FormToFormFields WHERE sortOrder = @sortOrder and formId = @parentId";
            SqlCommand checkBoundaries = new SqlCommand(queryString, conn);
            checkBoundaries.Parameters.AddWithValue("sortOrder", newSortOrder);
            checkBoundaries.Parameters.AddWithValue("parentId", parentId);

            int m_Count = (int)checkBoundaries.ExecuteScalar();

            if (m_Count == 1)
            {
                queryString = "UPDATE CMS_FormToFormFields SET sortOrder = @sortOrder WHERE formId = @parentId AND sortOrder = @newSortOrder";
                SqlCommand updateSort = new SqlCommand(queryString, conn);
                updateSort.Parameters.AddWithValue("sortOrder", oldSortOrder);
                updateSort.Parameters.AddWithValue("parentId", parentId);
                updateSort.Parameters.AddWithValue("newSortOrder", newSortOrder);
                updateSort.ExecuteNonQuery();

                queryString = "UPDATE CMS_FormToFormFields SET sortOrder = @sortOrder WHERE formId = @parentId AND formFieldId = @id";
                SqlCommand updateSort2 = new SqlCommand(queryString, conn);
                updateSort2.Parameters.AddWithValue("sortOrder", newSortOrder);
                updateSort2.Parameters.AddWithValue("parentId", parentId);
                updateSort2.Parameters.AddWithValue("id", id);
                updateSort2.ExecuteNonQuery();
            }
        }

        public static int getSortOrder(int formId)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT TOP 1 sortOrder FROM CMS_FormToFormFields WHERE formId = @formId ORDER BY sortOrder DESC";
            SqlCommand getSortOrder = new SqlCommand(queryString, conn);
            getSortOrder.Parameters.AddWithValue("formId", formId);
            object sortOrder = getSortOrder.ExecuteScalar();
            int m_sortOrder = 0;

            if (sortOrder == null)
            {
                m_sortOrder = 1;
            }
            else
            {
                m_sortOrder = Convert.ToInt32(sortOrder);
                m_sortOrder++;
            }

            conn.Close();

            return m_sortOrder;
        }

        public static void ToggleRequired(int formId, int formFieldId, int newValue)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "UPDATE CMS_FormToFormFields SET isRequired = @newValue WHERE formId = @formId AND formFieldId = @formFieldId";
            SqlCommand updateForm = new SqlCommand(queryString, conn);
            updateForm.Parameters.AddWithValue("newValue", newValue);
            updateForm.Parameters.AddWithValue("formId", formId);
            updateForm.Parameters.AddWithValue("formFieldId", formFieldId);
            updateForm.ExecuteNonQuery();

            conn.Close();
        }

        public static void InsertFormData(string formData, int formId)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "INSERT INTO CMS_FormData(formData, formId, submissionDate) VALUES(@formData, @formId, @submissionDate)";
            SqlCommand insertFormData = new SqlCommand(queryString, conn);
            insertFormData.Parameters.AddWithValue("formData", formData);
            insertFormData.Parameters.AddWithValue("formId", formId);
            insertFormData.Parameters.AddWithValue("submissionDate", DateTime.Now.ToString("MM/dd/yyyy"));
            insertFormData.ExecuteNonQuery();

            conn.Close();
        }

        public static string SpecialExistsOnForm(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "select ff.label from CMS_Forms f, CMS_FormToFormFields fff, CMS_FormFields ff WHERE f.id = fff.formId AND fff.formFieldId = ff.id AND f.id = @id and ff.fieldType = 10";
            SqlCommand getLabel = new SqlCommand(queryString, conn);
            getLabel.Parameters.AddWithValue("id", id);
            string m_Label = (string)getLabel.ExecuteScalar();

            conn.Close();

            return m_Label;
        }

        public static List<string> FormDataExtract(int FormId, string StartDate, string EndDate)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_FormData WHERE formId = @formId AND submissionDate >= @StartDate AND submissionDate <= @EndDate ORDER BY submissionDate ASC";
            SqlCommand getFormData = new SqlCommand(queryString, conn);
            getFormData.Parameters.AddWithValue("formId", FormId);
            getFormData.Parameters.AddWithValue("StartDate", StartDate);
            getFormData.Parameters.AddWithValue("EndDate", EndDate);

            SqlDataReader formDataReader = getFormData.ExecuteReader();

            List<string> m_FormData = new List<string>();

            while (formDataReader.Read())
            {
                string formData = "Submission Date::" + formDataReader.GetDateTime(3).ToString("yyyy-MM-dd") + "^^" + formDataReader.GetString(2);
                m_FormData.Add(formData);
            }

            return m_FormData;
        }
    }
}