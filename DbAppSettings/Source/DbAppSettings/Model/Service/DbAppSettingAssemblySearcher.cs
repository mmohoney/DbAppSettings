using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DbAppSettings.Model.Domain;

namespace DbAppSettings.Model.Service
{
    internal static class DbAppSettingAssemblySearcher
    {
        /// <summary>
        /// Returns all DbAppSettings implementing the generic signature
        /// </summary>
        /// <returns></returns>
        public static List<Type> GetGenericDbAppSettings()
        {
            List<Type> types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(t => t.GetLoadableTypes())
                .Where(t => t.BaseType != null && t.BaseType.IsGenericType)
                .Where(t => t.BaseType.GetGenericTypeDefinition() == typeof(DbAppSetting<,>))
                .OrderBy(t => t.Name)
                .ToList();
            return types;
        }
    }

    internal static class AssemblyExtensions
    {
        /// <summary>
        /// If types are not loaded, safely fail instead of throwing an exception
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }
    }
}
