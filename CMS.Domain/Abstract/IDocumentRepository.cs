using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Entities;
using System.Web;

namespace CMS.Domain.Abstract
{
    public interface IDocumentRepository
    {
        void Create(Document m_Document, HttpPostedFileBase fileUpload);
        Document RetrieveOne(int id);
        List<Document> RetrieveAll(int parentId);
        void Update(Document m_Document, string OldName, HttpPostedFileBase fileUpload);
        bool Delete(int Id);
        void MoveDoc(int parentId, int id);
    }
}
