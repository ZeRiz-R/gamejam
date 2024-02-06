using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CelesteLike
{
    internal class GameScreen : Screen
    {
        private GameManager theGameManager; // Manages the game content

        public override void LoadContent(ContentManager theContentManager)
        {
            nextScreen = null;
            theGameManager = new GameManager();
            theGameManager.Initialise();

            theGameManager.LoadContent(theContentManager);
            base.LoadContent(theContentManager);
        }

        public override void Update(GameTime theGameTime)
        {
            theGameManager.Update(theGameTime);
            LevelCompleteCheck();
        }

        public override void Draw(SpriteBatch theSpriteBatch)
        {
            theGameManager.Draw(theSpriteBatch);
        }

        public override Screen GetNextScreen()
        {
            return nextScreen;
        }

        public void LevelCompleteCheck()
        {
            if (theGameManager.levelComplete == true)
            {
                nextScreen = ScreenManager.menuScreen;
                theGameManager.Reset();
            }
        }

        public void Sleep()
        {
            nextScreen = null;
        }
    }
}
