using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Entities;

namespace CMS.Domain.Abstract
{
    public interface ISystemSettingsRepository
    {
        SystemSettings GetSystemSettings();
        void UpdateSystemSettings(SystemSettings m_Settings);
    }
}
