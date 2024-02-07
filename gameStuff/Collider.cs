using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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
        private Sensor activeSensor; // Used to store the active push sensor
        private List<Sensor> Sensors; // A list containing all of the sensors for easy access
        private Rectangle collisionBox; // The box used to position the sensors
        private Sensor winningSensor;

        private int tileWidth = 16; // The width of each tile
        private int[,] heightArray;
        private int[,] widthArray;
        private int[,] activeTileMap;

        public enum collisionMode // Enum of possible collision modes (based on angle)
        {
            floor,
            rightWall,
            ceiling,
            leftWall
        }
        private collisionMode mode; // Stores whether the player is on the floor/wall/ceiling
        public collisionMode Mode { get { return mode; } }

        public Collider()
        {
            A = new Sensor();
            B = new Sensor();
            C = new Sensor();
            D = new Sensor();
            E = new Sensor();
            F = new Sensor();
            activeSensor = new Sensor();
            Sensors = new List<Sensor> { A, B, C, D, E, F }; // Creates a list of the sensors

            activeTileMap = Tiles.TileMapL1;
            heightArray = Tiles.HeightArray;
            widthArray = Tiles.WidthArray;
        }

        // This method needs to be applied before any other ones are applied.
        public void StartCollision(Vector2 objectPosition, int objectWidth, int objectHeight, float currentAngle, bool AIRFLAG, Vector2 objectSpeed)
        {
            collisionBox = new Rectangle((int)objectPosition.X - objectWidth / 2, (int)objectPosition.Y - objectHeight / 2
                , objectWidth, objectHeight);

            GetLayer(objectPosition, objectSpeed); // Finds the active tileMap

            // The ceiling sensors are only active whilst airborne, so do not change based on floor mode
            C.position = new Vector2(collisionBox.Left, collisionBox.Top);
            C.direction = Sensor.Direction.up;
            D.position = new Vector2(collisionBox.Right, collisionBox.Top);
            D.direction = Sensor.Direction.up;

            DetermineMode(currentAngle, AIRFLAG);
            setSensorPositions(objectPosition, objectWidth, objectHeight);
            RowsAndColumns();

        }

        // Calculates the collision mode of the player based on their angle.
        private void DetermineMode(float currentAngle, bool AIRFLAG)
        {
            if (0 <= currentAngle && currentAngle <= 45 || 315 <= currentAngle && currentAngle <= 360) // Floor mode
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
            else if (225 < currentAngle && currentAngle < 315) // Left wall mode
            {
                mode = collisionMode.leftWall;
            }

            if (AIRFLAG) // Ensures that sensors dont accidentally move in air
            {
                mode = collisionMode.floor;
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
                // FLOOR SENSORS
                A.position = new Vector2(objectPosition.X - objectWidth / 2, objectPosition.Y + objectHeight / 2);
                B.position = new Vector2(objectPosition.X + objectWidth / 2, objectPosition.Y + objectHeight / 2);
                A.direction = Sensor.Direction.down;
                B.direction = Sensor.Direction.down;

                // WALL SENSORS
                E.position = new Vector2(objectPosition.X - objectWidth / 2, objectPosition.Y);
                F.position = new Vector2(objectPosition.X + objectWidth / 2, objectPosition.Y);
                E.direction = Sensor.Direction.left;
                F.direction = Sensor.Direction.right;

                // Collision Box
                collisionBox = new Rectangle((int)objectPosition.X - objectWidth / 2, (int)objectPosition.Y - objectHeight / 2
                , objectWidth, objectHeight);
            }
            else if (mode == collisionMode.rightWall)
            {
                // FLOOR SENSORS
                A.position = new Vector2(objectPosition.X + objectHeight / 2, objectPosition.Y + objectWidth / 2);
                B.position = new Vector2(objectPosition.X + objectHeight / 2, objectPosition.Y - objectWidth / 2);
                A.direction = Sensor.Direction.right;
                B.direction = Sensor.Direction.right;

                // WALL SENSORS
                E.position = new Vector2(objectPosition.X, objectPosition.Y - objectWidth / 2);
                F.position = new Vector2(objectPosition.X, objectPosition.Y + objectWidth / 2);
                E.direction = Sensor.Direction.down;
                F.direction = Sensor.Direction.up;

                // Collision Box
                new Rectangle((int)objectPosition.X - objectHeight / 2, (int)objectPosition.Y - objectWidth / 2
                , objectHeight, objectWidth);
            }
            else if (mode == collisionMode.ceiling)
            {
                // FLOOR SENSORS
                A.position = new Vector2(objectPosition.X + objectWidth / 2, objectPosition.Y - objectHeight / 2);
                B.position = new Vector2(objectPosition.X - objectWidth / 2, objectPosition.Y - objectHeight / 2);
                A.direction = Sensor.Direction.up;
                B.direction = Sensor.Direction.up;

                // WALL SENSORS
                E.position = new Vector2(objectPosition.X + objectWidth / 2, objectPosition.Y);
                F.position = new Vector2(objectPosition.X - objectWidth / 2, objectPosition.Y);
                E.direction = Sensor.Direction.right;
                F.direction = Sensor.Direction.left;

                // Collision Box
                collisionBox = new Rectangle((int)objectPosition.X - objectWidth / 2, (int)objectPosition.Y - objectHeight / 2
                , objectWidth, objectHeight);
            }
            else if (mode == collisionMode.leftWall)
            {
                // FLOOR SENSORS
                A.position = new Vector2(objectPosition.X - objectHeight / 2, objectPosition.Y - objectWidth / 2);
                B.position = new Vector2(objectPosition.X - objectHeight / 2, objectPosition.Y + objectWidth / 2);
                A.direction = Sensor.Direction.left;
                B.direction = Sensor.Direction.left;

                // WALL SENSORS
                E.position = new Vector2(objectPosition.X, objectPosition.Y - objectWidth / 2);
                F.position = new Vector2(objectPosition.X, objectPosition.Y + objectWidth / 2);
                E.direction = Sensor.Direction.up;
                F.direction = Sensor.Direction.down;

                // Collision Box
                new Rectangle((int)objectPosition.X - objectHeight / 2, (int)objectPosition.Y - objectWidth / 2
                , objectHeight, objectWidth);
            }
        }

        public float getFinalAngle()
        {
            if (winningSensor.TileID == 0) // If on a flat surface
            {
                if (mode == collisionMode.floor)
                {
                    return winningSensor.getAngleJustBelow();
                }
                else if (mode == collisionMode.rightWall)
                {
                    return winningSensor.getAngleJustBelow();
                }
                else if (mode == collisionMode.ceiling)
                {
                    return winningSensor.getAngleJustBelow();
                }
                else if (mode == collisionMode.leftWall)
                {
                    return winningSensor.getAngleJustBelow();
                }
            }
            else
            {
                return winningSensor.angle;
            }

            return 0;
        }

        public Rectangle GetCollisionBox()
        {
            return collisionBox;
        }

        // Gets the distance from the object to the floor
        public float floorCollision()
        {
            float finalDistance = 0;

            int[,] currentArray;

            if (mode == collisionMode.floor) // If running on floor
            {
                currentArray = heightArray; // On the floor, so the height array will be used
                A.tileHeight = floorSensorHeight(ref A, currentArray); // Gets the tileHeight of the tile at sensor A
                B.tileHeight = floorSensorHeight(ref B, currentArray); // Gets the tileHeight of the tile at sensor B

                A.distanceCalculator();
                B.distanceCalculator();
            }

            else if (mode == collisionMode.rightWall)
            {
                currentArray = widthArray;
                A.tileHeight = floorSensorHeight(ref A, currentArray);
                B.tileHeight = floorSensorHeight(ref B, currentArray);

                A.distanceCalculator();
                B.distanceCalculator();

            }

            else if (mode == collisionMode.ceiling) // If running on floor
            {
                currentArray = heightArray; // On the floor, so the height array will be used
                A.tileHeight = floorSensorHeight(ref A, currentArray); // Gets the tileHeight of the tile at sensor A
                B.tileHeight = floorSensorHeight(ref B, currentArray); // Gets the tileHeight of the tile at sensor B

                A.distanceCalculator();
                B.distanceCalculator();
            }

            else if (mode == collisionMode.leftWall)
            {
                currentArray = widthArray;
                A.tileHeight = floorSensorHeight(ref A, currentArray);
                B.tileHeight = floorSensorHeight(ref B, currentArray);

                A.distanceCalculator();
                B.distanceCalculator();
            }

            if (A.distance < B.distance) // If A is closer to the surface than B
            {
                finalDistance = A.distance;
                winningSensor = A;
            }
            else // If B is closer than A
            {
                finalDistance = B.distance;
                winningSensor = B;
            }
            Debug.WriteLine("A: {0}\nB: {1}", A.distance, B.distance);

            return finalDistance;
        }

        public float wallCollision(float groundSpeed)
        {
            int[,] currentArray;

            if (groundSpeed > 0) // If moving in the positive direction, then sensor F is used
            {
                activeSensor = F;
            }
            else if (groundSpeed < 0) // If moving in the negative direction, then sensor E is used
            {
                activeSensor = E;
            }
            else
            {
                return 100; // If not moving at all, return 0
            }


            if (mode == collisionMode.floor) // If running on floor
            {
                currentArray = widthArray; // On the floor, so the height array will be used
                activeSensor.tileHeight = floorSensorHeight(ref activeSensor, currentArray); // Gets the tileHeight of the tile at the active sensor
                activeSensor.distanceCalculator();

            }

            else if (mode == collisionMode.rightWall)
            {
                currentArray = heightArray;
                activeSensor.tileHeight = floorSensorHeight(ref activeSensor, currentArray);
                activeSensor.distanceCalculator();

            }

            else if (mode == collisionMode.ceiling) // If running on floor
            {
                currentArray = widthArray; // On the floor, so the height array will be used
                activeSensor.tileHeight = floorSensorHeight(ref activeSensor, currentArray); // Gets the tileHeight of the tile at sensor A
                activeSensor.distanceCalculator();
            }

            else if (mode == collisionMode.leftWall)
            {
                currentArray = heightArray;
                activeSensor.tileHeight = floorSensorHeight(ref activeSensor, currentArray);
                activeSensor.distanceCalculator();
            }

            if (activeSensor.angle != 0 && activeSensor.angle != 360)
            {
                 activeSensor.distance = 0;
            }
            Debug.WriteLine("E/F: {0}", activeSensor.distance);

            return activeSensor.distance;
        }

        public float ceilingCollision()
        {
            float finalDistance;
            int[,] currentArray = heightArray;

            C.tileHeight = floorSensorHeight(ref C, currentArray);
            D.tileHeight = floorSensorHeight(ref D, currentArray);

            C.distanceCalculator();
            D.distanceCalculator();
            Debug.WriteLine("C: {0}\nD: {1}", C.distance, D.distance);

            if (C.distance < D.distance) // If A is closer to the surface than B
            {
                finalDistance = C.distance;
                winningSensor = C;
            }
            else // If B is closer than A
            {
                finalDistance = D.distance;
                winningSensor = D;
            }
            finalDistance = winningSensor.groundNotCeilingCheck();

            return finalDistance;
        }

        private int floorSensorHeight(ref Sensor sensor, int[,] currentArray)
        {
            int tileHeight1, tileHeight2;

            tileHeight1 = sensor.getTileHeight(sensor.row, sensor.column, activeTileMap, currentArray); // Get the index of the tile from downward sensor
            if (tileHeight1 == 0) // If the tile is empty, extend to check the next tile
            {
                sensor.Extend(); //move the sensor down one row/column
                tileHeight2 = sensor.getTileHeight(sensor.row, sensor.column, activeTileMap, currentArray); // Gets the index of the tile below. (Usually the same, but flipped tiles have swapped indices)

                if (tileHeight2 != 0) // If the next tile is not empty (there is terrain), change the tileheight to the new one
                {
                    tileHeight1 = tileHeight2;
                } // If the tile is empty, then nothing changes
            }
            else if (tileHeight1 == 16) // If the tile is full, regress to check the previous tile
            {
                sensor.Regress();
                tileHeight2 = sensor.getTileHeight(sensor.row, sensor.column, activeTileMap, currentArray);
                sensor.Extend(); // Just moving the sensor back to where it belongs

                if (tileHeight2 != 0) // If there is more terrain above, change the tileheight and row
                {
                    tileHeight1 = tileHeight2;
                    if (sensor.direction != Sensor.Direction.up) // IDK why but it helps
                    {
                        sensor.Regress();
                    }
                    if (sensor.angle == 360)
                    {
                        sensor.Regress();
                    }
                }
            }

            return tileHeight1;
        }

        private void GetLayer(Vector2 objectPosition, Vector2 objectSpeed)
        {
            int[,] layerMap = Tiles.TileMapLS;
            int row = (int)(objectPosition.Y / tileWidth);
            int column = (int)(objectPosition.X / tileWidth);

            if (row < 0) // Validation to prevent crashes
            {
                row = 0;
            }
            if (column < 0)
            {
                column = 0;
            }
            if (row >= layerMap.GetLength(0))
            {
                row = layerMap.GetLength(0) - 1;
            }
            if (column >= layerMap.GetLength(1))
            {
                column = layerMap.GetLength(1) - 1;
            }

            if (layerMap[row, column] == 1) // 2 to 1 horiz
            {
                if (objectSpeed.X > 0)
                {
                    activeTileMap = Tiles.TileMapL1;
                }
                else if (objectSpeed.X < 0)
                {
                    activeTileMap = Tiles.TileMapL2;
                }
            }
            else if (layerMap[row,column] == 2) // 2 to 1 vert
            {
                if (objectSpeed.Y > 0)
                {
                    activeTileMap = Tiles.TileMapL1;
                }
                else if (objectSpeed.Y < 0)
                {
                    activeTileMap = Tiles.TileMapL2;
                }
            }
            else if (layerMap[row, column] == 0) // 1 to 2 horiz
            {
                if (objectSpeed.X > 0)
                {
                    activeTileMap = Tiles.TileMapL2;
                }
                else if (objectSpeed.X < 0)
                {
                    activeTileMap = Tiles.TileMapL1;
                }
            }
            else if (layerMap[row, column] == 3) // 1 to 2 vert
            {
                if (objectSpeed.Y > 0)
                {
                    activeTileMap = Tiles.TileMapL2;
                }
                else if (objectSpeed.Y < 0)
                {
                    activeTileMap = Tiles.TileMapL1;
                }
            }
            else if (layerMap[row, column] == 4) // Always 1
            {
                activeTileMap = Tiles.TileMapL1;
            }


        }

        public bool CheckKillPlane(Vector2 objectPosition)
        {
            int row = (int)(objectPosition.Y / tileWidth);
            int column = (int)(objectPosition.X / tileWidth);

            if (row < 0) // Validation to prevent crashes
            {
                row = 0;
            }
            if (column < 0)
            {
                column = 0;
            }
            if (row >= activeTileMap.GetLength(0))
            {
                row = activeTileMap.GetLength(0) - 1;
            }
            if (column >= activeTileMap.GetLength(1))
            {
                column = activeTileMap.GetLength(1) - 1;
            }

            if (activeTileMap[row, column] == 2) // Kill Plane
            {
                return true;
            }
            return false;
        }

        public Sensor.Direction getFloorSensorDirection()
        {
            return A.direction;
        }
        public Sensor.Direction getWallSensorDirection()
        {
            return activeSensor.direction;
        }
        public Sensor.Direction getCeilingSensorDirection()
        {
            return C.direction;
        }

        public void Reset()
        {
            activeTileMap = Tiles.TileMapL1;
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
                        if (Math.Abs(x - A.position.X) <= 5 || Math.Abs(y - A.position.Y) <= 5)
                        {
                            colours.Add(new Color(255, 0, 0, 255));
                        }
                        else if (Math.Abs(x - B.position.X) <= 16 || Math.Abs(y - B.position.Y) <= 16)
                        {
                            colours.Add(new Color(0, 255, 0, 255));
                        }
                        else
                        {
                            colours.Add(new Color(255, 255, 255, 255));
                        }
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
