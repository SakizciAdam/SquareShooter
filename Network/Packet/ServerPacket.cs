using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareShooter.Network.Packet
{
    public abstract class ServerPacket : NetworkPacket
    {
        public abstract void Process(GameClient client);
    }
}
