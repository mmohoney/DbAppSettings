using System.Collections.Generic;
using DbAppSettings.Maintenance.Model.DataAccess.Maintenance.Interfaces;
using DbAppSettings.Maintenance.Model.Service.Interfaces;
using DbAppSettings.Model.DataTransfer;

namespace DbAppSettings.Maintenance.Model.Service
{
    public class DbAppSettingMaintenanceService : IDbAppSettingMaintenanceService
    {
        private readonly IDbAppSettingMaintenanceDao _dbAppSettingMaintenanceDao;

        public DbAppSettingMaintenanceService(IDbAppSettingMaintenanceDao dao)
        {
            _dbAppSettingMaintenanceDao = dao;
        }

        public List<DbAppSettingDto> GetAll(string sessionId)
        {
            return _dbAppSettingMaintenanceDao.GetAll(sessionId);
        }

        public void SaveDbAppSetting(string sessionId, DbAppSettingDto dto)
        {
            _dbAppSettingMaintenanceDao.SaveDbAppSetting(sessionId, dto);
        }

        public void DeleteDbAppSetting(string sessionId, DbAppSettingDto dto)
        {
            _dbAppSettingMaintenanceDao.DeleteDbAppSetting(sessionId, dto);
        }
    }
}
