using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbAppSettings.Model.DataAccess.Interfaces;
using DbAppSettings.Model.DataTransfer;

namespace DbAppSettings.Test.Mock
{
    public class DummyLazyLoadSettingDao : ILazyLoadSettingDao
    {
        public DbAppSettingDto GetDbAppSetting(DbAppSettingDto dbAppSettingDto)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DbAppSettingDto> GetChangedDbAppSettings(DateTime? latestDbAppSettingChangedDate)
        {
            throw new NotImplementedException();
        }
    }
}
