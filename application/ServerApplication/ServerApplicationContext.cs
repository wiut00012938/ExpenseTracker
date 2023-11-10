using ServerApplication;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

public class BackendLogic
{
    private int _localPortNumber;
    private Thread _listenerThread;
    private bool _listenerStarted;
    private TcpListener _tcpListener;
    public ExpenditureManager manager = new ExpenditureManager();

    public void StartListening()
    {
        _localPortNumber = ServerAddress.LocalPortNumber;
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
                var descriptionLength = reader.ReadInt32();
                var description = new string(reader.ReadChars(descriptionLength));
                var date = new DateTime(reader.ReadInt64());
                var amount = reader.ReadDouble();

                List<Expenditure> ExpenseInMonth = new ExpenditureManager().Searching();
                var Overall = ExpenseInMonth.Sum(e => e.ExpenditureAmount) + amount;
                if (Overall <= 1000000)
                {
                    manager.Create(description, date, amount);
                    SendResponse(1);
                }
                else
                {
                    SendResponse(0);
                }
            }

            handlerSocket = null; // nullify socket
        }
    }

    private async void SendResponse(int value)
    {
        try
        {
            await SendMessage(value);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private Task SendMessage(int value)
    {
        return Task.Run(() =>
        {
            try
            {
                var client = new TcpClient();
                client.Connect(ClientApplicationAddress.LocalIPNumber, Convert.ToInt32(ClientApplicationAddress.LocalPortNumber));
                using (var writer = new BinaryWriter(client.GetStream()))
                {
                    writer.Write(value);
                    writer.Flush();

                    //get response
                }

            }
            catch (Exception)
            {
            }
        });
    }

    public void StopListening()
    {
        _listenerStarted = false;
        _tcpListener.Stop();
        _listenerThread.Interrupt();
        _listenerThread.Abort();
    }
}
