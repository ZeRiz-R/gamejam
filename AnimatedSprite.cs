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

        public AnimatedSprite(Vector2 thePosition, int width, int height) : base(null, thePosition, width, height)
        {
            animationManager = new AnimationManager();
        }

        // ANIMATED STUFF
        protected AnimationManager animationManager;
        protected Dictionary<String, Animation> animationBook;

        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
                if (animationManager != null)
                {
                    animationManager.position = position;
                    animationManager.origin = origin;
                    animationManager.rotation = rotation;
                }
            }
        }

        protected virtual void SetAnimation(string theAnimation)
        {
            animationManager.Play(animationBook[theAnimation]);
        }

        public override void Draw(SpriteBatch theSpriteBatch)
        {
            animationManager.Draw(theSpriteBatch);
        }


        protected Animation LoadAnimation(ContentManager theContentManager, string assetName, int frames, float frameSpeed)
        {
            return new Animation(theContentManager.Load<Texture2D>(assetName), frames, frameSpeed);
        }

    }
}
