using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SquareShooter.Network.Packet;
using System.Diagnostics;
using SquareShooter.Network.Packet.Impl.Server;
using System.Numerics;
using System.Drawing;

namespace SquareShooter.Network
{
    public class GameServer
    {
        public const int Port = 41244;
        private Socket listener;
        private bool isServerRunning;

        public bool Running=>isServerRunning;

        public static readonly int BufferSize = 256;

        public Thread serverThread,disconnectThread;

        public ManualResetEvent allDone = new ManualResetEvent(false);

        public List<NetworkClient> clients= new List<NetworkClient>();

        public void StartServer()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPEndPoint localEP = new IPEndPoint(IPAddress.Any.Address, Port);
            listener = new Socket(localEP.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(localEP);
            isServerRunning = true;
            Console.WriteLine("Server started on "+ localEP.Address);
            serverThread = new Thread(() =>
            {

                while (isServerRunning)
                {
                    allDone.Reset();
                    listener.Listen(10);
                    listener.BeginAccept(new AsyncCallback(acceptCallback), listener);
                    allDone.WaitOne();
                }
                Console.WriteLine("Closing server");
                listener.Close();
            });
            disconnectThread = new Thread(() =>
            {
                while (isServerRunning)
                {
                    for(int i=0;i<clients.Count; i++)
                    {
                        NetworkClient client = clients[i];

                        if(client==null) continue;

                        if(client.socket==null) continue;
                        
                        if (!client.IsConnected()|| client.pingWatch.ElapsedMilliseconds > 15000)
                        {
                            client.CloseConnection();
                            continue;
                        }
                        if (client.stopwatch.ElapsedMilliseconds > 500)
                        {
                            client.SendPing();
                            client.stopwatch.Restart();
                        }

                    }
                }
            });
            this.disconnectThread.Start();
            this.serverThread.Start();
           
        }

        public void StartGameHost()
        {

  

            foreach(NetworkClient client in clients)
            {
                if(client==null || !client.IsConnected() ||!client.loggedIn) continue;
      
                client.SendPacket(new ServerPacketStart(SquareShooter.instance.gameManager.Players, SquareShooter.instance.gameManager.walls));
            }




        }

        public void UpdatePlayers(string username,bool added)
        {
            ServerPacketPlayerChange packet=new ServerPacketPlayerChange(username, added);
            foreach(NetworkClient client in clients)
            {
                client.SendPacket(packet);
            }
        }

        public void StopServer()
        {
  
            isServerRunning = false;
            

        
        }

        public void acceptCallback(IAsyncResult ar)
        {
        
            Socket listener = (Socket)ar.AsyncState;
            
            if (listener != null)
            {
                
                Socket handler = listener.EndAccept(ar);
                Console.WriteLine("Joined");
                NetworkClient client = new NetworkClient();
                client.socket = handler;

                if (clients.Count >= 1)
                {
                    client.SendPacket(new ServerPacketDisconnect("Lobby filled up!"));
                    allDone.Set();
                    return;
                }
               
                clients.Add(client);
                allDone.Set();


                handler.BeginReceive(client.buffer, 0, BufferSize, 0, new AsyncCallback(readCallback), client);
            }
        }

        public void SpawnWall(Raylib_cs.Rectangle rec)
        {
            SquareShooter.instance.gameManager.gameServer.clients.ForEach(x =>
            {
                if (x != null && x.loggedIn && x.IsConnected())
                {
                    x.SendPacket(new ServerPacketSpawnObject("WLL",new Vector2(rec.X,rec.Y),new Vector2(rec.Width,rec.Height)));
                }
            });
            SquareShooter.instance.gameManager.SpawnLocalWall(rec);
        }

        public void SpawnBullet(string owner,Vector2 pos,Vector2 vel)
        {
            SquareShooter.instance.gameManager.gameServer.clients.ForEach(x =>
            {
                if (x != null && x.loggedIn && x.IsConnected())
                {
                    x.SendPacket(new ServerPacketSpawnObject(owner,pos, vel));
                }
            });
            SquareShooter.instance.gameManager.SpawnLocalBullet(owner, pos, vel);
        }



        public void readCallback(IAsyncResult ar)
        {
            NetworkClient client = (NetworkClient)ar.AsyncState;
            Socket handler = client.socket;
      
            if (!client.IsConnected())
            {
                try
                {
                    handler.Close();
                } catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return;
            }


            int read = handler.EndReceive(ar);
            
            if (read > 0)
            {
                ByteWrapper wrapper=new ByteWrapper(client.buffer);
                ReadPacket(wrapper, client);
                

                handler.BeginReceive(client.buffer, 0, BufferSize, 0, new AsyncCallback(readCallback), client);
            }
            else
            {
                handler.Close();
            }
        }

        public bool ReadPacket(ByteWrapper wrapper,NetworkClient client)
        {
            byte packetID = wrapper.ReadByte();

            ClientPacket? packet = PacketManager.GetClientPacket(packetID);
       
            if (packet == null)
            {
                Console.WriteLine("Client sent invalid packet id " + packetID);

                return false;
            }
            Console.WriteLine("Client sent " + packet.GetType().Name);

            packet.Read(wrapper);

            packet.Process(client);
            return true;
        }


        

        public void Send(NetworkClient client, byte[] data)
        {

            if (!client.IsConnected())
            {
                client.CloseConnection();
                return;
            }

            client.socket.BeginSend(data, 0, data.Length, 0,
                new AsyncCallback(SendCallback), client);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {

                NetworkClient client = (NetworkClient)ar.AsyncState;
                
        
                int bytesSent = client.socket.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);

                if(client.kicked)
                {
                    client.socket.Shutdown(SocketShutdown.Both);
                    client.socket.Close();
                }

 

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
