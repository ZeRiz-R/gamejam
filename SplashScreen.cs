using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelesteLike
{
    internal class SplashScreen : Screen
    {
        private float timer = 0;
        private float timeOnScreen = 4;
        public SplashScreen()
        {
            background = new Sprite("splashScreen", Vector2.Zero, 480, 270);

        }

        public override void Update(GameTime theGameTime)
        {
            nextScreen = null;
            timer += (float)theGameTime.ElapsedGameTime.TotalSeconds;
            if (timer > timeOnScreen)
            {
                nextScreen = ScreenManager.menuScreen;
                timer = 0;
            }
        }

        public override void LoadContent(ContentManager theContentManager)
        {
            background.LoadContent(theContentManager);
        }

        public override Screen GetNextScreen()
        {
            return nextScreen;
        }

    }
}
