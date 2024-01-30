using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CelesteLike
{
    // Assume all tiles were made facing right (and up). Assume all tiles are unflipped unless otherwise told. So if running on 
    // a ceiling tile, assume that it is not flipped and use it's height array data. Adjust the data if it has been flipped
    // vertically and/or horizontally.
    internal class Collider
    {
        private Sensor A, B, C, D, E, F; // The 6 sensors. A/B for floor. C/D for ceilings. E/F for walls.
        private List<Sensor> Sensors; // A list containing all of the sensors for easy access
        private Rectangle collisionBox; // The box used to position the sensors
        private int tileWidth = 16; // The width of each tile
        private int[,] tileMap;
        private int[,] heightArray;

        private enum collisionMode // Enum of possible collision modes (based on angle)
        {
            floor,
            rightWall,
            ceiling,
            leftWall
        }
        private collisionMode mode;

        public Collider()
        {
            A = new Sensor();
            B = new Sensor();
            C = new Sensor();
            D = new Sensor();
            E = new Sensor();
            F = new Sensor();
            Sensors = new List<Sensor> { A, B, C, D, E, F }; // Creates a list of the sensors

            tileMap = Tiles.TileMap;
            heightArray = Tiles.HeightArray;
        }

        // This method needs to be applied before any other ones are applied.
        public void StartCollision(Vector2 objectPosition, int objectWidth, int objectHeight, float currentAngle)
        {
            collisionBox = new Rectangle((int)objectPosition.X - (objectWidth / 2), (int)objectPosition.Y - (objectHeight / 2)
                , objectWidth, objectHeight);

            DetermineMode(currentAngle);
            setSensorPositions(objectPosition, objectWidth, objectHeight);
            RowsAndColumns();

        }

        // Calculates the collision mode of the player based on their angle.
        private void DetermineMode(float currentAngle)
        {
            if ((0 <= currentAngle && currentAngle <= 45) || (315 < currentAngle && currentAngle <= 360)) // Floor mode
            {
                mode = collisionMode.floor;
            }
            else if (45 < currentAngle && currentAngle <= 135) // Right Wall mode
            {
                mode = collisionMode.rightWall;
            }
            else if (135 < currentAngle && currentAngle <= 225) // Ceiling mode
            {
                mode = collisionMode.ceiling;
            }
            else if (225 < currentAngle && currentAngle <= 315) // Left wall mode
            {
                mode = collisionMode.leftWall;
            }
        }

        // Calculates the row and column within the tile map of each sensor
        private void RowsAndColumns()
        {
            foreach (var sensor in Sensors)
            {
                sensor.row = (int)sensor.position.Y / tileWidth;
                sensor.column = (int)sensor.position.X / tileWidth;
            }
        }

        // Sets the positions of the sensors based on the collision mode. The sensors are essentially rotated by 90 degrees each time
        private void setSensorPositions(Vector2 objectPosition, int objectWidth, int objectHeight)
        {
            if (mode == collisionMode.floor)
            {
                A.position = new Vector2(objectPosition.X - (objectWidth / 2), objectPosition.Y + (objectHeight / 2));
                B.position = new Vector2(objectPosition.X + (objectWidth / 2), objectPosition.Y + (objectHeight / 2));
            }
            else if (mode == collisionMode.rightWall)
            {
                A.position = new Vector2(objectPosition.X + (objectHeight / 2), objectPosition.Y + (objectWidth / 2));
                B.position = new Vector2(objectPosition.X + (objectHeight / 2), objectPosition.Y - (objectWidth / 2));
            }
            else if (mode == collisionMode.ceiling)
            {
                A.position = new Vector2(objectPosition.X + (objectWidth / 2), objectPosition.Y - (objectHeight / 2));
                B.position = new Vector2(objectPosition.X - (objectWidth / 2), objectPosition.Y - (objectHeight / 2));
            }
            else if (mode == collisionMode.leftWall)
            {
                A.position = new Vector2(objectPosition.X - (objectHeight / 2), objectPosition.Y - (objectWidth / 2));
                B.position = new Vector2(objectPosition.X - (objectHeight / 2), objectPosition.Y + (objectWidth / 2));
            }
        }

        // Gets the distance from the object to the floor
        public int floorCollision()
        {
            int finalDistance = 0;

            int[,] currentArray;

            if (mode == collisionMode.floor) // If running on floor
            {
                currentArray = heightArray; // On the floor, so the height array will be used
                A.tileHeight = floorSensorHeight(ref A, currentArray); // Gets the tileHeight of the tile at sensor A
                B.tileHeight = floorSensorHeight(ref B, currentArray); // Gets the tileHeight of the tile at sensor B

                A.distanceCalculator(Sensor.Direction.down);
                B.distanceCalculator(Sensor.Direction.down);
            }

            if (A.distance < B.distance) // If A is closer to the surface than B
            {
                finalDistance = A.distance;
            }
            else // If B is closer than A
            {
                finalDistance = B.distance;
            }

            return finalDistance;
        }

        private int floorSensorHeight(ref Sensor sensor, int[,] currentArray)
        {
            int tileHeight1, tileHeight2;

            tileHeight1 = sensor.getTileHeight(sensor.row, sensor.column, tileMap, currentArray, Sensor.Direction.down); // Get the index of the tile from downward sensor
            if (tileHeight1 == 0) // If the tile is empty, extend to check the next tile
            {
                sensor.row += 1; //move the sensor down one row
                tileHeight2 = sensor.getTileHeight(sensor.row, sensor.column, tileMap, currentArray, Sensor.Direction.down); // Gets the index of the tile below. (Usually the same, but flipped tiles have swapped indices)

                if (tileHeight2 != 0) // If the next tile is not empty (there is terrain), change the tileheight to the new one
                {
                    tileHeight1 = tileHeight2;
                } // If the tile is empty, then nothing changes
            }
            else if (tileHeight1 == 16) // If the tile is full, regress to check the previous tile
            {
                tileHeight2 = sensor.getTileHeight(sensor.row - 1, sensor.column, tileMap, currentArray, Sensor.Direction.down);

                if (tileHeight2 != 0) // If there is more terrain above, change the tileheight and row
                {
                    tileHeight1 = tileHeight2;
                    sensor.row -= 1;
                }
            }

            return tileHeight1;
        }

        //TESTING
        Texture2D boundingBox;
        private bool showRectangle = false;

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

        public void Update()
        {
            AlternateBox();
            setRectangleOutline(Game1._graphics.GraphicsDevice, collisionBox); ;
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            if (showRectangle)
            {
                theSpriteBatch.Draw(boundingBox, collisionBox, Color.White);
            }
        }
    }
}
