using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Peanut.Mappings
{
    class DObjectToBObject
    {
        public DObjectToBObject(Type dtype, Type btype)
        {
            foreach (PropertyInfo pi in dtype.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                PropertyInfo bpi = btype.GetProperty(pi.Name, BindingFlags.Public | BindingFlags.Instance);
                if (bpi != null && bpi.PropertyType == pi.PropertyType)
                {
                    DTBProperty dtbp = new DTBProperty();
                    dtbp.DProperty = new PropertyHandler(pi);
                    dtbp.BProperty = new PropertyHandler(bpi);
                    Properties.Add(dtbp);
                }
            }
        }
        private List<DTBProperty> Properties = new List<DTBProperty>();
        public void Cast(object dobj, object bobj)
        {
            for (int i = 0; i < Properties.Count; i++)
            {
                DTBProperty dtb = Properties[i];
                dtb.BProperty.Set(bobj, dtb.DProperty.Get(dobj));
            }
        }
        class DTBProperty
        {
            public PropertyHandler DProperty;
            public PropertyHandler BProperty;
        }
    }
}
