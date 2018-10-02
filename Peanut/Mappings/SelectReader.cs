using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Peanut.Mappings
{
    class SelectDataReader
    {
        public SelectDataReader()
        {
            Proxy = false;
        }
        public string Select
        {
            get;
            set;
        }
        public string Group
        {
            get;
            set;
        }
        public bool Proxy
        {
            get;
            set;
        }
        private IList<ReadProperty> mProperties = new List<ReadProperty>();
        public IList<ReadProperty> Properties
        {
            get
            {
                return mProperties;
            }

        }
        public void AddProperty(PropertyMapper pm)
        {
            ReadProperty rp = new ReadProperty();
            rp.Mapper = pm;
            rp.Index = 0;
            mProperties.Add(rp);
        }
        private IList<PropertyHandler> mProxyProperties = new List<PropertyHandler>();
        public IList<PropertyHandler> ProxyProperties
        {
            get
            {
                return mProxyProperties;
            }
        }
        private bool mLoadColumnIndex = false;
        public void ReaderToObject(System.Data.IDataReader reader, object obj)
        {
            if (!mLoadColumnIndex)
                SetColumnIndex(reader);
            for (int i = 0; i < Properties.Count; i++)
            {
                ReadProperty rp = Properties[i];
                if(!Proxy)
                    ReaderToProperty(reader, obj,rp, rp.Mapper,rp.Mapper.Handler);
                else
                    ReaderToProperty(reader, obj,rp,rp.Mapper, ProxyProperties[i]);
            }
            
        }   
        private void ReaderToProperty(System.Data.IDataReader reader, object obj, ReadProperty rp, PropertyMapper pm,PropertyHandler handler)
        {
            try
            {
                object dbvalue = reader[rp.Index];
                if (dbvalue != DBNull.Value)
                {
                    if (pm.Cast != null)
                    {
                        dbvalue = pm.Cast.ToProperty(dbvalue, pm.Handler.Property.PropertyType, obj);
                    }
                    handler.Set(obj, Convert.ChangeType(dbvalue, pm.Handler.Property.PropertyType));
                }
            }
            catch (Exception e_)
            {
                throw new PeanutException(string.Format(DataMsg.READER_TO_PROPERTY_ERROR, pm.ColumnName, pm.Handler.Property.Name), e_);
            }
        }
        private void SetColumnIndex(System.Data.IDataReader reader)
        {
            lock (this)
            {
                if (!mLoadColumnIndex)
                {
                    foreach (ReadProperty rp in Properties)
                    {
                        try
                        {
                            rp.Index = reader.GetOrdinal("p_" + rp.Mapper.Handler.Property.Name);
                        }
                        catch (Exception e_)
                        {
                            throw new PeanutException(string.Format(DataMsg.READER_COLUMN_NOFOUND, rp.Mapper.ColumnName), e_);
                        }
                    }
                    mLoadColumnIndex = true;
                }
            }
        }
        public class ReadProperty
        {
            public PropertyMapper Mapper;
            public int Index;
        }
    }
}
