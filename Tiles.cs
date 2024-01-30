using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace CelesteLike
{
    internal class Tiles
    {
        private static int tileWidth = 16;
        public static int TileWidth
        {
            get { return tileWidth; }
        }

        private static int[,] tileMap;

        // Allows other classes to access the tilemap
        public static int[,] TileMap
        {
            get { return tileMap; }
        }

        Sprite tileset;

        public Tiles()
        {
            tileset = new Sprite("testmap");
            FillTileMap("TextFiles/testTileMap.csv");
        }

        public void LoadContent(ContentManager theContentManager)
        {
            tileset.LoadContent(theContentManager);
        }

        private static int[,] heightArray = new int[3, 16] { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                      { 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16 },
                                                      { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 } };

        public static int[,] HeightArray { get { return heightArray; } }

        private static int[,] widthArray = new int[3, 16] { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                      { 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16 },
                                                      { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 } };
        public static int[,] WidthArray { get { return widthArray; } }

        private static int[] angleArray = new int[3] { 0, 0, 45 };

        private void FillTileMap(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);
            var tempSplit = lines[0].Split(","); // splits the first line to figure out columns
            tileMap = new int[lines.Length, tempSplit.Length];
            int count = 0;

            foreach (var line in lines)
            {
                var split = line.Split(",");
                for (int i = 0; i < split.Length; i++)
                {
                    tileMap[count, i] = Convert.ToInt32(split[i]);
                    Debug.Write(tileMap[count, i] + ",");
                }
                count += 1;
                Debug.WriteLine("\n");
            }
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            tileset.Draw(theSpriteBatch);
        }
    }
}
