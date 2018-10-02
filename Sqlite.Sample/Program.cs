using System;
using Peanut;
namespace Sqlite.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            DBContext.SetConnectionDriver<SqliteDriver>(DB.DB1);
            DBContext.SetConnectionString(DB.DB1, "Data Source=Northwind_small.sqlite;Pooling=true;FailIfMissing=false;");

            Expression exp = new Expression();
            var customer = exp.ListFirst<Customer>();
            customer.City = "gz";
            DBContext.Save(customer);
            foreach (var item in exp.List<Customer>())
            {
                Console.WriteLine(item.CompanyName);
            }

            foreach (var item in (Customer.country == "UK" & Customer.country == "British Isles").List<Customer>())
            {
                Console.WriteLine(item.CompanyName);
            }

            Console.Read();
        }
    }
}
