using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Domain.Entities;
using CMS.Domain.HelperClasses;
using System.Data.SqlClient;

namespace CMS.Domain.DataAccess
{
    public class DBFormField
    {
        public static void Create(FormField m_FormField)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "INSERT INTO CMS_FormFields(label, fieldType, parentId, validationType, fieldText) VALUES(@label, @fieldType, @parentId, @validationType, @fieldText)";
            SqlCommand insertFormField = new SqlCommand(queryString, conn);
            insertFormField.Parameters.AddWithValue("label", m_FormField.Label ?? "");
            insertFormField.Parameters.AddWithValue("fieldType", m_FormField.FieldType);
            insertFormField.Parameters.AddWithValue("parentId", m_FormField.ParentId);
            insertFormField.Parameters.AddWithValue("validationType", m_FormField.ValidationType);
            insertFormField.Parameters.AddWithValue("fieldText", m_FormField.LabelText ?? string.Empty);

            insertFormField.ExecuteNonQuery();

            queryString = "SELECT IDENT_CURRENT('CMS_FormFields')";
            SqlCommand getFormFieldId = new SqlCommand(queryString, conn);
            int m_FormFieldId = (int)(decimal)getFormFieldId.ExecuteScalar();

            foreach (FormField temp in m_FormField.Children)
            {
                queryString = "INSERT INTO CMS_FormFields(label, fieldType, parentId) VALUES(@label, @fieldType, @parentId)";
                SqlCommand insertTemp = new SqlCommand(queryString, conn);
                insertTemp.Parameters.AddWithValue("label", temp.Label ?? "");
                insertTemp.Parameters.AddWithValue("fieldType", temp.FieldType);
                insertTemp.Parameters.AddWithValue("parentId", m_FormFieldId);

                insertTemp.ExecuteNonQuery(); 
            }

            conn.Close();
        }

        public static FormField RetrieveOne(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_FormFields WHERE id = @id AND pageWorkFlowState != 4";
            SqlCommand getFormField = new SqlCommand(queryString, conn);
            getFormField.Parameters.AddWithValue("id", id);

            SqlDataReader formFieldReader = getFormField.ExecuteReader();

            FormField m_FormField = new FormField();

            if (formFieldReader.Read())
            {
                m_FormField.Id = formFieldReader.GetInt32(0);
                m_FormField.Label = formFieldReader.GetString(1);
                m_FormField.FieldType = formFieldReader.GetInt32(2);
                m_FormField.FieldTypeText = getFieldTypeText(formFieldReader.GetInt32(2));
                m_FormField.ParentId = formFieldReader.GetInt32(3);
                m_FormField.ValidationType = formFieldReader.GetInt32(5);
                if (!DBNull.Value.Equals(formFieldReader[6]))
                {
                    m_FormField.LabelText = formFieldReader.GetString(6);
                }

                if (m_FormField.FieldType == 3 || m_FormField.FieldType == 4 || m_FormField.FieldType == 5 || m_FormField.FieldType == 10)
                {
                    m_FormField.Children = RetrieveChildren(m_FormField.Id);
                }
            }

            conn.Close();
            return m_FormField;

        }

        public static List<FormField> RetrieveAll()
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_FormFields WHERE parentId = 0 AND pageWorkFlowState != 4 ORDER BY label";
            SqlCommand getFormFields = new SqlCommand(queryString, conn);
            SqlDataReader formFieldsReader = getFormFields.ExecuteReader();

            List<FormField> m_FormFields = new List<FormField>();

            while (formFieldsReader.Read())
            {
                FormField temp = new FormField();
                temp.Id = formFieldsReader.GetInt32(0);
                temp.Label = formFieldsReader.GetString(1);
                temp.FieldType = formFieldsReader.GetInt32(2);
                temp.FieldTypeText = getFieldTypeText(formFieldsReader.GetInt32(2));
                temp.ParentId = formFieldsReader.GetInt32(3);
                temp.Children = RetrieveChildren(temp.ParentId);
                temp.ValidationType = formFieldsReader.GetInt32(5);
                if (!DBNull.Value.Equals(formFieldsReader[6]))
                {
                    temp.LabelText = formFieldsReader.GetString(6);
                }

                m_FormFields.Add(temp);
            }

            conn.Close();
            return m_FormFields;
        }

        public static List<FormField> RetrieveChildren(int parentId)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_FormFields WHERE parentId = @parentId ORDER BY id";
            SqlCommand getChildren = new SqlCommand(queryString, conn);
            getChildren.Parameters.AddWithValue("parentId", parentId);

            SqlDataReader childrenReader = getChildren.ExecuteReader();

            List<FormField> m_Children = new List<FormField>();

            while (childrenReader.Read())
            {
                FormField temp = new FormField();
                temp.Id = childrenReader.GetInt32(0);
                temp.Label = childrenReader.GetString(1);
                temp.FieldType = childrenReader.GetInt32(2);
                temp.ParentId = childrenReader.GetInt32(3);

                m_Children.Add(temp);
            }

            conn.Close();

            return m_Children;
        }

        public static void Update(FormField m_FormField)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "UPDATE CMS_FormFields SET label = @label, fieldType = @fieldType, parentId = @parentId, validationType = @validationType, fieldText = @fieldText WHERE id = @id";
            SqlCommand updateFormField = new SqlCommand(queryString, conn);
            updateFormField.Parameters.AddWithValue("label", m_FormField.Label ?? "");
            updateFormField.Parameters.AddWithValue("fieldType", m_FormField.FieldType);
            updateFormField.Parameters.AddWithValue("parentId", m_FormField.ParentId);
            updateFormField.Parameters.AddWithValue("id", m_FormField.Id);
            updateFormField.Parameters.AddWithValue("validationType", m_FormField.ValidationType);
            updateFormField.Parameters.AddWithValue("fieldText", m_FormField.LabelText ?? "");

            updateFormField.ExecuteNonQuery();

            conn.Close();

        }

        public static void Delete(int id)
        {
            FormField m_FormField = DBFormField.RetrieveOne(id);

            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "INSERT INTO CMS_Trash(objectId, objectTable, objectName, deleteDate, deletedBy, objectColumn, objectType) VALUES(@objectId, 'CMS_FormFields', @objectName, @deleteDate, @deletedBy, 'id', 'Form Field')";
            SqlCommand insertTrash = new SqlCommand(queryString, conn);
            insertTrash.Parameters.AddWithValue("objectId", m_FormField.Id);
            insertTrash.Parameters.AddWithValue("objectName", m_FormField.Label);
            insertTrash.Parameters.AddWithValue("deleteDate", DateTime.Now);
            insertTrash.Parameters.AddWithValue("deletedBy", HttpContext.Current.Session["uid"]);
            insertTrash.ExecuteNonQuery();

            queryString = "UPDATE CMS_FormFields SET pageWorkFlowState = 4 WHERE id = @id";
            SqlCommand deleteForm = new SqlCommand(queryString, conn);
            deleteForm.Parameters.AddWithValue("id", id);
            deleteForm.ExecuteNonQuery();

            conn.Close();
        }

        public static void DeleteChildren(int parentId)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "DELETE FROM CMS_FormFields WHERE parentId = @parentId";
            SqlCommand deleteFormFields = new SqlCommand(queryString, conn);
            deleteFormFields.Parameters.AddWithValue("parentId", parentId);
            deleteFormFields.ExecuteNonQuery();

            conn.Close();
        }

        public static Dictionary<int, string> getFieldTypes()
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_FormFieldTypes ORDER BY fieldType";
            SqlCommand getFieldTypes = new SqlCommand(queryString, conn);
            SqlDataReader fiedlTypesReader = getFieldTypes.ExecuteReader();

            Dictionary<int, string> m_FieldTypes = new Dictionary<int, string>();

            while (fiedlTypesReader.Read())
            {
                m_FieldTypes.Add(fiedlTypesReader.GetInt32(0), fiedlTypesReader.GetString(1));
            }

            conn.Close();
            return m_FieldTypes;
        }

        public static string getFieldTypeText(int id)
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT fieldType FROM CMS_FormFieldTypes WHERE id = @id";
            SqlCommand getType = new SqlCommand(queryString, conn);
            getType.Parameters.AddWithValue("id", id);
            
            string m_Type = (string)getType.ExecuteScalar();

            conn.Close();
            return m_Type;
        }

        public static Dictionary<int, string> getValidationTypes()
        {
            SqlConnection conn = DB.DbConnect();
            conn.Open();

            string queryString = "SELECT * FROM CMS_FormFieldValidationTypes";
            SqlCommand getTypes = new SqlCommand(queryString, conn);
            SqlDataReader typesReader = getTypes.ExecuteReader();

            Dictionary<int, string> m_Types = new Dictionary<int, string>();

            while (typesReader.Read())
            {
                m_Types.Add(typesReader.GetInt32(0), typesReader.GetString(1));
            }


            conn.Close();
            return m_Types;
        }
    }
}