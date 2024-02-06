using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelesteLike
{
    internal class VisualEffect : AnimatedSprite
    {
        public bool animationEnd { get; private set; }

        public VisualEffect(string assetName, Vector2 position, int width, int height, int frameWidth, int frameHeight) 
            : base(assetName, position, width, height, frameWidth, frameHeight )
        {
            animationEnd = false;
            threshold = 20;
        }

        public void Animate(GameTime theGameTime)
        {
            if (timer  > threshold)
            {
                currentFrame += 1;
                if (currentFrame >= numFrames)
                {
                    currentFrame = 0;
                    animationEnd = true;
                }
                prevFrame = currentFrame;
                timer = 0;
            }
            sourceRectangle = frames[currentFrame];
            timer += (float)theGameTime.ElapsedGameTime.TotalMilliseconds;
        }

        public void Draw(SpriteBatch theSpriteBatch, Vector2 newPosition)
        {
            position = newPosition;
            if (!animationEnd)
            {
                base.Draw(theSpriteBatch);
            }
        }

        public void Reset()
        {
            currentFrame = 0;
            animationEnd = false;
        }
    }
}
