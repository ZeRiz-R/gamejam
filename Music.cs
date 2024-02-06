using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelesteLike
{
    public class Music
    {
        private Song music; // The music
        private string fileLocation; // Path to file / file name
        private bool musicLock;

        public Music(string filelocation, ref List<Music> musicList)
        {
            this.fileLocation = filelocation;
            musicList.Add(this); // Adds the song to a stored music list
        }

        public void LoadContent(ContentManager theContentManager)
        {
            music = theContentManager.Load<Song>(fileLocation);
            MediaPlayer.IsRepeating = true; // Ensures the music repeats
        }

        // Allows for music to be played
        public void PlayMusic()
        {
            if (!musicLock)
            {
                MediaPlayer.Play(music);
                musicLock = true;
            }
        }

        public void EndMusic()
        {
            MediaPlayer.Stop();
            musicLock = false;
        }
    }
}
