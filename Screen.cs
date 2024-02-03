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

        Sprite Background;
        List<Button> buttons;


        public virtual void LoadContent(ContentManager theContentManager)
        {

        }

        public virtual void Update()
        {

        }

        public virtual void Draw(SpriteBatch theSpriteBatch)
        {

        }

        public Screen GetNextScreen()
        {
            // If button pressed, update next screen to button press screen and return
            // else, return null

            return null;
        }
    }
}
