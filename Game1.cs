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
        public static int screenWidth = 480, screenHeight = 270;

        public static Rectangle mousePosition, mouseClick;
        private MouseState mouseState, prevMouseState;

        //private GameManager theGameManager; // Manages the game content
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
            mouseState = Mouse.GetState();
            prevMouseState = mouseState;
            //_graphics.IsFullScreen = true;

            //theGameManager = new GameManager();
            //theGameManager.Initialise();
            theScreenManager = new ScreenManager();

            //theScreenManager = new ScreenManager();
          
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("font");

            // TODO: use this.Content to load your game content here
            //theGameManager.LoadContent(this.Content);
            theScreenManager.LoadContent(this.Content);
            MusicBank.LoadContent(this.Content);
            SoundBank.LoadContent(this.Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            UpdateMouse();

            // TODO: Add your update logic here
            //theGameManager.Update(gameTime);
            theScreenManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            // TODO: Add your drawing code here

            //GraphicsDevice.Clear(Color.FloralWhite);

            //theGameManager.Draw(_spriteBatch);
            //_spriteBatch.Begin();
            theScreenManager.Draw(_spriteBatch);
            //_spriteBatch.End();


            base.Draw(gameTime);
        }

        private void UpdateMouse()
        {
            mouseClick = new Rectangle(0, 0, 0, 0); // Resets the mouse click
            mouseState = Mouse.GetState();
            mousePosition = new Rectangle(mouseState.X / 4, mouseState.Y / 4, 1, 1);

            // If the mouse has just been clicked
            if (prevMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
            {
                mouseClick = mousePosition;
            }
            prevMouseState = mouseState;
        }
    }
}
