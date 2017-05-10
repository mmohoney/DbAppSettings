using DbAppSettings.Model.DataAccess.Interfaces;
using DbAppSettings.Model.DataTransfer;

namespace DbAppSettings.Model.DataAccess.Implementations
{
    /// <summary>
    /// Default implementation if no implementation is provided by the caller
    /// </summary>
    internal class DefaultSaveNewSettingDao : ISaveNewSettingDao
    {
        public void SaveNewSettingIfNotExists(DbAppSettingDto dbAppSettingDto)
        {
            return;
        }
    }
}
