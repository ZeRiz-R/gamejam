using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelesteLike
{
    internal class ScreenManager
    {
        private Screen currentScreen;
        private Screen nextScreen;

        private Screen splashScreen;
        private Screen titleScreen;
        private Screen menuScreen;
        private Screen instructionsScreen;
        private Screen gameScreen;

        Button testButton;

        private List<Screen> allScreens;

        public ScreenManager()
        {
            splashScreen = new Screen();
            titleScreen = new Screen();
            menuScreen = new Screen();
            instructionsScreen = new Screen();
            gameScreen = new Screen();

            allScreens = new List<Screen>();
        }

        public void LoadContent(ContentManager theContentManager)
        {
            foreach (var screen in allScreens)
            {
                screen.LoadContent(theContentManager);
            }

            currentScreen = menuScreen;
        }

        public void Update()
        {
            currentScreen.Update();
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            currentScreen.Draw(theSpriteBatch);
        }

        private void ChangeScreen()
        {
            nextScreen = currentScreen.GetNextScreen();
            if (nextScreen != null)
            {
                currentScreen = nextScreen;
                nextScreen = null;
            }
        }
    }
}
