using System;
using System.Collections.Generic;
using System.Text;

namespace Peanut.Mappings
{
    [AttributeUsage(AttributeTargets.Class| AttributeTargets.Interface)]
    public class ConnectionAttribute:Attribute
    {
        public ConnectionAttribute(DB type)
        {
            Type = type;
        }

        public DB Type
        {
            get;
            set;
        }

        public static IConnectinContext GetContext(DB type)
        {
            return DBContext.GetConnection(type);
        }

        public IConnectinContext GetContext()
        {
            return GetContext(Type);
        }
    }
    
}
