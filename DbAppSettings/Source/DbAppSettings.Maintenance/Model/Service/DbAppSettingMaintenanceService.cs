using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using DbAppSettings.Maintenance.Model.DataAccess.Maintenance.Interfaces;
using DbAppSettings.Maintenance.Model.Service.Interfaces;
using DbAppSettings.Model.DataTransfer;
using DbAppSettings.Model.Domain;

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

        public bool ValidateValueForType(object value, string valueType)
        {
            return IsValidType(value, valueType);
            
            //TODO: Custom validation logic
        }

        public bool IsValidType(object value, string valueType)
        {
            if (value == null)
                return false;

            if (valueType == null)
                return false;

            if (!DbAppSupportedValueTypes.Types.ContainsKey(valueType))
                return false;

            Type type = DbAppSupportedValueTypes.Types[valueType];

            try
            {
                if (type == typeof(StringCollection))
                {
                    List<string> splits = ((string)value).Split(new string[] { System.Environment.NewLine }, StringSplitOptions.None).ToList();
                    if (splits.Count < 1)
                        return false;

                    StringCollection collection = new StringCollection();
                    collection.AddRange(splits.ToArray());

                    return true;
                }

                TypeDescriptor.GetConverter(type).ConvertFrom(value);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
