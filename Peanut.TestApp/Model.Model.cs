using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Peanut.Mappings;

namespace Peanut.TestApp
{
    ///<summary>
    ///Peanut Generator Copyright @ FanJianHan 2010-2013
    ///website:http://www.ikende.com
    ///</summary>
    [Table("Orders")]
    public partial class Order : Peanut.Mappings.DataObject
    {
        private int mOrderID;
        public static Peanut.FieldInfo<int> orderID = new Peanut.FieldInfo<int>("Orders", "OrderID");
        private string mCustomerID;
        public static Peanut.FieldInfo<string> customerID = new Peanut.FieldInfo<string>("Orders", "CustomerID");
        private int mEmployeeID;
        public static Peanut.FieldInfo<int> employeeID = new Peanut.FieldInfo<int>("Orders", "EmployeeID");
        ///<summary>
        ///Type:int
        ///</summary>
        [ID()]
        public virtual int OrderID
        {
            get
            {
                return mOrderID;
                
            }
            set
            {
                mOrderID = value;
                EntityState.FieldChange("OrderID");
                
            }
            
        }
        ///<summary>
        ///Type:string
        ///</summary>
        [Column()]
        public virtual string CustomerID
        {
            get
            {
                return mCustomerID;
                
            }
            set
            {
                mCustomerID = value;
                EntityState.FieldChange("CustomerID");
                
            }
            
        }
        ///<summary>
        ///Type:int
        ///</summary>
        [Column()]
        public virtual int EmployeeID
        {
            get
            {
                return mEmployeeID;
                
            }
            set
            {
                mEmployeeID = value;
                EntityState.FieldChange("EmployeeID");
                
            }
            
        }
        
    }
    ///<summary>
    ///Peanut Generator Copyright @ FanJianHan 2010-2013
    ///website:http://www.ikende.com
    ///</summary>
    [Table("Customers")]
    public partial class Customer : Peanut.Mappings.DataObject
    {
        private string mCustomerID;
        public static Peanut.FieldInfo<string> customerID = new Peanut.FieldInfo<string>("Customers", "CustomerID");
        private string mCompanyName;
        public static Peanut.FieldInfo<string> companyName = new Peanut.FieldInfo<string>("Customers", "CompanyName");
        ///<summary>
        ///Type:string
        ///</summary>
        [ID()]
        public virtual string CustomerID
        {
            get
            {
                return mCustomerID;
                
            }
            set
            {
                mCustomerID = value;
                EntityState.FieldChange("CustomerID");
                
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
        
    }
    ///<summary>
    ///Peanut Generator Copyright @ FanJianHan 2010-2013
    ///website:http://www.ikende.com
    ///</summary>
    [Table("Employees")]
    public partial class Employee : Peanut.Mappings.DataObject
    {
        private int mEmployeeID;
        public static Peanut.FieldInfo<int> employeeID = new Peanut.FieldInfo<int>("Employees", "EmployeeID");
        private string mFirstName;
        public static Peanut.FieldInfo<string> firstName = new Peanut.FieldInfo<string>("Employees", "FirstName");
        private string mLastName;
        public static Peanut.FieldInfo<string> lastName = new Peanut.FieldInfo<string>("Employees", "LastName");
        ///<summary>
        ///Type:int
        ///</summary>
        [ID()]
        public virtual int EmployeeID
        {
            get
            {
                return mEmployeeID;
                
            }
            set
            {
                mEmployeeID = value;
                EntityState.FieldChange("EmployeeID");
                
            }
            
        }
        ///<summary>
        ///Type:string
        ///</summary>
        [Column()]
        public virtual string FirstName
        {
            get
            {
                return mFirstName;
                
            }
            set
            {
                mFirstName = value;
                EntityState.FieldChange("FirstName");
                
            }
            
        }
        ///<summary>
        ///Type:string
        ///</summary>
        [Column()]
        public virtual string LastName
        {
            get
            {
                return mLastName;
                
            }
            set
            {
                mLastName = value;
                EntityState.FieldChange("LastName");
                
            }
            
        }
        
    }
    
}
