using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;

namespace CelesteLike
{
    internal class Sprite
    {
        public Vector2 position;

        protected Texture2D spriteTexture;
        private string assetName;

        public Vector2 origin { get; set; }
        protected Vector2 speed;

        protected int objectWidth, objectHeight;

        public Rectangle sourceRectangle { get; protected set; }
        public Rectangle collisionBox { get; protected set; }
        protected Texture2D boundingBox;
        protected bool showRectangle;

        public float scale { get; set; }

        protected SpriteEffects effect;
        public float rotation;
        protected Color colour;

        protected bool show;



        public Sprite(string theAssetName, Vector2 thePosition, int width, int height)
        {
            assetName = theAssetName;
            position = thePosition;
            objectWidth = width;
            objectHeight = height;
            effect = SpriteEffects.None;
            rotation = 0;
            colour = Color.White;
            scale = 1.0f;
            show = true;
            //if (spriteTexture != null)
            //{ origin = new Vector2(spriteTexture.Width / 2, spriteTexture.Height / 2); }

            if (origin != Vector2.Zero)
            {
                collisionBox = new Rectangle((int)position.X - (objectWidth / 2), (int)position.Y - (objectHeight / 2),
                            objectWidth, objectHeight);
            }
            else
            {
                collisionBox = new Rectangle((int)position.X, (int)position.Y,
            objectWidth, objectHeight);
            }
        }

        public virtual void LoadContent(ContentManager theContentManager)
        {

            spriteTexture = theContentManager.Load<Texture2D>(assetName);
            sourceRectangle = new Rectangle(0, 0, spriteTexture.Width, spriteTexture.Height);
            //origin = new Vector2(spriteTexture.Width/2, spriteTexture.Height/2); // Sets origin to centre
        }

        public virtual void Draw(SpriteBatch theSpriteBatch)
        {
            if (show)
            {
                theSpriteBatch.Draw(spriteTexture, position, sourceRectangle, colour, rotation, origin, scale, effect, 0);
                if (showRectangle)
                {
                    theSpriteBatch.Draw(boundingBox, collisionBox, Color.White);
                }
            }

        }

        private void AlternateBox()
        {
            KeyboardState key = Keyboard.GetState();
            if (key.IsKeyDown(Keys.P))
            {
                showRectangle = !showRectangle;
            }
        }
        private void setRectangleOutline(GraphicsDevice graphics, Rectangle box)
        {
            var colours = new List<Color>();

            for (int y = 0; y < box.Height; y++)
            {
                for (int x = 0; x < box.Width; x++)
                {
                    if (x == 0 || // left side
                        y == 0 || // top side
                        x == box.Width - 1 ||// right side
                        y == box.Height - 1) // bottom side
                    {

                        colours.Add(new Color(255, 255, 255, 255));
                    }
                    else
                    {
                        colours.Add(new Color(0, 0, 0, 0));
                    }
                }
            }

            boundingBox = new Texture2D(graphics, box.Width, box.Height);
            boundingBox.SetData(colours.ToArray());
        }

        protected void UpdateCollisionBox()
        {
            collisionBox = new Rectangle((int)position.X - (objectWidth / 2), (int)position.Y - (objectHeight / 2),
            objectWidth, objectHeight);
        }

        public virtual void Update()
        {
            AlternateBox();
            setRectangleOutline(Game1._graphics.GraphicsDevice, collisionBox); ;
        }

        // Checks if another object's rectangle is intersecting with them
        public bool Intersects(Rectangle foreignRectangle)
        {
            if (collisionBox.Intersects(foreignRectangle))
            {
                return true;
            }
            return false;
        }

        public void Reset()
        {

        }

        public void ChangePosition(float xChange, float yChange)
        {
            position.X += xChange;
            position.Y += yChange;
        }
    }

}
