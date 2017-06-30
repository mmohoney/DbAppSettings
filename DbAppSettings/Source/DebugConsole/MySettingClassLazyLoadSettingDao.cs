using System;
using System.Collections.Generic;
using System.Linq;
using DbAppSettings.Model.DataAccess.Interfaces;
using DbAppSettings.Model.DataTransfer;

namespace DebugConsole
{
    public class MySettingClassLazyLoadSettingDao : ILazyLoadSettingDao
    {
        public static readonly Dictionary<string, DbAppSettingDto> CachedKeys = new Dictionary<string, DbAppSettingDto>();

        public MySettingClassLazyLoadSettingDao()
        {

        }

        public DbAppSettingDto GetDbAppSetting(DbAppSettingDto dbAppSettingDto)
        {
            if (CachedKeys.ContainsKey(dbAppSettingDto.Key))
                return CachedKeys[dbAppSettingDto.Key];

            return null;
        }

        public IEnumerable<DbAppSettingDto> GetChangedDbAppSettings(DateTime? latestDbAppSettingChangedDate)
        {
            return new List<DbAppSettingDto>(CachedKeys.Values.ToList());
        }
    }

    public class MySettingClassSaveNewSettingDao : ISaveNewSettingDao
    {
        public void SaveNewSettingIfNotExists(DbAppSettingDto dbAppSettingDto)
        {
            if (MySettingClassLazyLoadSettingDao.CachedKeys.ContainsKey(dbAppSettingDto.Key))
                MySettingClassLazyLoadSettingDao.CachedKeys[dbAppSettingDto.Key] = dbAppSettingDto;
            else
                MySettingClassLazyLoadSettingDao.CachedKeys.Add(dbAppSettingDto.Key, dbAppSettingDto);
        }
    }
}
