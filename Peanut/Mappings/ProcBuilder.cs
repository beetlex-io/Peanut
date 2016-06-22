using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Data;
using System.Collections;
using IKende.com.core;
namespace Peanut.Mappings
{
    class ProcBuilder
    {
        public ProcBuilder(Type type)
        {
            Type = type;
            Init();
        }
        private List<ProcParameterAttribute> mParameters = new List<ProcParameterAttribute>();
        public Type Type
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        private void Init()
        {
            ProcAttribute[] pa =Utils.GetTypeAttributes<ProcAttribute>(Type, false);
            if (pa.Length == 0)
                throw new PeanutException(string.Format(DataMsg.OBJECT_PROCDESC_NOTFOUND, Type));
            if (string.IsNullOrEmpty(pa[0].Name))
            {
                Name = Type.Name;
            }
            else
            {
                Name = pa[0].Name;
            }
            foreach (PropertyInfo p in Type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                ProcParameterAttribute[] pas = (ProcParameterAttribute[])p.GetCustomAttributes(typeof(ProcParameterAttribute), false);
                if (pas.Length > 0)
                {
                    if (string.IsNullOrEmpty(pas[0].Name))
                    {
                        pas[0].Name = p.Name;
                        
                    }
                    pas[0].Handler = new PropertyHandler(p);
                    mParameters.Add(pas[0]);
                }
            }
        }
        public System.Data.IDataParameter[] GetParamters(object data,IDriver driver)
        {
            IDataParameter[] parameters = new IDataParameter[mParameters.Count];
            for (int i = 0; i < mParameters.Count; i++)
            {
                ProcParameterAttribute procp = mParameters[i];
                parameters[i] = driver.CreateProcParameter(procp.Name,
                    procp.Handler.Get(data), procp.Direction);
            }
            return parameters;
        }
        public void UpdateParameters(object data, IDbCommand cmd)
        {
            for (int i = 0; i < mParameters.Count; i++)
            {
                ProcParameterAttribute procp = mParameters[i];
                if (procp.Direction == ParameterDirection.Output || procp.Direction == ParameterDirection.ReturnValue)
                {
                    if (((IDataParameter)cmd.Parameters[i]).Value != null && ((IDataParameter)cmd.Parameters[i]).Value != DBNull.Value)
                        procp.Handler.Set(data, Convert.ChangeType(((IDataParameter)cmd.Parameters[i]).Value, procp.Handler.Property.PropertyType));
                }
            }
        }
        private static Hashtable mBuilders = new Hashtable();
        public static ProcBuilder GetBuilder(Type type)
        {
            ProcBuilder result = (ProcBuilder)mBuilders[type];
            if (result != null)
                return result;
            return createBuilder(type);
        }
        private static ProcBuilder createBuilder(Type type)
        {
            lock (mBuilders)
            {
                ProcBuilder result = (ProcBuilder)mBuilders[type];
                if (result == null)
                {
                    result = new ProcBuilder(type);
                    mBuilders.Add(type, result);
                }
                return result;
            }
        }
    }
}
