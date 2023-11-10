using ServerApplication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientApplication
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        private int _localPortNumber;
        private Thread _listenerThread;
        private bool _listenerStarted;
        private TcpListener _tcpListener;
        public ExpenditureManager manager = new ExpenditureManager();

        private void Home_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;

            _localPortNumber = ClientApplicationAddress.LocalPortNumber; ;
            _listenerStarted = true;
            _tcpListener = new TcpListener(IPAddress.Any, _localPortNumber);

            _listenerThread = new Thread(ListenerThreadMethod);
            _listenerStarted = true;
            _tcpListener.Start();
            _listenerThread.Start();
        }
        private void ListenerThreadMethod()
        {
            while (_listenerStarted)
            {
                try
                {
                    var handlerSocket = _tcpListener.AcceptSocket();
                    if (handlerSocket.Connected)
                    {
                        ThreadPool.QueueUserWorkItem(HandleConnection, handlerSocket);
                    }

                }
                catch (Exception)
                {
                }
            }
        }
        private void HandleConnection(object socketToProcess)
        {
            if (socketToProcess is Socket handlerSocket)
            {
                var networkStream = new NetworkStream(handlerSocket);

                using (var reader = new BinaryReader(networkStream))
                {
                    var DbInsertionResult = reader.ReadInt32();

                    using (var writer = new BinaryWriter(networkStream))
                    {
                        writer.Write(true); //indicates that the message was received
                        writer.Flush();
                    }
                    if (DbInsertionResult == 1)
                    {
                        MessageBox.Show("Expense accepted", MessageBoxOptions.DefaultDesktopOnly.ToString());
                    }
                    else
                    {
                        MessageBox.Show("Expense rejected", MessageBoxOptions.DefaultDesktopOnly.ToString());
                    }
                }

                handlerSocket = null; // nullify socket
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void viewAllExpensesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyForms.GetForm<ExpenseViewForm>().Show();
        }

        private void addNewExpenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyForms.GetForm<ExpenseAddForm>().Show();
        }

        private void Home_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopListening();
        }
        private void StopListening()
        {
            _listenerStarted = false;
            _tcpListener.Stop();
            _listenerThread.Interrupt();
            _listenerThread.Abort();
        }
    }
}
