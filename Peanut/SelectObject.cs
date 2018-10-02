using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Xml.Serialization;

namespace Peanut
{

    [Serializable]
    public class XMLNameValueCollection : System.Collections.Specialized.NameValueCollection, System.Xml.Serialization.IXmlSerializable
    {
        #region IXmlSerializable 成员

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            XmlSerializer keySerializer = Utils.GetXmlSerializer(typeof(string), null);
            XmlSerializer valueSerializer = Utils.GetXmlSerializer(typeof(string), null);
            string KeyValue;
            string value;


            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty)
                return;
            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                reader.ReadStartElement("item");
                reader.ReadStartElement("key");
                KeyValue = (string)keySerializer.Deserialize(reader);
                reader.ReadEndElement();
                reader.ReadStartElement("value");
                value = (string)valueSerializer.Deserialize(reader);
                reader.ReadEndElement();
                this.Add(KeyValue, value);
                reader.ReadEndElement();
                reader.MoveToContent();

            }
            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            string value;
            XmlSerializer keySerializer = Utils.GetXmlSerializer(typeof(string), null);
            XmlSerializer valueSerializer = Utils.GetXmlSerializer(typeof(string), null);
            foreach (string key in Keys)
            {
                value = this[key];
                if (value != null)
                {
                    writer.WriteStartElement("item");

                    writer.WriteStartElement("key");
                    keySerializer.Serialize(writer, key);
                    writer.WriteEndElement();
                    writer.WriteStartElement("value");
                    valueSerializer.Serialize(writer, value);
                    writer.WriteEndElement();

                    writer.WriteEndElement();
                }
            }
        }

        #endregion
    }

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
