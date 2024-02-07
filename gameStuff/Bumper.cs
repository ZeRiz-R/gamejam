using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CelesteLike
{
    internal class Bumper : AnimatedSprite
    {
        private int direction;
        private int newSpeed;
        private bool bumperSoundLock = false;

        public Bumper(Vector2 thePosition)
            : base(assetName: "Bumper2", thePosition, width: 32, height: 9, frameWidth: 32, frameHeight: 32)
        {
            origin = new Vector2(objectWidth / 2, frameHeight - (objectHeight / 2));
            collisionBox = new Rectangle((int)position.X - objectWidth / 2, (int)position.Y - objectHeight / 2, objectWidth, objectHeight);
            direction = 0;
            newSpeed = 10;

            threshold = 40;
            playAnimation = false;
            currentFrame = 0;
        }


        public void Update(GameTime theGameTime, Player player)
        {
            if (collisionBox.Intersects(player.collisionBox))
            {
                SoundBank.bumper.PlaySound(ref bumperSoundLock); // Remember to end sound

                player.Bump(newSpeed, direction);
                playAnimation = true;
            }
            else
            {
                SoundBank.bumper.EndSound(ref bumperSoundLock);
            }

            if (playAnimation)
            {
                Animate(theGameTime);
            }

            base.Update();
        }


        protected override void Animate(GameTime theGameTime)
        {
            // 9 Frames
            if (timer > threshold)
            {
                currentFrame += 1;
                if (currentFrame >= numFrames)
                {
                    currentFrame = 0;
                    playAnimation = false;
                }
                prevFrame = currentFrame;
                timer = 0;
            }

            sourceRectangle = frames[currentFrame];
            timer += (float)theGameTime.ElapsedGameTime.TotalMilliseconds;
        }

    }
}
