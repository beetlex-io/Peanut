using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using IKende.com.core;
namespace Peanut.Mappings
{
    [AttributeUsage(AttributeTargets.Property,AllowMultiple=true)]
    public  class RelationAttribute:Attribute
    {
        public RelationAttribute(Type partner, string column)
        {
            
            
            Partner = partner;
            Column = column;
            Initialize();
        }
        public Type Partner
        {
            get;
            set;
        }
        public string Column
        {
            get;
            set;
        }
        internal ObjectMapper Owner
        {
            get;
            set;
        }
        internal PropertyMapper OwnerProperty
        {
            get;
            set;
        }
        internal ObjectMapper PartnerMapper
        {
            get;
            set;
        }
        internal PropertyMapper PartnerProperty
        {
            get;
            set;
        }
        bool mIsAnalyse = false;
        internal void Analyse()
        {
            if (!mIsAnalyse)
            {
                lock (this)
                {
                    if (!mIsAnalyse)
                    {
                        PartnerMapper = ObjectMapper.GetOM(Partner);
                        PartnerProperty = PartnerMapper[Column];
                        if (PartnerProperty == null)
                            throw new PeanutException(string.Format(DataMsg.COLUMN_DESC_NOTFOUND, Partner.Name, Column));
                    }
                }
            }
        }
        private void Initialize()
        {
            foreach (Type entity in Partner.Assembly.GetTypes())
            {
                if (entity.GetInterface(Partner.Name) != null)
                {
                    if (Utils.GetTypeAttributes<TableAttribute>(entity, false).Length > 0)
                    {
                        Partner = entity;
                        return;
                    }

                }
            }
            throw new PeanutException(string.Format(DataMsg.OBJECT_MAPPING_NOTFOUND, Partner.FullName));
        }
    }
    
}
