DbAppSettings
=======================

#### Features

- Easy to use global application settings
- Realize value changes without app recycle/ restart
- Graceful fallback values if data layer cannot be accessed
- Data Access layer polled by default every 5 seconds

#### Download
Download the [latest release].

### Documentation
For complete examples and unit tests visit [DbAppSettings.Test]. 

#### Create the settings class
To keep it consistent with the .Net Settings.settings file follow these general rules
1. Create a class to house all the assembly's settings 
2. Place this in the assembly's Properties folder. 

Example:
```c#
namespace DbAppSettings.Test.Mock
{
    public class DbAppSettingsTestSettings
    {
    
    }
}
```

#### Create settings (`DbAppSetting`)
Each setting must extend the class `DbAppSetting<T, TValueType>`. This base class allows settings to realize updates from the data access layer through the property `Value`. 
- Note: `Value` will always provide the `InitialValue` if a value cannot be found in the data access layer.

Example:
```c#
public class EnableLogging : DbAppSetting<EnableLogging, bool> { public override bool InitialValue => false; }
```

- `EnableLogging` extends the base class `DbAppSetting<T, TValueType>` where `T` is itself an `TValueType` is the type of the provided value.
- `InitialValue` is an abstract value that must be implemeted. This always provides a fallback value to the caller if an up to date value cannot be provided from the data access layer. 
- All settings should be housed in the class created in the above step

Example:
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

#### Provide the implemenation for the data access layer
The public interface `IDbAppSettingDao` must be implemented in order to retrieve values from the data access layer. Implementation is left up to the caller. If no implementation is provided, a default implementation will be kicked back always providing the setting with the `InitialValue`.

```c#
/// <summary>
/// Represents the data access portion of the settings cache. Needs to be implemented for data access
/// </summary>
public interface IDbAppSettingDao
{
    /// <summary>
    /// Returns all settings from the database. Initially used to load all settings into the cache
    /// </summary>
    /// <returns>all settings</returns>
    IEnumerable<DbAppSettingDto> GetAllDbAppSettings();

    /// <summary>
    /// Returns all settings from the database that have changed since the last time a value was retrieved
    /// </summary>
    /// <param name="latestDbAppSettingChangedDate"></param>
    /// <returns>all settings the have changed</returns>
    IEnumerable<DbAppSettingDto> GetChangedDbAppSettings(DateTime? latestDbAppSettingChangedDate);
}
```

#### Provide the initialization parameters
Lastly in order to setup the inner workings of the settings class, a cache manager intialization must be provided. This can be as simple as passing in the implementation for the interface `IDbAppSettingDao` above by calling `DbAppSettingCacheManager.CreateAndIntialize`.

Example:
```c#
DbAppSettingCacheManager.CreateAndIntialize(new DummyDbAppSettingDao());
```

#### Use settings
Setup has been complete and values will be provided for each setting.
To retreive the value use the following:

`DbAppSettingsTestSettings.EnableLogging.Value` This will provide the up to date value (if available) from the data access layer OR the default value passed in `InitialValue`.

Example:
```c#
if (DbAppSettingsTestSettings.EnableLogging.Value)
{
    MyLogger.Log("Test");
}
```

#### Detailed `DbAppSettingCacheManager` arguments

[latest release]: https://github.com/mmohoney/DbAppSettings/releases
[DbAppSettings.Test]: https://github.com/mmohoney/DbAppSettings/tree/master/DbAppSettings/Source/DbAppSettings.Test
