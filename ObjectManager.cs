using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text.Json;
using System.Runtime.InteropServices;

namespace CelesteLike
{
    internal class ObjectManager
    {
        private List<Coin> coins;
        private List<Bumper> bumpers;
        private List<Spikes> spikes;
        public ObjectManager()
        {

        }

        public void LoadContent(ContentManager theContentManager)
        {
            coins = new List<Coin>();
            ReadCoinsFromJSON(ref coins);

            foreach (var coin in coins)
            {
                coin.LoadContent(theContentManager);
            }

            bumpers = new List<Bumper>();
            ReadBumpersFromJSON(ref bumpers);
            foreach ( var bumper in bumpers)
            {
                bumper.LoadContent(theContentManager);
            }

            spikes = new List<Spikes>();
            ReadSpikesFromJSON(ref spikes);
            foreach ( var spike in spikes)
            {
                spike.LoadContent(theContentManager);
            }
            
        }

        public void Update(GameTime theGameTime, Player player)
        {
            foreach (var coin in coins)
            {
                if (Math.Abs(player.position.X - coin.position.X) < 480)
                {
                    coin.Update(theGameTime, player);
                }
            }
            foreach (var bumper in bumpers)
            {
                if (Math.Abs(player.position.X - bumper.position.X) < 480)
                {
                    bumper.Update(theGameTime, player);
                }
            }
            foreach (var spike in spikes)
            {
                if (Math.Abs(player.position.X - spike.position.X) < 480)
                { 
                    spike.Update(player);
                }
            }
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            foreach (var coin in coins)
            {
                coin.Draw(theSpriteBatch);
            }
            foreach(var bumper in bumpers)
            {
                bumper.Draw(theSpriteBatch);
            }
            foreach (var spike in spikes)
            {
                spike.Draw(theSpriteBatch);
            }
        }

        private void AddObjectFromTileMap(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);
            var tempSplit = lines[0].Split(","); // splits the first line to figure out columns
            int count = 0;

            foreach (var line in lines)
            {
                var split = line.Split(",");
                for (int i = 0; i < split.Length; i++)
                {
                    int value = Convert.ToInt32(split[i]);
                    if (value == 0)
                    {
                        coins.Add(new Coin(new Vector2(i * Tiles.TileWidth, count * Tiles.TileWidth)));
                    }
                    //Debug.Write(array[count, i] + ",");
                }
                count += 1;
                //Debug.WriteLine("\n");
            }
        }

        public class GameObjects
        {
            public List<GameObject> objects { get; set; }
        }

        public class GameObject
        {
            public int gid { get; set; }
            public int height { get; set; }
            public int id { get; set; }
            public string name { get; set; }
            public int rotation { get; set; }
            public string type { get; set; }
            public bool visible { get; set; }
            public int width { get; set; }
            public float x { get; set; }
            public float y { get; set; }
        }

        private void ReadCoinsFromJSON(ref List<Coin> coinList)
        {
            var spawnCoin = new GameObjects();
            using (StreamReader r = new StreamReader("TextFiles/FinalLevel/coins.json.txt"))
            {
                string json = r.ReadToEnd();
                spawnCoin = JsonSerializer.Deserialize<GameObjects>(json);
                foreach (var coin in spawnCoin.objects)
                {
                    float coinX = coin.x; 
                    float coinY = coin.y;
                    coinList.Add(new Coin(new Vector2(coinX + 14, coinY - 12))); // Includes offset
                }
            }
        }

        private void ReadBumpersFromJSON(ref List<Bumper> bumperList)
        {
            var spawnBumper = new GameObjects();
            using (StreamReader r = new StreamReader("TextFiles/FinalLevel/bumpers.json.txt"))
            {
                string json = r.ReadToEnd();
                spawnBumper = JsonSerializer.Deserialize<GameObjects>(json);
                foreach (var coin in spawnBumper.objects)
                {
                    float bumperX = coin.x;
                    float bumperY = coin.y;
                    bumperList.Add(new Bumper(new Vector2(bumperX + 16, bumperY - 4))); // Includes offset
                }
            }

        }

        private void ReadSpikesFromJSON(ref List<Spikes> spikeList)
        {
            var spawnBumper = new GameObjects();
            using (StreamReader r = new StreamReader("TextFiles/FinalLevel/Spikes.json.txt"))
            {
                string json = r.ReadToEnd();
                spawnBumper = JsonSerializer.Deserialize<GameObjects>(json);
                foreach (var coin in spawnBumper.objects)
                {
                    float bumperX = coin.x;
                    float bumperY = coin.y;
                    spikeList.Add(new Spikes(new Vector2(bumperX, bumperY - 32))); // Includes offset
                }
            }

        }


        public void Reset()
        {
            foreach (var coin in coins) { coin.Reset(); }
        }
    }
}
