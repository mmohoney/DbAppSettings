using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbAppSettings.Model.Domain
{
    public abstract class ImmutableDbAppSetting<T, TValueType> : DbAppSetting<T, TValueType> where T : DbAppSetting<T, TValueType>, new()
    {
        protected ImmutableDbAppSetting()
        {

        }


    }
}
