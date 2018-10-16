using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;
using Peanut.Mappings;

namespace Sqlite.Sample
{
    ///<summary>
    ///Peanut Generator Copyright @ henryfan 2018 email:henryfan@msn.com
    ///website:http://www.ikende.com
    ///</summary>
    [Table()]
    public partial class Customer : Peanut.Mappings.DataObject
    {
        private string mID;
        public static Peanut.FieldInfo<string> iD = new Peanut.FieldInfo<string>("Customer", "ID");
        private string mCompanyName;
        public static Peanut.FieldInfo<string> companyName = new Peanut.FieldInfo<string>("Customer", "CompanyName");
        private string mContactName;
        public static Peanut.FieldInfo<string> contactName = new Peanut.FieldInfo<string>("Customer", "ContactName");
        private string mContactTitle;
        public static Peanut.FieldInfo<string> contactTitle = new Peanut.FieldInfo<string>("Customer", "ContactTitle");
        private string mAddress;
        public static Peanut.FieldInfo<string> address = new Peanut.FieldInfo<string>("Customer", "Address");
        private string mCity;
        public static Peanut.FieldInfo<string> city = new Peanut.FieldInfo<string>("Customer", "City");
        private string mRegion;
        public static Peanut.FieldInfo<string> region = new Peanut.FieldInfo<string>("Customer", "Region");
        private string mCountry;
        public static Peanut.FieldInfo<string> country = new Peanut.FieldInfo<string>("Customer", "Country");
        ///<summary>
        ///Type:string
        ///</summary>
        [ID()]
        public virtual string ID
        {
            get
            {
                return mID;
                
            }
            set
            {
                mID = value;
                EntityState.FieldChange("ID");
                
            }
            
        }
        ///<summary>
        ///Type:string
        ///</summary>
        [Column()]
        public virtual string CompanyName
        {
            get
            {
                return mCompanyName;
                
            }
            set
            {
                mCompanyName = value;
                EntityState.FieldChange("CompanyName");
                
            }
            
        }
        ///<summary>
        ///Type:string
        ///</summary>
        [Column()]
        public virtual string ContactName
        {
            get
            {
                return mContactName;
                
            }
            set
            {
                mContactName = value;
                EntityState.FieldChange("ContactName");
                
            }
            
        }
        ///<summary>
        ///Type:string
        ///</summary>
        [Column()]
        public virtual string ContactTitle
        {
            get
            {
                return mContactTitle;
                
            }
            set
            {
                mContactTitle = value;
                EntityState.FieldChange("ContactTitle");
                
            }
            
        }
        ///<summary>
        ///Type:string
        ///</summary>
        [Column()]
        public virtual string Address
        {
            get
            {
                return mAddress;
                
            }
            set
            {
                mAddress = value;
                EntityState.FieldChange("Address");
                
            }
            
        }
        ///<summary>
        ///Type:string
        ///</summary>
        [Column()]
        public virtual string City
        {
            get
            {
                return mCity;
                
            }
            set
            {
                mCity = value;
                EntityState.FieldChange("City");
                
            }
            
        }
        ///<summary>
        ///Type:string
        ///</summary>
        [Column()]
        public virtual string Region
        {
            get
            {
                return mRegion;
                
            }
            set
            {
                mRegion = value;
                EntityState.FieldChange("Region");
                
            }
            
        }
        ///<summary>
        ///Type:string
        ///</summary>
        [Column()]
        public virtual string Country
        {
            get
            {
                return mCountry;
                
            }
            set
            {
                mCountry = value;
                EntityState.FieldChange("Country");
                
            }
            
        }
        
    }
    ///<summary>
    ///Peanut Generator Copyright @ henryfan 2018 email:henryfan@msn.com
    ///website:http://www.ikende.com
    ///</summary>
    public partial class GroupByCountry : Peanut.Mappings.DataObject
    {
        private int mCount;
        public static Peanut.FieldInfo<int> count = new Peanut.FieldInfo<int>("GroupByCountry", "ID");
        private string mCountry;
        public static Peanut.FieldInfo<string> country = new Peanut.FieldInfo<string>("GroupByCountry", "Country");
        ///<summary>
        ///Type:int
        ///</summary>
        [Column("ID")]
        [Count]
        public virtual int Count
        {
            get
            {
                return mCount;
                
            }
            set
            {
                mCount = value;
                EntityState.FieldChange("Count");
                
            }
            
        }
        ///<summary>
        ///Type:string
        ///</summary>
        [Column()]
        public virtual string Country
        {
            get
            {
                return mCountry;
                
            }
            set
            {
                mCountry = value;
                EntityState.FieldChange("Country");
                
            }
            
        }
        
    }
    
}
