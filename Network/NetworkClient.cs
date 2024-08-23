using SquareShooter.Game.Player;
using SquareShooter.Network.Packet;
using SquareShooter.Network.Packet.Impl.Server;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SquareShooter.Network
{
    public class NetworkClient
    {
        public Socket socket = null;
        public byte[] buffer = new byte[GameServer.BufferSize];
        public bool kicked = false;
        private Random random = new Random();
        public Stopwatch pingWatch = new Stopwatch();
        public Stopwatch stopwatch = Stopwatch.StartNew();
        public Stopwatch lastBullet = Stopwatch.StartNew();

        public bool loggedIn;

        public NetworkPlayer player;





            
        public void CloseConnection()
        {
            Console.WriteLine("Closing connection");
            if(loggedIn)
            {
                SquareShooter.instance.gameManager.RemovePlayer(player);
            }
            try
            {
       
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            } catch(Exception ex)
            {

            }
         
            socket = null;
            
            SquareShooter.instance.gameManager.gameServer.clients.Remove(this);
            if (loggedIn)
            {
                SquareShooter.instance.gameManager.gameServer.UpdatePlayers(player.username, false);
            }
          
        }

        public void SendPlayer(GamePlayer player)
        {
            ServerPacketPlayerPosition packetPlayerPosition = new ServerPacketPlayerPosition(player.username,player.position,player.rotation);

            SendPacket(packetPlayerPosition);
        }

        public bool IsConnected()
        {
            if (socket == null)
            {
                return false;
            }
            return socket.Connected;
        }

        public void Disconnect(string message = "Disconnected")
        {
            if(IsConnected())
            {
                SendPacket(new ServerPacketDisconnect(message));
            }
            
            kicked = true;
            CloseConnection();
        }

        public void SendPing()
        {
            Console.WriteLine("Sent ping");
            int r=random.Next(1000000);
            ServerPacketPing packet=new ServerPacketPing(r);
            SendPacket(packet);
            pingWatch.Start();
        }

        public void SendPacket(ServerPacket packet)
        {
            ByteWrapper wrapper = new ByteWrapper(new byte[1]);
            wrapper.WriteByte(packet.getPacketID());
            packet.Write(wrapper);
            SquareShooter.instance.gameManager.gameServer.Send(this, wrapper.data);
        }


    }
}
