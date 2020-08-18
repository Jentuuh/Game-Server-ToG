using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace TowerOfGodServer
{
    class Client
    {
        private static readonly int DATA_BUFFER_SIZE = 4096;
        private int m_id { get; set; }
        public TCP m_tcp { get; set; }

        public Client(int clientID)
        {
            m_id = clientID;
            m_tcp = new TCP(m_id);
        }

        public class TCP
        {
            public TcpClient m_socket;
            private readonly int m_id;

            private NetworkStream m_stream;
            private byte[] m_receivebuffer;

            public TCP(int id)
            {
                m_id = id;

            }
            public void Connect(TcpClient socket)
            {
                m_socket = socket;
                m_socket.ReceiveBufferSize = DATA_BUFFER_SIZE;
                m_socket.SendBufferSize = DATA_BUFFER_SIZE;

                m_stream = m_socket.GetStream();
                m_receivebuffer = new byte[DATA_BUFFER_SIZE];

                m_stream.BeginRead(m_receivebuffer, 0, DATA_BUFFER_SIZE, ReceiveCallback, null);
                
                // TODO: send welcome packet
            }

            private void ReceiveCallback(IAsyncResult result)
            {
                try
                {
                    int bytelength = m_stream.EndRead(result);
                    if(bytelength <= 0)
                    {
                        // TODO: disconnect client
                        return;
                    }
                    byte[] data = new byte[bytelength];
                    Array.Copy(m_receivebuffer, data, bytelength);

                    // TODO: handle data
                    m_stream.BeginRead(m_receivebuffer, 0, DATA_BUFFER_SIZE, ReceiveCallback, null);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error receiving TCP data: {e}");
                    // TODO: disconnect client
                }
            }
        }

    }
}
