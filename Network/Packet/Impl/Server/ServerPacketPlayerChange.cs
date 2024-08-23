using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SquareShooter.Network.Packet;
using SquareShooter.Screen;

namespace SquareShooter.Network.Packet.Impl.Server
{
    public class ServerPacketPlayerChange : ServerPacket
    {

        public string username="";
        public bool added=false;

        public ServerPacketPlayerChange() { 
           
        }

        public ServerPacketPlayerChange(string username,bool added) { 
            this.username= username;
            this.added= added;
        }

        public override byte getPacketID()
        {
            return 3;
        }

        public override void Process(GameClient client)
        {
            if(added == SquareShooter.instance.gameManager.HasPlayer(username))
            {
                return;
            }
            if (added)
            {
                SquareShooter.instance.gameManager.AddNetworkPlayer(username);
                return;
            }
            //%100 exists no need to null check
            SquareShooter.instance.gameManager.RemovePlayer(SquareShooter.instance.gameManager.GetPlayer(username));    

            
        }

        public override void Read(ByteWrapper wrapper)
        {
            this.username=wrapper.ReadString();
            this.added = wrapper.ReadByte() == (byte)1;
        }

        public override void Write(ByteWrapper wrapper)
        {
            wrapper.WriteString(username);
            wrapper.WriteByte(this.added ? (byte)1 : (byte)0);
        }
    }
}
