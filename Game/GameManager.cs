using System;
using System.Collections.Generic;

using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Raylib_cs;
using SquareShooter.Game.Block;
using SquareShooter.Game.Player;
using SquareShooter.Game.Projectile;
using SquareShooter.Network;
using SquareShooter.Screen;
using SquareShooter.Utils;

namespace SquareShooter.Game
{
    public class GameManager
    {

        private readonly SquareShooter squareShooter;
        private List<GamePlayer> players=new List<GamePlayer>();
        public List<GamePlayer> Players { get { return players; } }

        public List<Bullet> bullets=new List<Bullet>();
        public List<Wall> walls = new List<Wall>();

        public bool isHost = false,connected=false;

        public GameServer gameServer;
        public GameClient gameClient;

        public GameManager(SquareShooter squareShooter) { 
            this.squareShooter = squareShooter;
        }

        public void SpawnLocalWall(Rectangle rectangle)
        {
            walls.Add(new Wall(rectangle));
        }

        public void SpawnLocalBullet(string owner,Vector2 pos,Vector2 velocity)
        {
            bullets.Add(new Bullet(owner, pos,velocity));
        }

        public GamePlayer? GetPlayer(string username)
        {
            return players.Find(x  => x.username == username);
        }

        public void RemovePlayer(GamePlayer player)
        {
            if(players.Contains(player))
            {
                Console.WriteLine("Removed " + player.username);
                players.RemoveAt(players.IndexOf(player));
                return;
            }

            Console.WriteLine("Could not remove " + player.username);
        
        }

        public void SetPosition()
        {
            int i = 0;
            foreach (GamePlayer player in players)
            {
                Vector2 vec = MathUtils.RotateAroundOrigin(new Vector2(512-24,512-24),Vector2.One*256,Raylib.DEG2RAD*180*i);
                player.position = vec;
                i++;
            }
        }

        public void StartGameHost()
        {
            SetPosition();
            Random random = new Random();
            for(int c = 0; c < 6; c++)
            {
                int size = random.Next(24) + 24;
                Rectangle rec = new Rectangle(random.Next(512-48) + 24, random.Next(512 - 48) + 24,size,size);
                SpawnLocalWall(rec);
            }
   
            

            gameServer.StartGameHost();
            SquareShooter.instance.currentScreen = new PlayScreen(SquareShooter.instance);
        }

        public void AddMyPlayer(String username)
        {
            if (isHost)
            {
                AddPlayer(new HostClientPlayer(username));
                return;
            }
            AddPlayer(new ClientPlayer(username));
        }

        public NetworkPlayer AddNetworkPlayer(string player)
        {
            var x = new NetworkPlayer(player);
            players.Add(x);
            return x;
        }

        public void AddPlayer(GamePlayer player)
        {
            players.Add(player);
        }

        public void RestartGame()
        {
            for (int i = 0; i < players.Count; i++)
            {
                players[i].health = GamePlayer.MaxHealth;
            }
            SetPosition();
            bullets.Clear();
            gameServer.StartGameHost();
            SquareShooter.instance.currentScreen = new PlayScreen(SquareShooter.instance);
        }

        public void Update()
        {
            int deadCount = 0;
            for (int i = 0;i<players.Count;i++)
            {
                if (players[i].health <= 0)
                {
                    deadCount++;
                }
                players[i].Update();
            }

            if(isHost&&deadCount >= players.Count-1&&players.Count>1&&SquareShooter.instance.currentScreen is PlayScreen)
            {
                
                RestartGame();
            }
            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Update();
            }

        }

        public ClientPlayer? getMyPlayer()
        {

            return players.Find(x => x is ClientPlayer) as ClientPlayer;
        }

  

        public void updateNetwork(GamePlayer player)
        {

        }


        public List<string> GetPlayerNameList()
        {
            List<string> list = new List<string>();

            players.ForEach(gg => list.Add(gg.username))
            ;
            return list;
        }
        

        public void host(string username)
        {
            this.gameServer = new GameServer();
            this.gameServer.StartServer();
            isHost = true;
            connected = true;
            this.squareShooter.currentScreen = new HostScreen(squareShooter, username);
            AddMyPlayer(username);
        }

        public void start()
        {
            squareShooter.currentScreen = new PlayScreen(squareShooter);
            
        }

        public bool HasPlayer(string username)
        {
            return players.Find(x => x.username == username)!=null;   
        }

        public void OnConnect()
        {
            



     
            connected = true;

      
            start();
        }

        public void LostConnection()
        {
            try
            {
                this.gameClient.Stop();
                this.gameClient = null;
            }
            catch (Exception e)
            {
  
            

            }
            SquareShooter.instance.currentScreen = new KickedScreen(SquareShooter.instance, "Lost connection");
        }

        public void tryToConnect(string ip,string username)
        {
            squareShooter.currentScreen = new ConnectingScreen(squareShooter);
            this.gameClient=new GameClient();
            this.gameClient.username = username;
            Console.WriteLine("Connecting to " + ip);
            try
            {
                this.gameClient.Start(ip);
            } catch (Exception e)
            {
                this.gameClient.Stop();
                this.gameClient = null;
                squareShooter.currentScreen = new KickedScreen(squareShooter, "Failed to connect");

            }
          
                
            //Connecting... connecting..
            
                
           
            
        }


    }
}
