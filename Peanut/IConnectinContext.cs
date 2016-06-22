using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
namespace Peanut
{
    /// <summary>
    /// 数据库访问上下文描述
    /// </summary>
    public interface IConnectinContext : IDbTransaction, IDisposable
    {
        /// <summary>
        /// 开启事务,并指定事务级别
        /// </summary>
        /// <param name="level">事务级别</param>
        void BeginTransaction(IsolationLevel level);
        /// <summary>
        /// 开始事务
        /// </summary>
        void BeginTransaction();
        /// <summary>
        /// 执行命令并返回受影响的记录数量
        /// </summary>
        /// <param name="cmd">SQL命令</param>
        /// <returns>int</returns>
        int ExecuteNonQuery(Command cmd);
        /// <summary>
        /// 执行命令并返回一个数据获取对象
        /// </summary>
        /// <param name="cmd">SQL命令</param>
        /// <returns>IDataReader</returns>
        IDataReader ExecuteReader(Command cmd);
        /// <summary>
        /// 执行命令并返回第一条记录第一行的值
        /// </summary>
        /// <param name="cmd">SQL命令</param>
        /// <returns>object</returns>
        object ExecuteScalar(Command cmd);
        /// <summary>
        /// 执行命令并返回一个数据集
        /// </summary>
        /// <param name="cmd">SQL命令</param>
        /// <returns>DataSet</returns>
        DataSet ExecuteDataSet(Command cmd);
        /// <summary>
        /// 执行命令并返回指定区间的对象列表
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="cmd">SQL命令</param>
        /// <param name="region">加载的数据区间</param>
        /// <returns>IList</returns>
        IList<T> List<T>(Command cmd, Region region) where T :  new();
        /// <summary>
        /// 执行存储过程并返回一个处理值
        /// </summary>
        /// <param name="parameter">存储过程对象</param>
        /// <returns>object</returns>
        object ExecProc(object parameter);
        /// <summary>
        /// 执行存储过程返回相应的对象列表
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="parameter">存储过程对象</param>
        /// <returns>IList</returns>
        IList<T> ListProc<T>(object parameter) where T : new();
        /// <summary>
        /// 执行存储过程返回相应的对象列表
        /// </summary>
        /// <param name="entity">对象类型</param>
        /// <param name="parameter">存储过程对象</param>
        /// <returns>IList</returns>
        IList ListProc(Type entity,object parameter);
        /// <summary>
        /// 执行命令返回列表中第一个对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="cmd">SQL命令</param>
        /// <returns>T</returns>
        T ListFirst<T>(Command cmd) where T :  new();
        /// <summary>
        ///  执行命令返回对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="cmd">SQL命令</param>
        /// <returns>T</returns>
        T Load<T>(Command cmd) where T : IEntityState, new();
        /// <summary>
        /// 执行命令返回指定区间的对象列表
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="cmd">命令</param>
        /// <param name="region">区间</param>
        /// <returns>IList</returns>
        IList List(Type type, Command cmd, Region region);
        /// <summary>
        /// 执行命令返回列表中第一个对象
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="cmd">SQL命令</param>
        /// <returns></returns>
        object ListFirst(Type type, Command cmd);
        /// <summary>
        /// 执行命令返回对象
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="cmd">SQL命令</param>
        /// <returns></returns>
        object Load(Type type, Command cmd);
        /// <summary>
        /// 获取或设置相应的数据库
        /// </summary>
        DB Type
        {
            get;
            set;
        }
        #region GetValue
        /// <summary>
        /// 执行命令并返回相应的值
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="cmd">SQL命令</param>
        /// <returns>T</returns>
        T GetValue<T>(Command cmd);
        /// <summary>
        /// 执行命令并返回相应的值列表
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="cmd">SQL命令</param>
        /// <returns>IList</returns>
        IList<T> GetValues<T>(Command cmd);
        /// <summary>
        /// 执行命令并返回指定区间的值列表
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="cmd">SQL命令</param>
        /// <param name="region">区间</param>
        /// <returns>IList</returns>
        IList<T> GetValues<T>(Command cmd, Region region);
        #endregion
    }
}
