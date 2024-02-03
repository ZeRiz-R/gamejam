using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelesteLike
{
    internal class Animation
    {
        public int currentFrame { get; set; }
        public int frameCount { get; private set; }
        public int frameHeight { get { return texture.Height; } }
        public float frameSpeed { get; set; }
        public int frameWidth { get { return texture.Width / frameCount; } }
        public bool isLooping { get; set; }
        public Texture2D texture { get; private set; }

        public Animation(Texture2D texture, int frameCount, float frameSpeed)
        {
            this.texture = texture;
            this.frameCount = frameCount;
            isLooping = true;
            this.frameSpeed = frameSpeed;
        }
    }
}

