using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
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

        private int playerWidth = 18;
        private int playerHeight = 20;

        private bool goLeft, goRight;
        private bool isCollide;
        Collider collisionDetector;

        private const float grv = 0.219f;


        private enum playerState
        {
            grounded,
            airborne
        }

        private playerState state = playerState.airborne;

        public Player(string newAssetName) : base(newAssetName)
        {
            position = new Vector2(340, 224);
            collisionDetector = new Collider();
        }

        public override void LoadContent(ContentManager theContentManager)
        {
            base.LoadContent(theContentManager);
            origin = new Vector2(spriteTexture.Width / 2, spriteTexture.Height / 2);
        }

        public override void Update()
        {
            int distanceToTile;
            UpdateInput();
            UpdateMovement();
            Vector2 newPosition = new Vector2(position.X, position.Y + ySpeed);
            collisionDetector.StartCollision(newPosition, playerWidth, playerHeight, 0);
            distanceToTile = collisionDetector.floorCollision();
            Debug.WriteLine(distanceToTile);

            if (distanceToTile < 0 && distanceToTile >= -14) // If the player is within the terrain (within 14 pixels)
            {
                newPosition.Y += distanceToTile;
                state = playerState.grounded;
            }
            else if (distanceToTile >= 0 && distanceToTile <= 14)
            {
                if (state == playerState.grounded) // If they are supposed to be on the floor
                {
                    newPosition.Y += distanceToTile;
                    state = playerState.grounded;
                }
            }
            else if (distanceToTile > 14)
            {
                state = playerState.airborne;
            }

            position = newPosition;

            collisionDetector.Update(); // TESTING

            base.Update();
        }

        public override void Draw(SpriteBatch theSpriteBatch)
        {
            base.Draw(theSpriteBatch);
            collisionDetector.Draw(theSpriteBatch);
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
                ySpeed = -7.5f;
                position.Y -= 1;
                state = playerState.airborne;
            }
        }

        private void UpdateMovement()
        {
            if (state == playerState.airborne)
            {
                ySpeed += 0.357f;
            }
            else
            {
                ySpeed = 0;
            }
        }

        private void UpdateCollision()
        {
            isCollide = false;
        }
    }
}
