using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CelesteLike
{
    internal class Sprite
    {
        protected Vector2 position;
        public Vector2 Position { get { return position; } }
        protected Texture2D spriteTexture;
        private string assetName;

        protected Vector2 origin;
        protected Vector2 speed;

        private Rectangle sourceRectangle;
        public Rectangle SourceRectangle { get { return sourceRectangle; } }
        private float scale = 1.0f;

        private SpriteEffects effect;
        protected float rotation;


        public Sprite(string theAssetName)
        {
            assetName = theAssetName;
            effect = SpriteEffects.None;
            rotation = 0;
        }

        public virtual void LoadContent(ContentManager theContentManager)
        {
            spriteTexture = theContentManager.Load<Texture2D>(assetName);
            sourceRectangle = new Rectangle(0,0, spriteTexture.Width, spriteTexture.Height);
            //origin = new Vector2(spriteTexture.Width/2, spriteTexture.Height/2); // Sets origin to centre
        }

        public virtual void Update()
        {
            
        }

        public virtual void Draw(SpriteBatch theSpriteBatch)
        {
            theSpriteBatch.Draw(spriteTexture, position, sourceRectangle, Color.White, rotation, origin, 1.0f, SpriteEffects.None, 0);
        }
    }
}
