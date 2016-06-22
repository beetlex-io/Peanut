using System;
using System.Collections.Generic;
using System.Text;

namespace Peanut.Mappings
{
    class ChangeTable<T>:IDisposable
    {
        ObjectMapper om;
      
        public ChangeTable(string name)
        {
            om = ObjectMapper.GetOM(typeof(T));
            om.SetCurrentTable(name);
          
        }
        public void Dispose()
        {
            if (om != null)
                om.CleanCurrentTable();
        }
    }
    class ChangeTables : IDisposable
    {
        private List<IDisposable> mTables = new List<IDisposable>(8);
        public void Add<T>(string name) where T : IEntityState, new()
        {
           

                mTables.Add(new ChangeTable<T>(name));
           
        }
        public void Dispose()
        {
            foreach (IDisposable item in mTables)
            {
                item.Dispose();
            }
        }
    }
    
}
