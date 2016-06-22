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
    class Program:IKende.com.core.ICommand
    {
        static void Main(string[] args)
        {
            Program program = new Program();
            mDispatch.Add(program);
            while (true)
            {
                mDispatch.Add(new Program());
                System.Threading.Thread.Sleep(5);
            }
            Console.Read();
        }
        private static IKende.com.core.Dispatch mDispatch = new Dispatch();
        public static void Test()
        {

           

            TimeWatch.Clean();
            TimeWatch.________________________________________________________("Peaunt Exeuctes");

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
            TimeWatch.________________________________________________________("Insert Employee");
            sql = "insert into employees(firstname,lastname) values(@p1,@p2)";
            sql["p1", "henry"]["p2", "fan"].Execute();
            TimeWatch.________________________________________________________();

            TimeWatch.________________________________________________________("del employee");
            sql = "delete from employees where firstname=@p1";
            sql["p1", "henry"].Execute();
            TimeWatch.________________________________________________________();

            TimeWatch.________________________________________________________("count employee");
            Query<int> getCount = "select count(*) from employees";
            TimeWatch.________________________________________________________();

            TimeWatch.________________________________________________________("get employee");
            Query<Employee> getEmp = "select * from employees where employeeid=3";
            TimeWatch.________________________________________________________();


            TimeWatch.________________________________________________________("list employee");
            Query<IList<Employee>> getEmps = "select * from employees";
            TimeWatch.________________________________________________________();

            TimeWatch.________________________________________________________("del employee with expression");
            (Employee.firstName == "henry").Delete<Employee>();
            TimeWatch.________________________________________________________();

            TimeWatch.________________________________________________________("modify employee");
            (Employee.employeeID == 3).Edit<Employee>(d => { d.LastName = "a"; });
            TimeWatch.________________________________________________________();

            ListOrders();

            TimeWatch.________________________________________________________();

            TimeWatch.Report(o => { Console.WriteLine(o.ToString()); });
        }

        private static void ListOrders()
        {
            TimeWatch.________________________________________________________("list order");
            TimeWatch.________________________________________________________("create table");
            JoinTable table = Order.employeeID.InnerJoin(Employee.employeeID)
    .InnerJoin(Order.customerID, Customer.customerID)
    .Select(Order.orderID.At(), Customer.companyName, Employee.firstName, Employee.lastName);
            TimeWatch.________________________________________________________();
            TimeWatch.________________________________________________________("List");
            IList<OrderView> orders = new Expression().List<OrderView>(table);
            TimeWatch.________________________________________________________();
            TimeWatch.________________________________________________________();
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
