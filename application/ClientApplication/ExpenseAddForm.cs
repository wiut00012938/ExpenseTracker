using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ServerApplication;

namespace ClientApplication
{
    public partial class ExpenseAddForm : Form
    {
        public ExpenseAddForm()
        {
            InitializeComponent();
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                await SendMessage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private Task SendMessage()
        {
            return Task.Run(() =>
            {
                try
                {
                    var client = new TcpClient();
                    client.Connect(ServerAddress.LocalIPNumber,Convert.ToInt32(ServerAddress.LocalPortNumber));
                    using (var writer = new BinaryWriter(client.GetStream()))
                    {
                        writer.Write(tbxDescription.Text.Length);
                        writer.Write(tbxDescription.Text.ToCharArray());
                        writer.Write(Convert.ToInt64(dtpDate.Value.Ticks));
                        writer.Write(Convert.ToDouble(nudAmount.Value));
                        writer.Flush();
                        MyForms.GetForm<ExpenseAddForm>().Close();
                    }

                }
                catch (Exception)
                {
                    MessageBox.Show("An error occurred while connecting to the server: ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });
        }

        private void ExpenseAddForm_Load(object sender, EventArgs e)
        {
            MdiParent = MyForms.GetForm<Home>();
        }
    }
}
