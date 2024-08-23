using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SquareShooter.Network.Packet;
using SquareShooter.Screen;

namespace SquareShooter.Network.Packet.Impl.Server
{
    public class ServerPacketDisconnect : ServerPacket
    {

        public string message;

        public ServerPacketDisconnect() { }

        public ServerPacketDisconnect(string message) { 
            this.message = message;
        }

        public override byte getPacketID()
        {
            return 1;
        }

        public override void Process(GameClient client)
        {
            Console.WriteLine(message);
            client.Stop();
            SquareShooter.instance.currentScreen = new KickedScreen(SquareShooter.instance, message);
        }

        public override void Read(ByteWrapper wrapper)
        {
            this.message = wrapper.ReadString();
        }

        public override void Write(ByteWrapper wrapper)
        {
            wrapper.WriteString(this.message);
        }
    }
}
