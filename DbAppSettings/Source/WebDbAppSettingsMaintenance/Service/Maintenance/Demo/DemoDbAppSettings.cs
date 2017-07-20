using System;
using System.Collections.Specialized;
using DbAppSettings.Model.Domain;

namespace WebDbAppSettingsMaintenance.Service.Maintenance.Demo
{
    internal class DemoDbAppSettings
    {
        public class DemoDbAppSettingBool : DbAppSetting<DemoDbAppSettingBool, bool> { public override bool InitialValue => true; }
        public class DemoDbAppSettingByte : DbAppSetting<DemoDbAppSettingByte, byte> { public override byte InitialValue => 255; }
        public class DemoDbAppSettingChar : DbAppSetting<DemoDbAppSettingChar, char> { public override char InitialValue => 'A'; }
        public class DemoDbAppSettingDecimal : DbAppSetting<DemoDbAppSettingDecimal, decimal> { public override decimal InitialValue => 1.234M; }
        public class DemoDbAppSettingDouble : DbAppSetting<DemoDbAppSettingDouble, double> { public override double InitialValue => 1.234; }
        public class DemoDbAppSettingFloat : DbAppSetting<DemoDbAppSettingFloat, float> { public override float InitialValue => 1.234F; }
        public class DemoDbAppSettingInt : DbAppSetting<DemoDbAppSettingInt, int> { public override int InitialValue => 123; }
        public class DemoDbAppSettingLong : DbAppSetting<DemoDbAppSettingLong, long> { public override long InitialValue => 12345678910; }
        public class DemoDbAppSettingSByte : DbAppSetting<DemoDbAppSettingSByte, sbyte> { public override sbyte InitialValue => 127; }
        public class DemoDbAppSettingShort : DbAppSetting<DemoDbAppSettingShort, short> { public override short InitialValue => 56; }
        public class DemoDbAppSettingString : DbAppSetting<DemoDbAppSettingString, string> { public override string InitialValue => "Demo Test String"; }
        public class DemoDbAppSettingStringCollection : DbAppSetting<DemoDbAppSettingStringCollection, StringCollection> { public override StringCollection InitialValue => new StringCollection { "String 1" }; }
        public class DemoDbAppSettingDateTime : DbAppSetting<DemoDbAppSettingDateTime, DateTime> { public override DateTime InitialValue => new DateTime(2017, 01, 01, 12, 30, 0); }
        public class DemoDbAppSettingGuid : DbAppSetting<DemoDbAppSettingGuid, Guid> { public override Guid InitialValue => new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"); }
        public class DemoDbAppSettingTimeSpan : DbAppSetting<DemoDbAppSettingTimeSpan, TimeSpan> { public override TimeSpan InitialValue => new TimeSpan(00, 01, 15, 0); }
        public class DemoDbAppSettingUInt : DbAppSetting<DemoDbAppSettingUInt, uint> { public override uint InitialValue => 9009; }
        public class DemoDbAppSettingULong : DbAppSetting<DemoDbAppSettingULong, ulong> { public override ulong InitialValue => 90090009; }
        public class DemoDbAppSettingUShort : DbAppSetting<DemoDbAppSettingUShort, ushort> { public override ushort InitialValue => 30; }

        //public class DemoSecondAppDbAppSettingBool : DbAppSetting<DemoSecondAppDbAppSettingBool, bool> { public override bool InitialValue => true; }
        //public class DemoSecondDbAppSettingInt : DbAppSetting<DemoSecondDbAppSettingInt, int> { public override int InitialValue => 100; }
        //public class DemoSecondDbAppSettingString : DbAppSetting<DemoSecondDbAppSettingString, string> { public override string InitialValue => "Demo Test String"; }

        //public class DemoAnotherAppDbAppSettingBool : DbAppSetting<DemoAnotherAppDbAppSettingBool, bool> { public override bool InitialValue => true; }
        //public class DemoAnotherAppDbAppSettingByte : DbAppSetting<DemoAnotherAppDbAppSettingByte, byte> { public override byte InitialValue => 255; }
        //public class DemoAnotherAppDbAppSettingChar : DbAppSetting<DemoAnotherAppDbAppSettingChar, char> { public override char InitialValue => 'A'; }
        //public class DemoAnotherAppDbAppSettingDecimal : DbAppSetting<DemoAnotherAppDbAppSettingDecimal, decimal> { public override decimal InitialValue => 1.5M; }

        //public class DemoAnotherAppDbAppSettingInt : DbAppSetting<DemoAnotherAppDbAppSettingInt, int> { public override int InitialValue => 100; }
        //public class DemoAnotherAppDbAppSettingString : DbAppSetting<DemoAnotherAppDbAppSettingString, string> { public override string InitialValue => "Demo Test String"; }
    }
}
