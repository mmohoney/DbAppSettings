using System;
using System.Collections.Generic;

namespace DbAppSettings
{
    /// <summary>
    /// List of supported DbAppSetting types. Currently supports types in the Settings.settings types
    /// </summary>
    public static class DbAppSupportedValueTypes
    {
        /// <summary>
        /// List of supported DbAppSetting types dictionary by type FullName. Currently supports types in the Settings.settings types
        /// </summary>
        public static readonly Dictionary<string, Type> Types = new Dictionary<string, Type>
        {
            { typeof(bool).FullName, typeof(bool) },
            { typeof(byte).FullName, typeof(byte) },
            { typeof(char).FullName, typeof(char) },
            { typeof(decimal).FullName, typeof(decimal) },
            { typeof(double).FullName, typeof(double) },
            { typeof(float).FullName, typeof(float) },
            { typeof(int).FullName, typeof(int) },
            { typeof(long).FullName,  typeof(long) },
            { typeof(sbyte).FullName,  typeof(sbyte) },
            { typeof(short).FullName,  typeof(short) },
            { typeof(string).FullName,  typeof(string) },
            { typeof(System.Collections.Specialized.StringCollection).FullName,  typeof(System.Collections.Specialized.StringCollection) },
            { typeof(DateTime).FullName,  typeof(DateTime) },
            { typeof(Guid).FullName, typeof(Guid) },
            { typeof(TimeSpan).FullName,  typeof(TimeSpan) },
            { typeof(uint).FullName,  typeof(uint) },
            { typeof(ulong).FullName,  typeof(ulong) },
            { typeof(ushort).FullName,  typeof(ushort) },
        };
    }
}
