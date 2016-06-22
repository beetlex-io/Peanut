using System;
using System.Collections.Generic;
using System.Text;

namespace Peanut.Mappings
{
    public class TableAttribute:Attribute
    {
        public TableAttribute()
        {
            DISTINCT = false;
            DefaultConnection = DB.DB1;
        }
        public TableAttribute(string name)
        {
            Name = name;
        }
       
        public string Name
        {
            get;
            set;
        }
        public bool DISTINCT
        {
            get;
            set;
        }
        public DB DefaultConnection
        {
            get;
            set;
        }
        
    }
}
