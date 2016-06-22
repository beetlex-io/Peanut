using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Peanut.Mappings;
namespace Peanut.TestApp
{
    [Table("Orders")]
    public interface IOrder
    {
        [ID]
        int OrderID { get; set; }
        [Column]
        string CustomerID { get; set; }
        [Column]
        int EmployeeID { get; set; }
    }
    [Table("Customers")]
    public interface ICustomer
    {
        [ID]
        string CustomerID { get; set; }
        [Column]
        string CompanyName { get; set; }
    }
    [Table("Employees")]
    public interface IEmployee
    {
        [ID]
        int EmployeeID { get; set; }
        [Column]
        string FirstName { get; set; }
        [Column]
        string LastName { get;set; }
    }
}
