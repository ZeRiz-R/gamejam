using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelesteLike
{
    public class Sound
    {
        private SoundEffect soundFX; // The sound
        private string fileLocation;
        //private bool soundLock;

        public Sound(string fileLocation, ref List<Sound> soundList)
        {
            this.fileLocation = fileLocation;
            soundList.Add(this);
        }

        public void LoadContent(ContentManager theContentManager)
        {
            soundFX = theContentManager.Load<SoundEffect>(fileLocation);
        }

        // SOUNDS MUST BE BOTHED PLAYED AND ENDED
        
        public void PlaySound(ref bool soundLock)
        {
            if (!soundLock)
            {
                soundFX.Play();
                soundLock = true;
            }
        }

        public void EndSound(ref bool soundLock) // Resets sound lock so sound can be played again
        {
            soundLock = false;
        }

        // Method for when the sound can't be played multiple times (or it does not matter)
        public void PlayEndSound()
        {
            soundFX.Play();
        }

    }
}
