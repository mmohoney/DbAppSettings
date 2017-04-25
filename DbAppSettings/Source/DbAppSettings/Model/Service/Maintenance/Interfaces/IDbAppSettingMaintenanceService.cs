using System.Collections.Generic;
using DbAppSettings.Model.DataTransfer;

namespace DbAppSettings.Model.Service.Maintenance.Interfaces
{
    public interface IDbAppSettingMaintenanceService
    {
        List<DbAppSettingDto> GetAll();
        void SaveDbAppSetting(DbAppSettingDto dto);
        void DeleteDbAppSetting(DbAppSettingDto dto);
    }
}