using System;
using System.Collections.Generic;
using System.Text;

namespace TowerOfGodServer
{
    /*
    * @author Jente Vandersanden
    * 18/08/2020
    * Class that handles the server side's confirmation of clients
    * receiving the welcome message (and verifies whether these clients
    * rightfully claimed their corresponding ID's)
    * 
    * Credits to Tom Weiland's tutorial
    */
    class ServerHandle
    {
        public static void welcomeReceived(int receiver, Packet packet)
        {
            // Read receiver's ID and username
            int receiverIDCheck = packet.ReadInt();
            string receiver_usern = packet.ReadString();

            Console.WriteLine($"{Server.client_dictionary[receiver].m_tcp.m_socket.Client.RemoteEndPoint} " +
                $"connected succesfully and is now player {receiver}.");

            // Check if client indeed claimed the correct ID
            if (receiver != receiverIDCheck)
            {
                Console.WriteLine($"Player \"{receiver_usern}\" (ID:{receiver}) " +
                    $"has claimed the wrong Client ID ({receiverIDCheck}).");
            }
            // TODO: Send player into game
        }
    }
}
