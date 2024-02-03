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
        private List<LayerSwitcher> layerSwitchers;
        public ObjectManager()
        {

        }

        public void LoadContent(ContentManager theContentManager)
        {
            layerSwitchers = AddLayerSwitcherFromTileMap("TextFiles/test4L1_LayerSwitchers.csv");
            foreach (var layerSwitcher in layerSwitchers)
            {
                layerSwitcher.LoadContent(theContentManager);
            }
        }

        public void Update()
        {
            foreach (var layerSwitcher in layerSwitchers)
            {
                layerSwitcher.Update(1, 1, Rectangle.Empty);
            }
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            foreach (var layerSwitcher in layerSwitchers)
            {
                layerSwitcher.Draw(theSpriteBatch);
            }
        }

        private List<LayerSwitcher> AddLayerSwitcherFromTileMap(string fileName)
        {
            List<LayerSwitcher> tempList = new List<LayerSwitcher>();
            string[] lines = File.ReadAllLines(fileName);
            var tempSplit = lines[0].Split(","); // splits the first line to figure out columns
            int count = 0;

            foreach (var line in lines)
            {
                var split = line.Split(",");
                for (int i = 0; i < split.Length; i++)
                {
                    int value = Convert.ToInt32(split[i]);
                    if (value == 1)
                    {
                        tempList.Add(new LayerSwitcher(LayerSwitcher.SwitchDirection.left,
                            new Vector2(i * Tiles.TileWidth, count * Tiles.TileWidth), 16, 16));
                    }
                    else if (value == 2)
                    {
                        tempList.Add(new LayerSwitcher(LayerSwitcher.SwitchDirection.up,
                            new Vector2(i * Tiles.TileWidth, count * Tiles.TileWidth), 16, 16));
                    }
                    //Debug.Write(array[count, i] + ",");
                }
                count += 1;
                //Debug.WriteLine("\n");
            }

            return tempList;
        }
    }
}
