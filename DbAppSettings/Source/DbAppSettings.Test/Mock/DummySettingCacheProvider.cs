using System;
using System.Collections.Generic;
using DbAppSettings.Model.DataAccess.Interfaces;
using DbAppSettings.Model.DataTransfer;
using DbAppSettings.Model.Domain;
using DbAppSettings.Model.Service.CacheManager.Arguments;
using DbAppSettings.Model.Service.SettingCacheProvider;

namespace DbAppSettings.Test.Mock
{
    internal class DummyCacheManagerArguments : CacheManagerArguments
    {
        
    }

    internal class DummySaveNewSettingDao : ISaveNewSettingDao
    {
        public int SaveNewSettingIfNotExistsHitCount = 0;
        public void SaveNewSettingIfNotExists(DbAppSettingDto dbAppSettingDto)
        {
            SaveNewSettingIfNotExistsHitCount++;
        }
    }

    internal class DummyCacheManagerArguments2 : CacheManagerArguments
    {

    }

    internal class DummySettingCacheProvider : SettingCacheProviderBase
    {
        private readonly DummyCacheManagerArguments _managerArguments;

        internal DummySettingCacheProvider(DummyCacheManagerArguments managerArguments)
        {
            _managerArguments = managerArguments;
        }

        internal override CacheManagerArguments ManagerArguments => _managerArguments;

        public int InitializeSettingCacheProviderHitCount = 0;
        internal override void InitializeSettingCacheProvider()
        {
            InitializeSettingCacheProviderHitCount++;
            return;
        }

        internal override List<DbAppSettingDto> GetChangedSettings()
        {
            throw new NotImplementedException();
        }

        public override DbAppSetting<T, TValueType> GetDbAppSetting<T, TValueType>()
        {
            throw new NotImplementedException();
        }

        internal override void InitalizeSettingWatchTask()
        {
            return;
        }
    }

    internal class DummySettingCacheProvider2 : SettingCacheProviderBase
    {
        private readonly DummyCacheManagerArguments _managerArguments;

        internal DummySettingCacheProvider2(DummyCacheManagerArguments managerArguments)
        {
            _managerArguments = managerArguments;
        }

        internal override CacheManagerArguments ManagerArguments => _managerArguments;

        internal override void InitializeSettingCacheProvider()
        {
            return;
        }

        public int GetChangedSettingsHitCount = 0;
        internal override List<DbAppSettingDto> GetChangedSettings()
        {
            GetChangedSettingsHitCount++;
            return new List<DbAppSettingDto>();
        }

        public override DbAppSetting<T, TValueType> GetDbAppSetting<T, TValueType>()
        {
            throw new NotImplementedException();
        }
    }

    public class DbAppSettingTestSetting : DbAppSetting<DbAppSettingTestSetting, int> { public override int InitialValue => 1; }

    internal class DummySettingCacheProvider3 : SettingCacheProviderBase
    {
        private readonly DummyCacheManagerArguments _managerArguments;

        internal DummySettingCacheProvider3(DummyCacheManagerArguments managerArguments)
        {
            _managerArguments = managerArguments;
        }

        internal override CacheManagerArguments ManagerArguments => _managerArguments;

        internal override void InitializeSettingCacheProvider()
        {
            return;
        }

        public int GetChangedSettingsHitCount = 0;
        internal override List<DbAppSettingDto> GetChangedSettings()
        {
            GetChangedSettingsHitCount++;
            DbAppSettingDto dto = new DbAppSettingTestSetting().ToDto();
            dto.ModifiedDate = DateTime.Today.AddDays(1);
            return new List<DbAppSettingDto> { dto };
        }

        public override DbAppSetting<T, TValueType> GetDbAppSetting<T, TValueType>()
        {
            throw new NotImplementedException();
        }

        internal override void SaveNewSettingIfNotExists(DbAppSettingDto dbAppSettingDto)
        {
            throw new NotImplementedException();
        }
    }

    internal class DummySettingCacheProvider4 : SettingCacheProviderBase
    {
        private readonly DummyCacheManagerArguments _managerArguments;

        internal DummySettingCacheProvider4(DummyCacheManagerArguments managerArguments)
        {
            _managerArguments = managerArguments;
        }

        internal override CacheManagerArguments ManagerArguments => _managerArguments;

        internal override void InitializeSettingCacheProvider()
        {
            return;
        }

        internal override List<DbAppSettingDto> GetChangedSettings()
        {
            return new List<DbAppSettingDto>();
        }

        public override DbAppSetting<T, TValueType> GetDbAppSetting<T, TValueType>()
        {
            throw new NotImplementedException();
        }
    }
}
