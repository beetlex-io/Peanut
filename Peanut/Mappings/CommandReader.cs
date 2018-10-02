using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;

namespace Peanut.Mappings
{
    class CommandReader
    {
        private static Dictionary<string, Dictionary<Type, CommandReader>> mCommandReaders = new Dictionary<string, Dictionary<Type, CommandReader>>(512);

        private IList<ReadProperty> mProperties = new List<ReadProperty>();

        public static CommandReader GetReader(string key, Type type)
        {
            CommandReader reader;
            Dictionary<Type, CommandReader> sqlreaders = GetSqlReaders(key);
            if (!sqlreaders.TryGetValue(type, out reader))
            {
                lock (mCommandReaders)
                {
                    if (!sqlreaders.TryGetValue(type, out reader))
                    {
                        reader = new CommandReader(type);
                        sqlreaders.Add(type, reader);
                    }
                }
            }
            return reader;
        }

        private static Dictionary<Type, CommandReader> GetSqlReaders(string key)
        {
            Dictionary<Type, CommandReader> result;
            if (!mCommandReaders.TryGetValue(key, out result))
            {
                lock (mCommandReaders)
                {
                    if (!mCommandReaders.TryGetValue(key, out result))
                    {
                        result = new Dictionary<Type, CommandReader>(8);
                        mCommandReaders.Add(key, result);
                    }
                }
            }
            return result;
        }

        public CommandReader(Type type)
        {
            foreach (PropertyInfo info in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                mProperties.Add(new ReadProperty() { Handler = new PropertyHandler(info), Name = info.Name, Cast = DBContext.GetCast(info.PropertyType) });
            }
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
            object dbvalue = null;
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
            public Mappings.PropertyCastAttribute Cast
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


    }
}
