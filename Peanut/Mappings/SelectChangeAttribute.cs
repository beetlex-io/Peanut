using System;
using System.Collections.Generic;

using System.Text;

namespace Peanut.Mappings
{
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class SelectChangeAttribute:Attribute
    {
        public abstract void Change(Command cmd);
    }
}
