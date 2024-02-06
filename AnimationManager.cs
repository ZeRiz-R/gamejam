using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelesteLike
{
    internal class AnimationManager
    {
        private Animation animation;
        private float timer;
        public Vector2 position { get; set; }
        public Vector2 origin { get; set; }
        public float rotation { get; set; }
        public AnimationManager()
        {
        }

        public void Play(Animation theAnimation)
        {
            if (animation == theAnimation) // Prevents restarting a mid progress animation
            {
                return;
            }

            animation = theAnimation;
            animation.currentFrame = 0;
            origin = new Vector2(animation.frameWidth / 2, animation.frameHeight / 2);
            timer = 0;
        }

        public void Stop()
        {
            timer = 0;
            animation.currentFrame = 0;
        }

        public void Update(GameTime theGameTime)
        {
            timer += (float)theGameTime.ElapsedGameTime.TotalSeconds;

            if (timer > animation.frameSpeed && !animation.finished)
            {
                timer = 0;
                animation.currentFrame += 1;

                if (animation.currentFrame >= animation.frameCount)
                {
                    animation.currentFrame = 0;
                    if (animation.isLooping == false)
                    {
                        animation.finished = true;
                    }
                }
            }
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            theSpriteBatch.Draw(animation.texture, position, 
                                new Rectangle(animation.currentFrame * animation.frameWidth, 0,
                                              animation.frameWidth, animation.frameHeight), Color.White, rotation, origin, 1.0f, SpriteEffects.None, 0);
        }
    }
}
