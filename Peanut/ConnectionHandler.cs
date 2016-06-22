using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
namespace Peanut
{
    /// <summary>
    /// 数据连接句柄对象
    /// </summary>
    class ConnectionHandler : IDbTransaction, IDisposable
    {
        public ConnectionHandler(IDbConnection connection)
        {
            mConnection = connection;
        }
        private IDbConnection mConnection;
        public IDbConnection Connection
        {
            get
            {
                return mConnection;
            }


        }
        internal IDriver Driver
        {
            get;
            set;
        }
        private IDbTransaction mTransaction;
        public IDbTransaction Transaction
        {
            get
            {
                return mTransaction;

            }
        }

        public void BeginTransaction(IsolationLevel level)
        {
            if (Transaction == null)
            {
                mIsolationLevel = level;
                mTransaction = Connection.BeginTransaction(level);
            }
        }
        public void BeginTransaction()
        {
            if (Transaction == null)
                mTransaction = Connection.BeginTransaction();
        }
        #region IDisposable 成员

        public void Dispose()
        {
            Rollback();
            if (mConnection != null)
                mConnection.Dispose();
        }

        #endregion
        #region IDbTransaction 成员
        public void Commit()
        {
            if (Transaction != null)
            {

                Transaction.Commit();
                mTransaction = null;
            }
        }
        private IsolationLevel mIsolationLevel = IsolationLevel.Unspecified;
        public IsolationLevel IsolationLevel
        {
            get { return mIsolationLevel; }
        }
        public void Rollback()
        {
            if (Transaction != null)
            {

                Transaction.Rollback();
                mTransaction = null;
            }
        }
        public IDataReader ExecProcReader(object parameter)
        {
            Mappings.ProcBuilder pb = Mappings.ProcBuilder.GetBuilder(parameter.GetType());
            IDataReader reader;
            IDbCommand cmd = Driver.Command;
            cmd.CommandText = pb.Name;
            cmd.CommandType = CommandType.StoredProcedure;
            IDataParameter[] dps = pb.GetParamters(parameter, Driver);
            for (int i = 0; i < dps.Length; i++)
            {
                cmd.Parameters.Add(dps[i]);
            }
            OnInitCommand(cmd);
            reader = cmd.ExecuteReader();
            pb.UpdateParameters(parameter, cmd);
            return reader;
        }
        public object ExecProc(object parameter)
        {
            object result;
            Mappings.ProcBuilder pb = Mappings.ProcBuilder.GetBuilder(parameter.GetType());
            IDbCommand cmd = Driver.Command;
            cmd.CommandText = pb.Name;
            cmd.CommandType = CommandType.StoredProcedure;
            IDataParameter[] dps = pb.GetParamters(parameter, Driver);
            for (int i = 0; i < dps.Length; i++)
            {
                cmd.Parameters.Add(dps[i]);
            }
            OnInitCommand(cmd);
            result = cmd.ExecuteScalar();
            pb.UpdateParameters(parameter, cmd);
            return result;

        }
        #endregion
        private void OnInitCommand(IDbCommand cmd)
        {
            Expression.NameSeed.Value = 0;
            DBContext.LastCommand = cmd;
            cmd.Connection = Connection;
            if (Transaction != null)
                cmd.Transaction = Transaction;
            if (DBContext.Executing != null)
            {
                DBContext.Executing(cmd);
            }
        }
        public int ExecuteNonQuery(Command cmd)
        {
            IDbCommand _execmd = cmd.CreateCommand(Driver);
            OnInitCommand(_execmd);
            return _execmd.ExecuteNonQuery();
        }
        public IDataReader ExecuteReader(Command cmd)
        {
            IDbCommand _execmd = cmd.CreateCommand(Driver);
            OnInitCommand(_execmd);
            
            return _execmd.ExecuteReader();
        }
        public object ExecuteScalar(Command cmd)
        {

            IDbCommand _execmd = cmd.CreateCommand(Driver);
            OnInitCommand(_execmd);
            return _execmd.ExecuteScalar();
        }
        public DataSet ExecuteDataSet(Command cmd)
        {
            IDbCommand _execmd = cmd.CreateCommand(Driver);
            OnInitCommand(_execmd);
            IDataAdapter da = Driver.DataAdapter(_execmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            return ds;
        }
    }

}
