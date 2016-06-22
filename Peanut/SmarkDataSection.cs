using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
namespace Peanut
{
   public class PeanutSection : ConfigurationSection

    {
        public PeanutSection()
        {
            mConnections = new ConnectionCollection();
            mAssemblies = new AssemblyCollection();
        }
        [ConfigurationProperty("initHandler")]
        public string InitHandler
        {
            get
            {
                return (string)this["initHandler"];
            }
            set
            {
                this["initHandler"] = value;
            }
        }
        public PeanutSection(int connections)
        {
            mConnections = (ConnectionCollection)base["Connection"];
            
            for (int i = 0; i < connections; i++)
            {
                mConnections.Add(new ConnectionElement(i.ToString(), "Peanut.MSSQL,Peanut", ""));
            }
        }
        private ConnectionCollection mConnections;
        [ConfigurationProperty("Connection", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ConnectionCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public ConnectionCollection Connections
        {

            get
            {
                 mConnections =
                (ConnectionCollection)base["Connection"];
                return mConnections;
            }
        }
        private AssemblyCollection mAssemblies;
        [ConfigurationProperty("Assembly", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(AssemblyCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public AssemblyCollection Assemblies
        {
            get
            {
                mAssemblies =
               (AssemblyCollection)base["Assembly"];
                return mAssemblies;
            }
        }
    }
   public class ConnectionCollection : ConfigurationElementCollection
    {
        public ConnectionCollection()
        {
           
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ConnectionElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((ConnectionElement)element).Name;
        }

        public ConnectionElement this[int index]
        {
            get
            {
                return (ConnectionElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        new public ConnectionElement this[string Name]
        {
            get
            {
                return (ConnectionElement)BaseGet(Name);
            }
        }

        public int IndexOf(ConnectionElement url)
        {
            return BaseIndexOf(url);
        }

        public void Add(ConnectionElement url)
        {
            BaseAdd(url);
        }
        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(ConnectionElement url)
        {
            if (BaseIndexOf(url) >= 0)
                BaseRemove(url.Name);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void Clear()
        {
            BaseClear();
        }
        
    }
    public class ConnectionElement : ConfigurationElement
    {
        public ConnectionElement(String name, string type, String connectionstring)
        {
            this.Name = name;
            this.ConnectionString = connectionstring;
            this.Type = type;
        }

        public ConnectionElement()
        {
            
        }
        [ConfigurationProperty("encrypt")]
        public string Encrypt
        {
            get
            {
                return (string)this["encrypt"];
            }
            set
            {
                this["encrypt"] = value;
            }
        }
        [ConfigurationProperty("name",IsRequired = true, IsKey = true)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("type",IsRequired = true)]
        public string Type
        {
            get
            {
                return (string)this["type"];
            }
            set
            {
                this["type"] = value;
            }
        }
        public string mConnectionString;
        [ConfigurationProperty("connectionstring",IsRequired = false)]
        public string ConnectionString
        {
            get
            {
                if (mConnectionString == null)
                {
                    string conn = (string)this["connectionstring"];
                    bool isencrypt = false;
                    if (bool.TryParse(Encrypt, out isencrypt))
                    {
                        if (isencrypt)
                            return DBContext.Blowfish.Decrypt_ECB(conn);
                    }
                    
                    return conn;
                    
                    
                }
                else
                    return mConnectionString;
            }
            set
            {
                this["connectionstring"] = value;
            }
        }
        public void SetConnection(string connectionstring)
        {
            mConnectionString = connectionstring;
        }
        private Type mDriverType = null;
        public IDriver Driver
        {
            get
            {
                if (string.IsNullOrEmpty(Type))
                    return null;
                if (mDriverType == null)
                    mDriverType = System.Type.GetType(Type, true);
                return (IDriver)Activator.CreateInstance(mDriverType);



            }
        }
        public void SetDriver<T>() where T : IDriver
        {
            mDriverType = typeof(T);
        }
    }
    public class AssemblyElement : ConfigurationElement
    {
        public AssemblyElement( string type)
        {
            this.Type = type;
        }
        public AssemblyElement()
        {
            
        }
        [ConfigurationProperty("type", IsRequired = true, IsKey=true)]

        public string Type
        {
            get
            {
                return (string)this["type"];
            }
            set
            {
                this["type"] = value;
            }
        }
    }
    public class AssemblyCollection : ConfigurationElementCollection
    {
        public AssemblyCollection()
        {
           
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new AssemblyElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((AssemblyElement)element).Type;
        }

        public AssemblyElement this[int index]
        {
            get
            {
                return (AssemblyElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        new public AssemblyElement this[string Name]
        {
            get
            {
                return (AssemblyElement)BaseGet(Name);
            }
        }

        public int IndexOf(AssemblyElement url)
        {
            return BaseIndexOf(url);
        }

        public void Add(AssemblyElement url)
        {
            BaseAdd(url);
        }
        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(AssemblyElement url)
        {
            if (BaseIndexOf(url) >= 0)
                BaseRemove(url.Type);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void Clear()
        {
            BaseClear();
        }

    }
}
