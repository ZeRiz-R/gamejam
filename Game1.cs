using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CelesteLike
{
    public class Game1 : Game
    {
        public static GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public static SpriteFont font;

        private GameManager theGameManager; // Manages the game content
        private ScreenManager theScreenManager;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            //_graphics.IsFullScreen = true;

            theGameManager = new GameManager();
            theGameManager.Initialise();

            //theScreenManager = new ScreenManager();
          
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("font");

            // TODO: use this.Content to load your game content here
            theGameManager.LoadContent(this.Content);
            //theScreenManager.LoadContent(this.Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            theGameManager.Update(gameTime);
            //theScreenManager.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            // TODO: Add your drawing code here

            GraphicsDevice.Clear(Color.CornflowerBlue);

            theGameManager.Draw(_spriteBatch);
            //theScreenManager.Draw(_spriteBatch);


            base.Draw(gameTime);
        }

        private void UpdateMouse()
        {
            
        }
    }
}
