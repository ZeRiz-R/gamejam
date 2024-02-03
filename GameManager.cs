using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System.Net.Security;
using CelesteLike;

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

        private Coin coin1 = new Coin(new Vector2(327, 100), 24, 16);


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
            coin1.LoadContent(theContentManager);

            level.LoadContent(theContentManager);
            ball.LoadContent(theContentManager);
        }

        public void Update(GameTime theGameTime)
        {
            theObjectManager.Update();
            ball.Update();
            camera.Follow2(ball);
            coin1.Update(theGameTime);
        }

        private void DrawScenetoTexture(GraphicsDevice  graphicsDevice, SpriteBatch theSpriteBatch)
        {
            graphicsDevice.SetRenderTarget(renderTarget);
            graphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };

            graphicsDevice.Clear(Color.CornflowerBlue);

            //Draw here
            theSpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, camera.Transform);
            level.DrawLayer1(theSpriteBatch);
            theObjectManager.Draw(theSpriteBatch);
            coin1.Draw(theSpriteBatch);
            ball.Draw(theSpriteBatch);
            level.DrawLayer2(theSpriteBatch);
            theSpriteBatch.End();

            theSpriteBatch.Begin();
            ball.VariableDisplay(theSpriteBatch, Game1.font);
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
    }
}
