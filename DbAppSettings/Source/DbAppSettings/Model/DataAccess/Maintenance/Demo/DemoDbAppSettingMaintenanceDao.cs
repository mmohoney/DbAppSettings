using System.Collections.Generic;
using System.Linq;
using DbAppSettings.Model.DataAccess.Maintenance.Interfaces;
using DbAppSettings.Model.DataTransfer;

namespace DbAppSettings.Model.DataAccess.Maintenance.Demo
{
    internal class DemoDbAppSettingMaintenanceDao : IDbAppSettingMaintenanceDao
    {
        private static readonly Dictionary<string, DbAppSettingDto> DemoSettings = new Dictionary<string, DbAppSettingDto>();

        static DemoDbAppSettingMaintenanceDao()
        {
            DbAppSettingDto dto1 = new DbAppSettingDto() { Key = new DemoDbAppSettings.DemoDbAppSettingBool().FullSettingName, Value = "2", Type = typeof(int).FullName, ApplicationKey = "DbAppSettingApp" };
            DbAppSettingDto dto2 = new DbAppSettingDto() { Key = new DemoDbAppSettings.DemoDbAppSettingInt().FullSettingName, Value = "NEW TEST", Type = typeof(string).FullName, ApplicationKey = "DbAppSettingApp" };
            DbAppSettingDto dto3 = new DbAppSettingDto() { Key = new DemoDbAppSettings.DemoDbAppSettingString().FullSettingName, Value = "true", Type = typeof(bool).FullName, ApplicationKey = "DbAppSettingApp" };

            DemoSettings.Add(dto1.Key, dto1);
            DemoSettings.Add(dto2.Key, dto2);
            DemoSettings.Add(dto3.Key, dto3);
        }

        public List<DbAppSettingDto> GetAll()
        {
            return DemoSettings.Values.ToList();
        }

        public void SaveDbAppSetting(DbAppSettingDto dto)
        {
            if (DemoSettings.ContainsKey(dto.Key))
                DemoSettings[dto.Key] = dto;
            else
                DemoSettings.Add(dto.Key, dto);
        }

        public void DeleteDbAppSetting(DbAppSettingDto dto)
        {
            if (DemoSettings.ContainsKey(dto.Key))
                DemoSettings.Remove(dto.Key);
        }
    }
}
