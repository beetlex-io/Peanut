using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;
using Peanut.Mappings;
namespace Sqlite.Sample
{
    public class SqliteDriver : Peanut.DriverTemplate<
   SQLiteConnection,
   SQLiteCommand,
    SQLiteDataAdapter,
    SQLiteParameter,
    Peanut.SqlitBuilder>
    {
    }
    [Table]
    public interface ICustomer
    {
        [ID]
        string ID { get; set; }
        [Column]
        string CompanyName { get; set; }
        [Column]
        string ContactName { get; set; }
        [Column]
        string ContactTitle { get; set; }
        [Column]
        string Address { get; set; }
        [Column]
        string City { get; set; }
        [Column]
        string Region { get; set; }
        [Column]
        string Country { get; set; }
    }
}
