using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareShooter.Network.Packet
{
    public abstract class NetworkPacket
    {
        public NetworkPacket() { 
        }



        public abstract void Write(ByteWrapper wrapper);
        public abstract void Read(ByteWrapper wrapper);
        

        public abstract byte getPacketID();
    }
}
