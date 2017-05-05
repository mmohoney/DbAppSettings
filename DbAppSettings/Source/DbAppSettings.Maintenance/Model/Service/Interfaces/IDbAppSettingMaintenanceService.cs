using System.Collections.Generic;
using DbAppSettings.Model.DataTransfer;

namespace DbAppSettings.Maintenance.Model.Service.Interfaces
{
    public interface IDbAppSettingMaintenanceService
    {
        List<DbAppSettingDto> GetAll();
        void SaveDbAppSetting(DbAppSettingDto dto);
        void DeleteDbAppSetting(DbAppSettingDto dto);
    }
}