using System.Collections.Generic;
using DbAppSettings.Model.DataTransfer;

namespace DbAppSettings.Maintenance.Model.Service.Interfaces
{
    public interface IDbAppSettingMaintenanceService
    {
        List<DbAppSettingDto> GetAll(string sessionId);
        void SaveDbAppSetting(string sessionId, DbAppSettingDto dto);
        void DeleteDbAppSetting(string sessionId, DbAppSettingDto dto);
    }
}