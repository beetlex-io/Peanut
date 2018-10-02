using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

using System.Collections;

namespace Peanut.Mappings
{
    class ProcDataReader
    {
        private static Hashtable mReaders = new Hashtable();
        private List<ReadProperty> mProperties = new List<ReadProperty>();

        public ProcDataReader(Type type)
        {
            foreach (PropertyInfo p in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                ReadProperty rp = new ReadProperty();
                rp.Handler = new PropertyHandler(p);
                rp.Cast = DBContext.GetCast(p.PropertyType);
                IDAttribute[] ida = Utils.GetPropertyAttributes<IDAttribute>(p, false);
                rp.Name = p.Name;
                if (ida.Length > 0)
                {
                    if (!string.IsNullOrEmpty(ida[0].Name))
                    {
                        rp.Name = ida[0].Name;
                    }
                }
                else
                {
                    ColumnAttribute[] cas = Utils.GetPropertyAttributes<ColumnAttribute>(p, false);
                    if (cas.Length > 0)
                    {
                        if (!string.IsNullOrEmpty(cas[0].Name))
                        {
                            rp.Name = cas[0].Name;
                        }
                    }
                    
                }
                mProperties.Add(rp);

            }
        }

        public Type Type
        {
            get;
            set;
        }

        private bool mLoadColumnIndex = false;

        public void ReaderToObject(System.Data.IDataReader reader, object obj)
        {
            if (!mLoadColumnIndex)
                SetColumnIndex(reader);
            for (int i = 0; i < mProperties.Count; i++)
            {
                ReaderToProperty(reader, obj, mProperties[i]);
            }

        }

        private void ReaderToProperty(System.Data.IDataReader reader, object obj, ReadProperty p)
        {
            object dbvalue=null;
            try
            {
                
                if (p.Index >= 0)
                {
                    
                    dbvalue = reader[p.Index];
                    if (dbvalue != DBNull.Value)
                    {
                        if (p.Cast != null)
                        {
                            dbvalue = p.Cast.ToProperty(dbvalue, p.Handler.Property.PropertyType, null);
                        }
                        p.Handler.Set(obj, Convert.ChangeType(dbvalue, p.Handler.Property.PropertyType));
                    }

                }
            }
            catch (Exception e_)
            {
                throw new PeanutException(string.Format(DataMsg.READER_TO_PROPERTY_ERROR, dbvalue, p.Handler.Property.Name), e_);
            }
        }

        private void SetColumnIndex(System.Data.IDataReader reader)
        {
            lock (this)
            {
                if (!mLoadColumnIndex)
                {
                    foreach (ReadProperty pm in mProperties)
                    {
                        try
                        {
                            pm.Index = reader.GetOrdinal(pm.Name);
                        }
                        catch
                        {
                        }
                    }
                    mLoadColumnIndex = true;
                }
            }
        }

        class ReadProperty
        {
            public ReadProperty()
            {
                Index = -1;
            }
            public string Name
            {
                get;
                set;
            }
            public PropertyCastAttribute Cast
            {
                get;
                set;
            }
            public PropertyHandler Handler
            {
                get;
                set;
            }
            public int Index
            {
                get;
                set;
            }
        }
        
        public static ProcDataReader GetDataReader(Type proc, Type type)
        {
            lock (mReaders)
            {
                Hashtable rh = (Hashtable)mReaders[proc];
                if (rh == null)
                {
                    rh = new Hashtable();
                    mReaders.Add(proc, rh);
                }
                ProcDataReader reader = (ProcDataReader)rh[type];
                if (reader == null)
                {
                    reader = new ProcDataReader(type);
                    rh.Add(type, reader);
                }
                return reader;
            }
        }
    }
}
