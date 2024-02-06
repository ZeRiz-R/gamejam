using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelesteLike
{
    internal class AnimatedSprite : Sprite
    {
        // Define animations inside inherited class
        protected int numFrames, frameWidth, frameHeight, rows, columns;
        protected float timer, threshold;
        protected float frameSpeed;
        protected Rectangle[] frames;
        protected int currentFrame, prevFrame;
        protected bool playAnimation;

        public AnimatedSprite(string assetName, Vector2 thePosition, int width, int height, int frameWidth
            , int frameHeight) : 
            base(assetName, thePosition, width, height)
        {     
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            origin = new Vector2(frameWidth / 2, frameHeight / 2);
            collisionBox = new Rectangle((int)position.X - (objectWidth / 2), (int)position.Y - (objectHeight / 2),
            objectWidth, objectHeight);
        }

        // ANIMATED STUFF
        
        public override void LoadContent(ContentManager theContentManager)
        {
            base.LoadContent(theContentManager);
            columns = spriteTexture.Width / frameWidth;
            rows = spriteTexture.Height / frameHeight;
            numFrames = rows * columns;
            frames = new Rectangle[numFrames];

            int currentRow = 0, currentColumn = 0;

            for (int i = 0; i < numFrames; i++)
            {
                if (currentColumn > columns )
                {
                    currentColumn = 0;
                    currentRow += 1;
                }
                frames[i] = new Rectangle(currentColumn * frameWidth, currentRow * frameHeight, frameWidth, frameHeight);
                currentColumn += 1;
            }
            sourceRectangle = frames[0];
        }

        protected virtual void Animate(GameTime theGameTime)
        {
            if (timer > threshold)
            {
                currentFrame += 1;
                if (currentFrame >= numFrames)
                {
                    currentFrame = 0;
                }
                prevFrame = currentFrame;
                timer = 0;
            }
            sourceRectangle = frames[currentFrame];
            timer += (float)theGameTime.ElapsedGameTime.TotalMilliseconds;
        }

        protected virtual void Update()
        {
            base.Update();
        }



        protected Animation LoadAnimation(ContentManager theContentManager, string assetName, int frames, float frameSpeed, bool looping)
        {
            return new Animation(theContentManager.Load<Texture2D>(assetName), frames, frameSpeed, looping);
        }

    }
}
