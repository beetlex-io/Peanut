using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using IKende.com.core;
namespace Peanut
{
    /// <summary>
    /// 数据库连接上下文
    /// </summary>
    class ConnectionContext : IConnectinContext
    {
        public ConnectionContext(string db, IDriver driver, DB type)
        {
            TimeWatch.________________________________________________________("Peanut->Create IConnection Context");
            mDB = db;
            mHandler = DBContext.CurrentHandler(db);
            if (mHandler == null)
            {
                mHandler = DBContext.AddConnectionHandler(mDB, driver);
                mActiveConnection = true;
            }
            Type = type;
            TimeWatch.________________________________________________________();
        }

        public int ExecuteNonQuery(Command cmd)
        {
            TimeWatch.________________________________________________________("Peanut->Execute NonQuery");
            int value= mHandler.ExecuteNonQuery(cmd);
            TimeWatch.________________________________________________________();
            return value;
        }
        public IDataReader ExecuteReader(Command cmd)
        {
            TimeWatch.________________________________________________________("Peanut->Execute Reader");
            IDataReader result= mHandler.ExecuteReader(cmd);
            TimeWatch.________________________________________________________();
            return result;
        }

        public object ExecuteScalar(Command cmd)
        {
            TimeWatch.________________________________________________________("Peanut->Execute Scalar");
            object result= mHandler.ExecuteScalar(cmd);
            TimeWatch.________________________________________________________();
            return result;

        }

        public object ExecProc(object parameter)
        {
            TimeWatch.________________________________________________________("Peanut->Execute Proce Return Object");
            object result= mHandler.ExecProc(parameter);
            TimeWatch.________________________________________________________();
            return result;
        }

        public IList<T> ListProc<T>(object parameter) where T : new()
        {    
            IList<T> result= (IList<T>)ListProc(typeof(T), parameter);         
            return result;
            
        }

        public IList ListProc(Type entity, object parameter)
        {
            TimeWatch.________________________________________________________("Peanut->Execute Proce Return List");
            System.Type itemstype = System.Type.GetType("System.Collections.Generic.List`1");
            itemstype = itemstype.MakeGenericType(entity);
            IList result = (IList)Activator.CreateInstance(itemstype);
            Mappings.ProcDataReader dr = Mappings.ProcDataReader.GetDataReader(parameter.GetType(), entity);
            using (IDataReader reader = mHandler.ExecProcReader(parameter))
            {
                while (reader.Read())
                {
                    object item = Activator.CreateInstance(entity);
                    dr.ReaderToObject(reader, item);
                    result.Add(item);
                }
            }
            TimeWatch.________________________________________________________();
            return result;
        }

        public DataSet ExecuteDataSet(Command cmd)
        {
            return mHandler.ExecuteDataSet(cmd);
        }
        private string mDB;
        private bool mActiveConnection = false;
        private bool mActiveTransaction = false;
        ConnectionHandler mHandler;

        #region IDisposable 成员

        public void Dispose()
        {
            if (mActiveTransaction)
                mHandler.Rollback();
            if (mActiveConnection)
            {
                mHandler.Dispose();
                DBContext.RemoveConnetionHandler(mDB);
            }
        }

        #endregion

        #region IDbTransaction 成员
        public void BeginTransaction(IsolationLevel level)
        {
            if (mHandler.Transaction == null)
            {
                mHandler.BeginTransaction(level);
                mActiveTransaction = true;

            }
        }
        public void BeginTransaction()
        {
            if (mHandler.Transaction == null)
            {
                mHandler.BeginTransaction();
                mActiveTransaction = true;

            }
        }
        public void Commit()
        {
            if (mActiveTransaction)
            {
                mHandler.Commit();
            }
        }

        public IDbConnection Connection
        {
            get
            {
                return mHandler.Connection;
            }
        }

        public IsolationLevel IsolationLevel
        {
            get { return mHandler.IsolationLevel; }
        }



        #endregion

        #region IDbTransaction 成员


        public void Rollback()
        {
            if (mActiveTransaction)
            {
                mHandler.Rollback();
            };
        }

        #endregion

        #region IConnectinContext 成员


        public IList<T> List<T>(Command cmd, Region region) where T : new()
        {

            return (IList<T>)List(typeof(T), cmd, region);
        }

        public T Load<T>(Command cmd) where T : IEntityState, new()
        {

            return (T)Load(typeof(T), cmd);

        }

        #endregion

        #region IConnectinContext 成员


        public T ListFirst<T>(Command cmd) where T : new()
        {


            return (T)ListFirst(typeof(T), cmd);
        }

        #endregion

        #region IConnectinContext 成员


        public DB Type
        {
            get;
            set;
        }

        #endregion

        #region IConnectinContext 成员


        public IList List(Type type, Command cmd, Region region)
        {
            TimeWatch.________________________________________________________("Peanut->Execute SQL Return List");
            if (region == null)
                region = new Region { Start = 0, Size = 99999999 };
            IList items = null;
            if (Mappings.ObjectMapper.CurrentOM.SelectChange != null)
                Mappings.ObjectMapper.CurrentOM.SelectChange.Change(cmd);
            System.Type itemstype = System.Type.GetType("System.Collections.Generic.List`1");
            itemstype = itemstype.MakeGenericType(type);
            if (region.Size > DBContext.DefaultListMaxSize)
                items = (IList)Activator.CreateInstance(itemstype, DBContext.DefaultListMaxSize);
            else
                items = (IList)Activator.CreateInstance(itemstype, region.Size);
            object item = null;
            using (IDataReader reader = ExecuteReader(cmd))
            {
                int index = 0;
                while (reader.Read())
                {
                    if (index >= region.Start)
                    {
                        item = Activator.CreateInstance(type);

                        if (item is IEntityState)
                        {
                            
                            ((IEntityState)item).ConnectionType = this.Type;
                            ((IEntityState)item).LoadData(reader);
                        }
                        else
                        {
                            Mappings.ObjectMapper.CurrentSelectReader.ReaderToObject(reader, item);
                        }
                        items.Add(item);
                        if (items.Count == region.Size)
                        {
                            cmd.DbCommand.Cancel();
                            reader.Close();
                            break;
                        }
                    }
                    index++;
                }
            }
            TimeWatch.________________________________________________________();
            return items;
        }

        public object ListFirst(Type type, Command cmd)
        {
            IList items = List(type, cmd, new Region(0, 2));
            if (items.Count > 0)
                return items[0];
            return null;
        }

        public object Load(Type type, Command cmd)
        {
            IList items = List(type, cmd, null);
            if (items.Count > 0)
                return items[0];
            return null;
        }

        #endregion

        #region GetValue
        public T GetValue<T>(Command cmd)
        {
            IList<T> items = GetValues<T>(cmd, new Region(0, 2));
            if (items.Count == 0)
                return default(T);
            return items[0];
        }
        public IList<T> GetValues<T>(Command cmd)
        {
            return GetValues<T>(cmd, null);
        }
        public IList<T> GetValues<T>(Command cmd, Region region)
        {
            TimeWatch.________________________________________________________("Peanut->Execute SQL Return Values");
            List<T> result = null;
            object value;
            if (region == null)
            {
                region = new Region(0, 9999999);
            }
            if (region.Size > DBContext.DefaultListMaxSize)
                result = new List<T>(DBContext.DefaultListMaxSize);
            else
                result = new List<T>(region.Size);
            using (IDataReader reader = ExecuteReader(cmd))
            {
                int index = 0;
                while (reader.Read())
                {
                    if (index >= region.Start)
                    {
                        value = reader[0];
                        if (value != DBNull.Value)
                        {
                            result.Add((T)Convert.ChangeType(value, typeof(T)));
                        }
                        else
                        {
                            result.Add(default(T));
                        }
                        if (result.Count == region.Size)
                        {
                            cmd.DbCommand.Cancel();
                            reader.Close();
                            break;
                        }
                    }
                    index++;
                }
            }
            TimeWatch.________________________________________________________();
            return result;
        }
        #endregion


       
    }
}
