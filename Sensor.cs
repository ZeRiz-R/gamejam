using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelesteLike
{
    internal class Sensor
    {
        public Vector2 position; // Position of the sensor
        public int row, column; // Row and column of tile map that the sensor is in
        public float distance; // Distance from sensor to tile
        public int tileHeight; // Height of the tile
        public float angle; // Angle of the tile

        private bool hFlip, vFlip; // Stores whether the tile has been horizontally/vertically flipped
        private int tileID; // Stores the ID of the tile (the number in the tile map), which can be used to look up the angle.
        public int TileID { get { return tileID; } }

        int tileWidth = Tiles.TileWidth;

        // This allows other classes to specify which direction the sensor should face
        public enum Direction
        {
            up,
            down,
            left,
            right
        }

        public Direction direction;


        // Checks the tile the sensor is at and returns information about it
        public int getTileHeight(int row, int column, int[,] tileMap, int[,] array)
        {
            int tilePosition = 0; // Position of the top-left corner of the tile
            int tileIndex = 0; // Index of the tile that the player is at

            // Retrieves the tileID from the tilemap
            tileID = getTileType(row, column, tileMap);

            // If the sensor faces up or down, then the x-axis index is needed
            if (direction == Direction.down || direction == Direction.up) 
            {
                tilePosition = column * tileWidth; // Finds top left of tile
                tileIndex = (int)position.X - tilePosition; // Finds difference in position (index)

                // If the tile has been flipped horizontally, then the index has also been flipped
                if (hFlip) 
                {
                    tileIndex = tileWidth - tileIndex - 1; // Finds the reverse index (15 - index)
                }

                if (tileIndex < 0 || tileIndex >= tileWidth) // Validation to prevent crashes
                {
                    tileIndex = 1;
                }

                // Retrieves the height of the tile at the index from the height/width array
                tileHeight = array[tileID, tileIndex];

            }

            // If the sensor is facing left or right, then the y-axis index is needed
            else if (direction == Direction.right || direction == Direction.left) // Use y co-ordinates
            {
                tilePosition = row * tileWidth; //Finds top-left of the tile
                tileIndex = (int)position.Y - tilePosition; // Finds the index through the difference in position

                // If the tile has been flipped vertically, then the index has also been flipped
                if (vFlip)
                {
                    tileIndex = tileWidth - tileIndex - 1; // Finds the reverse index (15 - index)
                }

                if (tileIndex < 0 || tileIndex >= tileWidth) // Validation to prevent crashes
                {
                    tileIndex = 1;
                }

                // Retrieves the tile height from the height/width array
                tileHeight = array[tileID, tileIndex];
            }

            return tileHeight;
        }

        
        // Calculates the distance between the tile and the sensor
        public void distanceCalculator()
        {
            // A negative distance means object is overlapping (when sensing down/right)
            // A positive distance means the object is above the surface (when sensing down/right)

            int heightPositionTile = 0; // Position of the tile with the height of the tile added

            // If the sensor is facing up or down, then the height is added vertically
            if (direction == Direction.down || direction == Direction.up) 
            {
                if (vFlip || direction == Direction.up) // If the tile has been flipped vertically, then the height grows from the top of the tile
                {
                    heightPositionTile = (row * tileWidth) + tileHeight;
                }
                else // If unflipped, then the tile grows from the bottom of the tile
                {
                    heightPositionTile = (row + 1) * tileWidth - tileHeight;
                }

                distance = heightPositionTile - position.Y; // Finds the difference between the tile and sensor

                // Negates the distance if sensing up, so that a collision returns a negative distance too
                if (direction == Direction.up)
                {
                    distance *= -1;
                }
            }

            // If the sensor is facing left or right, then the height is added horizontally
            else if (direction == Direction.right || direction == Direction.left)
            {
                if (hFlip || direction == Direction.left) // If the tile has been flipped horizontally, then the tile grows from the left of the tile
                {
                    heightPositionTile = (column * tileWidth) + tileHeight;
                }
                else
                {
                    heightPositionTile = (column + 1) * tileWidth - tileHeight; ;
                }

                distance = heightPositionTile - position.X -1; // Finds the difference between the tile and sensor

                // Negates the distance if sensing left, so that a collisions still returns a negative value
                if (direction == Direction.left)
                {
                    distance += 2;
                    distance *= -1;
                }
            }
        }

        // Note about flipped tiles: They are represented by a 32 bit binary string.
        // Index 0 stores whether the tile has been flipped horizontally.
        // Index 1 stores whether the tile has been flipped vertically.
        // Index 2 stores whether the tile has been rotated (not relevant with this project).
        // The rest represents the tileID that the tile originated from.
        public int getTileType(int row, int column, int[,] tileMap)
        {
            if (row < 0) // Validation to prevent crashes
            {
                row = 0;
            }
            if (column < 0)
            {
                column = 0;
            }
            if (row >= tileMap.GetLength(0))
            {
                row = tileMap.GetLength(0) - 1;
            }
            if (column >= tileMap.GetLength(1))
            {
                column = tileMap.GetLength(1) - 1;
            }

            tileID = tileMap[row, column]; // Retrieves the tileID from the tilemap
            hFlip = false; // Resets the flip variables
            vFlip = false;

            // If the tile is negative or greater than (2^29), the tile has been flipped in some way
            if (tileID < 0  || tileID > 536870912)
            {
                string binary = Convert.ToString(tileID, 2).PadLeft(32, '0'); // Convert the value into a binary string

                if (binary[0] == '1') // If the first character is true, then the tile is flipped horizontally
                {
                    hFlip = true;
                }
                if (binary[1] == '1') // If the second character is true, then the tile is flipped vertically
                {
                    vFlip = true;
                }

                tileID = Convert.ToInt32(binary.Substring(3), 2); // Connverts the tileID into a regular number (of its unflipped coutnerpart)
            }

            angle = Tiles.AngleArray[tileID]; // Retrieves angle from the tilemap

            // Adjusts the angles for flipped tiles
            if (hFlip && vFlip) 
            {
                angle = 180 + angle;
            }
            else if (hFlip)
            {
                angle = 360 - angle;
            }
            else if (vFlip)
            {
                angle = 180 - angle;
            }

            return tileID;
        }

        // Extends the tile in the specified direction
        public void Extend()
        {
            if (direction == Direction.down)
            {
                row += 1; // Moves the tile down one row
            }
            else if (direction == Direction.right)
            {
                column += 1; // Moves the tile forwards 
            }
            else if (direction == Direction.up)
            {
                row -= 1; // Moves the tile up one row
            }
            else if (direction == Direction.left)
            {
                column -= 1; // Moves the tile backwards
            }
        }

        // Regresses the tile in the specified direction
        public void Regress()
        {
            if (direction == Direction.down)
            {
                row -= 1;
            }
            else if (direction == Direction.right)
            {
                column -= 1;
            }
            else if (direction == Direction.up)
            {
                row += 1;
            }
            else if (direction == Direction.left)
            {
                column += 1;
            }
        }

        // Gets the angle of the tile that is 1 pixel below the current position.
        // This has a use case for when the player is on completely flat ground.
        // Usually an angle of 0 (from an empty tile) is returned, which prevents
        // angle snapping. This allows angle snapping.
        public float getAngleJustBelow()
        {
            // Moves the sensor by 1 pixel in the specified direction
            if (direction == Direction.down)
            {
                position.Y += 1;
            }
            if (direction == Direction.up)
            {
                position.Y -= 1;
            }
            if (direction == Direction.left)
            {
                position.X -= 1;
            }
            if (direction == Direction.right)
            {
                position.X += 1;
            }

            // Recalculates the row and column of the sensor
            row = (int)position.Y / tileWidth;
            column = (int)position.X / tileWidth;

            // Retrieves the angle of this "new" tile
            angle = Tiles.AngleArray[getTileType(row, column, Tiles.TileMap)];

            // Adjusts the angles of any flipped tiles
            if (hFlip && vFlip)
            {
                angle = 180 + angle;
            }
            else if (hFlip)
            {
                angle = 360 - angle;
            }
            else if (vFlip)
            {
                angle = 180 - angle; // Gets the reflected angle
            }

            return angle;
        }

        public float groundNotCeilingCheck()
        {
            if (direction == Direction.up)
            {
                if (!vFlip && !(tileID == 0 || tileID == 1))
                {
                    return 0; // Make no distance
                }
            }
            return distance;
        }
    }
}
