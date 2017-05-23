using System.Collections.Generic;
using System.Linq;
using DbAppSettings.Maintenance.Model.DataAccess.Maintenance.Interfaces;
using DbAppSettings.Model.DataTransfer;

namespace DbAppSettings.Maintenance.Model.DataAccess.Maintenance.Demo
{
    internal class DemoDbAppSettingMaintenanceDao : IDbAppSettingMaintenanceDao
    {
        private static readonly Dictionary<string, DbAppSettingDto> DemoSettings = new Dictionary<string, DbAppSettingDto>();

        static DemoDbAppSettingMaintenanceDao()
        {
            DbAppSettingDto dto1 = new DbAppSettingDto() { Key = new DemoDbAppSettings.DemoDbAppSettingBool().FullSettingName, Value = "2", Type = typeof(int).FullName, ApplicationKey = "DbAppSettingApp" };
            DbAppSettingDto dto2 = new DbAppSettingDto() { Key = $"AnotherAssembly.{new DemoDbAppSettings.DemoDbAppSettingInt().SettingName}", Value = "NEW TEST", Type = typeof(string).FullName, ApplicationKey = "DbAppSettingApp" };
            DbAppSettingDto dto3 = new DbAppSettingDto() { Key = $"AnotherAssembly.{new DemoDbAppSettings.DemoDbAppSettingString().SettingName}", Value = "true", Type = typeof(bool).FullName, ApplicationKey = "DbAppSettingApp" };

            DemoSettings.Add(dto1.Key, dto1);
            DemoSettings.Add(dto2.Key, dto2);
            DemoSettings.Add(dto3.Key, dto3);
            
            DbAppSettingDto dto4 = new DbAppSettingDto() { Key = new DemoDbAppSettings.DemoSecondAppDbAppSettingBool().FullSettingName, Value = "2", Type = typeof(int).FullName, ApplicationKey = "SecondApp" };
            DbAppSettingDto dto5 = new DbAppSettingDto() { Key = new DemoDbAppSettings.DemoSecondDbAppSettingInt().FullSettingName, Value = "NEW TEST", Type = typeof(string).FullName, ApplicationKey = "SecondApp" };
            DbAppSettingDto dto6 = new DbAppSettingDto() { Key = new DemoDbAppSettings.DemoSecondDbAppSettingString().FullSettingName, Value = "true", Type = typeof(bool).FullName, ApplicationKey = "SecondApp" };

            DemoSettings.Add(dto4.Key, dto4);
            DemoSettings.Add(dto5.Key, dto5);
            DemoSettings.Add(dto6.Key, dto6);

            List<DbAppSettingDto> allTypeDtos = new List<DbAppSettingDto>
            {
                new DemoDbAppSettings.DemoAnotherAppDbAppSettingBool().ToDto(),
                new DemoDbAppSettings.DemoAnotherAppDbAppSettingByte().ToDto(),
                new DemoDbAppSettings.DemoAnotherAppDbAppSettingChar().ToDto(),
                new DemoDbAppSettings.DemoAnotherAppDbAppSettingDecimal().ToDto()
            };

            foreach (DbAppSettingDto dto in allTypeDtos)
            {
                dto.ApplicationKey = "Another App";
                DemoSettings.Add(dto.Key, dto);
            }
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
