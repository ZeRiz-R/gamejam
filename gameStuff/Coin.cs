using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace CelesteLike
{
    internal class Coin : AnimatedSprite
    {

        private VisualEffect collectEffect;

        public Coin(Vector2 thePosition)
            : base(assetName: "Water", thePosition, width: 24, height: 16, frameWidth: 28, frameHeight: 24)
        {
            threshold = 40;
            collectEffect = new VisualEffect("waterCollect2", this.position, 26, 26, 32, 32);
        }

        public override void LoadContent(ContentManager theContentManager)
        {
            base.LoadContent(theContentManager);
            collectEffect.LoadContent(theContentManager);
        }


        public void Update(GameTime theGameTime, Player player)
        {
            if (collisionBox.Intersects(player.collisionBox) && show)
            {
                player.CollectCoin();
                SoundBank.collect_coin.PlayEndSound();
                show = false;
            }

            if (!show && collectEffect.animationEnd == false)
            {
                collectEffect.Animate(theGameTime);
            }

            Animate(theGameTime);
            base.Update();
        }

        public override void Draw(SpriteBatch theSpriteBatch)
        {
            base.Draw(theSpriteBatch);
            if (!show)
            {
                collectEffect.Draw(theSpriteBatch, position);
            }
        }

        public void Reset()
        {
            show = true;
            currentFrame = 0;
            collectEffect.Reset();
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
