using System.Collections.Generic;
using System.Linq;
using DbAppSettings.Model.Domain;

namespace WebDbAppSettingsMaintenance.Areas.Maintenance.Models
{
    public class DbAppSettingsViewModel
    {
        public List<string> Applications { get; set; } = new List<string>();
        public List<string> Types => DbAppSupportedValueTypes.Types.Select(t => t.Key).ToList();
    }
}