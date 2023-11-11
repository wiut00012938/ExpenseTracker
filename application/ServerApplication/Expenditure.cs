using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApplication
{
    public class Expenditure
    {
        private string description;
        //class that represents one row of the database
        public int Id { get; set; }
        public string Description {
            get => description;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("Description cannot be empty");
                description = value;
            }
        }
        public DateTime ExpenditureDate { get; set; }   
        public double ExpenditureAmount { get; set; }
        // use of constructor
        public Expenditure()
        {

        }
        public Expenditure(int id, string description, DateTime expenditureDate, double expenditureAmount)
        {
            Id = id;
            Description = description;
            ExpenditureDate = expenditureDate;
            ExpenditureAmount = expenditureAmount;
        }
    }
}
