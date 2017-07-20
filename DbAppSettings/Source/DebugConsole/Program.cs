using System;
using System.Linq;
using System.Reflection;
using DbAppSettings;
using DbAppSettings.Model.DataTransfer;
using DbAppSettings.Model.Domain;
using DbAppSettings.Model.Service.CacheManager.Arguments;
using DebugConsole.Properties;

namespace DebugConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Assembly.Load("WebDbAppSettingsMaintenance");

            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(t => t.Namespace == "WebDbAppSettingsMaintenance.Service.Maintenance.Demo")
                .Where(t => t.IsClass)
                .ToList();

            foreach (Type type in types)
            {

                    var newType = Activator.CreateInstance(type);
                InternalDbAppSettingBase baseTest = newType as InternalDbAppSettingBase;
                if (baseTest != null)
                {
                    DbAppSettingDto dto = baseTest.ToDto();

                }
            }


            DbAppSettingCacheManager.CreateAndIntialize(new LazyLoadManagerArguments
            {
                LazyLoadSettingDao = new MySettingClassLazyLoadSettingDao(),
                SaveNewSettingDao = new MySettingClassSaveNewSettingDao()
            });

            bool initialValue = DbAppSetting.GetValue(() => Settings.Default.BoolSetting);

            MySettingClassLazyLoadSettingDao.CachedKeys.First().Value.Value = "False";

            bool secondValue = DbAppSetting.GetValue(() => Settings.Default.BoolSetting);
        }
    }

    //class Program
    //{
    //    static void Main(string[] args)
    //    {
    //        Console.WriteLine($"Setting default value: { new DebugConsoleSettings.EnableLogging().InitialValue }");

    //        //Intialize the cache and our DAL
    //        DbAppSettingCacheManager.CreateAndIntialize(new MyImplLazyLoadSettingDao());

    //        Console.WriteLine($"Setting value: { new DebugConsoleSettings.EnableLogging().InstanceValue }");

    //        //Update the DAL value and wait until the cache is refreshed
    //        MyImplLazyLoadSettingDao.Setting.Value = false.ToString();
    //        SpinWait.SpinUntil(() => !DebugConsoleSettings.EnableLogging.Value);

    //        Console.WriteLine($"Setting value: { new DebugConsoleSettings.EnableLogging().InstanceValue }");

    //        Console.WriteLine("Done...");
    //        Console.ReadLine();

    //        /*
    //        Setting default value: False
    //        Setting value: True
    //        Setting value: False
    //        Done...
    //        */
    //    }
    //}
}
