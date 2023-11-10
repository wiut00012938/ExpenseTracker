using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerApplication
{
    public partial class ExpenditureManager: dbManager
    {
        public void Create(string description, DateTime date, double amount)
        {
            var connection = Connection; // Connection is a part of dbManager. It reference to our real database connection
            try //in every void, list we will use try-catch method
            {
                var sql = "INSERT INTO Expense (ExpenseDescription, ExpenseDate, ExpenseAmount) VALUES (@description, @date, @amount)";
                var command = new SQLiteCommand(sql, connection);
                command.Parameters.AddWithValue("@description", description);
                command.Parameters.AddWithValue("@date", date.Ticks);
                command.Parameters.AddWithValue("@amount", amount);
                command.Parameters["@amount"].DbType = DbType.Double;
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)//we need to close a connection. In case if it stays open our database will be locked when creating new connection
                    connection.Close();
            }
        }
        public List<Expenditure> GetAll()// GetAll() will gate all data, not only one row. Therefore, using list is needed
        {
            var connection = Connection;
            var result = new List<Expenditure>();
            try
            {
                var sql = "SELECT Id, ExpenseDescription, ExpenseDate, ExpenseAmount FROM Expense";
                var command = new SQLiteCommand(sql, connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                    while (reader.Read()) //loop for loading data row by row
                    {
                        var c = new Expenditure
                        {
                            Id = Convert.ToInt32(reader.GetValue(0)),
                            Description = Convert.ToString(reader.GetValue(1)),
                            ExpenditureDate = new DateTime(Convert.ToInt64(reader.GetValue(2))),
                            ExpenditureAmount = Convert.ToDouble(reader.GetValue(3))
                        };
                        result.Add(c);
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
            }

            return result;
        }
        
        public List<Expenditure> Searching()
        {
            //before finding a sum of hours worked, we need to filter the data. We need to fitter by employeeid and the month of a report
            return GetAll().Where(a => Convert.ToDateTime(a.ExpenditureDate).Month == DateTime.Now.Month).ToList();
        }

    }
}
