DbAppSettings
=======================

<a href="http://mattshomedomain.ddns.net:47850/viewType.html?buildTypeId=DbAppSettings_Build&guest=1">
<img src="http://mattshomedomain.ddns.net:47850/app/rest/builds/buildType:(id:DbAppSettings_Build)/statusIcon"/>
</a>

[![Build Status](https://travis-ci.org/mmohoney/DbAppSettings.svg?branch=master)](https://travis-ci.org/mmohoney/DbAppSettings)

### Features
- Easy to use global application settings
- Realize value changes without app recycle/ restart
- Graceful fallback values if data layer cannot be accessed

### Download
Download the [latest release].

### Get it on NuGet
`Install-Package DbAppSettings.DbAppSettings`

### Documentation/ Release Notes
Visit the [Wiki] for quick setup guide.

### Example
Settings File:
```c#
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
    }
}
```
Setting Usage:
```c#
public void DoSomeWork()
{
    if (MyAssemblySettings.MyOtherSetting.Value.PropertyOne == 1)
    {
        //Do other work
    }

    if (MyAssemblySettings.EnableLogging.Value)
    {
        //Log some statements
    }
}
```

[latest release]: https://github.com/mmohoney/DbAppSettings/releases
[DbAppSettings.Test]: https://github.com/mmohoney/DbAppSettings/tree/master/DbAppSettings/Source/DbAppSettings.Test
[wiki]: https://github.com/mmohoney/DbAppSettings/wiki
