using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelesteLike
{
    public static class MusicBank
    {
        private static List<Music> musicList;
        public static Music menuMusic; // Music options

        public static void LoadContent(ContentManager theContentManager)
        {
            musicList = new List<Music>();
            menuMusic = new Music("Uppercut (Menu)", ref musicList);

            foreach (Music music in musicList)
            {
                music.LoadContent(theContentManager);
            }
        }
    }
}
