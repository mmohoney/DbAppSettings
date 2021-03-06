﻿using DbAppSettings.Model.Domain;

namespace DbAppSettings.Maintenance.Model.DataAccess.Maintenance.Demo
{
    internal class DemoDbAppSettings
    {
        public class DemoDbAppSettingBool : DbAppSetting<DemoDbAppSettingBool, bool> { public override bool InitialValue => true; }
        public class DemoDbAppSettingInt : DbAppSetting<DemoDbAppSettingInt, int> { public override int InitialValue => 100; }
        public class DemoDbAppSettingString : DbAppSetting<DemoDbAppSettingString, string> { public override string InitialValue => "Demo Test String"; }

        public class DemoSecondAppDbAppSettingBool : DbAppSetting<DemoSecondAppDbAppSettingBool, bool> { public override bool InitialValue => true; }
        public class DemoSecondDbAppSettingInt : DbAppSetting<DemoSecondDbAppSettingInt, int> { public override int InitialValue => 100; }
        public class DemoSecondDbAppSettingString : DbAppSetting<DemoSecondDbAppSettingString, string> { public override string InitialValue => "Demo Test String"; }

        public class DemoAnotherAppDbAppSettingBool : DbAppSetting<DemoAnotherAppDbAppSettingBool, bool> { public override bool InitialValue => true; }
        public class DemoAnotherAppDbAppSettingByte : DbAppSetting<DemoAnotherAppDbAppSettingByte, byte> { public override byte InitialValue => 255; }
        public class DemoAnotherAppDbAppSettingChar : DbAppSetting<DemoAnotherAppDbAppSettingChar, char> { public override char InitialValue => 'A'; }
        public class DemoAnotherAppDbAppSettingDecimal : DbAppSetting<DemoAnotherAppDbAppSettingDecimal, decimal> { public override decimal InitialValue => 1.5M; }

        public class DemoAnotherAppDbAppSettingInt : DbAppSetting<DemoAnotherAppDbAppSettingInt, int> { public override int InitialValue => 100; }
        public class DemoAnotherAppDbAppSettingString : DbAppSetting<DemoAnotherAppDbAppSettingString, string> { public override string InitialValue => "Demo Test String"; }
    }
}
