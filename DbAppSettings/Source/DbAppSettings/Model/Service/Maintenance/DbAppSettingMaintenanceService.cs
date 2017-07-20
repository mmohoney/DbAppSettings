using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using DbAppSettings.Model.DataAccess.Interfaces;
using DbAppSettings.Model.DataTransfer;
using DbAppSettings.Model.Domain;
using DbAppSettings.Model.Service.Maintenance.Interfaces;

namespace DbAppSettings.Model.Service.Maintenance
{
    /// <summary>
    /// Manages all interactions between the DAL and the caller
    /// </summary>
    public class DbAppSettingMaintenanceService : IDbAppSettingMaintenanceService
    {
        private readonly IDbAppSettingMaintenanceDao _dbAppSettingMaintenanceDao;

        /// <summary>
        /// Manages all interactions between the DAL and the caller
        /// </summary>
        public DbAppSettingMaintenanceService(IDbAppSettingMaintenanceDao dao)
        {
            _dbAppSettingMaintenanceDao = dao;
        }

        /// <summary>
        /// Returns all settings from the DAL
        /// </summary>
        /// <returns></returns>
        public List<DbAppSettingDto> GetAll()
        {
            return _dbAppSettingMaintenanceDao.GetAll();
        }

        /// <summary>
        /// Inserts or updates the setting in the DAL
        /// </summary>
        /// <param name="dto"></param>
        public void SaveDbAppSetting(DbAppSettingDto dto)
        {
            _dbAppSettingMaintenanceDao.SaveDbAppSetting(dto);
        }

        /// <summary>
        /// Removes the setting from the DAL
        /// </summary>
        /// <param name="dto"></param>
        public void DeleteDbAppSetting(DbAppSettingDto dto)
        {
            _dbAppSettingMaintenanceDao.DeleteDbAppSetting(dto);
        }

        /// <summary>
        /// Returns whether or not the value passed in is a valid value for the type
        /// </summary>
        /// <param name="value"></param>
        /// <param name="valueType"></param>
        /// <returns></returns>
        public bool ValidateValueForType(object value, string valueType)
        {
            return IsValidType(value, valueType);

            //TODO: Custom validation logic
        }

        private bool IsValidType(object value, string valueType)
        {
            //Always check for value
            if (value == null)
                return false;

            //We need to define a type
            if (valueType == null)
                return false;

            //Only valid list of supported types can pass
            if (!DbAppSupportedValueTypes.Types.ContainsKey(valueType))
                return false;

            Type type = DbAppSupportedValueTypes.Types[valueType];

            //Try and change the value to the specified type
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
