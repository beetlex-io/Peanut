using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using IKende.com.core;
namespace Peanut
{
    [Serializable]
    public class SelectObject
    {
        private XMLNameValueCollection mProperties = new XMLNameValueCollection();
        public XMLNameValueCollection Properties
        {
            get
            {
                return mProperties;
            }
            set
            {
                mProperties = value;
            }
        }
        public object this[string name]
        {
            get
            {
                return Properties[name];
            }
        }
        public object this[int column]
        {
            get
            {
                return Properties[column];
            }
        }
        public T Value<T>(string name)
        {
            object value = this[name];
            if (value == null || value == DBNull.Value)
                return default(T);
            return (T)Convert.ChangeType(value, typeof(T));
            
        }
        public T Value<T>(int column)
        {
            object value = this[column];
            if (value == null || value == DBNull.Value)
                return default(T);
            return (T)Convert.ChangeType(value, typeof(T));

        }
    }


    class SelectObjectMap
    {
        public SelectObjectMap(object obj)
        {
            foreach (PropertyInfo pi in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                Columns.Add(pi.Name);
            }
        }
        private List<string> mColumns = new List<string>();
        private Type mSelectType =null;
        public List<string> Columns
        {
            get
            {
                return mColumns;
            }
        }
        public Type SelectType
        {
            get
            {
                return mSelectType;
            }
        }
        private static Dictionary<Type, SelectObjectMap> mSelectObjectMapTable = new Dictionary<Type, SelectObjectMap>();
        public static SelectObjectMap Get(object obj)
        {
            Type type = obj.GetType();
            if (!mSelectObjectMapTable.ContainsKey(type))
            {
                lock (mSelectObjectMapTable)
                {
                    if (!mSelectObjectMapTable.ContainsKey(type))
                    {
                        SelectObjectMap som = new SelectObjectMap(obj);
                        mSelectObjectMapTable.Add(type, som);
                    }
                }
            }
            return mSelectObjectMapTable[type];

        }
    }
}
