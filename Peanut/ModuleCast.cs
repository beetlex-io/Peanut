using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;

namespace Peanut
{
    class ModuleCast
    {
        private List<CastProperty> mProperties = new List<CastProperty>();
        
        static Dictionary<Type, Dictionary<Type, ModuleCast>> mCasters = new Dictionary<Type, Dictionary<Type, ModuleCast>>(256);

        private static Dictionary<Type, ModuleCast> GetModuleCast(Type sourceType)
        {
            Dictionary<Type, ModuleCast> result;
            lock (mCasters)
            {
                if (!mCasters.TryGetValue(sourceType, out result))
                {
                    result = new Dictionary<Type, ModuleCast>(8);
                    mCasters.Add(sourceType, result);
                }
            }
            return result;
        }
        
        public static ModuleCast GetCast(Type sourceType, Type targetType)
        {
            Dictionary<Type, ModuleCast> casts = GetModuleCast(sourceType);
            ModuleCast result;
            lock (casts)
            {
                if (!casts.TryGetValue(targetType, out result))
                {
                    result = new ModuleCast(sourceType, targetType);
                    casts.Add(targetType, result);
                }
            }
            return result;
        }   

        public ModuleCast(Type sourceType, Type targetType)
        {
            foreach (PropertyInfo sp in sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                foreach (PropertyInfo tp in targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (sp.Name == tp.Name && sp.PropertyType == tp.PropertyType)
                    {
                        CastProperty cp = new CastProperty();
                        cp.SourceProperty = new PropertyHandler(sp);
                        cp.TargetProperty = new PropertyHandler(tp);
                        mProperties.Add(cp);
                    }
                }
            }
        }
      
        public void Cast(object source, object target)
        {
            for (int i = 0; i < mProperties.Count; i++)
            {
                CastProperty cp = mProperties[i];
                cp.TargetProperty.Set(target, cp.SourceProperty.Get(source));
            }
        }

        public class CastProperty
        {
            public PropertyHandler SourceProperty
            {
                get;
                set;
            }

            public PropertyHandler TargetProperty
            {
                get;
                set;
            }
        }
    }
}
