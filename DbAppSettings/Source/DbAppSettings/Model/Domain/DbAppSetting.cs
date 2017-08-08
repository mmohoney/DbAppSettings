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
        /// Protected constructor 
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
        /// Internal method used to convert a string into a StringCollection seperating on new lines
        /// </summary>
        /// <param name="stringCollectionValue"></param>
        /// <returns></returns>
        internal static StringCollection ConvertStringToStringCollection(string stringCollectionValue)
        {
            List<string> breaks = stringCollectionValue.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None).ToList();
            StringCollection stringCollection = new StringCollection();
            stringCollection.AddRange(breaks.ToArray());
            return stringCollection;
        }

        /// <summary>
        /// Internal method used to convert a StringCollection to a new line seperated string
        /// </summary>
        /// <param name="stringCollection"></param>
        /// <returns></returns>
        internal static string ConvertStringCollectionToString(StringCollection stringCollection)
        {
            List<string> stringList = stringCollection.Cast<string>().ToList();
            return string.Join(System.Environment.NewLine, stringList);
        }

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

        /// <summary>
        /// Convert the string representation of the value to the given type
        /// </summary>
        /// <typeparam name="TValueType"></typeparam>
        /// <param name="typeString"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static TValueType GetValueFromString<TValueType>(string typeString, string value)
        {
            if (!DbAppSupportedValueTypes.Types.ContainsKey(typeString))
                return new JavaScriptSerializer().Deserialize<TValueType>(value);

            //Get the type from the list of valid types
            Type type = DbAppSupportedValueTypes.Types[typeString];

            //Logic to populate the value as the value type
            if (type == typeof(StringCollection))
                return (TValueType)(ConvertJsonToStringCollection(value) as object);

            return (TValueType)TypeDescriptor.GetConverter(type).ConvertFromInvariantString(value);
        }

        /// <summary>
        /// Convert the given type value to the string representation of the value
        /// </summary>
        /// <typeparam name="TValueType"></typeparam>
        /// <param name="typeString"></param>
        /// <param name="internalValue"></param>
        /// <returns></returns>
        internal static string ConvertValueToString<TValueType>(string typeString, TValueType internalValue)
        {
            if (!DbAppSupportedValueTypes.Types.ContainsKey(typeString))
                return new JavaScriptSerializer().Serialize(internalValue);

            //Get the type from the list of valid types
            Type type = DbAppSupportedValueTypes.Types[typeString];

            //Logic to populate the value as the value type
            if (type == typeof(StringCollection))
                return ConvertStringCollectionToJson(internalValue as StringCollection);

            return internalValue.ToString();
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
        private TValueType _value;

        /// <summary>
        /// Protected constructor 
        /// </summary>
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
        /// Returns whether or not the domain was hydrated from the data access layer
        /// </summary>
        internal bool HydratedFromDataAccess { get; private set; }
        /// <summary>
        /// Ties a setting to an application. The cache will only load settings for the specified application. Will be null until
        /// hydrated from the data access layer
        /// </summary>
        public string ApplicationKey { get; private set; }
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
                if (!HydratedFromDataAccess)
                    return InitialValue;
                return _value;
            }
            private set { _value = value; }
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

            //Get the internal value
            InternalValue = GetValueFromString<TValueType>(TypeString, value);

            //Let the class know that it was hydrated from the data access layer
            HydratedFromDataAccess = true;
        }

        /// <summary>
        /// Internal method used to convert the domain into the dto representation
        /// </summary>
        /// <returns></returns>
        internal override DbAppSettingDto ToDto()
        {
            DbAppSettingDto dpAppSettingDto = new DbAppSettingDto
            {
                ApplicationKey = ApplicationKey,
                Key = FullSettingName,
                Type = TypeString,
                Value = ConvertValueToString(TypeString, InternalValue),
            };

            return dpAppSettingDto;
        }
    }
}
