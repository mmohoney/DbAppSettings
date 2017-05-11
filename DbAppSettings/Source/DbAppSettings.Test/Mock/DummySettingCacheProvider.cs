using System;
using System.Collections.Generic;
using DbAppSettings.Model.DataTransfer;
using DbAppSettings.Model.Domain;
using DbAppSettings.Model.Service.CacheManager.Arguments;
using DbAppSettings.Model.Service.SettingCacheProvider;

namespace DbAppSettings.Test.Mock
{
    internal class DummyCacheManagerArguments : CacheManagerArguments
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
}
