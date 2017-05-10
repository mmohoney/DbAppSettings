namespace DbAppSettings.Model.Domain
{
    /// <summary>
    /// Not ready yet
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TValueType"></typeparam>
    internal abstract class ImmutableDbAppSetting<T, TValueType> : DbAppSetting<T, TValueType> where T : DbAppSetting<T, TValueType>, new()
    {
        protected ImmutableDbAppSetting()
        {

        }
    }
}
