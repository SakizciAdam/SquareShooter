using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using SquareShooter.Game.Block;
using SquareShooter.Game.Player;
using SquareShooter.Network.Packet;
using SquareShooter.Screen;

namespace SquareShooter.Network.Packet.Impl.Server
{
    public class ServerPacketStart : ServerPacket
    {



        public Dictionary<string,Vector2> map=new Dictionary<string,Vector2>();
        public List<Wall> walls = new List<Wall>();
        public ServerPacketStart() { 
     
        }

        public ServerPacketStart(int x,int y, List<GamePlayer> players,List<Wall> walls) {
  
            foreach (GamePlayer p in players)
            {
                map[p.username] = p.position;
            }
            this.walls = walls;
        }

        public ServerPacketStart(List<GamePlayer> players, List<Wall> walls)
        {
      

            foreach (GamePlayer p in players)
            {
                map[p.username]=p.position;
            }
            this.walls = walls;
        }

        public override byte getPacketID()
        {
            return 4;
        }

        public override void Process(GameClient client)
        {
            foreach (KeyValuePair<string, Vector2> entry in map)
            {
                var player = SquareShooter.instance.gameManager.GetPlayer(entry.Key);
                player.position = entry.Value;

                if(player is NetworkPlayer)
                {
                    ((NetworkPlayer)player).MatchPosition();
                }
            }
            SquareShooter.instance.gameManager.walls=this.walls;
            //SquareShooter.instance.gameManager.getMyPlayer().position=new System.Numerics.Vector2((float)x, (float)y);
            SquareShooter.instance.currentScreen = new PlayScreen(SquareShooter.instance);
            
        }

        public override void Read(ByteWrapper wrapper)
        {
  ;

            int len=wrapper.ReadInt();  

            for(int i = 0;i < len; i++)
            {
                string username=wrapper.ReadString();
                int Px = wrapper.ReadInt();
                int Py = wrapper.ReadInt();
                map[username]=new Vector2(Px, Py);
            }

            int len2 = wrapper.ReadInt();
            
            for (int i = 0; i < len2; i++)
            {
                walls.Add(new Wall(new Raylib_cs.Rectangle(wrapper.ReadInt(), wrapper.ReadInt(), wrapper.ReadInt(), wrapper.ReadInt())));
            }
        }

        public override void Write(ByteWrapper wrapper)
        {
   

            wrapper.WriteInt(map.Count);

            foreach (KeyValuePair<string, Vector2> entry in map)
            {
                wrapper.WriteString(entry.Key);
                wrapper.WriteInt((int)entry.Value.X);
                wrapper.WriteInt((int)entry.Value.Y);
            }

            wrapper.WriteInt(walls.Count);

            foreach(Wall wall in walls)
            {
                wrapper.WriteInt((int)wall.rectangle.X);
                wrapper.WriteInt((int)wall.rectangle.Y);
                wrapper.WriteInt((int)wall.rectangle.Width);
                wrapper.WriteInt((int)wall.rectangle.Height);
            }

        }
    }
}
