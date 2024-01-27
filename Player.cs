using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

// For the ceiling versions of sloped tiles, represent them as (-x) in the tilemap. Design system that recognises the negative,
// then seeks out the positive index. This is the unflipped height array. This can then be used by treating it as flipped.
// Same for width and angle arrays. This prevents the need for four versions of each tile. Remember to only flip in x axis, not y.

// When in floor mode, ground sensors will use regular height array. Running right, push sensors use regular width array. Running
// left, push sensors use flipped width array (shouldnt matter too much). Ceiling sensors use flipped height array

// When in right wall mode, floor sensors use regular width array. Running up, push sensors use flipped height array. Running
// down, push sensors use regular height array. Ceiling sensors inactive

// When in ceiling mode, floor sensors use flipped height array. Running left, push sensors use flipped width. Running right,
// push sensors use regular width array. Ceiling sensors inactive.

// When in left wall mode, floor sensors use flipped width array. Running up, push sensors flipped height array. Running down,
// push sensors use regular height array. Ceiling sensors inactive.

namespace CelesteLike
{
    internal class Player : Sprite
    {
        private float groundSpeed;
        private float xSpeed;
        private float ySpeed;

        private bool goLeft, goRight;


        public Player(string newAssetName) : base(newAssetName)
        {
        }

        public override void Update()
        {
            UpdateInput();
            base.Update();
        }

        private void UpdateInput()
        {
            KeyboardState key = Keyboard.GetState();

            if (key.IsKeyDown(Keys.D)) // Right
            {
                position.X += 5;
            }
            if (key.IsKeyDown(Keys.A)) // Left
            {
                position.X += -5;
            }
            if (key.IsKeyDown(Keys.S)) // Down
            {
                position.Y += 5;
            }
            if (key.IsKeyDown(Keys.W)) // Up
            {
                position.Y += -5;
            }
        }

        private void UpdateMovement()
        {

        }
    }
}
