using System;
using System.Collections.Generic;
using DbAppSettings.Model.DataAccess.Interfaces;
using DbAppSettings.Model.DataTransfer;

namespace DbAppSettings.Test.Mock
{
    public class DummyDbAppSettingDao : IRetrieveAllSettingDao
    {
        public IEnumerable<DbAppSettingDto> GetAllDbAppSettings()
        {
            return new DummyDbAppSettingsDtos().GetAllDbAppSettings();
        }

        public IEnumerable<DbAppSettingDto> GetChangedDbAppSettings(DateTime? latestDbAppSettingChangedDate)
        {
            throw new NotImplementedException();
        }
    }
}
