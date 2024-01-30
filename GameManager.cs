using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System.Net.Security;

namespace CelesteLike
{
    internal class GameManager
    {
        private RenderTarget2D renderTarget;
        private int screenWidth = 480, screenHeight = 320;

        private Player ball;

        private Tiles level;

        public void Initialise()
        {
            level = new Tiles();
            ball = new Player("blueSphere");
        }

        public void LoadContent(ContentManager theContentManager)
        {
            renderTarget = new RenderTarget2D(Game1._graphics.GraphicsDevice, screenWidth, screenHeight, false, Game1._graphics.PreferredBackBufferFormat, 
                DepthFormat.Depth24);

            level.LoadContent(theContentManager);
            ball.LoadContent(theContentManager);
        }

        public void Update(GameTime theGameTime)
        {
            ball.Update();
        }

        private void DrawScenetoTexture(GraphicsDevice  graphicsDevice, SpriteBatch theSpriteBatch)
        {
            graphicsDevice.SetRenderTarget(renderTarget);
            graphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };

            graphicsDevice.Clear(Color.CornflowerBlue);

            //Draw here
            theSpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            level.Draw(theSpriteBatch);
            ball.Draw(theSpriteBatch);
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
