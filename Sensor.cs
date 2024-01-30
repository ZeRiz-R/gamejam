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
        public int distance; // Distance from sensor to tile
        public int tileHeight; // Height of the tile
        public float angle; // Angle of the tile

        private bool hFlip, vFlip; // Stores whether the tile has been horizontally/vertically flipped
        private int tileID; // Stores the ID of the tile (the number in the tile map), which can be used to look up the angle.

        public enum Direction
        {
            up,
            down,
            left,
            right
        }


        // Checks the tile the sensor is at and returns information about it
        public int getTileHeight(int row, int column, int[,] tileMap, int[,] array, Sensor.Direction givenDirection)
        {
            int tilePosition = 0;
            int tileIndex = 0;
            int tileWidth = Tiles.TileWidth;

            tileID = getTileType(row, column, tileMap);

            if (givenDirection == Direction.down) // Use x co-ordinates
            {
                tilePosition = column * tileWidth; // Finds top left of tile
                tileIndex = (int)position.X - tilePosition; // Finds difference in position (index)
                if (hFlip) // If the tile has been flipped horizontally, then the index is in reverse
                {
                    tileIndex = Tiles.TileWidth - tileIndex - 1; // Finds the reverse index (15 - index)
                }

                if (tileIndex < 0 || tileIndex >= tileWidth) // Validation to prevent crashes
                {
                    tileIndex = 1;
                }
                tileHeight = array[tileID, tileIndex];

            }
            else if (givenDirection == Direction.right) // Use y co-ordinates
            {
                tilePosition = row * tileWidth;
            }
            else if (givenDirection == Direction.up)
            {

            }

            return tileHeight;
        }

        public void distanceCalculator(Direction direction)
        {
            // Negative means object is overlapping
            // Left and upwards pointing sensors must be negated for the correct distance to add for the object

            int heightPositionTile = 0;

            if (direction == Direction.down) // If pointing down 
            {
                if (vFlip) // If the tile has been flipped, then the height grows from the top (base) of the tile
                {
                    heightPositionTile = (row * Tiles.TileWidth) + tileHeight;
                }
                else // If unflipped, then the tile grows from the bottom of the tile
                {
                    heightPositionTile = (row + 1) * Tiles.TileWidth - tileHeight;
                }

                distance = heightPositionTile - (int)position.Y; // Finds the difference between the tile and sensor
                // Positive if sensor is above tile, negative if below tile.
            }
        }

        // Note about flipped tiles, represented by 32 bit binary string. Index 0 is flipped horizontally. Index 1 is flipped
        // vertically. Index 2 is rotation (not relevant). The rest represents the tileID (so convert the rest back to an integer).
        public int getTileType(int row, int column, int[,] tileMap)
        {
            if (row < 0)
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
            hFlip = false;
            vFlip = false;

            if (tileID < 0 ) // If the ID is negative, then the tile must be flipped in some way.
            {
                string binary = Convert.ToString(tileID, 2); // Convert the value into a binary string

                if (binary[0] == '1') // If the first character is true, then the tile is flipped horizontally
                {
                    hFlip = true;
                }
                if (binary[1] == '1') // If the second character is true, then the tile is flipped vertically
                {
                    vFlip = true;
                }

                tileID = Convert.ToInt32(binary.Substring(3), 2);
            }

            return tileID;
        }

    }
}
