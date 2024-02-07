using CelesteLike.Content;
using CelesteLike.gameStuff;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace CelesteLike
{
    internal class ScreenManager
    {
        private RenderTarget2D renderTarget;
        int screenWidth = 480, screenHeight = 270;

        private Screen currentScreen;
        private Screen nextScreen;

        public static SplashScreen splashScreen { get; private set; }
        private Screen titleScreen;
        public static Screen menuScreen { get; private set; }
        public static Screen instructionsScreen;
        public static Screen gameScreen { get; private set; }
        public static Screen creditsScreen { get; private set; }

        private Sprite redTriangle;
        private Sprite blackTriangle;
        private float triangleSpeed = 1f;


        private List<Screen> allScreens;

        public ScreenManager()
        {
            splashScreen = new SplashScreen();
            titleScreen = new Screen();
            menuScreen = new MenuScreen();
            instructionsScreen = new InstructionScreen();
            gameScreen = new GameScreen();
            creditsScreen = new CreditsScreen();

            allScreens = new List<Screen>();
            allScreens.Add(splashScreen);
            allScreens.Add(menuScreen);
            allScreens.Add(gameScreen);
            allScreens.Add(creditsScreen);
            allScreens.Add(instructionsScreen);

            redTriangle = new Sprite("newRedT", new Vector2(-480, -20), 960, 68);
            blackTriangle = new Sprite("newBlackT", new Vector2(312, 0), 168, 552);

            currentScreen = menuScreen;

        }

        public void LoadContent(ContentManager theContentManager)
        {
            renderTarget = new RenderTarget2D(Game1._graphics.GraphicsDevice, screenWidth, screenHeight, false, Game1._graphics.PreferredBackBufferFormat,
    DepthFormat.Depth24);

            foreach (var screen in allScreens)
            {
                screen.LoadContent(theContentManager);
            }

            redTriangle.LoadContent(theContentManager);
            blackTriangle.LoadContent(theContentManager);
        }

        public void Update(GameTime theGameTime)
        {
            currentScreen.Update(theGameTime);
            ChangeScreen();
            UpdateTriangles();
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            if (currentScreen != gameScreen)
            {
                DrawScenetoTexture(Game1._graphics.GraphicsDevice, theSpriteBatch);

                theSpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp,
        DepthStencilState.Default, RasterizerState.CullNone);
                theSpriteBatch.Draw(renderTarget, new Rectangle(0, 0, 1920, 1080), Color.White);
                theSpriteBatch.End();
            }
            else
            {
                gameScreen.Draw(theSpriteBatch);
            }
        }

        private void DrawScenetoTexture(GraphicsDevice graphicsDevice, SpriteBatch theSpriteBatch)
        {
            graphicsDevice.SetRenderTarget(renderTarget);
            graphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };

            graphicsDevice.Clear(Color.FloralWhite);

            //Draw here
            theSpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            currentScreen.Draw(theSpriteBatch);
            DrawTriangles(theSpriteBatch);
            theSpriteBatch.End();


            graphicsDevice.SetRenderTarget(null);

        }


        private void ChangeScreen()
        {
            nextScreen = currentScreen.GetNextScreen();
            if (nextScreen != null)
            {
                currentScreen.Sleep();
                currentScreen = nextScreen;
                nextScreen = null;
            }
        }


        private void UpdateTriangles()
        {
            if (currentScreen == menuScreen)
            {
                redTriangle.position.X += triangleSpeed;
                if (redTriangle.position.X >= 0)
                {
                    redTriangle.position.X = -480;
                }

                blackTriangle.position.Y += -triangleSpeed;
                if (blackTriangle.position.Y <= -288)
                {
                    blackTriangle.position.Y = 0;
                }
            }
        }

        private void DrawTriangles(SpriteBatch theSpriteBatch)
        {
            if (currentScreen == menuScreen)
            {
                redTriangle.Draw(theSpriteBatch);
                blackTriangle.Draw(theSpriteBatch);
            }
        }

    }
}
