DbAppSettings
=======================

### Features
- Easy to use global application settings
- Realize value changes without app recycle/ restart
- Graceful fallback values if data layer cannot be accessed

### Download
Download the [latest release].

### Get it on NuGet
`Install-Package DbAppSettings.DbAppSettings`

### Documentation
Visit the [Wiki] for quick setup guide.

### Preview Example
Settings File:
```c#
namespace DbAppSettings.Test.Mock
{
    public class DbAppSettingsTestSettings
    {
        public class EnableLogging : DbAppSetting<EnableLogging, bool> { public override bool InitialValue => false; }
        public class MySecondSetting : DbAppSetting<MySecondSetting, int> { public override int InitialValue => 1; }
    }
}
```
Setting Usage:
```c#
if (DbAppSettingsTestSettings.EnableLogging.Value)
{
    MyLogger.Log("Test");
}
```

[latest release]: https://github.com/mmohoney/DbAppSettings/releases
[DbAppSettings.Test]: https://github.com/mmohoney/DbAppSettings/tree/master/DbAppSettings/Source/DbAppSettings.Test
[wiki]: https://github.com/mmohoney/DbAppSettings/wiki/Setup
