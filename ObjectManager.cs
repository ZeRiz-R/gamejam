using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CelesteLike
{
    internal class ObjectManager
    {
        private List<Coin> coins;
        public ObjectManager()
        {

        }

        public void LoadContent(ContentManager theContentManager)
        {
            coins = new List<Coin>();
            AddObjectFromTileMap("TextFiles/test4_GameObjects.csv");

            foreach (var coin in coins)
            {
                coin.LoadContent(theContentManager);
            }
        }

        public void Update(GameTime theGameTime, Player player)
        {
            foreach (var coin in coins)
            {
                coin.Update(theGameTime, player);
            }
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            foreach (var coin in coins)
            {
                coin.Draw(theSpriteBatch);
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

        public void Reset()
        {
            foreach (var coin in coins) { coin.Reset(); }
        }
    }
}
