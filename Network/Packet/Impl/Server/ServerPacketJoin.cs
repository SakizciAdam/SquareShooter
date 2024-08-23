using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SquareShooter.Network.Packet;
using SquareShooter.Screen;

namespace SquareShooter.Network.Packet.Impl.Server
{
    public class ServerPacketJoin : ServerPacket
    {

        public List<string> players;

        public ServerPacketJoin() { 
            this.players= new List<string>();   
        }

        public ServerPacketJoin(List<string> players) { 
            this.players = players;
        }

        public override byte getPacketID()
        {
            return 0;
        }

        public override void Process(GameClient client)
        {
            foreach (var player in players)
            {
                if (player == SquareShooter.instance.gameManager.gameClient.username)
                {
                    SquareShooter.instance.gameManager.AddMyPlayer(SquareShooter.instance.gameManager.gameClient.username);
                    continue;
                }
                SquareShooter.instance.gameManager.AddNetworkPlayer(player);
            }
            SquareShooter.instance.currentScreen = new HostScreen(SquareShooter.instance, "");
            
        }

        public override void Read(ByteWrapper wrapper)
        {
            int count = wrapper.ReadInt();

            for(int i = 0; i < count; i++)
            {
                players.Add(wrapper.ReadString());
            }
        }

        public override void Write(ByteWrapper wrapper)
        {
            wrapper.WriteInt(players.Count);

            foreach(var player in players)
            {
                wrapper.WriteString(player);
            }
        }
    }
}
