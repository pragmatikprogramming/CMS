using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Domain.Abstract;
using CMS.Domain.Entities;
using CMS.Domain.DataAccess;

namespace CMS.Domain.Models
{
    public class SystemSettingsRepository : ISystemSettingsRepository
    {
        public SystemSettings GetSystemSettings()
        {
            SystemSettings m_Settings = DBSystemSettings.GetSystemSettings();
            return m_Settings;
        }

        public void UpdateSystemSettings(SystemSettings m_Settings)
        {
            DBSystemSettings.UpdateSystemSettings(m_Settings);
        }
    }
}