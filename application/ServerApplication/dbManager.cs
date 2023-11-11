using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApplication
{
    public class DbManager
    {
        public SQLiteConnection Connection
        {
            // in our Connection we only get, not setting data
            get
            {
                return new SQLiteConnection(Properties.Settings.Default.ConnectionString); //in a setting our db  was specified by name ConnectionString
            }
        }
    }
}
