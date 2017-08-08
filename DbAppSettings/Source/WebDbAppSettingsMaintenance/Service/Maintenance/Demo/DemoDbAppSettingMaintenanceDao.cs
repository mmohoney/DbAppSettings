using System;
using System.Collections.Generic;
using System.Linq;
using DbAppSettings.Model.DataAccess.Interfaces;
using DbAppSettings.Model.DataTransfer;
using DbAppSettings.Model.Domain;

namespace WebDbAppSettingsMaintenance.Service.Maintenance.Demo
{
    internal class DemoDbAppSettingMaintenanceDao : IDbAppSettingMaintenanceDao
    {
        private static readonly Dictionary<string, DbAppSettingDto> DemoSettingsBySession = CreateSettings();

        public List<DbAppSettingDto> GetAll()
        {
            return DemoSettingsBySession.Values.ToList();
        }

        public void SaveDbAppSetting(DbAppSettingDto dto)
        {
            if (DemoSettingsBySession.ContainsKey(dto.Key))
                DemoSettingsBySession[dto.Key] = dto;
            else
                DemoSettingsBySession.Add(dto.Key, dto);
        }

        public void DeleteDbAppSetting(DbAppSettingDto dto)
        {
            if (DemoSettingsBySession.ContainsKey(dto.Key))
                DemoSettingsBySession.Remove(dto.Key);
        }

        private static Dictionary<string, DbAppSettingDto> CreateSettings()
        {
            Dictionary<string, DbAppSettingDto> settingsByKey = new Dictionary<string, DbAppSettingDto>();

            List<DbAppSettingDto> dbAppSettingDtos = GetAllDbAppSettingDtos();

            dbAppSettingDtos.ForEach(s => s.ApplicationKey = "Application One");
            dbAppSettingDtos.ForEach(s => s.Key = $"One_{s.Key}");
            dbAppSettingDtos.ForEach(s => settingsByKey.Add(s.Key, s));

            List<DbAppSettingDto> dbAppSettingDtos2 = GetAllDbAppSettingDtos();

            dbAppSettingDtos2.ForEach(s => s.ApplicationKey = "Application Two");
            dbAppSettingDtos2.ForEach(s => s.Key = $"Two_{s.Key}");
            dbAppSettingDtos2.ForEach(s => settingsByKey.Add(s.Key, s));

            return settingsByKey;
        }

        private static List<DbAppSettingDto> GetAllDbAppSettingDtos()
        {
            List<DbAppSettingDto> dbAppSettingDtos = new List<DbAppSettingDto>();

            List<Type> types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(t => t.Namespace == "WebDbAppSettingsMaintenance.Service.Maintenance.Demo")
                .Where(t => t.IsClass)
                .ToList();

            foreach (Type type in types)
            {
                object activatedType = Activator.CreateInstance(type);
                InternalDbAppSettingBase dbAppSettingBase = activatedType as InternalDbAppSettingBase;
                if (dbAppSettingBase != null)
                {
                    DbAppSettingDto dto = dbAppSettingBase.ToDto();
                    dbAppSettingDtos.Add(dto);
                }
            }

            return dbAppSettingDtos;
        }
    }
}
