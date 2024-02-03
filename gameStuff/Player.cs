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

        private int playerWidth = 29; // Width of the player's collision box
        private int playerHeight = 29; // Height of the player's collision box

        private bool goLeft, goRight, tryJump; // Variables storing input
        private bool isCollide;
        private bool jumping;
        Collider collisionDetector; // The collision detector for the player
        private int controlLockTimer;
        Vector2 newPosition; // A variable that stores the player's new position to test for collisions before a move is made

        private const float ACC = 0.047f, DEC = 0.5f, FRC = 0.047f, TOP = 8f;
        private const float GRV = 0.219f, AIR = 0.094f, JMP = 5.5f;
        private const float normalSLOPE = 0.125f, rollUPSLOPE = 0.078f, rollDOWNSLOPE = 0.312f;

        private float tempAngle;
        private float distanceToTile;

        private enum playerState // Stores the state the player is currently in
        {
            grounded,
            airborne
        }

        private enum aerialDirection
        {
            mostlyRight,
            mostlyLeft,
            mostlyUp,
            mostlyDown
        }
        private aerialDirection airDir;

        private playerState state = playerState.airborne;

        public Player(string newAssetName, Vector2 thePosition, int width, int height) : base(newAssetName, thePosition, width, height)
        {
            position = new Vector2(327, 100);
            objectWidth = width;
            objectHeight = height;
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
            AngledSpeed();
            UpdateJump();
            SlipAndFall();

            speed = new Vector2(xSpeed, ySpeed);
            newPosition = new Vector2(position.X + xSpeed, position.Y + ySpeed);
            if (state == playerState.grounded)
            {
                groundedCollisions();
            }
            else if (state == playerState.airborne)
            {
                airborneCollisions();
            }


            position = newPosition; // Update the position to the corrected new position


            Animate();
            collisionDetector.Update(); // TESTING

            base.Update();
        }

        private void groundedCollisions()
        {
            //Sets up the sensors in the collision box
            collisionDetector.StartCollision(newPosition, playerWidth, playerHeight, currentAngle, AIRFLAG: false, speed);

            if (collisionDetector.Mode == Collider.collisionMode.floor || currentAngle % 90 == 0)
            {
                WallCollide();

            }

            collisionDetector.StartCollision(newPosition, playerWidth, playerHeight, currentAngle, AIRFLAG: false, speed);
            if (state == playerState.grounded) // Ensures floor sensors only active when falling/on ground
            {
                FloorCollide();
            }
        }

        private void WallCollide()
        {
            float pSpeed;
            if (state == playerState.grounded)
            { pSpeed = groundSpeed; }
            else { pSpeed = xSpeed; }

            distanceToTile = collisionDetector.wallCollision(pSpeed);
            if (distanceToTile <= 0 && distanceToTile >= -14) // If in the wall
            {
                AddDistance(distanceToTile, collisionDetector.getWallSensorDirection());

                if (state == playerState.airborne) // Allows for bounciness with walls
                {
                    xSpeed *= -0.674f;
                }
                else
                {
                    groundSpeed *= -0.674f;
                    //groundSpeed = 0;
                    //xSpeed = 0;
                }
                controlLockTimer = 5;
            }
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

            if (distanceToTile < 0 && distanceToTile >= -14) // If the player is within the terrain (within 14 pixels)
            {
                // Add the distance in the appropriate direction.
                AddDistance(distanceToTile, collisionDetector.getFloorSensorDirection());
                if (state != playerState.grounded) // If the player has just landed
                {
                    state = playerState.grounded;
                    LandingSpeed();
                }
            }
            else if (distanceToTile >= 0 && distanceToTile <= 14) // If the player is above the ground
            {
                if (state == playerState.grounded) // If the player is supposed to be on the ground
                {
                    AddDistance(distanceToTile, collisionDetector.getFloorSensorDirection());
                }
            }
            else if (distanceToTile > 14) // If the player is far enough away from a tile, make them airborne
            {
                state = playerState.airborne;
                currentAngle = 0;
            }
        }

        private void airborneCollisions()
        {
            //Sets up the sensors in the collision box
            collisionDetector.StartCollision(newPosition, playerWidth, playerHeight, currentAngle, AIRFLAG: true, speed);

            if (true) // The push sensors get free access at all times lol
            {
                WallCollide();
            }

            collisionDetector.StartCollision(newPosition, playerWidth, playerHeight, currentAngle, AIRFLAG: false, speed);
            if ((airDir == aerialDirection.mostlyRight || airDir == aerialDirection.mostlyLeft) && ySpeed >= 0
                || airDir == aerialDirection.mostlyDown) // Ensures floor sensors only active when falling/on ground
            {
                FloorCollide();
            }

            if (airDir == aerialDirection.mostlyRight || airDir == aerialDirection.mostlyLeft || airDir == aerialDirection.mostlyUp)
            {
                CeilingCollide();
                // MAYBE ADD LADNING ON CEILINGS
            }
        }

        private void CeilingCollide()
        {
            distanceToTile = collisionDetector.ceilingCollision();
            float tempAngle = collisionDetector.getFinalAngle();
            //Debug.WriteLine(distanceToTile);

            if (distanceToTile < 0 && distanceToTile >= -16)
            {
                newPosition.Y -= distanceToTile;
                LandingCeilingSpeed(tempAngle);
            }
        }


        // Draws the player in the correct position.
        public override void Draw(SpriteBatch theSpriteBatch)
        {
            // Draws the player
            base.Draw(theSpriteBatch);

            // Draws the collision box (if applied), as well as the variable display
            collisionDetector.Draw(theSpriteBatch);
            //VariableDisplay(theSpriteBatch, Game1.font);
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

            if (key.IsKeyDown(Keys.D) || key.IsKeyDown(Keys.Right))//D
            {
                goRight = true;//increases to the right
            }
            if (key.IsKeyUp(Keys.D) && key.IsKeyUp(Keys.Right))//stops
            {
                goRight = false;
            }
            if (key.IsKeyDown(Keys.A) || key.IsKeyDown(Keys.Left))//A
            {
                goLeft = true;//moves to the left
            }
            if (key.IsKeyUp(Keys.A) && key.IsKeyUp(Keys.Left))
            {
                goLeft = false;//stops
            }
            //if the player is not already jumping or falling
            if (key.IsKeyDown(Keys.Space) || key.IsKeyDown(Keys.W))
            {
                tryJump = true;
            }
            if (key.IsKeyUp(Keys.Space) && key.IsKeyUp(Keys.W))
            {
                tryJump = false;
            }
            if (key.IsKeyDown(Keys.O))//used for testing. Remove after finsihed.
            {
                position.Y -= 25;
                //ySpeed = -5;
                //KillPlayer();
            }
        }

        // Calculates the x and y speeds based on the ground speed and angle
        private void UpdateMovement()
        {
            // If the player is grounded
            if (state == playerState.grounded)
            {
                if (controlLockTimer == 0) // If the player has not slipped
                {
                    if (goRight) // If holding right
                    {
                        RightInput();
                    }
                    if (goLeft) // If holding left
                    {
                        LeftInput();
                    }
                    if (!goLeft && !goRight) // If not holding in either direction
                    {
                        NoInput();
                    }
                }
                else
                {
                    if (!goLeft && !goRight) // If not holding in either direction
                    {
                        NoInput();
                    }
                }
            }
            else if (state == playerState.airborne)
            {
                AirInput();

                // MAYBE ADD AIR DRAG
                ySpeed += GRV; // Adds gravity to the player
                if (ySpeed > 16) { ySpeed = 16; } // Prevents the player from falling through tiles
            }

            // Tracks the aerial direction of the player
            if (state == playerState.airborne)
            {
                AerialDirectionCaclulator();
            }
        }

        private void UpdateJump()
        {
            if (tryJump)
            {
                if (state != playerState.airborne)
                {
                    float radians = (float)(Math.PI / 180) * currentAngle;
                    xSpeed -= JMP * (float)Math.Sin(radians);
                    ySpeed -= JMP * (float)Math.Cos(radians);
                    state = playerState.airborne;
                    jumping = true;
                }
            }
            //else if (!tryJump && jumping && ySpeed < -3f) // Variable jump height SUBJECT TO CHANGE!!!
            //{
            //    ySpeed = -3f;
            //}
            else if (state == playerState.grounded)
            {
                jumping = false;
            }
            //MAYBE VARIABLE JUMP/JUMP BUFFER/COYOTE TIME
        }

        private void RightInput()
        {
            if (groundSpeed < 0) // If the player is moving left currently
            {
                groundSpeed += DEC; // Add deceleration
                if (groundSpeed >= 0) // If the sign has swapped
                {
                    groundSpeed = 0.5f; // Set ground speed to 0.5.
                }
            }
            else if (groundSpeed < TOP) // If the player is already moving right
            {
                groundSpeed += ACC; // Add acceleration
                if (groundSpeed > TOP)
                {
                    groundSpeed = TOP; // Set the player with max speed
                }
            }
        }

        private void LeftInput()
        {
            if (groundSpeed > 0) // If the player is moving right currently
            {
                groundSpeed -= DEC; // Add deceleration
                if (groundSpeed <= 0) // If the sign has swapped
                {
                    groundSpeed = -0.5f; // Set ground speed to -0.5.
                }
            }
            else if (groundSpeed > -TOP) // If the player is already moving left
            {
                groundSpeed -= ACC; // Add acceleration
                if (groundSpeed < -TOP)
                {
                    groundSpeed = -TOP; // Set the player with max speed
                }
            }
        }

        private void NoInput()
        {
            int sign = Math.Sign(groundSpeed); // Direction of travel
            groundSpeed += -FRC * sign; // If speed is negative, FRC is added. If positive, FRC is subtracted
            if (sign != Math.Sign(groundSpeed)) // If the sign swaps, then the player should stop moving
            {
                groundSpeed = 0;
            }
        }

        private void AirInput()
        {
            if (goRight)
            {
                if (xSpeed < 0) // If the player is moving left currently
                {
                    xSpeed += AIR; // Add deceleration
                    if (xSpeed >= 0) // If the sign has swapped
                    {
                        xSpeed = 0.5f; // Set ground speed to 0.5.
                    }
                }
                else if (xSpeed < TOP) // If the player is already moving right
                {
                    xSpeed += AIR; // Add acceleration
                    if (xSpeed > TOP)
                    {
                        xSpeed = TOP; // Set the player with max speed
                    }
                }
            }
            if (goLeft)
            {
                if (xSpeed > 0) // If the player is moving right currently
                {
                    xSpeed -= AIR; // Add deceleration
                    if (xSpeed <= 0) // If the sign has swapped
                    {
                        xSpeed = -0.5f; // Set ground speed to -0.5.
                    }
                }
                else if (xSpeed > -TOP) // If the player is already moving left
                {
                    xSpeed -= AIR; // Add acceleration
                    if (xSpeed < -TOP)
                    {
                        xSpeed = -TOP; // Set the player with max speed
                    }
                }
            }
            groundSpeed = xSpeed;
        }

        private void AerialDirectionCaclulator()
        {
            float degrees;
            if (xSpeed != 0)
            {
                double radians = Math.Atan2((float)-ySpeed, xSpeed);
                degrees = ((float)radians * (float)(180 / Math.PI) + 360) % 360;
            }
            else
            {
                if (ySpeed > 0)
                {
                    degrees = 270;
                }
                else
                {
                    degrees = 90;
                }
            }
            //Debug.WriteLine(degrees);

            if (0 <= degrees && degrees <= 45 || 315 < degrees && degrees <= 360)
            {
                airDir = aerialDirection.mostlyRight;
            }
            else if (45 < degrees && degrees <= 135)
            {
                airDir = aerialDirection.mostlyUp;
            }
            else if (135 < degrees && degrees <= 225)
            {
                airDir = aerialDirection.mostlyLeft;
            }
            else if (225 < degrees && degrees <= 315)
            {
                airDir = aerialDirection.mostlyDown;
            }

        }

        private void AngledSpeed()
        {
            // Converts the angle from degrees to radians
            double radians = Math.PI / 180 * currentAngle;

            //Subtract slope factor
            groundSpeed -= normalSLOPE * (float)Math.Sin(radians);
            if (collisionDetector.Mode == Collider.collisionMode.ceiling)
            {
                groundSpeed -= normalSLOPE * 0.55f * Math.Sign(groundSpeed); ; // Ensures the player loses speed on ceilings
            }
            //IF ROLLING IS ADDED, ADD OTHER SLP FACTORS

            // Updates the x and y speed using trig values with the angle.
            if (state != playerState.airborne) // Only adjust ySpeed if grounded
            {
                xSpeed = groundSpeed * (float)Math.Cos(radians);
                ySpeed = groundSpeed * -(float)Math.Sin(radians);
            }

            if (Math.Abs(groundSpeed) < 0.001f)
            {
                groundSpeed = 0;
            }
        }

        // Sets the groundSpeed upon landing based on the x/y speed and angle of slope
        private void LandingSpeed()
        {
            float radians = (float)(Math.PI / 180) * currentAngle;
            if (0 <= currentAngle && currentAngle <= 23 || 339 <= currentAngle && currentAngle <= 360) // Flat ground
            {
                groundSpeed = xSpeed; // Just continue with xSpeed
            }
            else if (0 <= currentAngle && currentAngle <= 45 || 315 <= currentAngle && currentAngle <= 360) // Slight Slope
            {
                if (airDir == aerialDirection.mostlyLeft || airDir == aerialDirection.mostlyRight) // Already horizontal
                {
                    groundSpeed = xSpeed;
                }
                else
                {
                    // Speed is half the yspeed. Direction is based on angle.
                    groundSpeed = ySpeed * 0.5f * -(float)Math.Sign(Math.Sin(radians));
                }
            }
            else // Steep Slope
            {
                if (airDir == aerialDirection.mostlyLeft || airDir == aerialDirection.mostlyRight) // Already horizontal
                {
                    groundSpeed = xSpeed;
                }
                else
                {
                    // Speed is the yspeed. Direction is based on angle.
                    groundSpeed = ySpeed * -(float)Math.Sign(Math.Sin(radians));
                }
            }
        }

        // Allows the player to "attach" to ceilings if they are angled well enough from a jump
        private void LandingCeilingSpeed(float nextAngle)
        {
            float radians = (float)(Math.PI / 180) * nextAngle;
            if (136 <= nextAngle && nextAngle <= 225 || nextAngle == 360) // Flat ceiling (or a full block)
            {
                ySpeed = 0;
            }
            else // Steep Slope
            {
                if (airDir == aerialDirection.mostlyUp) // (can land on it)
                {
                    state = playerState.grounded;
                    // Speed is the yspeed. Direction is based on angle.
                    groundSpeed = ySpeed * -(float)Math.Sign(Math.Sin(radians));
                    currentAngle = nextAngle;
                }
                else
                {
                    ySpeed = 0;
                }
            }
        }

        private void SlipAndFall()
        {
            //Debug.WriteLine(controlLockTimer);
            //if (state == playerState.grounded) // If the player is grounded
            {
                if (controlLockTimer == 0) // If the player has not been locked from movement
                {
                    // If the player is too slow up a steep slope
                    if (75 <= currentAngle && currentAngle <= 285 && Math.Abs(groundSpeed) <= 2.5f)
                    {
                        state = playerState.airborne; // Detach the player from the ground (make them fall)

                        // Lock their controls for 30 frames
                        controlLockTimer = 45;
                        //xSpeed = 0;
                        currentAngle = 0;
                        groundSpeed = 0;
                    }
                }
                else
                {   // Reduce lock timer
                    controlLockTimer -= 1;
                }
            }
        }

        private void Animate()
        {
            if (groundSpeed > 0)
            {
                rotation += 0.05f * groundSpeed;
            }
            else if (groundSpeed < 0)
            {
                rotation -= -0.05f * groundSpeed;
            }
        }

        private float StepCollide(float speed)
        {
            int steps = 5;
            return speed / steps;
        }

        // Snaps the angle to the nearest 90 degrees.
        private float snapAngle(float oldAngle)
        {
            float newAngle = (float)Math.Round((double)(oldAngle / 90) % 4) * 90;
            return newAngle;
        }

        // Draws text to the screen, allowing variables to be viewed in real time
        public void VariableDisplay(SpriteBatch theSpriteBatch, SpriteFont font)
        {
            float[] variableList = new float[] { position.X, position.Y, groundSpeed, xSpeed, ySpeed, currentAngle, (float)collisionDetector.Mode, (float)state };
            string[] variableNames = new string[] { "POSX: ", "POSY: ", "GSP: ", "XSP: ", "YSP: ", "ANG: ", "MODE: ", "STATE: " };

            Vector2 textPosition = new Vector2(0, 0);

            for (int i = 0; i < variableNames.Length; i++)
            {
                theSpriteBatch.DrawString(font, variableNames[i] + variableList[i], textPosition, Color.White);
                textPosition.Y += 32;
            }
        }
    }
}
