using System;
using System.Collections.Generic;
using System.Net.Security;
using Microsoft.Xna.Framework;

namespace CelesteLike
{
    internal class Button : Sprite
    {
        private bool selected;
        private bool clicked;

        public Button(string assetName, Vector2 thePosition, int width, int height) : base(assetName, thePosition, width, height)
        {
        }

        public void LoadContent()
        {
            selected = false;
            clicked = false;
        }

        public void UpdateSelected()
        {
            selected = !selected;
        }


    }
}
