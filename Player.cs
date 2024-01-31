using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
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
        private float groundSpeed; // The main speed variable of the player
        private float xSpeed; // The component of speed in the x axis
        private float ySpeed; // The component of speed in the y axis
        private float currentAngle; // The angle of the floor

        private int playerWidth = 18; // Width of the player's collision box
        private int playerHeight = 20; // Height of the player's collision box

        private bool goLeft, goRight; // Variables storing input
        private bool isCollide;
        Collider collisionDetector; // The collision detector for the player
        Vector2 newPosition; // A variable that stores the player's new position to test for collisions before a move is made

        private const float grv = 0.219f;

        private float tempAngle;
        private float distanceToTile;

        private enum playerState // Stores the state the player is currently in
        {
            grounded,
            airborne
        }

        private playerState state = playerState.airborne;

        public Player(string newAssetName) : base(newAssetName)
        {
            position = new Vector2(327, 275);
            collisionDetector = new Collider();
            newPosition = position;
        }

        public override void LoadContent(ContentManager theContentManager)
        {
            base.LoadContent(theContentManager);
            origin = new Vector2(spriteTexture.Width / 2, spriteTexture.Height / 2);
        }

        public override void Update()
        {

            UpdateInput(); // Detects user inputs and updates variables accordingly
            //groundSpeed = -2f;
            UpdateMovement(); // Deals with the inputs regarding movement

            float stepX = StepCollide(xSpeed);
            float stepY = StepCollide(ySpeed);
            bool temp;

            for (int i = 0; i < 5; i++)
            {
                newPosition = new Vector2(position.X + stepX, position.Y + stepY);

                //Sets up the sensors in the collision box
                collisionDetector.StartCollision(newPosition, playerWidth, playerHeight, currentAngle);

                if (collisionDetector.Mode == Collider.collisionMode.floor || (currentAngle % 90 == 0))
                {
                    temp = WallCollide();
                    if (temp) { stepX = 0; }
                    
                }

                collisionDetector.StartCollision(newPosition, playerWidth, playerHeight, currentAngle);
                if (state == playerState.grounded || ySpeed >= 0) // Ensures floor sensors only active when falling/on ground
                {
                    FloorCollide();
                }

                if (state == playerState.airborne)
                {
                    CeilingCollide();
                    // MAYBE ADD LADNING ON CEILINGS
                }

                position = newPosition; // Update the position to the corrected new position
            }

            collisionDetector.Update(); // TESTING

            base.Update();
        }

        private bool WallCollide()
        {
            distanceToTile = collisionDetector.wallCollision(groundSpeed);
            if (distanceToTile <= 0 && distanceToTile >= -14) // If in the wall
            {
                AddDistance(distanceToTile, collisionDetector.getWallSensorDirection());
                groundSpeed = 0;
                xSpeed = 0;
                return true;
            }
            return false;
        }

        private void FloorCollide()
        {
            // Retrieves the distance to the closest surface (if any)
            distanceToTile = collisionDetector.floorCollision(); // Gets the distance to the surface 
            tempAngle = collisionDetector.getFinalAngle(); // Gets the angle of the new surface

            // // An angle of 360 is flagged. This snaps the angle to the closest 90. Used to allow a full tile to be walked on from all sides
            if (tempAngle == 360)
            {
                currentAngle = snapAngle(currentAngle);
            }
            else
            {
                currentAngle = tempAngle;
            }

            if (distanceToTile < 0 && distanceToTile >= -20) // If the player is within the terrain (within 14 pixels)
            {
                // Add the distance in the appropriate direction.
                AddDistance(distanceToTile, collisionDetector.getFloorSensorDirection());
                state = playerState.grounded;
            }
            else if (distanceToTile >= 0 && distanceToTile <= 14) // If the player is above the ground
            {
                if (state == playerState.grounded) // If the player is supposed to be on the ground
                {
                    AddDistance(distanceToTile, collisionDetector.getFloorSensorDirection());
                    state = playerState.grounded;
                }
            }
            else if (distanceToTile > 14) // If the player is far enough away from a tile, make them airborne
            {
                state = playerState.airborne;
                currentAngle = 0;
            }
        }

        private void CeilingCollide()
        {
            distanceToTile = collisionDetector.ceilingCollision();
            //currentAngle = collisionDetector.getFinalAngle();
            Debug.WriteLine(distanceToTile);

            if (distanceToTile < 0 && distanceToTile >= -14)
            {
                AddDistance(distanceToTile, Sensor.Direction.up);
                ySpeed = 0;
            }
        }

        // Draws the player in the correct position.
        public override void Draw(SpriteBatch theSpriteBatch)
        {
            // Draws the player
            base.Draw(theSpriteBatch);

            // Draws the collision box (if applied), as well as the variable display
            collisionDetector.Draw(theSpriteBatch);
            VariableDisplay(theSpriteBatch, Game1.font);
        }

        // Aligns the player with the tiles by adding the distance in the appropriate direction
        private void AddDistance(float distance, Sensor.Direction direction)
        {
            if (direction == Sensor.Direction.down)
            {
                newPosition.Y += distance;
            }
            else if (direction == Sensor.Direction.right)
            {
                newPosition.X += distance;
            }
            else if (direction == Sensor.Direction.up)
            {
                newPosition.Y -= distance;
            }
            else if (direction == Sensor.Direction.left)
            {
                newPosition.X -= distance;
            }

        }

        // Retrieves keyboard inputs and translates them using boolean variables
        private void UpdateInput()
        {
            KeyboardState key = Keyboard.GetState();

            if (key.IsKeyDown(Keys.D)) // Right
            {
                groundSpeed = 5f;
            }
            if (key.IsKeyDown(Keys.A)) // Left
            {
                groundSpeed = -5f;
            }
            if (key.IsKeyUp(Keys.D) && key.IsKeyUp(Keys.A)) // Right
            {
                groundSpeed = 0;
            }
            if (key.IsKeyDown(Keys.S)) // Down
            {
                position.Y += 5;
            }
            if (key.IsKeyDown(Keys.W)) // Up
            {
                ySpeed = -6.5f;

                // By moving the player up by 1, they can be detached from the floor collision
                state = playerState.airborne;
            }
        }

        // Calculates the x and y speeds based on the ground speed and angle
        private void UpdateMovement()
        {
            // Converts the angle from degrees to radians
            double radians = (Math.PI / 180) * currentAngle;

            // Updates the x and y speed using trig values with the angle.
            xSpeed = groundSpeed * (float)Math.Cos(radians);
            if (state != playerState.airborne)
            {
                ySpeed = groundSpeed * -(float)Math.Sin(radians);
            }

            // Adds gravity to the player's y speed if they are in the air
            if (state == playerState.airborne)
            {
                ySpeed += 0.357f;
            }
        }

        private float StepCollide(float speed)
        {
            int steps = 5;
            return (speed / steps);
        }

        // Snaps the angle to the nearest 90 degrees.
        private float snapAngle(float oldAngle)
        {
            float newAngle = (float)Math.Round((double)(oldAngle / 90) % 4) * 90;
            return newAngle;
        }

        // Draws text to the screen, allowing variables to be viewed in real time
        private void VariableDisplay(SpriteBatch theSpriteBatch, SpriteFont font)
        {
            float[] variableList = new float[] { position.X, position.Y, groundSpeed, xSpeed, ySpeed, currentAngle, (float)collisionDetector.Mode };
            string[] variableNames = new string[] { "POSX: ", "POSY: ", "GSP: ", "XSP: ", "YSP: ", "ANG: ", "MODE: " };

            Vector2 textPosition = new Vector2(32, 32);

            for (int i = 0; i < variableNames.Length; i++)
            {
                theSpriteBatch.DrawString(font, variableNames[i] + variableList[i], textPosition, Color.White);
                textPosition.Y += 32;
            }
        }
    }
}
