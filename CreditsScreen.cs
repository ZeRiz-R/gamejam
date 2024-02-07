using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelesteLike
{
    internal class CreditsScreen : Screen
    {

        public CreditsScreen()
        {
            background = new Sprite("credits2", Vector2.Zero, 480, 810);
        }


        public override void LoadContent(ContentManager theContentManager)
        {
            background.LoadContent(theContentManager);
            base.LoadContent(theContentManager);
        }

        public override void Update(GameTime theGameTime)
        {
            UpdateCredits(theGameTime);
            base.Update(theGameTime);
        }

        public override void Draw(SpriteBatch theSpriteBatch)
        {
            Game1._graphics.GraphicsDevice.Clear(Color.Black);
            base.Draw(theSpriteBatch);
        }

        private float delayTimer = 0;
        private bool startMoving = false;
        private void UpdateCredits(GameTime theGameTime)
        {
            delayTimer += (float)theGameTime.ElapsedGameTime.TotalSeconds;
            if (delayTimer > 2)
            {
                delayTimer = 0;
                startMoving = true;
                
            }

            if (startMoving)
            {
                background.position.Y -= 1;
                if (background.position.Y <= -810)
                {
                    nextScreen = ScreenManager.menuScreen;
                    delayTimer = 0;
                }
            }

            KeyboardState key = Keyboard.GetState();
            if (key.IsKeyDown(Keys.Enter))
            {
                nextScreen = ScreenManager.menuScreen;
            }
        }

        public void Sleep()
        {
            background.position = new Vector2(0, 0);
            delayTimer = 0;
            startMoving = false;
            nextScreen = null;
        }
    }
}
