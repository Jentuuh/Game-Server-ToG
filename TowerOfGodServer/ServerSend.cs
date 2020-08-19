using System;
using System.Collections.Generic;
using System.Text;

namespace TowerOfGodServer
{
    /*
    * @author Jente Vandersanden
    * 18/08/2020
    * Class that handles the server's outgoing data stream.
    * 
    * Credits to Tom Weiland's tutorial
    */
    class ServerSend
    {
        private static void SendTCPData(int receiving_client, Packet packet)
        {
            packet.WriteLength();
            Server.client_dictionary[receiving_client].m_tcp.SendData(packet);
        }

        // Sends a packet to all connected clients.
        private static void SendTCPDataToAll(Packet packet)
        {
            packet.WriteLength();
            for(int i = 0; i <= Server.m_max_players; i++)
            {
                Server.client_dictionary[i].m_tcp.SendData(packet);
            }
        }

        // Sends a packet to all connected clients except for one.
        private static void SendTCPDataToAll(int exceptPlayer, Packet packet)
        {
            packet.WriteLength();
            for (int i = 0; i <= Server.m_max_players; i++)
            {
                if (i != exceptPlayer)
                {
                    Server.client_dictionary[i].m_tcp.SendData(packet);
                }
            }
        }
        public static void Welcome(int receiving_client, string msg)
        {
            using (Packet packet = new Packet((int)ServerPackets.welcome))
            {
                packet.Write(msg);
                packet.Write(receiving_client);

                SendTCPData(receiving_client, packet);
            }
        }
    }
}
