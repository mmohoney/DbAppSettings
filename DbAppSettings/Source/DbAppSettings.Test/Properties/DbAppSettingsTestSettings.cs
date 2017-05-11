using DbAppSettings.Model.Domain;

namespace DbAppSettings.Test.Properties
{
    public class MySettingsClass
    {
        public int PropertyOne => 1;
        public string PropertyTwo => "Settings are fun.";
    }

    public class DbAppSettingsTestSettings
    {
        public class EnableLogging : DbAppSetting<EnableLogging, bool> { public override bool InitialValue => false; }
        public class MyCustomObjectSetting : DbAppSetting<MyCustomObjectSetting, MySettingsClass> { public override MySettingsClass InitialValue => new MySettingsClass(); }
    }
}

namespace DbAppSettings.MyAssembly
{
    public class MyAssemblySettings
    {
        public class EnableLogging : DbAppSetting<EnableLogging, bool> { public override bool InitialValue => false; }
        public class MyOtherSetting : DbAppSetting<MyOtherSetting, SettingsObject> { public override SettingsObject InitialValue => new SettingsObject(); }
    }

    public class SettingsObject
    {
        public int PropertyOne => 1;
        public string PropertyTwo => "String settings";

        public void DoSomeWork()
        {
            if (MyAssemblySettings.MyOtherSetting.Value.PropertyOne == 1)
            {
                //Do other work
            }
        }
    }
}
