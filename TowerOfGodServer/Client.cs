using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace TowerOfGodServer
{
    /*
    * @author Jente Vandersanden
    * 18/08/2020
    * Class that will represent the client and handle its connection to the
    * server and the receiving of data from the server.
    * 
    * Credits to Tom Weiland's tutorial
    */
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

            private Packet m_received_data;

            public TCP(int id)
            {
                m_id = id;

            }
            public void Connect(TcpClient socket)
            {
                m_socket = socket;
                m_socket.ReceiveBufferSize = DATA_BUFFER_SIZE;
                m_socket.SendBufferSize = DATA_BUFFER_SIZE;

                m_received_data = new Packet();

                m_stream = m_socket.GetStream();
                m_receivebuffer = new byte[DATA_BUFFER_SIZE];

                m_stream.BeginRead(m_receivebuffer, 0, DATA_BUFFER_SIZE, ReceiveCallback, null);

                ServerSend.Welcome(m_id, "Welcome to the Tower of God Game Server!");
            }

            public void SendData(Packet packet)
            {
                try
                {
                    if(m_socket != null)
                    {
                        m_stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine($"Error sending data to player {m_id} via TCP: {e}.");
                }
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

                    m_received_data.Reset(HandleData(data));
                    m_stream.BeginRead(m_receivebuffer, 0, DATA_BUFFER_SIZE, ReceiveCallback, null);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error receiving TCP data: {e}");
                    // TODO: disconnect client
                }
            }

            // Function that decides whether the received_data array needs to be reset
            // (based on whether the packet that currently's being processed is already
            // empty).
            private bool HandleData(byte[] data)
            {
                int packetlength = 0;

                m_received_data.SetBytes(data);

                if (m_received_data.UnreadLength() >= 4)
                {
                    packetlength = m_received_data.ReadInt();
                    if (packetlength <= 0)
                    {
                        return true;
                    }
                }
                // As long as the packet we're looking at isn't empty
                while (packetlength > 0 && packetlength <= m_received_data.UnreadLength())
                {
                    // Store the packet's bytes
                    byte[] packetbytes = m_received_data.ReadBytes(packetlength);

                    // Run this on the main thread
                    ThreadManager.ExecuteOnMainThread(() =>
                    {
                        using (Packet packet = new Packet(packetbytes))
                        {
                            int packetID = packet.ReadInt();
                            // Delegate call
                            Server.m_packethandlers[packetID](m_id, packet);
                        }
                    });
                    packetlength = 0;

                    if (m_received_data.UnreadLength() >= 4)
                    {
                        packetlength = m_received_data.ReadInt();
                        if (packetlength <= 0)
                        {
                            return true;
                        }
                    }
                }
                // If the packet is now empty, we can reset the m_received_data array
                // (so we return true)
                if (packetlength <= 1)
                    return true;
                // Otherwise we return false, since the packet is not completely emptied yet.
                return false;
            }
        }

    }

}

