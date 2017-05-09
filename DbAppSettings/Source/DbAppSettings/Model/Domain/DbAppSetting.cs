using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Web.Script.Serialization;
using DbAppSettings.Model.DataTransfer;
using DbAppSettings.Model.Service;

namespace DbAppSettings.Model.Domain
{
    /// <summary>
    /// Represents the most basic DbAppSetting. This cannot be implemented outside the assembly due to the internal constructor
    /// </summary>
    [Browsable(false)]  //Hide from intellisense
    [EditorBrowsable(EditorBrowsableState.Never)]   //Hide from intellisense
    public abstract class InternalDbAppSettingBase
    {
        private string _assemblyName;
        private string _settingName;

        /// <summary>
        /// Don't allow instantiation outside of inherited classes
        /// </summary>
        protected InternalDbAppSettingBase()
        {
            
        }

        /// <summary>
        /// Assembly name where the setting resides 
        /// </summary>
        public string AssemblyName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_assemblyName))
                    _assemblyName = GetType().Assembly.GetName(false).Name;
                return _assemblyName;
            }
        }
        /// <summary>
        /// Class name of the setting
        /// </summary>
        public string SettingName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_settingName))
                    _settingName = GetType().Name;
                return _settingName;
            }
        }
        /// <summary>
        /// Combined name of the assembly and the class. This is the unique key of the setting
        /// </summary>
        public string FullSettingName => $"{AssemblyName}.{SettingName}";

        /// <summary>
        /// Internal method used to hydrate the object without knowning its generic representation
        /// </summary>
        /// <param name="dto"></param>
        internal abstract void From(DbAppSettingDto dto);

        /// <summary>
        /// Internal method used to convert the domain into the dto representation
        /// </summary>
        /// <returns></returns>
        internal abstract DbAppSettingDto ToDto();

        /// <summary>
        /// Convert a string collection into a json string to pass to the data access layer
        /// </summary>
        /// <param name="stringCollection"></param>
        /// <returns></returns>
        internal static string ConvertStringCollectionToJson(StringCollection stringCollection)
        {
            List<string> inputList = stringCollection.Cast<string>().ToList();
            return new JavaScriptSerializer().Serialize(inputList);
        }

        /// <summary>
        /// Convert a json into a string collection
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        internal static StringCollection ConvertJsonToStringCollection(string json)
        {
            List<string> resultList = new JavaScriptSerializer().Deserialize<List<string>>(json);
            StringCollection stringCollection = new StringCollection();
            stringCollection.AddRange(resultList.ToArray());
            return stringCollection;
        }
    }

    /// <summary>
    /// Represents a setting containing a default value and an up to date value from the data access layer. If the data access layer
    /// cannot update the underlying value, a default value will always be provided 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TValueType"></typeparam>
    public abstract class DbAppSetting<T, TValueType> : InternalDbAppSettingBase where T : DbAppSetting<T, TValueType>, new()
    {
        private string _typeString;
        private bool _hydratedFromDataAccess;
        private TValueType _value;

        protected DbAppSetting()
        {

        }

        /// <summary>
        /// All DbAppSettings should contain a default value that will be returned if a data access value cannot be provided.
        /// This allows safe fallback if a data access connection cannot be made.
        /// </summary>
        public abstract TValueType InitialValue { get; }

        /// <summary>
        /// The static level representing either a default value, or an up to date value from the data access layer
        /// </summary>
        public static TValueType Value => SettingCache.GetDbAppSetting<T, TValueType>().InternalValue;
        /// <summary>
        /// The static level representing the initial value
        /// </summary>
        public static TValueType DefaultValue => new T().InitialValue;

        /// <summary>
        /// Ties a setting to an application. The cache will only load settings for the specified application. Will be null until
        /// hydrated from the data access layer
        /// </summary>
        public string ApplicationKey { get; protected set; }
        /// <summary>
        /// The instance level value representing either a default value, or an up to date value from the data access layer
        /// </summary>
        public TValueType InstanceValue => Value;
        /// <summary>
        /// Type of TValueType
        /// </summary>
        public Type Type => typeof(TValueType);
        /// <summary>
        /// The default value of the TValueType or the value provided from the data access layer
        /// </summary>
        internal TValueType InternalValue
        {
            get
            {
                if (!_hydratedFromDataAccess)
                    return InitialValue;
                return _value;
            }
            set { _value = value; }
        }
        /// <summary>
        /// The string representation of the type
        /// </summary>
        internal string TypeString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_typeString))
                    _typeString = typeof(TValueType).FullName;
                return _typeString;
            }
        }

        /// <summary>
        /// Specific method that hydrates a setting from the dto provided by the data access layer
        /// </summary>
        /// <param name="dto"></param>
        internal override void From(DbAppSettingDto dto)
        {
            //Application key
            ApplicationKey = dto.ApplicationKey;

            //Type string
            _typeString = dto.Type;

            //String value
            string value = dto.Value;

            //Types cannot be null. If we encounter this case, we need to fail as types cannot be hydrated
            if (DbAppSupportedValueTypes.Types.ContainsKey(TypeString))
            {
                //Get the type from the list of valid types
                Type type = DbAppSupportedValueTypes.Types[TypeString];

                //Logic to populate the value as the value type
                if (type == typeof(StringCollection))
                {
                    InternalValue = (TValueType) (ConvertJsonToStringCollection(value) as object);
                }
                else
                {
                    InternalValue = (TValueType) TypeDescriptor.GetConverter(type).ConvertFromInvariantString(value);
                }
            }
            else
            {
                InternalValue = new JavaScriptSerializer().Deserialize<TValueType>(value);
            }

            //Let the class know that it was hydrated from the data access layer
            _hydratedFromDataAccess = true;
        }

        /// <summary>
        /// Internal method used to convert the domain into the dto representation
        /// </summary>
        /// <returns></returns>
        internal override DbAppSettingDto ToDto()
        {
            string value;

            //Types cannot be null. If we encounter this case, we need to fail as types cannot be hydrated
            if (DbAppSupportedValueTypes.Types.ContainsKey(TypeString))
            {
                //Get the type from the list of valid types
                Type type = DbAppSupportedValueTypes.Types[TypeString];

                //Logic to populate the value as the value type
                if (type == typeof(StringCollection))
                {
                    value = ConvertStringCollectionToJson(InternalValue as StringCollection);
                }
                else
                {
                    value = InternalValue.ToString();
                }
            }
            else
            {
                value = new JavaScriptSerializer().Serialize(InternalValue);
            }

            DbAppSettingDto dpAppSettingDto = new DbAppSettingDto
            {
                ApplicationKey = ApplicationKey,
                Key = FullSettingName,
                Type = TypeString,
                Value = value,
            };

            return dpAppSettingDto;
        }
    }
}
