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
    internal class MenuScreen : Screen
    {
        private Vector2 bgStart = new Vector2(-Game1.screenWidth, 0);
        private float bgSpeed = 1;
        private Sprite title;
        private Button playGame;
        private Button instructions;

        public MenuScreen()
        {
            background = new Sprite("menuScreen2", bgStart, 960, 544);
            title = new Sprite("Title", new Vector2(20, 70), 218, 96);
            playGame = new Button("playGameButton", new Vector2(140, 210), 120, 13, false);
            instructions = new Button("instructionButton", new Vector2(140, 230), 157, 13, false);
            buttons.Add(playGame);
            buttons.Add(instructions);
        }

        public override void LoadContent(ContentManager theContentManager)
        {
            background.LoadContent(theContentManager);
            title.LoadContent(theContentManager);
            base.LoadContent(theContentManager);
        }

        public override void Update(GameTime theGameTime)
        {
            MusicBank.menuMusic.PlayMusic();
            MoveBackground();
            base.Update(theGameTime);
            if (playGame.clicked == true)
            {
                nextScreen = ScreenManager.gameScreen;
            }
            if (instructions.clicked == true)
            {
                nextScreen = ScreenManager.instructionsScreen;
            }
        }

        public override void Draw(SpriteBatch theSpriteBatch)
        {
            base.Draw(theSpriteBatch);
            title.Draw(theSpriteBatch);
        }

        private void MoveBackground()
        {
            background.position.X += bgSpeed;
            background.position.Y -= bgSpeed;
            if (background.position.Y <= -272)
            {
                background.position.X = bgStart.X;
                background.position.Y = bgStart.Y;
            }
        }

        public void Sleep()
        {
            nextScreen = null;
            background.position = bgStart;
        }

        public void StartWakeUp()
        {
            nextScreen = null;
            background.position = bgStart;
        }

        public void WakeUp()
        {

        }
    }
}
