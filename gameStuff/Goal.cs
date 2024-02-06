using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelesteLike.gameStuff
{
    internal class Goal : AnimatedSprite
    {
        public Goal(Vector2 thePosition) : 
            base(assetName: "nebulaGoal2", thePosition, width: 60, height: 60, frameWidth: 100, frameHeight: 100)
        {
            threshold = 8;
        }

        private bool goalReached;
        public bool levelComplete { get; set; }
        private float levelEndTimer = 0; // Delay until the level actually ends

        public void Update(GameTime theGameTime, Player player)
        {
            if (collisionBox.Intersects(player.collisionBox))
            {
                goalReached = true;
                player.GoalReached();
            }

            if (goalReached)
            {
                levelEndTimer += (float)theGameTime.ElapsedGameTime.TotalSeconds;
                if (levelEndTimer > 2)
                {
                    levelComplete = true;
                }
            }
            base.Update();

            Animate(theGameTime);
        }

        public void Reset()
        {
            goalReached = false;
            levelComplete = false;
            levelEndTimer = 0;
        }

        protected override void Animate(GameTime theGameTime)
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
    }
}
