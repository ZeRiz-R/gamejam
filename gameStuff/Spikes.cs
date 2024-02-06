using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelesteLike.gameStuff
{
    internal class Spikes : Sprite
    {

        public Spikes(Vector2 thePosition) : base(theAssetName: "Spikes", thePosition, width: 32, height: 32)
        {
            collisionBox = new Rectangle((int)position.X, (int)position.Y, objectWidth, objectHeight);
        }

        public void Update(Player player)
        {
            if (collisionBox.Intersects(player.collisionBox))
            {
                int topIntersect = player.collisionBox.Bottom - collisionBox.Top;
                int LeftIntersect = player.collisionBox.Right - collisionBox.Left;
                int bottomIntersect = collisionBox.Bottom - player.collisionBox.Top;
                int rightIntersect = collisionBox.Right - player.collisionBox.Left;
                int[] intersections = new int[] { topIntersect, LeftIntersect, bottomIntersect, rightIntersect};

                int minimum = getMinIntersection(intersections);
                if (minimum == 0) // Top
                {
                    player.GetHurt();
                }
                else if (minimum == 1) // Left
                {
                    player.CollideX(-LeftIntersect);
                }
                else if (minimum == 2) // Bottom
                {
                    player.CollideY(bottomIntersect);
                }
                else // Right
                {
                    player.CollideX(rightIntersect);
                }

                base.Update();
            }
        }

        // Returns which of the 4 intersections has the minimum value (this tells us where the player has collided from.
        private int getMinIntersection(int[] allIntersections)
        {
            int minIntersection = allIntersections.Min();

            for (int i = 0; i < allIntersections.Length; i++)
            {
                if (minIntersection == allIntersections[i])
                {
                    return i;
                }

            }
            return 0;
        }
    }
}
