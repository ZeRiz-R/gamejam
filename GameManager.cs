using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System.Net.Security;
using CelesteLike;
using CelesteLike.gameStuff;
using System.Threading;

namespace CelesteLike
{
    internal class GameManager
    {
        private RenderTarget2D renderTarget;
        private static int screenWidth = 480, screenHeight = 270;

        public static int ScreenWidth { get { return screenWidth; } }
        public static int ScreenHeight {  get { return screenHeight; } }

        private ObjectManager theObjectManager;

        private Player ball;

        private Tiles level;

        private Camera camera;

        // TESTS
        private Sprite bg = new Sprite("purpleNebula6", new Vector2(0, 0), 512, 512);
        private Goal goal = new Goal(new Vector2(5856, 1450));

        public bool levelComplete { get; private set; }

        private float DeathTimer = 0;
        public void Initialise()
        {
            level = new Tiles();
            ball = new Player("earth2", Vector2.Zero, 30, 30);
            theObjectManager = new ObjectManager();
        }

        public void LoadContent(ContentManager theContentManager)
        {
            renderTarget = new RenderTarget2D(Game1._graphics.GraphicsDevice, screenWidth, screenHeight, false, Game1._graphics.PreferredBackBufferFormat, 
                DepthFormat.Depth24);
            camera = new Camera();

            theObjectManager.LoadContent(theContentManager);
            //TESTS
            goal.LoadContent(theContentManager);
            bg.LoadContent(theContentManager);
            bg.scale = 1.0f;
            //TESTS

            level.LoadContent(theContentManager);
            ball.LoadContent(theContentManager);
        }

        public void Update(GameTime theGameTime)
        {
            theObjectManager.Update(theGameTime, ball);
            ball.Update(theGameTime);
            camera.Follow2(ball);
            //TEST
            goal.Update(theGameTime, ball);

            LevelCompleteCheck();

            UpdatePlayerDeath(theGameTime);
        }

        private void DrawScenetoTexture(GraphicsDevice  graphicsDevice, SpriteBatch theSpriteBatch)
        {
            graphicsDevice.SetRenderTarget(renderTarget);
            graphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };

            graphicsDevice.Clear(Color.CornflowerBlue);

            theSpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null);
            bg.Draw(theSpriteBatch);
            theSpriteBatch.End();

            //Draw here

            theSpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, camera.Transform);
            level.DrawLayer2(theSpriteBatch);
            theObjectManager.Draw(theSpriteBatch);
            goal.Draw(theSpriteBatch);
            ball.Draw(theSpriteBatch);
            level.DrawLayer1(theSpriteBatch);
            theSpriteBatch.End();

            theSpriteBatch.Begin();
            //ball.VariableDisplay(theSpriteBatch, Game1.font);
            ball.UIDisplay(theSpriteBatch, Game1.font);
            theSpriteBatch.End();


            graphicsDevice.SetRenderTarget(null);
            
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            DrawScenetoTexture(Game1._graphics.GraphicsDevice, theSpriteBatch);

            theSpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp,
                DepthStencilState.Default, RasterizerState.CullNone);
            theSpriteBatch.Draw(renderTarget, new Rectangle(0, 0, 1920, 1080), Color.White);
            theSpriteBatch.End();
        }

        public void LevelCompleteCheck()
        {
            if (goal.levelComplete == true)
            {
                this.levelComplete = true;
            }
        }


        private void UpdatePlayerDeath(GameTime theGameTime)
        {
            if (ball.dead == true)
            {
                DeathTimer += (float)theGameTime.ElapsedGameTime.TotalSeconds;

                if (DeathTimer > 1)
                {
                    ball.Reset();
                    theObjectManager.Reset();
                    DeathTimer = 0;
                }

            }
        }

        public void Reset()
        {
            ball.Reset();
            theObjectManager.Reset();
            goal.Reset();
            levelComplete = false;
            DeathTimer = 0;
        }
    }
}
