using DbAppSettings.Model.DataTransfer;

namespace WebDbAppSettingsMaintenance.Areas.Maintenance.Models
{
    public class DbAppSettingModel
    {
        public DbAppSettingModel()
        {
            
        }

        public string Application { get; set; }
        public string Assembly { get; set; }
        public string Key { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }

        public static DbAppSettingModel FromDto(DbAppSettingDto dto)
        {
            return new DbAppSettingModel
            {
                Application = dto.ApplicationKey,
                Assembly = dto.Assembly,
                Key = dto.Key,
                Type = dto.Type,
                Value = dto.Value,
            };
        }

        public DbAppSettingDto ToDto()
        {
            return new DbAppSettingDto
            {
                ApplicationKey = Application,
                Key = $"{Assembly}.{Key}",
                Value =Value,
            };
        }
    }
}