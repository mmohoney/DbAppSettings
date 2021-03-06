﻿using System.Collections.Generic;
using System.Linq;
using DbAppSettings.Maintenance.Model.DataAccess.Maintenance.Interfaces;
using DbAppSettings.Model.DataTransfer;

namespace DbAppSettings.Maintenance.Model.DataAccess.Maintenance.Demo
{
    internal class DemoDbAppSettingMaintenanceDao : IDbAppSettingMaintenanceDao
    {
        private static readonly Dictionary<string, Dictionary<string, DbAppSettingDto>> DemoSettingsBySession = new Dictionary<string, Dictionary<string, DbAppSettingDto>>();
       
        public List<DbAppSettingDto> GetAll(string sessionId)
        {
            GetOrCreateSessionSettings(sessionId);

            return DemoSettingsBySession[sessionId].Values.ToList();
        }

        public void SaveDbAppSetting(string sessionId, DbAppSettingDto dto)
        {
            Dictionary<string, DbAppSettingDto> settingsByKey = GetOrCreateSessionSettings(sessionId);

            if (settingsByKey.ContainsKey(dto.Key))
                settingsByKey[dto.Key] = dto;
            else
                settingsByKey.Add(dto.Key, dto);
        }

        public void DeleteDbAppSetting(string sessionId, DbAppSettingDto dto)
        {
            Dictionary<string, DbAppSettingDto> settingsByKey =  GetOrCreateSessionSettings(sessionId);

            if (settingsByKey.ContainsKey(dto.Key))
                settingsByKey.Remove(dto.Key);
        }

        private Dictionary<string, DbAppSettingDto> CreateSettings()
        {
            Dictionary<string, DbAppSettingDto> settingsByKey = new Dictionary<string, DbAppSettingDto>();
            DbAppSettingDto dto1 = new DbAppSettingDto() { Key = new DemoDbAppSettings.DemoDbAppSettingBool().FullSettingName, Value = "2", Type = typeof(int).FullName, ApplicationKey = "DbAppSettingApp" };
            DbAppSettingDto dto2 = new DbAppSettingDto() { Key = $"AnotherAssembly.{new DemoDbAppSettings.DemoDbAppSettingInt().SettingName}", Value = "NEW TEST", Type = typeof(string).FullName, ApplicationKey = "DbAppSettingApp" };
            DbAppSettingDto dto3 = new DbAppSettingDto() { Key = $"AnotherAssembly.{new DemoDbAppSettings.DemoDbAppSettingString().SettingName}", Value = "true", Type = typeof(bool).FullName, ApplicationKey = "DbAppSettingApp" };

            settingsByKey.Add(dto1.Key, dto1);
            settingsByKey.Add(dto2.Key, dto2);
            settingsByKey.Add(dto3.Key, dto3);

            DbAppSettingDto dto4 = new DbAppSettingDto() { Key = new DemoDbAppSettings.DemoSecondAppDbAppSettingBool().FullSettingName, Value = "2", Type = typeof(int).FullName, ApplicationKey = "SecondApp" };
            DbAppSettingDto dto5 = new DbAppSettingDto() { Key = new DemoDbAppSettings.DemoSecondDbAppSettingInt().FullSettingName, Value = "NEW TEST", Type = typeof(string).FullName, ApplicationKey = "SecondApp" };
            DbAppSettingDto dto6 = new DbAppSettingDto() { Key = new DemoDbAppSettings.DemoSecondDbAppSettingString().FullSettingName, Value = "true", Type = typeof(bool).FullName, ApplicationKey = "SecondApp" };

            settingsByKey.Add(dto4.Key, dto4);
            settingsByKey.Add(dto5.Key, dto5);
            settingsByKey.Add(dto6.Key, dto6);

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
                settingsByKey.Add(dto.Key, dto);
            }

            return settingsByKey;
        }

        private Dictionary<string, DbAppSettingDto> GetOrCreateSessionSettings(string sessionId)
        {
            if (DemoSettingsBySession.ContainsKey(sessionId))
                return DemoSettingsBySession[sessionId];

            Dictionary<string, DbAppSettingDto> settingsByKey = CreateSettings();
            DemoSettingsBySession.Add(sessionId, settingsByKey);

            return DemoSettingsBySession[sessionId];
        }
    }
}
