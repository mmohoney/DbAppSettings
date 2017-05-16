using System;
using System.Configuration;
using System.Linq.Expressions;
using System.Threading;
using DbAppSettings;
using DebugConsole.Properties;

namespace DebugConsole
{
    class Program
    {
        public static TV Get<T, TV>(T item, Func<T, IComparable> getProp) where T : ApplicationSettingsBase
        {
            return default(TV);
        }
         

        public static T GetValue<T>(Expression<Func<T>> expression)
        {
            string name = ((MemberExpression)expression.Body).Member.Name;
            T value = expression.Compile()();
            return value;
        }

        static void Main(string[] args)
        {

            bool boolSettign = GetValue(() => Settings.Default.BoolSettig);


            string result = Settings.Default.TestSetting;

            string get = Get<Settings, string>(Settings.Default, x => x.TestSetting);


            return;










            Console.WriteLine($"Setting default value: { new DebugConsoleSettings.EnableLogging().InitialValue }");

            //Intialize the cache and our DAL
            DbAppSettingCacheManager.CreateAndIntialize(new MyImplLazyLoadSettingDao());

            Console.WriteLine($"Setting value: { new DebugConsoleSettings.EnableLogging().InstanceValue }");

            //Update the DAL value and wait until the cache is refreshed
            MyImplLazyLoadSettingDao.Setting.Value = false.ToString();
            SpinWait.SpinUntil(() => !DebugConsoleSettings.EnableLogging.Value);

            Console.WriteLine($"Setting value: { new DebugConsoleSettings.EnableLogging().InstanceValue }");

            Console.WriteLine("Done...");
            Console.ReadLine();
        }
    }
}

/*
Setting default value: False
Setting value: True
Setting value: False
Done...
*/
