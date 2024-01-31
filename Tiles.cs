using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System.Threading;

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
            tileset = new Sprite("theRat");
            FillArrays("TextFiles/testMap2.csv", "TextFiles/heightArray.txt", "TextFiles/widthArray.txt",
                "TextFiles/angleArray.txt");
        }

        public void LoadContent(ContentManager theContentManager)
        {
            tileset.LoadContent(theContentManager);
        }

        private static int[,] heightArray;

        public static int[,] HeightArray { get { return heightArray; } }

        private static int[,] widthArray;
        public static int[,] WidthArray { get { return widthArray; } }

        private static float[] angleArray;
        public static float[] AngleArray { get { return angleArray; } }

        private int[,] ReadTo2DArray(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);
            var tempSplit = lines[0].Split(","); // splits the first line to figure out columns
            int[,] array = new int[lines.Length, tempSplit.Length];
            int count = 0;

            foreach (var line in lines)
            {
                var split = line.Split(",");
                for (int i = 0; i < split.Length; i++)
                {
                    array[count, i] = Convert.ToInt32(split[i]);
                    //Debug.Write(array[count, i] + ",");
                }
                count += 1;
                //Debug.WriteLine("\n");
            }

            return array;
        }

        private int[,] FillWidthArray(int[,] heightArray)
        {
            int[,] array = new int[heightArray.GetLength(0), heightArray.GetLength(1)];

            for (int i = 0; i < array.GetLength(0); i++) // Iterate through each row
            {
                int currentHeight = tileWidth;
                for (int j = 0; j < array.GetLength(1); j++) // Iterate across each column
                {
                    int count = 0;
                    for (int k = tileWidth -1; k >= 0; k--) // Iterate across each index
                    {
                        if (heightArray[i, k] >= currentHeight)
                        {
                            count += 1;
                        }
                        else
                        {
                            k = -1;
                            currentHeight -= 1;
                        }
                    }
                    array[i, j] = count;
                }
            }

            return array;

        }
        
        private void FillArrays(string tileMapName, string heightArrayName, string widthArrayName, string angleArrayName)
        {
            tileMap = ReadTo2DArray(tileMapName);
            heightArray = ReadTo2DArray(heightArrayName);
            //widthArray = ReadTo2DArray(widthArrayName);
            widthArray = FillWidthArray(heightArray);

            string[] lines = File.ReadAllLines(angleArrayName);

            angleArray = new float[lines.Length];

            int count = 0;
            foreach (var line in lines)
            {
                angleArray[count] = float.Parse(line);
                count += 1;
                //Debug.WriteLine("\n");
            }
        }
        public void Draw(SpriteBatch theSpriteBatch)
        {
            tileset.Draw(theSpriteBatch);
        }
    }
}
