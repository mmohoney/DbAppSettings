using System.Collections.Generic;
using DbAppSettings.Model.DataTransfer;

namespace DbAppSettings.Maintenance.Model.DataAccess.Maintenance.Interfaces
{
    public interface IDbAppSettingMaintenanceDao
    {
        List<DbAppSettingDto> GetAll();
        void SaveDbAppSetting(DbAppSettingDto dto);
        void DeleteDbAppSetting(DbAppSettingDto dto);
    }
}