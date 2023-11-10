using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ServerApplication;

namespace ClientApplication
{
    public partial class ExpenseViewForm : Form
    {
        public ExpenseViewForm()
        {
            InitializeComponent();
        }

        private void ExpenseViewForm_Load(object sender, EventArgs e)
        {
            MdiParent = MyForms.GetForm<Home>();
            LoadData();
        }
        public void LoadData()
        {
            dgv.DataMember = "";
            dgv.DataSource = null;
            dgv.DataSource = new ExpenditureManager().GetAll(); //populating dgv
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            MyForms.GetForm<ExpenseViewForm>().Close();
            MyForms.GetForm<ExpenseAddForm>().Show();
        }
    }
}
