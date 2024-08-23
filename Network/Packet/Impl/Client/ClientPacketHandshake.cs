using SquareShooter.Network.Packet.Impl.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareShooter.Network.Packet.Impl.Client
{
    public class ClientPacketHandshake : ClientPacket
    {

        public string username;

        public ClientPacketHandshake() { }

        public ClientPacketHandshake(string username) { 
            this.username = username;
        }

        public override byte getPacketID()
        {
            return 0;
        }

        public override void Process(NetworkClient client)
        {
            if(SquareShooter.instance.gameManager.HasPlayer(username))
            {
                client.Disconnect("Player with the same username exists!");
                
                return;
            }
            if (SquareShooter.instance.gameManager.gameServer.clients.Count >= 2)
            {
                client.Disconnect("Lobby filled up!");
            }
          
            ServerPacketJoin packetJoin = new ServerPacketJoin();
            client.player=SquareShooter.instance.gameManager.AddNetworkPlayer(username);
            packetJoin.players=SquareShooter.instance.gameManager.GetPlayerNameList();
            client.SendPacket(packetJoin);
            client.loggedIn=true;
            SquareShooter.instance.gameManager.gameServer.UpdatePlayers(this.username, true);
        }

        public override void Read(ByteWrapper wrapper)
        {
            this.username=wrapper.ReadString();
        }

        public override void Write(ByteWrapper wrapper)
        {
            wrapper.WriteString(this.username);
        }
    }
}
