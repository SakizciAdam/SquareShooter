using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SquareShooter.Network.Packet;
using SquareShooter.Screen;

namespace SquareShooter.Network.Packet.Impl.Server
{
    public class ClientPacketPing : ClientPacket
    {

        public int id;

        public ClientPacketPing() { 
            this.id=0;   
        }

        public ClientPacketPing(int id) { 
            this.id=id;
        }

        public override byte getPacketID()
        {
            return 2;
        }
        
        public override void Process(NetworkClient client)
        {
            Console.WriteLine("Ping received");
            client.pingWatch.Restart();
       
        }

        public override void Read(ByteWrapper wrapper)
        {
            this.id = wrapper.ReadInt();

       
        }

        public override void Write(ByteWrapper wrapper)
        {
            wrapper.WriteInt(this.id);

        }
    }
}
