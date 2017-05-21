using System.Collections.Generic;

namespace WebDbAppSettingsMaintenance.Areas.Maintenance.Models
{
    public class DbAppSettingsViewModel
    {
        public List<string> Applications { get; set; } = new List<string>();
    }
}