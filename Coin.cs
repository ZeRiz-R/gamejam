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
        public Coin(Vector2 thePosition, int width, int height) : base(thePosition, width, height)
        {
        }

        public override void LoadContent(ContentManager theContentManager)
        {
            animationBook = new Dictionary<string, Animation>();
            animationBook.Add("Idle", LoadAnimation(theContentManager, "Water", 6, 0.05f));
            base.LoadContent(theContentManager);
            SetAnimation("Idle");
        }

        public void Update(GameTime theGameTime)
        {
            animationManager.Update(theGameTime);
            base.Update();
        }
    }
}
