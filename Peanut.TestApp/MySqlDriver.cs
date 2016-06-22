using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Peanut
{
    public class MySqlDriver:DriverTemplate<MySqlConnection,MySqlCommand,MySqlDataAdapter,MySqlParameter,MysqlBuilder>
    {
    }
}
