using System;
using System.Linq.Expressions;
using DbAppSettings.Model.Service;

namespace DbAppSettings
{
    public static class DbAppSetting
    {
        public static TValueType GetValue<TValueType>(Expression<Func<TValueType>> expression)
        {
            if (expression == null )
                return default(TValueType);

            string assembly = ((MemberExpression) expression.Body)?.Member?.ReflectedType?.Assembly?.GetName()?.Name;
            if (string.IsNullOrWhiteSpace(assembly))
                return default(TValueType);

            string propertyName = ((MemberExpression)expression.Body).Member.Name;
            if (string.IsNullOrWhiteSpace(propertyName))
                return default(TValueType);

            string fullSettingName = $"{assembly}.{propertyName}";
            TValueType value = SettingCache.GetDbAppSettingValue<TValueType>(fullSettingName);
            return value;
        }
    }
}
