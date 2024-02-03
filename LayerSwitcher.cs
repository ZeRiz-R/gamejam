using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelesteLike
{
    internal class LayerSwitcher : Sprite
    {
        private bool show;

        public enum SwitchDirection
        {
            left,
            right,
            up,
            down,
        }
        private SwitchDirection switchDir;

        public LayerSwitcher(SwitchDirection direction, Vector2 thePosition, int width, int height) : base("layerSwitch", thePosition, width, height)
        {
            show = true;
            switchDir = direction;
        }

        public override void LoadContent(ContentManager theContentManager)
        {
            base.LoadContent(theContentManager);
            //if (switchDir == SwitchDirection.left)
            {
                sourceRectangle = new Rectangle(16, 0, 16, 16);
            }
            //else if (switchDir == SwitchDirection.up)
            {
             //   sourceRectangle = new Rectangle(32, 0, 16, 16);
            }

        }
           
        public void Update(float xSpeed, float ySpeed, Rectangle objectRectangle)
        {
            Show(); // Update whether they should show

        }

        private void Show()
        {
            KeyboardState key = Keyboard.GetState();
            if (key.IsKeyUp(Keys.I))
            {
                show = true;
            }
            else
            {
                show = false;
            }   
        }

        public override void Draw(SpriteBatch theSpriteBatch)
        {
            //if (show)
            {
                base.Draw(theSpriteBatch);
            }
        }



    }
}
