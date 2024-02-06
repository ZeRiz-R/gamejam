using System;
using System.Collections.Generic;
using System.Net.Security;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;

namespace CelesteLike
{
    internal class Button : Sprite
    {
        private bool hovered;
        public bool clicked { get; private set; }
        private bool startClick; // Lets the delay occur
        private bool backButton;
        float clickDelay; // How long until the button should be "clicked" (in ms)
        float timer = 0;

        public Button(string assetName, Vector2 thePosition, int width, int height) : base(assetName, thePosition, width, height)
        {
            hovered = false;
            clicked = false;
            backButton = false;
            startClick = false;
            clickDelay = 0.2f;
        }

        public void Update(GameTime theGameTime)
        {
            if (collisionBox.Contains(Game1.mousePosition) && !clicked) // If the mouse is over the button
            {
                //colour = Color.LightGray; // Darken the button
                scale = 1.1f;
                colour = Color.Cyan;
            }
            else if (!clicked)
            {
                colour = Color.White; // Return the button to normal colour
                scale = 1f;
            }

            if (collisionBox.Intersects(Game1.mouseClick)) // If the button has been clicked
            {
                colour = Color.DarkGray;
                timer = 0;
                startClick = true;
                SoundBank.button_press.PlayEndSound();
            }

            if (startClick)
            {
                DelayTimer(theGameTime);
            }

            base.Update();
        }

        private void DelayTimer(GameTime theGameTime)
        {
            timer += (float)theGameTime.ElapsedGameTime.TotalSeconds;
            if (timer > clickDelay)
            {
                colour = Color.White;
                timer = 0;
                clicked = true;
                startClick = false;
            }
        }

        public void UnclickButton()
        {
            clicked = false;
        }


    }
}
