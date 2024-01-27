using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CelesteLike
{
    internal class Sprite
    {
        protected Vector2 position;
        private Texture2D spriteTexture;
        private string assetName;

        private Vector2 origin;
        protected Vector2 speed;

        private Rectangle sourceRectangle;
        private float scale = 1.0f;

        private SpriteEffects effect;
        private float rotation;


        public Sprite(string theAssetName)
        {
            assetName = theAssetName;
            effect = SpriteEffects.None;
            rotation = 0;
        }

        public void LoadContent(ContentManager theContentManager)
        {
            spriteTexture = theContentManager.Load<Texture2D>(assetName);
            sourceRectangle = new Rectangle(0,0, spriteTexture.Width, spriteTexture.Height);
        }

        public virtual void Update()
        {
            
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            theSpriteBatch.Draw(spriteTexture, position, sourceRectangle, Color.White);
        }
    }
}
