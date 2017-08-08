using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using DbAppSettings.Model.DataTransfer;
using DbAppSettings.Model.Domain;

namespace WebDbAppSettingsMaintenance.Areas.DbAppSettings.Models
{
    public class DbAppSettingModel
    {
        public DbAppSettingModel()
        {
            
        }

        public string Application { get; set; }
        public string Assembly { get; set; }
        public string Key { get; set; }
        public string DisplayKey
        {
            get
            {
                List<string> keySplits = Key.Split('.').ToList();
                return keySplits[keySplits.Count - 1];
            }
        }
        public string Type { get; set; }
        public string Value { get; set; }

        public static DbAppSettingModel FromDto(DbAppSettingDto dto)
        {
            string value =dto.Value;

            if (dto.Type == typeof(StringCollection).FullName)
            {
                StringCollection collection = InternalDbAppSettingBase.ConvertJsonToStringCollection(dto.Value);
                string stringValue = InternalDbAppSettingBase.ConvertStringCollectionToString(collection);
                value = stringValue;
            }

            return new DbAppSettingModel
            {
                Application = dto.ApplicationKey,
                Assembly = dto.Assembly,
                Key = dto.Key,
                Type = dto.Type,
                Value = value,
            };
        }

        public DbAppSettingDto ToDto()
        {
            string value = Value;

            if (Type == typeof(StringCollection).FullName)
            {
                StringCollection collection = InternalDbAppSettingBase.ConvertStringToStringCollection(Value);
                string jsonCollection = InternalDbAppSettingBase.ConvertStringCollectionToJson(collection);
                value = jsonCollection;
            }

            return new DbAppSettingDto
            {
                ApplicationKey = Application,
                Key = Key,
                Type = Type,
                Value = value,
            };
        }
    }
}