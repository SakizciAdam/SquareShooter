using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SquareShooter.Network.Packet;
using SquareShooter.Network.Packet.Impl.Client;
using SquareShooter.Screen;

namespace SquareShooter.Network
{
    public class GameClient
    {

        private ManualResetEvent sendDone =
            new ManualResetEvent(false);

     

        public string username;
        public byte[] buffer=new byte[GameServer.BufferSize];

        public Socket client;

        public void Start(String ip)
        {

            try
            {

          
                IPAddress ipAddress = IPAddress.Parse(ip); 
                IPEndPoint remoteEP = new IPEndPoint(ipAddress.Address, GameServer.Port);

      
                client = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);

                client.BeginConnect(remoteEP,
                    new AsyncCallback(ConnectCallback), client);

             


                

            }
            catch (Exception e)
            {
                SquareShooter.instance.gameManager.LostConnection();
                Console.WriteLine(e.ToString());
            }
        }

        public void Stop(bool lobby=true)
        {
            try
            {
                client.Shutdown(SocketShutdown.Both);
                client.Close();
            } catch(Exception e) { Console.WriteLine(e.Message); }
            client = null;
            buffer = new byte[GameServer.BufferSize];
            this.username = "";
            SquareShooter.instance.gameManager.gameClient = null;
            SquareShooter.instance.gameManager.Players.Clear();
            SquareShooter.instance.gameManager.bullets.Clear();
            if (lobby)
            {
                SquareShooter.instance.currentScreen=new LobbyScreen(SquareShooter.instance);
            } else
            {
                
                SquareShooter.instance.currentScreen = new KickedScreen(SquareShooter.instance,"Lost connection");
            }
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                this.client = (Socket)ar.AsyncState;
           
        
                // Complete the connection.
                client.EndConnect(ar);
       
                Console.WriteLine("Socket connected to {0}",
                    client.RemoteEndPoint.ToString());
                SendHandshake();

                //connectDone.Set();

          
                Receive();


            }
            catch (Exception e)
            {
         
                SquareShooter.instance.gameManager.LostConnection();
                Console.WriteLine(e.ToString());
            }
        }

        private void Receive()
        {
            try
            {
        
                client.BeginReceive(buffer, 0, GameServer.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), client);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }


        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
              
                this.client = (Socket)ar.AsyncState;
                if(!this.client.Connected)
                {
                    Stop(false);
                    return;
                }
         
                int bytesRead = client.EndReceive(ar);
                Console.WriteLine("Server sent "+bytesRead);
                if (bytesRead > 0)
                {
                    ByteWrapper wrapper = new ByteWrapper(buffer);
                    byte id = wrapper.ReadByte();

                    ServerPacket packet = PacketManager.GetServerPacket(id);

                    if (packet != null)
                    {
                        Console.WriteLine("Server sent " + packet.GetType().Name);
                        packet.Read(wrapper);
                        packet.Process(this);
                    }
                    else
                    {
                        Console.WriteLine("Server sent invalid packet with id " + id);
                    }
                    client.BeginReceive(buffer, 0, GameServer.BufferSize, 0,
                        new AsyncCallback(ReceiveCallback), this.client);
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void SendPacket(ClientPacket packet)
        {
            if (!this.client.Connected)
            {
                Stop(false);
                return;
            }
            ByteWrapper byteWrapper = new ByteWrapper(new byte[1]);

            byteWrapper.WriteByte(packet.getPacketID());
            packet.Write(byteWrapper);

            Send(byteWrapper.data);
        }

        private void SendHandshake()
        {
            ClientPacketHandshake packet=new ClientPacketHandshake(username);
            SendPacket(packet);
        }

        private void Send(byte[] byteData)
        {
           
            client.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), client);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket client = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = client.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to server.", bytesSent);

          
                sendDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
