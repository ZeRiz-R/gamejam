using Microsoft.Xna.Framework;
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

        private static int[,] tileMapL1;
        private static int[,] tileMapL2;
        private static int[,] tileMapLS; // Layer Switcher

        // Allows other classes to access the tilemap
        public static int[,] TileMapL1
        {
            get { return tileMapL1; }
        }
        public static int[,] TileMapL2
        {
            get { return tileMapL2; }
        }
        public static int[,] TileMapLS
        {
            get { return tileMapLS; }
        }


        Sprite layer1;
        Sprite layer2;

        public Tiles()
        {
            layer1 = new Sprite("FinalLevel/level1UltraFinal", Vector2.Zero, 0, 0);
            layer2 = new Sprite("FinalLevel/level2Final", Vector2.Zero, 0, 0);
            FillArrays("TextFiles/FinalLevel/Level_Layer1.csv", "TextFiles/FinalLevel/Level_Layer2.csv",
                "TextFiles/FinalLevel/Level_LayerSwitcher.csv", "TextFiles/heightArray.txt", "TextFiles/widthArray.txt",
                "TextFiles/angleArray.txt");
        }

        public void LoadContent(ContentManager theContentManager)
        {
            layer1.LoadContent(theContentManager);
            layer2.LoadContent(theContentManager);
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
                    if (array[count, i] == -1)
                    {
                        array[count, i] = 0; // prevents empty spaces breaking the game
                    }

                    //Debug.Write(array[count, i] + ",");
                }
                count += 1;
                //Debug.WriteLine("\n");
            }

            return array;
        }

        private int[,] ReadToLSArray(string fileName)
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
                    for (int k = tileWidth - 1; k >= 0; k--) // Iterate across each index
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

        private void FillArrays(string tileMapL1Name, string tileMapL2Name, string tileMapLSName, string heightArrayName, string widthArrayName, string angleArrayName)
        {
            tileMapL1 = ReadTo2DArray(tileMapL1Name);
            tileMapL2 = ReadTo2DArray(tileMapL2Name);
            tileMapLS = ReadToLSArray(tileMapLSName);
            heightArray = ReadTo2DArray(heightArrayName);
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
        public void DrawLayer1(SpriteBatch theSpriteBatch)
        {
            layer1.Draw(theSpriteBatch);
        }
        public void DrawLayer2(SpriteBatch theSpriteBatch)
        {
            layer2.Draw(theSpriteBatch);
        }
    }
}
