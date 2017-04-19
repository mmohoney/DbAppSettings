using DbAppSettings.Model.Service.Interfaces;

namespace DbAppSettings.Test.Mock
{
    internal class DummySettingCache : ISettingCache
    {
        public DummySettingCache()
        {
            Instance = this;
        }

        public ISettingCache Instance { get; }

        public void InitializeCache(ISettingInitialization cacheManager)
        {
            return;
        }
    }
}
