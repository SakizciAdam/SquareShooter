﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareShooter.Network.Packet
{
    public abstract class ClientPacket : NetworkPacket
    {
        public abstract void Process(NetworkClient client);
    }
}
