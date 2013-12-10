using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Entities;

namespace CMS.Domain.Abstract
{
    public interface INewsRepository
    {
        List<BlogPost> GetNews();
        WidgetContainer GetNewsContainer();
        BlogPost GetArticle(int id);
    }
}
