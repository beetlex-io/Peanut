using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;

namespace Peanut
{
   
    public class Query<T>
    {

        private static IExecuteHandler mExecuteHandler;

        internal static IExecuteHandler ExecuteHandler
        {
            get
            {
                if (mExecuteHandler == null)
                    mExecuteHandler = GetHandler();
                return mExecuteHandler;
            }
        }

        static IExecuteHandler GetHandler()
        {
            IExecuteHandler handler = new GetValue();
            Type type = typeof(T);
            if (type.IsValueType || type == typeof(String))
            {
                handler = new GetValue();
                 handler.Type = type;
            }
            else
            {
                if (type.GetInterface("System.Collections.Generic.IList`1") != null ||

                    type.Name == "IList`1")
                {
                    Type[] subtype = type.GetGenericArguments();
                    handler = new GetObjects();
                    handler.Type = subtype[0];
                }
                else
                {
                    handler = new GetObject();
                     handler.Type = type;
                }
            }
           
            return handler;
        }

        internal void Execute(HandlerValueType type,object value)
        {
            using (IConnectinContext cc = DBContext.GetConnection(DBContext.CurrentConnectonType))
            {
                IExecuteHandler handler = ExecuteHandler;

                object result = ExecuteHandler.Execute(cc, value, type);
                if (handler is GetValue)
                {
                    if (result != null && result != DBNull.Value)
                    {
                        Value = (T)result;
                    }
                }
                else
                {
                    Value = (T)result;
                }
                    
            }
        }

        public static implicit operator Query<T>(ValueType value)
        {
            Query<T> result = new Query<T>();
            result.Execute(HandlerValueType.ValueType, value);
            return result;
        }
        //implicit

        public static implicit operator Query<T>(string value)
        {
            Query<T> result = new Query<T>();
            result.Execute(HandlerValueType.String, value);
            return result;
        }

        public static implicit operator Query<T>(Expression exp)
        {
            Query<T> result = new Query<T>();
            result.Execute(HandlerValueType.EXPRESSION, exp);
            return result;
        }

        public static implicit operator Query<T>(SQL sql)
        {
            Query<T> result = new Query<T>();
            result.Execute(HandlerValueType.SQL, sql);
            return result;
        }

        public static implicit operator Query<T>(StoredProcedure proc)
        {
            Query<T> result = new Query<T>();
            result.Execute(HandlerValueType.PROC, proc);
            return result;
        }    

        public T Value
        {
            get;
            set;
        }

       
    }

    interface IExecuteHandler
    {
        Type Type
        {
            get;
            set;
        }
        object Execute(IConnectinContext cc,object value,HandlerValueType type);
    }

    class GetValue : IExecuteHandler
    {
        public Type Type
        {
            get;
            set;
        }

        public object Execute(IConnectinContext cc, object value, HandlerValueType type)
        {
           switch(type)
           {
               case HandlerValueType.String:
                   return ExecuseAsSQL(cc, new SQL((string)value)); 
               case HandlerValueType.SQL:
                   return ExecuseAsSQL(cc, (SQL)value);
               case HandlerValueType.PROC:
                   return DBContext.ExecProc(cc, value);
               default:
                   throw new PeanutException("object is not a [SQL,StoredProcedure]!");
           }
        }

        internal static bool IsChangeDataSQL(string value)
        {
            return value.IndexOf("delete", StringComparison.CurrentCultureIgnoreCase) >= 0
                || value.IndexOf("update", StringComparison.CurrentCultureIgnoreCase) >= 0
                || value.IndexOf("insert", StringComparison.CurrentCultureIgnoreCase) >= 0;
        }

        internal static bool IsSelectSQL(string value)
        {
            return value.IndexOf("select") >= 0;
        }

        internal static bool IsValueKey(string value)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(value, "^([a-zA-Z0-9]+)$", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        }
       
        private object ExecuseAsSQL(IConnectinContext cc, SQL value)
        {
            object result = null;
            if (IsChangeDataSQL(value.Command.Text.ToString()))
                result= cc.ExecuteNonQuery(value.Command);
            result= cc.ExecuteScalar(value.Command);
            return result;
        }
    }

    class GetObject : IExecuteHandler
    {
        public Type Type
        {
            get;
            set;
        }

        public object Execute(IConnectinContext cc, object value, HandlerValueType type)
        {
            string[] orderby = DBContext.CurrentOrderBy;
            Region curegion = DBContext.CurrentRegion;
            DBContext.CurrentRegion = null;
            DBContext.CurrentOrderBy = null;
            switch (type)
            {
                case HandlerValueType.ValueType:
                    return DBContext.Load(Type, value, cc);
                case HandlerValueType.PROC:
                    return DBContext.ExecProcToObject(Type, cc, value);
                case HandlerValueType.SQL:
                    return ((SQL)value).ListFirst(Type, cc);
                case HandlerValueType.EXPRESSION:
                    return ((Expression)value).ListFirst(Type, cc,orderby);
                case HandlerValueType.String:
                    string str = (string)value;
                    if (GetValue.IsValueKey(str))
                    {
                        return DBContext.Load(Type, value, cc); 
                    }
                    else
                    {
                        if (GetValue.IsSelectSQL(str))
                        {
                           return  new SQL(str).ListFirst(Type, cc);
                        }
                        else
                        {
                            Expression exp = new Expression(str);
                            return exp.ListFirst(Type, cc, orderby);
                        }
                    }
            }
            return null;
        }
    }

    class GetObjects : IExecuteHandler
    {
        public Type Type
        {
            get;
            set;
        }    

        public object Execute(IConnectinContext cc, object value, HandlerValueType type)
        {
            string[] orderby = DBContext.CurrentOrderBy;
            Region curegion = DBContext.CurrentRegion;
            DBContext.CurrentRegion = null;
            DBContext.CurrentOrderBy = null;
            
            switch (type)
            {
                case HandlerValueType.String:
                    string str = (string)value;
                    if (GetValue.IsSelectSQL(str))
                    {
                        return new SQL(str).List(Type, cc,curegion);
                    }
                    else
                    {
                        Expression exp = new Expression(str);
                        return exp.List(Type, cc,curegion, orderby);
                    }
                  
                case HandlerValueType.SQL:
                    return ((SQL)value).List(Type, cc,curegion);
                    break;
                case HandlerValueType.PROC:
                    return cc.ListProc(Type, value);
                  
                case HandlerValueType.EXPRESSION:
                   return ((Expression)value).List(Type, cc, curegion, orderby);
                  
                default:
                    throw new PeanutException("object is not a [SQL,StoredProcedure,Expression]!");
            }
            
        }
    }

    enum HandlerValueType
    {
        ValueType,
        String,
        SQL,
        EXPRESSION,
        PROC
    }
   

}
