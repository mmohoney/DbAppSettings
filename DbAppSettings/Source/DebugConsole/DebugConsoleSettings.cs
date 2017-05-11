using DbAppSettings.Model.Domain;

namespace DebugConsole
{
    public class DebugConsoleSettings
    {
        public class EnableLogging : DbAppSetting<EnableLogging, bool> { public override bool InitialValue => false; }
    }
}
