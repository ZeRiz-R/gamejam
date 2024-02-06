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
    internal class Screen
    {
        // Splash Screen, Main Menu, Instructions, Game, ... 

        protected Sprite background;
        protected List<Button> buttons;
        protected Screen nextScreen;

        public Screen()
        {
            nextScreen = null;
            buttons = new List<Button>();
        }

        public virtual void LoadContent(ContentManager theContentManager)
        {
            foreach (var button in buttons)
            {
                button.LoadContent(theContentManager);
            }
        }

        public virtual void Update(GameTime theGameTime)
        {
            foreach (var button in buttons)
            {
                button.Update(theGameTime);
            }
        }

        public virtual void Draw(SpriteBatch theSpriteBatch)
        {
            background.Draw(theSpriteBatch);
            foreach (var button in buttons)
            {
                button.Draw(theSpriteBatch);
            }
        }

        public virtual Screen GetNextScreen()
        {
            // If button pressed, update next screen to button press screen and return
            // else, return null

            return nextScreen;
        }

        // Code for screen manager to execute when screen switches
        public void StartWakeUp()
        {

        }

        // Code to execute when waking up
        private void WakeUp()
        {

        }

        public void Sleep()
        {
            nextScreen = null;
            foreach (var button in buttons)
            {
                button.UnclickButton();
            }    
        }
    }
}
