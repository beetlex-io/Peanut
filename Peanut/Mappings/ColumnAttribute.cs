using System;
using System.Collections.Generic;
using System.Text;

namespace Peanut.Mappings
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute:Attribute
    {

        public ColumnAttribute()
        {
        }
        public ColumnAttribute(string name)
        {
            Name = name;
        }
    
        public string Name
        {
            get;
            set;
        }
        
    }
}
