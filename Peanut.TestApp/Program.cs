using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IKende.com.core;
using Peanut.Mappings;
using System.Reflection;
using System.Data.SqlClient;
namespace Peanut.TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
           
            Console.Read();
        }
     
        public static void Test()
        {

           

         
     
            CustOrderHist coh = new CustOrderHist();
            coh.CustomerID = "ANATR";
            IList<CustOrderHistItem> cohs = coh.List<CustOrderHistItem>();
            foreach (CustOrderHistItem item in cohs)
            {
                Console.WriteLine(item.ProductName);
            }

           
            SQL sql;
            Employee employee = (Employee.employeeID == 3).ListFirst<Employee>(DB.DB3);
            Console.WriteLine(employee);
           
            sql = "insert into employees(firstname,lastname) values(@p1,@p2)";
            sql["p1", "henry"]["p2", "fan"].Execute();
           
            sql = "delete from employees where firstname=@p1";
            sql["p1", "henry"].Execute();
           
            Query<int> getCount = "select count(*) from employees";
          

          
            Query<Employee> getEmp = "select * from employees where employeeid=3";
           


           
            Query<IList<Employee>> getEmps = "select * from employees";
          

          
            (Employee.firstName == "henry").Delete<Employee>();
         

         
            (Employee.employeeID == 3).Edit<Employee>(d => { d.LastName = "a"; });  

            ListOrders();

         
        }

        private static void ListOrders()
        {
         
            JoinTable table = Order.employeeID.InnerJoin(Employee.employeeID)
    .InnerJoin(Order.customerID, Customer.customerID)
    .Select(Order.orderID.At(), Customer.companyName, Employee.firstName, Employee.lastName);
         
            IList<OrderView> orders = new Expression().List<OrderView>(table);
          
        }

        public class OrderView
        {
            public int OrderID { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string CompanyName { get; set; }
        }
        public class UserBase
        {
            public string Name { get; set; }
        }
        public class CustOrderHistItem
        {

            public string ProductName
            {
                get;
                set;
            }
            public int Total
            {
                get;
                set;
            }
        }
        [Proc]
        public class CustOrderHist : Peanut.StoredProcedure
        {
            [ProcParameter]
            public string CustomerID { get; set; }

        }


        public void Execute()
        {
            
        }
    }
}
